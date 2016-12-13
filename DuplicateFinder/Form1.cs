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
		private readonly List<string> _searchDirectoriesList = new List<string>();
		private readonly List<string> _includedExtensionsList = new List<string>();
		private List<DirectoryInfo> _fullDirectoryList;
		private FileDictionary _fDict;

		private int _count = 0;

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
			foreach (var elem in _searchDirectoriesList)
				Console.WriteLine(elem);
		}

		private void BuildDirectoryList()
		{
			_fullDirectoryList = new List<DirectoryInfo>();
			foreach (var elem in _searchDirectoriesList)
				_fullDirectoryList.Add(new DirectoryInfo(elem));
			for (var i = 0; i < _fullDirectoryList.Count; i++)
			{
				try
				{
					_fullDirectoryList.AddRange(_fullDirectoryList[i].GetDirectories());
					ThreadSafe_SetTextboxText(txtDirectoriesFound, _fullDirectoryList.Count.ToString());
				}
				catch (UnauthorizedAccessException) { }
				
			}
			BuildFileDictionary();
		}

		private void BuildFileDictionary()
		{
			_fDict = new FileDictionary();
			foreach (var dElem in _fullDirectoryList)
			{
				try
				{
					foreach (var fElem in SelectExtensions(dElem.GetFiles().ToList()))
					{
						try
						{
							_fDict.Add(fElem.FullName);
							AddToDataGrid(_fDict.LastKey, fElem.FullName);
							_count++;
							ThreadSafe_SetTextboxText(txtCounter, _count.ToString());
						}
						catch (PathTooLongException) { }
					}
				}
				catch (UnauthorizedAccessException) { }
			}
			ThreadSafe_EnableControl(btnFindDupes, true);
		}

		private List<FileInfo> SelectExtensions(List<FileInfo> input)
		{
			if (_includedExtensionsList.Count.Equals(0))
				return input;
			for (var i = input.Count() - 1; i >= 0; i--)
			{
				try
				{
					if (!_includedExtensionsList.Contains(Path.GetExtension(input[i].FullName)))
						input.RemoveAt(i);
				}
				catch (PathTooLongException)
				{
					// Can something be done?
					// FileInfo t = new FileInfo("\\?\\" +);
					// Console.WriteLine(input[i].FullName);
				}
			}
			return input;
		}

		private int RKey(string key)
		{
			var d = dgrdFileDuplicates.Rows
				.Cast<DataGridViewRow>()
				.First(r => r.Cells["gridColKey"].Value.ToString().Equals(key));

			return d.Index;
		}

		private void AddToDataGrid(string lastKey, string value)
		{
			if (_fDict[lastKey].Count < 2)
				return;
			if (_fDict[lastKey].Count.Equals(2))
				ThreadSafe_AddGridRow(dgrdFileDuplicates, lastKey, value);
			else
				ThreadSafe_IncGridRowCount(dgrdFileDuplicates, lastKey);
		}

		private void dgrdFileDuplicates_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			dgrdFileList.Rows.Clear();
			dgrdFileDuplicates.Rows[e.RowIndex].Selected = true;
			foreach (var elem in _fDict[dgrdFileDuplicates.Rows[e.RowIndex].Cells[0].Value.ToString()])
			{
				var temp = new DataGridViewRow();
				temp.Cells.Add(new DataGridViewTextBoxCell());
				temp.Cells[0].Value = elem;
				dgrdFileList.Rows.Add(temp);
			}
		}

	}
}

