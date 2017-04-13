using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Concurrent;

namespace DuplicateFinder
{
	internal class FileDictionary : IEnumerable<List<string>>
	{
		private readonly bool _isSkipEmptyFiles;

		public ConcurrentQueue<Tuple<string, FileInfo>> FilesHashedQueue { get; } = new ConcurrentQueue<Tuple<string, FileInfo>>();

		// Initial comparison of file sizes
		private ConcurrentDictionary<long, List<FileInfo>> _fileSizeStorage = new ConcurrentDictionary<long, List<FileInfo>>();

		// Secondary comparison of equal sized files
		private ConcurrentDictionary<string , List<string>> _concurrentStorage = new ConcurrentDictionary<string, List<string>>();

		// TODO: File sizes should be compared before we spend the time calculating MD5
		// TODO: Additionally, the option to require matching file names should be added

		public FileDictionary(bool isSkipEmptyFiles)
		{
			_isSkipEmptyFiles = isSkipEmptyFiles;
		}

		public bool TryGetNextKey(out Tuple<string, FileInfo> result)
		{
			return FilesHashedQueue.TryDequeue(out result);
		}

		public List<string> this[string key]
		{
			get
			{
				return _concurrentStorage[key];
			}
			set
			{
				_concurrentStorage[key] = value;
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

			if (_concurrentStorage.ContainsKey(key))
			{
				_concurrentStorage.TryGetValue(key, out targetList);
				if (targetList != null)
					//lock (targetList)					
						targetList.Add(fInfo.FullName);
				return;
			}
			targetList = new List<string> { fInfo.FullName };
			_concurrentStorage.TryAdd(key, targetList);

			FilesHashedQueue.Enqueue(Tuple.Create(key, fInfo));
		}

		public IEnumerator<List<string>> GetEnumerator()
		{
			return _concurrentStorage.Values.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}