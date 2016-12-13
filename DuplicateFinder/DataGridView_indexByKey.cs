using System.Linq;
using System.Windows.Forms;

namespace DuplicateFinder_Jenny
{
	internal class DataGridView_indexByKey : DataGridView
	{
		public DataGridViewRow this[string key]
		{
			
			get
			{
				return Rows.
					   Cast<DataGridViewRow>().
				  	   First(val => val.Cells["gridColKey"].Value.ToString().Equals(key));
			}
		}
	}
}