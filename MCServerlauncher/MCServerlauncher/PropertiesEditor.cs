﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace MCServerLauncher
{
    public partial class PropertiesEditor : Form
    {
        static bool Modify = false;
        static string sPropDir = "";
        ServerProperties sprop = null;
        public PropertiesEditor(string dir)
        {
            InitializeComponent();
            sPropDir = dir;
        }
        List<TextBox> keys = new List<TextBox>();
        List<Object> values = new List<Object>();
        private void PropertiesEditor_Load(object sender, EventArgs e)
        {
            sprop = ServerProperties.FromFile(sPropDir + "/server.properties");
            int y = 0;
            Font fnt = new Font("Consolas", 8); 
            foreach (KeyValuePair<string,SPropSetting> KVP in sprop.ServerVars)
            {

                var tbNewBox = new TextBox();
                tbNewBox.Text = KVP.Key;
                tbNewBox.Location = new Point(0, y);
               
                tbNewBox.BackColor = SystemColors.InactiveCaption;
                tbNewBox.Size = new Size(200, 20);
                tbNewBox.TextAlign = HorizontalAlignment.Right;
                tbNewBox.Font = fnt;
                tbNewBox.ReadOnly = true;
                keys.Add(tbNewBox);
                switch(KVP.Value.type)
                {
                    case 0: // String
                    case 1: // Int
                        var tbNewBox2 = new TextBox();
                        tbNewBox2.Text = KVP.Value.value;
                        tbNewBox2.Location = new Point(200, y);
                        tbNewBox2.BackColor = SystemColors.ActiveCaption;
                        tbNewBox2.Size = new Size(200, 20);
                        tbNewBox2.TextAlign = HorizontalAlignment.Left;
                        tbNewBox2.Font = fnt;
                        tbNewBox2.TextChanged += ItemChanged;
                        values.Add(tbNewBox2);
                        Controls.Add(tbNewBox2);
                        break;
                    case 2:
                        var chkNewCheck = new CheckBox();
                        chkNewCheck.Checked = bool.Parse(KVP.Value.value);
                        chkNewCheck.Location = new Point(200, y);
                        chkNewCheck.Size = new Size(200, 20);
                        chkNewCheck.CheckedChanged += ItemChanged;
                        Controls.Add(chkNewCheck);
                        values.Add(chkNewCheck);
                        break;
                    default:break;

                }
                y += 20;

                Controls.Add(tbNewBox);
            }
                
        }
        private void Save()
        {
            var sprop = new ServerProperties();
            int p = 0;
            foreach (TextBox tb in keys)
            {
                var stype = sprop.ServerVars[tb.Text].type;

                TextBox valtext = null;
                CheckBox valcheck = null;
                if (stype == 0 || stype == 1)
                {
                    valtext = (TextBox)values[p];
                    sprop.ServerVars[tb.Text].value = valtext.Text;
                }
                else
                {
                    valcheck = (CheckBox)values[p];
                    sprop.ServerVars[tb.Text].value = valcheck.Checked.ToString().ToLower();
                }

                p++;
            }

            File.WriteAllText(sPropDir + "/server.properties", sprop.ToString());
        }
        bool prevent_close()
        {
            if(Modify)
            {
               DialogResult dr =  MessageBox.Show("You have unsaved changes, would you like to save?", "Unsaved Work", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    Save();
                    return false;
                }
                if(dr == DialogResult.No)
                    return false;
                if (dr == DialogResult.Cancel)
                    return true;
            }
            return true;
        }



        private void PropertiesEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Modify)
            {
                
                e.Cancel = prevent_close(); 
            }


            int p = 0;
            foreach (TextBox tb in keys)
            {
                var stype = sprop.ServerVars[tb.Text].type;

                TextBox valtext = null;
                CheckBox valcheck = null;
                if (stype == 0 || stype == 1)
                {
                    valtext = (TextBox)values[p];
                    valtext.Dispose();
                }
                else
                {
                    valcheck = (CheckBox)values[p];
                    valcheck.Dispose();
                }

                p++;
            }
            
            foreach (TextBox tb in keys)
                tb.Dispose();

        }

        private void ItemChanged(object sender,EventArgs e)
        {
            Modify = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Save();
            Modify = false;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
