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
using System.Collections;

namespace MCServerLauncher
{
    public partial class ServerLauncher : Form
    {

        bool bModifyProp = false;
        public ServerLauncher()
        {
            InitializeComponent();
            lbVersion.Items.Add("Latest");
            if (Program.sQLaunchSType == "Vanilla")
                clbSType.SetItemChecked(0, true);
            else
                clbSType.SetItemChecked(1, true);
            tbVersion.Text = Program.sQLaunchSVersion;
            ignoreCheck = false;
            // First thing we need to do is redirect console output
            // seeing as we hid the console.
            Console.SetOut(new ConRedir(tbConsole));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Select a version, or type one in");
            
            btnQLaunch.Hide();
            btnModifyProp.Hide();
            btnLaunch.Hide();

            clbSType.Show();
            lbVersion.Show();
            tbVersion.Show();
            btnLaunchS.Show();
            btnReturn.Show();
            chkCOSL.Show();
            chkCOSL.Checked = Program.bCloseOnServerLaunch;
            //Program.start_server_vanilla("1.8.6");
        }



        // Apparently we need to make a class for this :)
        private class ConRedir : TextWriter
        {
            private TextBox tbBox;

            public ConRedir(TextBox tbref)
            {
                tbBox = tbref;
            }
            public override void Write(string value)
            {
                tbBox.AppendText(value);
            }

            public override void WriteLine(string value)
            {
                tbBox.AppendText(value + "\r\n");
            }
            
            // this is important, for some reason
            public override Encoding Encoding
            {
                get { return Encoding.Unicode; }
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void LbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {

            tbVersion.Text = lbVersion.Items[lbVersion.SelectedIndex].ToString();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!bModifyProp)
            {
                switch (clbSType.CheckedItems[0].ToString())
                {
                    case "Vanilla":
                    Program.start_server_vanilla(tbVersion.Text);
                        break;
                    case "Spigot":
                        Program.start_server_spigot(tbVersion.Text);
                        break;

            }
            }
            else
            {
                var pe = new PropertiesEditor("Servers/" + clbSType.CheckedItems[0].ToString() + "/" + lbVersion.Text);
                pe.Text = "Properties Editor - " + (clbSType.CheckedItems[0].ToString()) + " " + tbVersion.Text;
                pe.ShowDialog();
                pe.Dispose();
            }
        }


        private bool ignoreCheck = false;
        private void ClbSType_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;

            if (e.NewValue == CheckState.Unchecked && clb.SelectedIndex == e.Index)
            {
                e.NewValue = CheckState.Checked;
                return;
            }
            
            if (ignoreCheck)
            {
                ignoreCheck = false;
                return;
            }
            ignoreCheck = true;
            
            if (e.NewValue == CheckState.Checked)
            {
                if (e.Index == 0)
                {
                    clb.SetItemChecked(1, false);
                    GetVersions("Vanilla");
                }
                else
                {
                    clb.SetItemChecked(0, false);
                    GetVersions("Spigot");
                    

                }
            }

           

            

        }

        private void ServerLauncher_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void ClbSType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GetVersions(string stype)
        {
            lbVersion.Items.Clear();
            lbVersion.Items.Add("Latest");
            if (!Directory.Exists("Servers"))
                Directory.CreateDirectory("Servers");
            if (!Directory.Exists("Servers/" + stype))
                Directory.CreateDirectory("Servers/" + stype);

            string[] dirs = Directory.GetDirectories("Servers/" + stype);
            ArrayList q = new ArrayList();


            foreach (string dir in dirs)
            {
                var longs = "Servers/" + stype + "/";
                q.Add(dir.Substring(longs.Length));
            }

            VersionSorter vs = new VersionSorter();

            q.Sort(vs);


            foreach (string s in q)
            {
                lbVersion.Items.Add(s);
                if (s == Program.sQLaunchSVersion && stype == Program.sQLaunchSType)
                    lbVersion.SetSelected(lbVersion.Items.Count - 1, true);
            }
        }

        private void BtnQLaunch_Click(object sender, EventArgs e)
        {
            Program.launch(Program.sQLaunchSType, Program.sQLaunchSVersion);
        }

        private void BtnModifyProp_Click(object sender, EventArgs e)
        {
            btnQLaunch.Hide();
            btnModifyProp.Hide();
            btnLaunch.Hide();

            clbSType.Show();
            lbVersion.Show();
            tbVersion.Show();
            btnLaunchS.Show();
            btnReturn.Show();
            bModifyProp = true;
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            btnQLaunch.Show();
            btnModifyProp.Show();
            btnLaunch.Show();

            clbSType.Hide();
            lbVersion.Hide();
            tbVersion.Hide();
            btnLaunchS.Hide();
            btnReturn.Hide();
            bModifyProp = false;
            chkCOSL.Hide();

            chkCOSL.Checked = Program.bCloseOnServerLaunch;
            tbConsole.Text = "";
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ck = (CheckBox)sender;

            Program.bCloseOnServerLaunch = ck.Checked;
        }
    }

    public class VersionSorter : IComparer
    {
        public int Compare(object a, object b)
        {
            var s1 = (string)a;
            var s2 = (string)b;

            var s1s = s1.Split('.');
            var s2s = s2.Split('.');

            var s1Minor = Convert.ToInt32(s1s[1]);
            var s2Minor = Convert.ToInt32(s2s[1]);

            var s1Fix = s1s.Length > 2 ? Convert.ToInt32(s1s[2]) : 0;
            var s2Fix = s2s.Length > 2 ? Convert.ToInt32(s2s[2]) : 0;
            if (s1Minor > s2Minor)
                return -1;
            else if (s1Minor == s2Minor && s1Fix > s2Fix)
                return -1;
            else return 1;
        }

    }
}
