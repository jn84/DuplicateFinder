using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;

namespace DuplicateFinder
{
	internal class FileHandler
	{
		private long _hashCounter = 0;

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

		public string LastKey { get; private set; }

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

		// Method to process the lists of files whose sizes are equal
		// Compare the MD5 SUMS, then <maybe> potentially a byte<->byte comparison
		public int HashNextFileInfoList()
		{
			// For each list of same size files in _fileSizeDict...
				// If the list has only one element, it must not be duplicate -> go to next list
				if (fList.Count == 1)
					return 1;

				// Calculate each file hash (MD5, SHA1, SHA256, SHA512) and add it to _fileHashDictionary
				foreach (FileInfo file in fList)
				{
					string currentHash = _hasher.GetHash(file);
					if (!_fileHashDict.ContainsKey(currentHash))
						_fileHashDict.Add(_hasher.GetHash(file), new List<FileInfo> { file } );
					else
						_fileHashDict[currentHash].Add(file);
				}
				return
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