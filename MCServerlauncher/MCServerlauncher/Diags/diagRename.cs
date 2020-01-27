using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCServerLauncher.Diags
{
    public partial class diagRename : Form
    {
        public string name;
        public bool cancel = false;
        public diagRename(string oldname)
        {
            InitializeComponent();
            name = oldname;
           
        }

        private void TbName_TextChanged(object sender, EventArgs e)
        {
            name = tbName.Text;
        }

        private void DiagRename_Load(object sender, EventArgs e)
        {
            tbName.Text = name;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            cancel = true;
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                Close();
        }
    }
}
