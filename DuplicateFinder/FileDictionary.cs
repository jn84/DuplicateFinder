using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;

namespace DuplicateFinder
{
	internal class FileDictionary : IEnumerable<List<string>>
	{
		// TODO: Add some way to switch between crypto types based on a single variable (which would selectable from the GUI)
		private MD5 _MD5Hasher;
		private SHA1 _SHA1Hasher;
		private SHA256 _SHA256Hasher;
		private SHA512 _SHA512Hasher;
		private readonly Dictionary<string, List<string>> _fileHashDictionary = new Dictionary<string, List<string>>();
		private Dictionary<long, List<FileInfo>> _fileSizeDictionary = new Dictionary<long, List<FileInfo>>();
		private List<FileInfo> _equalSizeList = new List<FileInfo>();
		
		// TODO: File sizes should be compared before we spend the time calculating MD5
		// TODO: Additionally, the option to require matching file names should be added

		public List<string> this[string key]
		{
			get { return _fileHashDictionary[key]; }
			set { _fileHashDictionary[key] = value; }
		}

		public string LastKey { get; private set; }

		internal FileDictionary()
		{
			
		}


		// This method should build lists of files whos sizes are equal as a pre-filter for equivalent files
		// Any files whos sizes are the same should then be compared by hashing and possibly byte<->byte comparison
		//		to determine if they are truly equal
		public void AddDirectoryFiles(DirectoryInfo dInfo)
		{
			FileInfo[] fileArr = dInfo.GetFiles();
			foreach (FileInfo file in fileArr)
			{
				long fLen = file.Length;

				if (!_fileSizeDictionary.ContainsKey(fLen))
					_fileSizeDictionary.Add(fLen, new List<FileInfo> { file });
				else
					_fileSizeDictionary[fLen].Add(file);
			}
		}

		// Method to process the lists of files whose sizes are equal
		// Compare the MD5 SUMS, then <maybe> potentially a byte<->byte comparison
		public void BuildComparisonList()
		{
			_MD5Hasher = new MD5CryptoServiceProvider();

			// For each list of same size files in _fileSizeDictionary...
			foreach (List<FileInfo> fileList in _fileSizeDictionary.Values)
			{
				// Where the list is greater than 1 (more than one file of the given size)
				if (fileList.Count > 1)
				{
					// Calculate the file's hash (MD5, SHA1, SHA256, SHA512) and add it to _fileHashDictionary
				}
			}
		}

		public void Add(string filePath)
		{
			_MD5Hasher = new MD5CryptoServiceProvider();
			try
			{
				var inputFileInfo = new FileInfo(filePath);

				// TODO: NOTE: I don't know why this was here. Maybe we'll find out...
				//File.SetAttributes(filePath, FileAttributes.Normal);

				// Computes the hash of the first 10MB of the file 
				// Probably much better ways to go about this
				// TODO: CAN WEED OUT FILES WITH A SIZE OF 0 (DO IN BuildComparisonList)
				if (inputFileInfo.Length > 10485760)
				{
					const long bytesToRead = 10485760;
					const int bufferSize = 1048576;

					long read = 0;
					var r = -1;
					byte[] buffer = new byte[bufferSize];

					using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
					{
						while (read <= bytesToRead && r != 0)
						{
							read += (r = stream.Read(buffer, 0, bufferSize));
							_MD5Hasher.TransformBlock(buffer, 0, r, null, 0);
						}
					}
					_MD5Hasher.TransformFinalBlock(buffer, 0, 0);
				}
				else
				{
					using (Stream inputFile = File.Open(filePath, FileMode.Open))
					{
						_MD5Hasher.ComputeHash(inputFile);
					}
				}
			}
			catch (IOException) { return; }
			
			LastKey = BitConverter.ToString(_MD5Hasher.Hash);

			List<string> targetList;

			if (_fileHashDictionary.ContainsKey(LastKey))
			{
				_fileHashDictionary.TryGetValue(LastKey, out targetList);
				targetList.Add(filePath);
				return;
			}
			targetList = new List<string> { filePath };
			_fileHashDictionary.Add(LastKey, targetList);
		}

		public IEnumerator<List<string>> GetEnumerator()
		{
			return _fileHashDictionary.Values.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}