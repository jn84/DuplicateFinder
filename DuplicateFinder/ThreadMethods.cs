using System;
using System.IO;
using System.Windows.Forms;

namespace DuplicateFinder
{
	internal delegate void SetTextBoxText_callback(TextBox t, string key);

	internal delegate void EnableControl_callBack(Control c, bool val);

	internal delegate void AddGridRow_callBack(DataGridView g, string key, FileInfo value);

	internal delegate void IncGridRowCount_callBack(DataGridView g, string key);

	internal delegate void SetProgressBarState_callback(ProgressBar p, int currentValue, int maxValue);


	public partial class FrmDuplicateFinder_Main : Form
	{

		private void ThreadSafe_SetTextboxText(Control t, string val)
		{
			if (txtTotalFiles.InvokeRequired)
			{
				SetTextBoxText_callback del = SetTextBoxText;
				Invoke(del, new object[] { t, val });
			}
			else
				t.Text = val;
		}

		private static void SetTextBoxText(TextBox t, string val)
		{
			t.Text = val;
		}

		private void ThreadSafe_EnableControl(Control c, bool val)
		{
			if (c.InvokeRequired)
			{
				EnableControl_callBack del = EnableControl;
				Invoke(del, new object[] {c, val});
			}
			else
				c.Enabled = val;
		}

		private static void EnableControl(Control c, bool val)
		{
			c.Enabled = val;
		}

		private void ThreadSafe_AddGridRow(DataGridView g, string key, FileInfo file)
		{
			if (g.InvokeRequired)
			{
				AddGridRow_callBack del = AddGridRow;
				Invoke(del, new object[] { g, key, file });
			}
			else
				AddGridRow(g, key, file);
		}

		private void AddGridRow(DataGridView g, string key, FileInfo file)
		{
			var temp = new FileInfo(file.FullName);
			g.Rows.Add(key, Path.GetFileName(file.FullName), _fileDict[key].Count, temp.Length/1024m);
		}		

		private void ThreadSafe_IncGridRowCount(DataGridView g, string key)
		{
			if (g.InvokeRequired)
			{
				IncGridRowCount_callBack del = IncGridRowCount;
				Invoke(del, new object[] {g, key});
			}
			else
				IncGridRowCount(g, key);
		}
		private void IncGridRowCount(DataGridView g, string key)
		{
			var index = GridIndexFromKey(key);
			g.Rows[index].Cells[2].Value =
					(Convert.ToInt32(g.Rows[index].Cells[2].Value) + 1);
		}

		private void ThreadSafe_SetProgressBar(ProgressBar p, int current, int max)
		{
			if (p.InvokeRequired)
			{
				SetProgressBarState_callback del = SetProgressBarState;
				Invoke(del, new object[] { p, current, max });
			}
			else
				SetProgressBarState(p, current, max);
		}

		private void SetProgressBarState(ProgressBar p, int current, int max)
		{
			p.Maximum = max;
			p.Value = current;
		}
	}
}
