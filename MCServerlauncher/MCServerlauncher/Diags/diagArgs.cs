using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCServerLauncher
{
    public partial class diagArgs : Form
    {

        public string _args = "";
        public diagArgs(string args)
        {
            InitializeComponent();
            _args = args;
            textBox1.Text = _args;
        }

        private void DiagArgs_Load(object sender, EventArgs e)
        {
            this.Text = "Argument Editor";
        }
        void save()
        {
            _args = textBox1.Text;
            this.Text = "Argument Editor";
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = "Argument Editor*";
        }

        private void DiagArgs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                save();
            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                save();
            }
        }

        private void BtnSave_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void BtnSave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void DiagArgs_FormClosing(object sender, FormClosingEventArgs e)
        {

            if(this.Text == "Argument Editor*")
            {
                DialogResult dr = MessageBox.Show("You have unsaved changes, would you like to save?","Unsaved Changes",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);

                if(dr == DialogResult.Yes)
                   _args = textBox1.Text;
                if (dr == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void btnArgumentGenerator_Click(object sender, EventArgs e)
        {
            Diags.diagArgumentGenerator dag = new Diags.diagArgumentGenerator(textBox1.Text);
            dag.ShowDialog();
            
            if (dag._args != "CLOSED")
                textBox1.Text = dag._args;

            dag.Dispose();
        }
    }
}
