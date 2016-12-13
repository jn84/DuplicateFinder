﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace DuplicateFinder
{
	internal class FileDictionary : IEnumerable<List<string>>
	{
		private MD5 hasher;
		private Dictionary<string, List<string>> _storage = new Dictionary<string, List<string>>();


		public List<string> this[string k]
		{
			get { return _storage[k]; }
			set { _storage[k] = value; }
		}

		public string LastKey { get; private set; }

		public void Add(string filePath)
		{
			hasher = new MD5CryptoServiceProvider();
			try
			{
				var inputFileInfo = new FileInfo(filePath);
				File.SetAttributes(filePath, FileAttributes.Normal);

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
							hasher.TransformBlock(buffer, 0, r, null, 0);
						}
					}
					hasher.TransformFinalBlock(buffer, 0, 0);
				}
				else
				{
					using (Stream inputFile = File.Open(filePath, FileMode.Open))
					{
						hasher.ComputeHash(inputFile);
					}
				}
			}
			catch (IOException) { return; }
			
			LastKey = BitConverter.ToString(hasher.Hash);

			List<string> targetList;

			if (_storage.ContainsKey(LastKey))
			{
				_storage.TryGetValue(LastKey, out targetList);
				targetList.Add(filePath);
				return;
			}
			targetList = new List<string> { filePath };
			_storage.Add(LastKey, targetList);
		}

		public IEnumerator<List<string>> GetEnumerator()
		{
			return _storage.Values.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}