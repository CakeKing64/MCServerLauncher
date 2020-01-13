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
using System.IO.Compression;

namespace MCServerLauncher
{
    public partial class WorldMod : Form
    {

        private string svdir;
        ServerProperties svp;
        public WorldMod(string dir)
        {
            InitializeComponent();
            svdir = dir;
            svp  = ServerProperties.FromFile(svdir + "/server.properties");
        }

        void RefreshWorlds()
        {
            string[] dirs = Directory.GetDirectories(svdir);

            lstWorlds.Items.Clear();

            foreach (string dir in dirs)
            {
                var dir_s = dir.Substring(svdir.Length + 1);
                if (File.Exists(dir + "/level.dat") && !dir_s.Contains("_the_end") && !dir.Contains("_nether"))
                lstWorlds.Items.Add(dir_s);

                if (dir_s == svp.ServerVars["level-name"].value)
                    lblCurrent.Text = "Current World: " + dir_s;
            }
        }

        private void WorldMod_Load(object sender, EventArgs e)
        {
            // Aight, it's setup time. First we're gonna grab all the folders in the current server directory provided in [svdir]
            // scan tru all them and sort them based on these three things: They aren't a level folder, they have a region folder but not a level.dat file, they have a level.dat file
            // if they have a region but not a level.dat, the folder is a world folder but the level hasn't generated.



            RefreshWorlds();

        }

        private void LstBackups_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LstWorlds_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnCopy.Text = "";
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshWorlds();
        }

        private void WorldMod_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            ZipArchive za = ZipFile.Open(fileList[0], ZipArchiveMode.Read);
            var tempp = Path.GetTempPath() + "MCServerLauncher";
            MessageBox.Show(tempp);
            if(!Directory.Exists(tempp))
                Directory.CreateDirectory(tempp);

            za.ExtractToDirectory(tempp);
            za.Dispose();

            string[] dirs = Directory.GetDirectories(tempp);

            foreach(string dir in dirs)
            {
                if(File.Exists(dir + "/level.dat"))
                {
                    var ds = dir.Split('\\');
                    Directory.Move(dir, svdir + "/" + ds[ds.Length - 1]);
                   
                    //svp.ServerVars["level-name"].value = ds[ds.Length - 1];
                    //lblCurrent.Text = "Current World: " + ds[ds.Length - 1];
                    //File.WriteAllText(svdir + "/server.properties", svp.ToString());
                    RefreshWorlds();
                }
            }
        }

        private void WorldMod_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void LstWorlds_DragDrop(object sender, DragEventArgs e)
        {

        }


        public void SetWorldName(string wn)
        {
            lblCurrent.Text = "Current World: " + wn;
        }
        private void BtnSet_Click(object sender, EventArgs e)
        {
            SetWorldName(lstWorlds.Items[lstWorlds.SelectedIndex].ToString());
            svp.ServerVars["level-name"].value = lstWorlds.Items[lstWorlds.SelectedIndex].ToString();
            File.WriteAllText(svdir + "/server.properties", svp.ToString());
        }
    }
}
