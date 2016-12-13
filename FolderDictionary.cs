using System;

namespace DuplicateFinder_Jenny
{
	internal class FolderDictionary : IEnumerable<List<string>>
	{
		private MD5 hasher;
		private Dictionary<string, List<string>> _storage = new Dictionary<string, List<string>>();
		private string lastKey;


		public List<string> this[string k]
		{
			get { return _storage[k]; }
			set { _storage[k] = value; }
		}

		public string LastKey
		{
			get { return lastKey; }
		}

		public void Add(string folderPath)
		{
			hasher = new MD5CryptoServiceProvider();
		




			lastKey = BitConverter.ToString(hasher.Hash);

			List<string> targetList;

			if (_storage.ContainsKey(lastKey))
			{
				_storage.TryGetValue(lastKey, out targetList);
				targetList.Add(filePath);
				return;
			}
			targetList = new List<string> { filePath };
			_storage.Add(lastKey, targetList);
		}

		public IEnumerator<List<string>> GetEnumerator()
		{
			return _storage.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
