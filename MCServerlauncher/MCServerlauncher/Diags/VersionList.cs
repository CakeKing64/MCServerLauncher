using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace MCServerLauncher.Diags
{
    public partial class VersionList : Form
    {
        public string SelectedVersion;
        public static List<MinecraftVersion> mcversion = null;
        private bool FAIL_CHECK = false;
        public VersionList()
        {
            InitializeComponent();
        }

        private void VersionList_Load(object sender, EventArgs e)
        {

            if (mcversion == null)
            {
                WebClient client = new WebClient();

                try
                {
                    byte[] dat = client.DownloadData("https://launchermeta.mojang.com/mc/game/version_manifest.json");
                    string data = Encoding.UTF8.GetString(dat);
                    JObject job = JObject.Parse(data);
                    mcversion = new List<MinecraftVersion>();

                    foreach (JToken token in job["versions"])
                    {


                        mcversion.Add(new MinecraftVersion(token["id"].ToString(), token["type"].ToString() == "snapshot"));


                    }
                } catch
                {
                    MessageBox.Show("Unable to connect to download version list :(");
                    this.Close();
                    FAIL_CHECK = true;
                }
            }
            else
                foreach (MinecraftVersion ver in mcversion)
                    listBox1.Items.Add(ver.version);
            CheckBox1_CheckedChanged(checkBox1, null);
            checkBox1.Checked = false;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (FAIL_CHECK)
                return;
            if (listBox1.SelectedIndex == -1)
                SelectedVersion = "yes";
            SelectedVersion = listBox1.SelectedItem.ToString();
            this.Close();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (FAIL_CHECK)
                return;

            CheckBox ck = (CheckBox)sender;
            listBox1.Items.Clear();
            bool chk = ck.Checked;
            
                foreach(MinecraftVersion mcv in mcversion)
                {

                bool adv = chk ? true : !mcv.snapshot;

                if (adv)
                    listBox1.Items.Add(mcv.version);
                    

                }
            
        }

        private void CheckBox1_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void VersionList_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkBox1.Checked = false;
        }
    }
}
