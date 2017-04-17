using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DuplicateFinder
{
	internal class FileDictionary : IEnumerable<List<string>>
	{
		private readonly bool _isSkipEmptyFiles;

		public ConcurrentQueue<Tuple<string, FileInfo>> FilesHashedQueue { get; } = new ConcurrentQueue<Tuple<string, FileInfo>>();

		private readonly FrmDuplicateFinder_Main _formMain_ref;

		// Initial comparison of file sizes
		private ConcurrentDictionary<long, List<FileInfo>> _concurrentSizeDictionary = new ConcurrentDictionary<long, List<FileInfo>>();

		// Secondary comparison of equal sized files

		public ConcurrentDictionary<string, List<string>> _concurrentHashDictionary { get; } = new ConcurrentDictionary<string, List<string>>();

		private List<DirectoryInfo> _fullDirectoryList = new List<DirectoryInfo>();
		private int _filesProcessedCount = 0,  
		            _totalFilesCount = 0;       

		// TODO: File sizes should be compared before we spend the time calculating MD5
		// TODO: Additionally, the option to require matching file names should be added

		public FileDictionary(bool isSkipEmptyFiles, FrmDuplicateFinder_Main form) 
		{
			_isSkipEmptyFiles = isSkipEmptyFiles;
			_formMain_ref = form;
		}

		public List<string> this[string key]
		{
			get => _concurrentHashDictionary[key];
			set => _concurrentHashDictionary[key] = value;
		}

		// TODO: Getting the list of directories/files can be threaded. Should perhaps be moved into FileDictionary. The form doesn't need to handle this. We will however need to relay information back to the form.
		public void BuildDirectoryList(List<string> initialDirecotryList)
		{
			// Add the initial list of directories to the dictionary. This is the list of directories added by the user and show in the listbox
			_fullDirectoryList = new List<DirectoryInfo>();
			foreach (string strElem in initialDirecotryList)
				_fullDirectoryList.Add(new DirectoryInfo(strElem));

			// Do not use foreach since the collection is modified within the loop. Since I keep trying to convert it...
			for (int i = 0; i < _fullDirectoryList.Count; i++)
			{
				try
				{
					List<FileInfo> fi = _fullDirectoryList[i].GetFiles().ToList();

					_totalFilesCount += fi.Count;
					_fullDirectoryList.AddRange(_fullDirectoryList[i].GetDirectories());

					_formMain_ref.UpdateDirectory_FileCounts(_totalFilesCount, _fullDirectoryList.Count);

					new Thread(() => BuildFileSizeDictionaryPiece(fi)).Start();
				}
				catch (PathTooLongException) { /* This is, as of commit #4, an unfixable error. Will keep an eye out for potential workarounds. */ }
				catch (UnauthorizedAccessException) { /* Just skip the folder. No handling necessary. */ }
			}
		}

		private void BuildFileSizeDictionaryPiece(List<FileInfo> fileList)
		{
			Parallel.For(0, fileList.Count, i =>
			{
				List<FileInfo> currentFiles;
				if (_concurrentSizeDictionary.TryGetValue(fileList.Count, out currentFiles))
				{
					lock (currentFiles)
					{
						currentFiles.Add(fileList[i]);
					}
				}
				else
					_concurrentSizeDictionary.TryAdd(fileList[i].Length, new List<FileInfo>() { fileList[i] } );
			});
		}

		// Add a file to the file dictionary
		public void _BuildFileDictionary()
		{
			if (_concurrentSizeDictionary.IsEmpty) // || likely other failure scenarios
				throw new Exception("Placeholder: BuildDirectoryList must be successfully run prior to this method (BuildFileDictionary)");

			foreach (DirectoryInfo dirElem in _fullDirectoryList)
			{
				try
				{
					Parallel.ForEach(dirElem.GetFiles().ToList(), fElem =>
					{
						Add(fElem);
						_filesProcessedCount++;
						_formMain_ref.UpdateProgressBar(_filesProcessedCount, _totalFilesCount);
					});
				}
				catch (PathTooLongException) { /* This is, as of commit #4, an unrecoverable error. Will keep an eye out for potential workarounds */ }
				catch (UnauthorizedAccessException) { }
			}
		}

		// Add a file to the file dictionary
		public void BuildFileDictionary()
		{
			if (_concurrentSizeDictionary.IsEmpty) // || likely other failure scenarios
				throw new Exception("Placeholder: BuildDirectoryList must be successfully run prior to this method (BuildFileDictionary)");

			foreach (KeyValuePair<long, List<FileInfo>> kvPair in _concurrentSizeDictionary)
			{
				// We only care about files with duplicate sizes. If size of list = 1, then there are no duplicates
				if (kvPair.Value.Count == 1)
					continue;

				try
				{
					Parallel.ForEach(kvPair.Value, fileListElem =>
					{
						Add(fileListElem);
						_filesProcessedCount++;
						_formMain_ref.UpdateProgressBar(_filesProcessedCount, _totalFilesCount);
					});
				}
				catch (PathTooLongException) { /* This is, as of commit #4, an unrecoverable error. Will keep an eye out for potential workarounds */ }
				catch (UnauthorizedAccessException) { }
			}
		}

		public void Add(FileInfo fInfo)
		{
			var hasher = new MD5CryptoServiceProvider();
			try
			{
				if (_isSkipEmptyFiles && fInfo.Length.Equals(0))
				{
					//Console.WriteLine(fInfo.FullName + @" : " + fInfo.Length);
					return;
				}

				// Don't like this. Why was it here? TBD
				// We Shouldn't be modifying the attributes of file processed..
				// Must have been one of my stupid SO finds to solve some issue
				//File.SetAttributes(filePath, FileAttributes.Normal);

				if (fInfo.Length > 10485760)
				{
					const long bytesToRead = 10485760;
					const int bufferSize = 1048576;

					long read = 0;
					var r = -1;
					byte[] buffer = new byte[bufferSize];

					using (var stream = new FileStream(fInfo.FullName, FileMode.Open, FileAccess.Read))
					{
						while (read <= bytesToRead && r != 0)
						{
							read += (r = stream.Read(buffer, 0, bufferSize));
							hasher.TransformBlock(buffer, 0, r, null, 0);
						}
					}
					hasher.TransformFinalBlock(buffer, 0, 0);
				}
				else
				{
					using (Stream inputFile = File.Open(fInfo.FullName, FileMode.Open))
					{
						hasher.ComputeHash(inputFile);
					}
				}
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine(@"*** Isufficient priveleges to process " + fInfo.FullName + @"n     Skipping file.");
				return;
			}

			catch (IOException e)
			{
				Console.WriteLine(@"*** IOException: Add method caused a problem: " + e.Message);
				return;
			}

			string key = BitConverter.ToString(hasher.Hash);

			List<string> targetList;

			if (_concurrentHashDictionary.ContainsKey(key))
			{
				_concurrentHashDictionary.TryGetValue(key, out targetList);
				if (targetList != null)
					//lock (targetList)					
						targetList.Add(fInfo.FullName);
				return;
			}
			targetList = new List<string> { fInfo.FullName };
			_concurrentHashDictionary.TryAdd(key, targetList);
		}

		public IEnumerator<List<string>> GetEnumerator()
		{
			return _concurrentHashDictionary.Values.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}