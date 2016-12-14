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

namespace DuplicateFinder
{
	public partial class Form1 : Form
	{
		private readonly List<FileInfo> _hashedFileList;

		private readonly List<string> _searchDirectoriesList = new List<string>();
		private readonly List<string> _includedExtensionsList = new List<string>();
		private List<DirectoryInfo> _fullDirectoryList;
		private FileHandler _fileDict;

		private int _filesProcessedCount = 0,   //
		            _totalFilesCount = 0;		// For use with progress bar

		public Form1()
		{
			InitializeComponent();
		}

		private void btnAddDirectory_Click(object sender, EventArgs e)
		{
			AddDirectory();
		}

		private void AddDirectory()
		{
			var tempDir = new FolderBrowserDialog();
			tempDir.ShowDialog();
			_searchDirectoriesList.Add(tempDir.SelectedPath);
			lstDirectories.Items.Add(tempDir.SelectedPath);
		}

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
			_searchDirectoriesList.Remove(lstDirectories.SelectedItem.ToString());
			lstDirectories.Items.RemoveAt(lstDirectories.SelectedIndex);
			foreach (string elem in _searchDirectoriesList)
				Console.WriteLine(elem);
		}

		private void BuildDirectoryList()
		{
			_fullDirectoryList = new List<DirectoryInfo>();
			foreach (string elem in _searchDirectoriesList)
				_fullDirectoryList.Add(new DirectoryInfo(elem));

			// Do not use foreach since the collection is modified within the loop
			for (int i = 0; i < _fullDirectoryList.Count; i++)
			{
				try
				{
					_totalFilesCount += _fullDirectoryList[i].GetFiles().Length; 
					_fullDirectoryList.AddRange(_fullDirectoryList[i].GetDirectories());
					ThreadSafe_SetTextboxText(txtTotalFolders, _fullDirectoryList.Count.ToString());
					ThreadSafe_SetTextboxText(txtTotalFiles, _totalFilesCount.ToString());
				}
				catch (UnauthorizedAccessException) { }
				
			}
			BuildFileDictionary();
		}

		private void BuildFileDictionary()
		{
			_fileDict = new FileHandler();
			foreach (DirectoryInfo dirElem in _fullDirectoryList)
			{
				try
				{
					foreach (FileInfo fElem in SelectExtensions(dirElem.GetFiles().ToList()))
					{
						try
						{
							_fileDict.Add(fElem.FullName);
							AddToDataGrid(_fileDict.LastKey, fElem.FullName);
							_filesProcessedCount++;
							ThreadSafe_SetProgressBar(progbarFiles, _filesProcessedCount, _totalFilesCount);
						}
						catch (PathTooLongException) { }
					}
				}
				catch (UnauthorizedAccessException) { }
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

