using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DuplicateFinder
{
	internal class FileHandler
	{
		private int _hashListCounter = 0;

		// TODO: Add some way to switch between crypto types based on a single variable (which would selectable from the GUI)
		private readonly HashBase<MD5CryptoServiceProvider> _hasher = new HashBase<MD5CryptoServiceProvider>();

		private readonly Dictionary<string, List<FileInfo>> _fileHashDict = new Dictionary<string, List<FileInfo>>();
		private readonly Dictionary<long, List<FileInfo>> _fileSizeDict = new Dictionary<long, List<FileInfo>>();

		// TODO: Additionally, the option to require matching file names should be added

		public List<FileInfo> this[string key]
		{
			get { return _fileHashDict[key]; }
			set { _fileHashDict[key] = value; }
		}

		public Dictionary<long, List<FileInfo>> FileSizeDictionary => _fileSizeDict;
		public Dictionary<string, List<FileInfo>> FileHashDictionary => _fileHashDict;


		public string LastKey { get; private set; }

		public void Reset()
		{

		}

		// This method should build lists of files whos sizes are equal as a pre-filter for equivalent files
		// Any files whos sizes are the same should then be compared by hashing and possibly byte<->byte comparison
		//		to determine if they are truly equal

		// Returns the total number of files in the directory
		public int AddDirectoryFiles(DirectoryInfo dInfo)
		{
			FileInfo[] fileArr = dInfo.GetFiles();
			foreach (FileInfo file in fileArr)
			{
				// Grab the file length
				long fLen = file.Length;

				if (!_fileSizeDict.ContainsKey(fLen))
					_fileSizeDict.Add(fLen, new List<FileInfo> { file });
				else
					_fileSizeDict[fLen].Add(file);
			}
			return fileArr.Length;
		}

		// Method to process the lists of files whose sizes are equal
		// Compare the MD5 SUMS, then <maybe> potentially a byte<->byte comparison
		// Return value indicates the number of files that were processed. 
		// -1 indicates end of dictionary (all files processed) 
		public int HashNextFileInfoList()
		{
			int counter = 0;

			List<FileInfo> fList;

			// If the list has only one element, it must not be duplicate -> go to next list
			//		and increment the file counter
			while (true)
			{
				try
				{
					fList = _fileSizeDict.ElementAt(_hashListCounter).Value;
				}
				catch (Exception e)
				{
					// TODO: Debug line (will flood console)
					Console.WriteLine(e.GetType());
					return -1;
				}

				// Cannot have duplicate, so don't bother adding to the hash collection
				// Cannot (should not) ever be < 1
				if (fList.Count != 1)
					break;
				if (fList[0].Length == 0)
					counter += HandleZeroSizeFiles();
				else
					counter++;
				_hashListCounter++;
			}
			
			// Calculate each file hash (MD5, SHA1, SHA256, SHA512) and add it to _fileHashDict
			Parallel.ForEach(fList, file =>
			{
				string currentHash = _hasher.GetHash(file);
				if (!_fileHashDict.ContainsKey(currentHash))
					_fileHashDict.Add(_hasher.GetHash(file), new List<FileInfo> { file });
				else
					_fileHashDict[currentHash].Add(file);
			});

			// TODO: Commented for later in case the Parallel variant has issues
			//foreach (FileInfo file in fList)
			//{
			//	string currentHash = _hasher.GetHash(file);
			//	if (!_fileHashDict.ContainsKey(currentHash))
			//		_fileHashDict.Add(_hasher.GetHash(file), new List<FileInfo> { file });
			//	else
			//		_fileHashDict[currentHash].Add(file);
			//}

			_hashListCounter++;
			return fList.Count + counter;
		}

				// Special handling for zero size file since there are typically many of them
		// Don't waste time hashing them all
		private int HandleZeroSizeFiles()
		{
			List<FileInfo> zeroSizeFiles;

			// If there are any files with a size of zero
			if (!_fileSizeDict.TryGetValue(0, out zeroSizeFiles)) return 0;
			if (zeroSizeFiles.Count == 1) return zeroSizeFiles.Count;
			string hash = _hasher.GetHash(zeroSizeFiles[0]);
			_fileHashDict.Add(hash, zeroSizeFiles);
			return zeroSizeFiles.Count;
		}

		//public void Add(string filePath)
		//{
		//	_MD5Hasher = new MD5CryptoServiceProvider();
		//	try
		//	{
		//		var inputFileInfo = new FileInfo(filePath);

		//		// TODO: NOTE: I don't know why this was here. Maybe we'll find out...
		//		//File.SetAttributes(filePath, FileAttributes.Normal);

		//		// Computes the hash of the first 10MB of the file 
		//		// Probably much better ways to go about this
		//		if (inputFileInfo.Length > 10485760)
		//		{
		//			const long bytesToRead = 10485760;
		//			const int bufferSize = 1048576;

		//			long read = 0;
		//			var r = -1;
		//			byte[] buffer = new byte[bufferSize];

		//			using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		//			{
		//				while (read <= bytesToRead && r != 0)
		//				{
		//					read += (r = stream.Read(buffer, 0, bufferSize));
		//					_MD5Hasher.TransformBlock(buffer, 0, r, null, 0);
		//				}
		//			}
		//			_MD5Hasher.TransformFinalBlock(buffer, 0, 0);
		//		}
		//		else
		//		{
		//			using (Stream inputFile = File.Open(filePath, FileMode.Open))
		//			{
		//				_MD5Hasher.ComputeHash(inputFile);
		//			}
		//		}
		//	}
		//	catch (IOException) { return; }

		//	LastKey = BitConverter.ToString(_MD5Hasher.Hash);

		//	List<string> targetList;

		//	if (_fileHashDictionary.ContainsKey(LastKey))
		//	{
		//		_fileHashDictionary.TryGetValue(LastKey, out targetList);
		//		targetList.Add(filePath);
		//		return;
		//	}
		//	targetList = new List<string> { filePath };
		//	_fileHashDictionary.Add(LastKey, targetList);
		//}
	}
}