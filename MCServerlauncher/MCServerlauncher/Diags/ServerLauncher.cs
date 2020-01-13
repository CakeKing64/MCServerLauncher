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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace MCServerLauncher
{
    public partial class ServerLauncher : Form
    {

        bool bModifyProp = false;
        bool bReady = false;
        public ServerLauncher()
        {
            InitializeComponent();


            if (Program.sQLaunchSType == "Vanilla")
            {
                lbVersion.Items.Add("Latest");
                clbSType.SetItemChecked(0, true);
            }
            else
                clbSType.SetItemChecked(1, true);
            tbVersion.Text = Program.sQLaunchSVersion;
            ignoreCheck = false;
            bReady = true;
            // First thing we need to do is redirect console output
            // seeing as we hid the console.
            Console.SetOut(new ConRedir(tbConsole));

            lblQLaunch.Text = Program.sQLaunchSType + " / " + Program.sQLaunchSVersion;
            lblQLaunch.Show();
            btnManageWorlds.Top = 12;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Select a version, or type one in");
            
            btnQLaunch.Hide();
            btnModifyProp.Hide();
            btnLaunch.Hide();
            btnConsoleMode.Hide();
            lblInstalled.Show();
            clbSType.Show();
            lbVersion.Show();
            tbVersion.Show();
            btnLaunchS.Show();
            btnReturn.Show();
            chkCOSL.Show();
            btnRedownload.Show();
            lblQLaunch.Hide();
            chkCOSL.Checked = Program.bCloseOnServerLaunch;
            btnQuit.Hide();
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
                        Program.start_server_vanilla(tbVersion.Text, false);
                        GetVersions("Vanilla");
                        break;
                    case "Spigot":
                        Program.start_server_spigot(tbVersion.Text, false);
                        GetVersions("Spigot");
                        break;

                }
            }
            else
            {
                PropertiesEditor pe = new PropertiesEditor("Servers/" + clbSType.CheckedItems[0].ToString() + "/" + lbVersion.Text)
                {
                    Text = "Properties Editor - " + (clbSType.CheckedItems[0].ToString()) + " " + tbVersion.Text
                };
                pe.ShowDialog();
                pe.Dispose();
                pe = null;
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
            if(stype == "Vanilla")
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
            btnConsoleMode.Hide();
            lblInstalled.Show();
            lblQLaunch.Hide();
            clbSType.Show();
            lbVersion.Show();
            tbVersion.Show();
            btnLaunchS.Show();
            btnReturn.Show();
            btnQuit.Hide();
            btnManageWorlds.Show();
            bModifyProp = true;
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            btnQLaunch.Show();
            btnModifyProp.Show();
            btnLaunch.Show();
            btnConsoleMode.Show();
            lblInstalled.Hide();
            btnManageWorlds.Hide();
            clbSType.Hide();
            lbVersion.Hide();
            tbVersion.Hide();
            btnLaunchS.Hide();
            btnReturn.Hide();
            bModifyProp = false;
            chkCOSL.Hide();
            btnRedownload.Hide();
            chkCOSL.Checked = Program.bCloseOnServerLaunch;
            tbConsole.Text = "";
            lblQLaunch.Text = Program.sQLaunchSType + " / " + Program.sQLaunchSVersion;
            lblQLaunch.Show();
            btnQuit.Show();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ck = (CheckBox)sender;

            Program.bCloseOnServerLaunch = ck.Checked;
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            if (!bModifyProp)
            {
                switch (clbSType.CheckedItems[0].ToString())
                {
                    case "Vanilla":
                        Program.start_server_vanilla(tbVersion.Text.ToLower(), true);
                        break;
                    case "Spigot":
                        Program.start_server_spigot(tbVersion.Text.ToLower(), true);
                        break;

                }
            }
        }

        private void TbVersion_TextChanged(object sender, EventArgs e)
        {
            var TextBoxv = (TextBox)sender;
            if (TextBoxv.Text == "")
            {
                btnRedownload.Enabled = false;
                return;
            }
            if (!bReady)
                return;
            if (!Directory.Exists("Servers"))
                Directory.CreateDirectory("Servers");

            if (clbSType.CheckedItems.Count == 1)
            {
                if (!Directory.Exists("Servers/" + clbSType.CheckedItems[0].ToString()))
                    Directory.CreateDirectory("Servers/" + clbSType.CheckedItems[0].ToString());


                btnRedownload.Enabled = Directory.Exists("Servers/" + clbSType.CheckedItems[0].ToString() + "/" + TextBoxv.Text);
                btnManageWorlds.Enabled = Directory.Exists("Servers/" + clbSType.CheckedItems[0].ToString() + "/" + TextBoxv.Text);
            }
                
        }

        public static string GetSnapshotParent(string ver)
        {
            if (!Directory.Exists("Servers"))
                Directory.CreateDirectory("Servers");
            if (!Directory.Exists("Servers/Vanilla"))
                Directory.CreateDirectory("Servers/Vanilla");
            if(!File.Exists("Servers/Vanilla/snapshot_parents.json"))
            {
                File.WriteAllText("Servers/Vanilla/snapshot_parents.json", "{}");
            }
            var c = JObject.Parse(File.ReadAllText("Servers/Vanilla/snapshot_parents.json"));
            //var c = JObject.Parse("{\"a\":\"b\"}");
            if (c[ver] == null)
                return "1.0";
            else
                return c[ver].ToString();
        }

        private void Button1_Click_2(object sender, EventArgs e)
        {
            Program.bUseGUI = false;
            var cp = Process.GetCurrentProcess(); 
            string exe = cp.MainModule.FileName;
            ProcessStartInfo a = new ProcessStartInfo(exe);
            System.Diagnostics.Process.Start(a);
            this.Close();
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ServerLauncher_Load(object sender, EventArgs e)
        {

        }

        private void BtnManageWorlds_Click(object sender, EventArgs e)
        {
            var WM = new WorldMod("Servers/" + clbSType.CheckedItems[0].ToString() + "/" + tbVersion.Text);
            WM.ShowDialog();
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
            var s1Snapshot = false;
            var s2Snapshot = false;
            var s1Pre = false;
            var s2Pre = false;


            if (s1.Contains("pre"))
                s1Pre = true;
            if (s2.Contains("pre"))
                s2Pre = true;




            if (s1s.Length == 1)
            {
                s1s = s1.Split('w');
                s1Snapshot = true;
            }
            if (s2s.Length == 1)
            {
                s2s = s2.Split('w');
                s2Snapshot = true;
            }

            if(s1Snapshot && s2Snapshot)
            {
                var s1n1 = Convert.ToInt32(s1s[0]);
                var s1n2 = Convert.ToInt32(s1s[1].Substring(0, 2));
                var s2n1 = Convert.ToInt32(s2s[0]);
                var s2n2 = Convert.ToInt32(s2s[1].Substring(0, 2));

               
                if (s1n1 > s2n1)
                    return -1;
                if (s1n1 < s2n1)
                    return 1;
                //throw new Exception();
                if (s1n2 > s2n2)
                    return -1;
                if (s1n2 < s2n2)
                    return 1;

                
            }


            
                if (s1Pre || s2Pre)
                {
                if (s1Snapshot)
                    return 1;
                if (s2Snapshot)
                    return -1;

                    var cc1 = s1.Split(new string[] { "-pre" }, StringSplitOptions.None);
                    var cc2 = s2.Split(new string[] { "-pre" }, StringSplitOptions.None);
                    var cc1Split = cc1[0].Split('.');
                    var cc2Split = cc2[0].Split('.');
                    var Minor1 = Convert.ToInt32(cc1Split[1]);
                    var Minor2 = Convert.ToInt32(cc2Split[1]);
                var Rev1 = cc1Split.Length == 3 ? Convert.ToInt32(cc1Split[2]) : 0;
                var Rev2 = cc2Split.Length == 3 ? Convert.ToInt32(cc2Split[2]) : 0;


                    if (Minor1 > Minor2)
                        return 1;
                    else if (Minor1 < Minor2)
                        return -1;

                if (Rev1 > Rev2)
                    return -1;
                else if (Rev1 < Rev2)
                    return 1;

                if (cc2.Length == 1)
                        return 1;
                    if (cc1.Length == 1)
                        return -1;

                    if (Convert.ToInt32(cc1[1]) > Convert.ToInt32(cc2[1]) && (Minor1 > Minor2 || Minor1 == Minor2))
                        return -1;
                    else
                        return 1;
                }




            var s1Minor = !s1Snapshot && !s1Pre ? Convert.ToInt32(s1s[!s1Snapshot ? 1 : 0]) : 0 ;
            var s2Minor = !s2Snapshot && !s2Pre ? Convert.ToInt32(s2s[!s2Snapshot ? 1 : 0]) : 0;
            var s1Fix = 0;
            var s2Fix = 0;

            if (!s1Pre)
            s1Fix = s1s.Length > 2 ? Convert.ToInt32(s1s[2]) : 0;
            if(!s2Pre)
            s2Fix = s2s.Length > 2 ? Convert.ToInt32(s2s[2]) : 0;

            var snparent = s1Snapshot ? ServerLauncher.GetSnapshotParent(s1) : s2Snapshot ? ServerLauncher.GetSnapshotParent(s2) : "";

            if (s1Snapshot)
                if (s2Minor.ToString() == snparent.Split('.')[1])
                    return 1;
            if (s2Snapshot)
                if (s1Minor.ToString() == snparent.Split('.')[1])
                    return 1;

           
            if (s1Minor > s2Minor)
                return -1;
            else if (s1Minor == s2Minor && s1Fix > s2Fix)
                return -1;
            else return 1;
        }

    }
}
