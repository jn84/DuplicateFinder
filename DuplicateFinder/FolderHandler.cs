using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace DuplicateFinder
{
	internal class FolderHandler
	{
		private Dictionary<int, List<string>> _storage = new Dictionary<int, List<string>>();

		public List<string> this[int k]
		{
			get { return _storage[k]; }
			set { _storage[k] = value; }
		}

		public int LastKey { get; private set; }

		public void Add(string folderPath)
		{
			var temp = new DirectoryInfo(folderPath);
			try
			{
				if (temp.Parent != null) LastKey = temp.Parent.GetFileSystemInfos().GetHashCode();
			}
			catch (SecurityException)
			{
				Console.WriteLine("Folder " + temp.Name + " is blocked due to security.");
				return;
			}
			catch (Exception)
			{
				return;
			}

			// Just file names?

			// Doesn't account for contents of subfolders.
			// If a folder is equivalent, it should match including all subfolders.
			//if (_storage.ContainsKey(key) && _storage[key].Count > 1)
			//	return;

			LastKey = temp.GetFileSystemInfos(folderPath).GetHashCode();
			
			List<string> targetList;

			if (_storage.ContainsKey(LastKey))
			{
				_storage.TryGetValue(LastKey, out targetList);
				targetList.Add(folderPath);
				return;
			}
			targetList = new List<string> { folderPath };
			_storage.Add(LastKey, targetList);
		}
	}
}
