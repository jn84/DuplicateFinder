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
	public partial class Form1 : Form
	{
		// For eliminating LastKey
		public ConcurrentQueue<string> ProcessedFileQueue { get; } = new ConcurrentQueue<string>();

		private readonly List<string> _searchDirectoriesList = new List<string>();
		private readonly List<string> _includedExtensionsList = new List<string>();
		private List<DirectoryInfo> _fullDirectoryList;
		private FileDictionary _fileDict;

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

		private void RunCompare()
		{
			
		}


		// TODO: Getting the list of directories/files can be threaded. Should perhaps be moved into FileDictionary. The form doesn't need to handle this. We will however need to relay information back to the form.
		private void BuildDirectoryList()
		{
			// Add the initial list of directories to the dictionary. This is the list of directories added by the user and show in the listbox
			_fullDirectoryList = new List<DirectoryInfo>();
			foreach (string elem in _searchDirectoriesList)
				_fullDirectoryList.Add(new DirectoryInfo(elem));

			// Do not use foreach since the collection is modified within the loop. Since I keep trying to convert it...
			for (int i = 0; i < _fullDirectoryList.Count; i++)
			{
				try
				{
					_totalFilesCount += _fullDirectoryList[i].GetFiles().Length; 
					_fullDirectoryList.AddRange(_fullDirectoryList[i].GetDirectories());
					ThreadSafe_SetTextboxText(txtTotalFolders, _fullDirectoryList.Count.ToString());
					ThreadSafe_SetTextboxText(txtTotalFiles, _totalFilesCount.ToString());
				}
				catch (PathTooLongException) { /* This is, as of commit #4, an unfixable error. Will keep an eye out for potential workarounds. */ }
				catch (UnauthorizedAccessException) { }
				
			}

			// Performance tracking code
			Console.WriteLine("Starting stopwatch");
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			

			BuildFileDictionary();

		
			stopwatch.Stop();
			Console.WriteLine(@"Method took " + stopwatch.Elapsed.TotalSeconds + @" seconds to complete");
		}


		// Add a file to the file dictionary
		private void BuildFileDictionary()
		{
			_fileDict = new FileDictionary(chkSkipEmptyFiles.Checked);

			foreach (DirectoryInfo dirElem in _fullDirectoryList)
			{
				try
				{
					Parallel.ForEach(dirElem.GetFiles().ToList(), fElem =>
					{
						_fileDict.Add(fElem);
						_filesProcessedCount++;
						//ThreadSafe_SetTextboxText(txtTotalFiles, _totalFilesCount.ToString()); // Redundant unless changed
						ThreadSafe_SetProgressBar(progbarFiles, _filesProcessedCount, _totalFilesCount);
					});
					
				}
				catch (PathTooLongException) { /* This is, as of commit #4, an unrecoverable error. Will keep an eye out for potential workarounds */ }
				catch (UnauthorizedAccessException) { }

				Thread dataGridPopulater = new Thread(PopulateDataGrid);
				dataGridPopulater.Start();
			}
			ThreadSafe_EnableControl(btnFindDupes, true);
		}

		/*
		 * Placeholder methods for sending information to/updating the UI.
		 * 
		public void UpdateProgressBar(int countCompleted, int totalCount)
		{
			
		}

		public void UpdateDirectory_FileCounts(int directoryCount, int fileCount)
		{
			
		}

		*/
		private void UserInterfaceInteractable(bool canInteract)
		{
			btnFindDupes.Enabled = canInteract;
			btnAddDirectory.Enabled = canInteract;
		}

		private void PopulateDataGrid()
		{
			Tuple<string, FileInfo> fileToProcess;
			while (_fileDict.TryGetNextKey(out fileToProcess))
			{
				AddToDataGrid(fileToProcess.Item1, fileToProcess.Item2);
			}
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

