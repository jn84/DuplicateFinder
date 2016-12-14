using System;
using System.IO;
using System.Security.Cryptography;

namespace DuplicateFinder
{
	public class HashBase<T> where T : HashAlgorithm, new()
	{
		private readonly T _hashType = new T();

		public string GetHash(FileInfo file)
		{
			using (Stream inputFile = File.Open(file.FullName, FileMode.Open))
			{
				_hashType.ComputeHash(inputFile);
				return BitConverter.ToString(_hashType.Hash);
			}
		}

		// Process only part of the file (The first len bytes)
		public string GetHash(FileInfo file, long len)
		{
			using (Stream inputFile = File.Open(file.FullName, FileMode.Open))
			{
				_hashType.ComputeHash(inputFile);
				return BitConverter.ToString(_hashType.Hash);
			}
		}
	}
}