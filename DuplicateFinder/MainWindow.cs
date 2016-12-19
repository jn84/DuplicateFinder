using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// New branch test commit

namespace DuplicateFinder
{
	public partial class frmDuplicateFinderMain : Form
	{
		// Preferred long, but without modifying progress bar it wont work
		private int _totalFilesCount = 0,
					 _filesProcessedCount = 0,
					 _totalDirectoriesCount = 0,
					 _directoriesProcessedCount = 0;

		// List of locations to include in search for duplicates
		private readonly List<DirectoryInfo> _searchDirectoriesList = new List<DirectoryInfo>();
		private List<DirectoryInfo> _allDirectoriesList;

		// Local references to 
		// Final hashed collection of duplicates 
		// <key: hash, value: list of files matching hash>
		private Dictionary<string, List<FileInfo>> _fileHashDict;
		// Collection of file sizes stored in lists of equal size 
		// <key: file size in bytes, value: list of files matching the key size>
		private Dictionary<long, List<FileInfo>> _fileSizeDict;

		// FileHandler class instance to process the lists for duplicates
		private FileHandler _fileComparer;


		public frmDuplicateFinderMain()
		{
			InitializeComponent();
			Reset();
			lstDirectories.Sorted = true;
		}

		public Dictionary<string, List<FileInfo>> FileHashDict => _fileHashDict;

		public Dictionary<long, List<FileInfo>> FileSizeDict => _fileSizeDict;

		private void Reset()
		{
			_fileComparer = new FileHandler();
			_fileHashDict = _fileComparer.FileHashDictionary;
			_fileSizeDict = _fileComparer.FileSizeDictionary;
			_searchDirectoriesList.Clear();
			lstDirectories.Items.Clear();
			_allDirectoriesList = new List<DirectoryInfo>();
		}

		private void btnAddDirectory_Click(object sender, EventArgs e)
		{
			AddDirectory();
		}

		// Add a directory to include in the search
		private void AddDirectory()
		{
			var tempDir = new FolderBrowserDialog();
			tempDir.ShowDialog();
			_searchDirectoriesList.Add(new DirectoryInfo(tempDir.SelectedPath));
			lstDirectories.Items.Add(tempDir.SelectedPath);
			// objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
			// TODO: Test whether the sorting of of the initial dir list and the underlying list match up after deleting a directory
			_searchDirectoriesList.Sort(
				(x, y) => string.Compare(x.FullName, y.FullName, StringComparison.Ordinal));
		}

		// TODO: Rework
		// Execute the duplicate search
		private void btnFindDupes_Click(object sender, EventArgs e)
		{
			var build = new Thread(BuildDirectoryList);
			btnFindDupes.Enabled = false;
			build.Start();
		}

		//// Context menu code

		private void lstDirectories_MouseDown(object sender, MouseEventArgs e)
		{
			lstDirectories.SelectedIndex = lstDirectories.IndexFromPoint(e.X, e.Y);

			if (e.Button == System.Windows.Forms.MouseButtons.Right)
				ctxMenuDir.Show(lstDirectories, e.X, e.Y);
		}

		private void addDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddDirectory();
		}

		private void removeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (lstDirectories.Items.Count.Equals(0))
				return;
			_searchDirectoriesList.RemoveAt(lstDirectories.SelectedIndex);
			lstDirectories.Items.RemoveAt(lstDirectories.SelectedIndex);
			foreach (DirectoryInfo elem in _searchDirectoriesList)
				Console.WriteLine(elem.FullName);
		}
		// TODO: Should eventually be moved to FolderHandler
		private void BuildDirectoryList()
		{
			foreach (DirectoryInfo directoryInfo in _searchDirectoriesList)
				_allDirectoriesList.Add(directoryInfo);

			Parallel.For(0, _allDirectoriesList.Count, i =>
			{
				try
				{
					_totalFilesCount += _allDirectoriesList[i].GetFiles().Length;
					_allDirectoriesList.AddRange(_allDirectoriesList[i].GetDirectories());
					ThreadSafe_SetTextboxText(txtTotalFolders, _allDirectoriesList.Count.ToString());
					ThreadSafe_SetTextboxText(txtTotalFiles, _totalFilesCount.ToString());
				}
				catch (UnauthorizedAccessException) { }
			});

			// TODO: Keep in case of issues with Parallel
			// Do not use foreach since the collection is modified within the loop
			//for (int i = 0; i < _allDirectoriesList.Count; i++)
			//{
			//	try
			//	{
			//		_totalFilesCount += _allDirectoriesList[i].GetFiles().Length;
			//		_allDirectoriesList.AddRange(_allDirectoriesList[i].GetDirectories());
			//		ThreadSafe_SetTextboxText(txtTotalFolders, _allDirectoriesList.Count.ToString());
			//		ThreadSafe_SetTextboxText(txtTotalFiles, _totalFilesCount.ToString());
			//	}
			//	catch (UnauthorizedAccessException) { }

			//}
			BuildFileDictionary();
		}

		private void BuildFileDictionary()
		{

			//_fileDict = new FileHandler();
			foreach (DirectoryInfo dirElem in _allDirectoriesList)
			{
				try
				{
					_filesProcessedCount += _fileComparer.HashNextFileInfoList();
					ThreadSafe_SetProgressBar(progbarFiles, _filesProcessedCount, _totalFilesCount);
				}
				catch (PathTooLongException) { Console.WriteLine(@"File path was too long"); }
				catch (UnauthorizedAccessException) { Console.WriteLine(@"File access restricted");}
			}
			ThreadSafe_EnableControl(btnFindDupes, true);
		}


		// TODO: May not use. Removed from code for now
		//private IEnumerable<FileInfo> SelectExtensions(IList<FileInfo> input)
		//{
		//	if (_includedExtensionsList.Count.Equals(0))
		//		return input;
		//	for (int i = input.Count() - 1; i >= 0; i--)
		//	{
		//		try
		//		{
		//			if (!_includedExtensionsList.Contains(Path.GetExtension(input[i].FullName)))
		//				input.RemoveAt(i);
		//		}
		//		catch (PathTooLongException)
		//		{
		//			// Can something be done?
		//			// FileInfo t = new FileInfo("\\?\\" +);
		//			// Console.WriteLine(input[i].FullName);
		//		}
		//	}
		//	return input;
		//}

		private int RKey(string key)
		{
			var d = dgrdFileDuplicates.Rows
				.Cast<DataGridViewRow>()
				.First(r => r.Cells["gridColKey"].Value.ToString().Equals(key));

			return d.Index;
		}


		// TODO: How to connect the duplicate hash dictionary to the DGV? BindingSource doesn't seem to be feasible. Mirror the Dictionary values to a BindingList?
		private void bsrcDirectoriesSelected_CurrentChanged(object sender, EventArgs e)
		{

		}

		private void AddToDataGrid(string lastKey, string value)
		{
			if (_fileDict[lastKey].Count < 2)
				return;
			if (_fileDict[lastKey].Count.Equals(2))
				ThreadSafe_AddGridRow(dgrdFileDuplicates, lastKey, value);
			else
				ThreadSafe_IncGridRowCount(dgrdFileDuplicates, lastKey);
		}

		private void dgrdFileDuplicates_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			dgrdFileList.Rows.Clear();
			dgrdFileDuplicates.Rows[e.RowIndex].Selected = true;
			foreach (var elem in _fileDict[dgrdFileDuplicates.Rows[e.RowIndex].Cells[0].Value.ToString()])
			{
				var temp = new DataGridViewRow();
				temp.Cells.Add(new DataGridViewTextBoxCell());
				temp.Cells[0].Value = elem;
				dgrdFileList.Rows.Add(temp);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}

