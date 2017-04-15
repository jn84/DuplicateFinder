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
using System.Collections.Concurrent;

namespace DuplicateFinder
{
	public partial class FrmDuplicateFinder_Main : Form
	{
		// For eliminating LastKey
		public ConcurrentQueue<string> ProcessedFileQueue { get; } = new ConcurrentQueue<string>();

		private readonly List<string> _searchDirectoriesList = new List<string>();
		private readonly List<string> _includedExtensionsList = new List<string>();
		private List<DirectoryInfo> _fullDirectoryList;
		private FileDictionary _fileDict;

		public FrmDuplicateFinder_Main()
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
			UserInterfaceInteractable(false);
			var build = new Thread(RunCompare);
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
		//// End context menu code 


		// Execute comparison of files/folders
		private void RunCompare()
		{
			_fileDict = new FileDictionary(chkSkipEmptyFiles.Checked, this);

			_fileDict.BuildDirectoryList(
				lstDirectories.Items.Cast<string>().ToList());

			_fileDict.BuildFileDictionary();

			PopulateDataGrid();
		}

		public void UpdateProgressBar(int countCompleted, int totalCount)
		{
			ThreadSafe_SetProgressBar(progbarFiles, countCompleted, totalCount);
		}

		public void UpdateDirectory_FileCounts(int directoryCount, int fileCount)
		{
			ThreadSafe_SetTextboxText(txtTotalFolders, directoryCount.ToString());
			ThreadSafe_SetTextboxText(txtTotalFiles, fileCount.ToString());
		}

		private void PopulateDataGrid()
		{
			ConcurrentDictionary<string, List<string>> ref_hashedDictionary = _fileDict.HashedDictionary;
			foreach (KeyValuePair<string, List<string>> elem in ref_hashedDictionary)
			{
				AddToDataGrid(elem.Key, new FileInfo(elem.Value.First()));
			}
		}

		private void UserInterfaceInteractable(bool canInteract)
		{
			btnFindDupes.Enabled = canInteract;
			btnAddDirectory.Enabled = canInteract;
		}

		// Currently unused
		private IEnumerable<FileInfo> SelectExtensions(IList<FileInfo> input)
		{
			if (_includedExtensionsList.Count.Equals(0))
				return input;
			for (int i = input.Count() - 1; i >= 0; i--)
			{
				try
				{
					if (!_includedExtensionsList.Contains(Path.GetExtension(input[i].FullName)))
						input.RemoveAt(i);
				}
				catch (PathTooLongException e)
				{
					// Can something be done?
					// FileInfo t = new FileInfo("\\?\\" +);
					// Console.WriteLine(input[i].FullName);
				}
			}
			return input;
		}

		private int GridIndexFromKey(string key)
		{
			try
			{
				var d = dgrdFileDuplicates.Rows
					.Cast<DataGridViewRow>()
					.First(r => r.Cells["gridColKey"].Value.ToString().Equals(key));

				return d.Index;
			}
			catch (InvalidOperationException e)
			{
				return -1;
			}

		}


		// TODO: Instead of using lastkey, should use a queue. // Add each file to the queue to be processed. // This will solve threading problems arising from concurrent file processing

		// Add a file to the data grid
		// If the file in question has no duplicates (duplicate count is less than 2)
		//		do nothing and return
		// If the file in question has exactly one duplicate (duplicate count is exactly 2)
		//		Add the row to the datagridview control
		// If the previous two conditions are not met, the file must already be in the datagridview control, and has at least two duplicates
		//		Increment the duplicate count
		// End method

		private void AddToDataGrid(string lastKey, FileInfo file)
		{
			if (_fileDict[lastKey].Count < 2)
				return;

			// Due to threading,

			if (GridIndexFromKey(lastKey) == -1)
				ThreadSafe_AddGridRow(dgrdFileDuplicates, lastKey, file);
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

