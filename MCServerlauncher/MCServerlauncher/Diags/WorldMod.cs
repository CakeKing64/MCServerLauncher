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
using System.Diagnostics;

namespace MCServerLauncher
{
    public partial class WorldMod : Form
    {

        private string svdir;
        ServerProperties svp;

        private bool BackupMode = false; // false Worlds -> Backups. true Backups -> Worlds, you get it
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
                if ((File.Exists(dir + "/level.dat") || Directory.Exists(dir + "/region")) && !dir_s.Contains("_the_end") && !dir.Contains("_nether"))
                lstWorlds.Items.Add(dir_s);

                if (dir_s == svp.ServerVars["level-name"].value)
                    lblCurrent.Text = "Current World: " + dir_s;
            }


            if (!Directory.Exists(svdir + "/world_backups"))
                Directory.CreateDirectory(svdir + "/world_backups");

            dirs = Directory.GetDirectories(svdir + "/world_backups");

            lstBackups.Items.Clear();

            foreach (string dir in dirs)
            {
                var dir_s = dir.Substring(svdir.Length + 15);
                if ((File.Exists(dir + "/level.dat") || Directory.Exists(dir + "/region")) && !dir_s.Contains("_the_end") && !dir.Contains("_nether"))
                    lstBackups.Items.Add(dir_s);

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
            if (lstBackups.SelectedIndex != -1)
            {
                lstWorlds.SelectedIndex = -1;
                btnCopy.Text = "Copy from backups";
                btnMoveBack.Text = "Move from backups";
                BackupMode = true;
                btnSet.Enabled = false;
            }
        }

        private void LstWorlds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstWorlds.SelectedIndex != -1)
            {
                lstBackups.SelectedIndex = -1;
                btnCopy.Text = "Copy to backups";
                btnMoveBack.Text = "Move to backups";
                BackupMode = false;
                btnSet.Enabled = true;
            }
        }



        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshWorlds();
        }

        private void WorldMod_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (fileList[0].Contains(".zip"))
            {
                ZipArchive za = ZipFile.Open(fileList[0], ZipArchiveMode.Read);
                var tempp = Path.GetTempPath() + "MCServerLauncher";
                if (!Directory.Exists(tempp))
                    Directory.CreateDirectory(tempp);

                za.ExtractToDirectory(tempp);
                za.Dispose();

                string[] dirs = Directory.GetDirectories(tempp);

                foreach (string dir in dirs)
                {
                    if (File.Exists(dir + "/level.dat"))
                    {
                        var ds = dir.Split('\\');
                        Directory.Move(dir, svdir + "/" + ds[ds.Length - 1]);

                        //svp.ServerVars["level-name"].value = ds[ds.Length - 1];
                        //lblCurrent.Text = "Current World: " + ds[ds.Length - 1];
                        //File.WriteAllText(svdir + "/server.properties", svp.ToString());
                        RefreshWorlds();
                    }
                }

            } else if(Directory.Exists(fileList[0]))
                if (File.Exists(fileList[0] + "/level.dat"))
            {
                var ds = fileList[0].Split('\\');
                Directory.Move(fileList[0], svdir + "/" + ds[ds.Length - 1]);
                    Refresh();
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
            WorldMod_DragDrop(sender, e);
        }


        public void SetWorldName(string wn)
        {
            lblCurrent.Text = "Current World: " + wn;
        }
        private void BtnSet_Click(object sender, EventArgs e)
        {
            if (lstWorlds.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a world first!");
                return;
            }

            SetWorldName(lstWorlds.Items[lstWorlds.SelectedIndex].ToString());
            svp.ServerVars["level-name"].value = lstWorlds.Items[lstWorlds.SelectedIndex].ToString();
            File.WriteAllText(svdir + "/server.properties", svp.ToString());
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LstWorlds_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }


        private void BtnDelete_Click(object sender, EventArgs e)
        {
            
            if(!BackupMode ? lstWorlds.SelectedIndex == -1 : lstBackups.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a world first!");
                return;
            }
            DialogResult dg = DialogResult.Cancel;

            if (lstWorlds.SelectedItems.Count > 1 || lstBackups.SelectedItems.Count > 1)
                dg = MessageBox.Show("Are you sure you want to delete multiple worlds?\nThis cannot be undone", "Confirm.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            else
            dg = MessageBox.Show("Are you sure you want to delete \"" + (!BackupMode ? lstWorlds.Items[lstWorlds.SelectedIndex].ToString() : lstBackups.Items[lstBackups.SelectedIndex].ToString()) + "\"?\nThis cannot be undone", "Confirm.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                

            if (dg == DialogResult.Yes)
            {
                if (!BackupMode)
                {
                    foreach (object obj in lstWorlds.SelectedItems)
                    {
                        Program.DELETE_FILES(svdir + "/" + obj.ToString());
                        Directory.Delete(svdir + "/" + obj.ToString());
                    }
                }
                else
                {
                    foreach (object obj in lstBackups.SelectedItems)
                    {
                        Program.DELETE_FILES(svdir + "/world_backups/" + obj.ToString());
                        Directory.Delete(svdir + "/world_backups/" + obj.ToString());
                    }
                }
                RefreshWorlds();
            }
            else
                return;
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (!BackupMode)
            {
                if (lstWorlds.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a world first!");
                    return;
                }
                dir = "\"" + Directory.GetCurrentDirectory() + "\\" + svdir + "\\" + lstWorlds.Items[lstWorlds.SelectedIndex].ToString() + "\"";
                
            } else
            {
                if (lstBackups.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a world first!");
                    return;
                }
                dir = "\"" + Directory.GetCurrentDirectory() + "\\" + svdir + "\\world_backups\\" + lstBackups.Items[lstBackups.SelectedIndex].ToString() + "\"";
            }

            ProcessStartInfo a = new ProcessStartInfo("explorer", "\"" + dir + "\"");
            System.Diagnostics.Process.Start(a);
        }

        #region "Copy and paste code, don't look here, it's horrible :)"
        /*
         * Honestly there was probably a much better (and prettier way)
         * but this works and I'm gonna stick with it.
         * 
         */
        private void MoveToBackups(string worldn)
        {
            var ovr = 0;
            if (!Directory.Exists(svdir + "\\world_backups"))
                Directory.CreateDirectory(svdir + "\\world_backups");

            if (Directory.Exists(svdir + "\\world_backups\\" + worldn))
            {
                DialogResult res = MessageBox.Show("World already exists in backups, would you like to override it?", "World already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Program.DELETE_FILES(svdir + "\\world_backups\\" + worldn);
                    ovr = 2; // 2 is unused but it looks like i'm doing work :)
                }
                if (res == DialogResult.No)
                    ovr = 1;


                if (res == DialogResult.Cancel)
                    return;
            }

            var worldx = worldn;
            var worldi = 1;
            if (ovr == 1)
            {
                worldx = worldn + " (" + worldi + ")";
                while (Directory.Exists(svdir + "\\world_backups\\" + worldn + " (" + worldi + ")"))
                {
                    worldi++;
                    worldx = worldn + " (" + worldi + ")";
                }

            }

            /*
            if (Directory.Exists(svdir + "\\" + worldn + "_nether"))
                Directory.Move(svdir + "\\" + worldn + "_nether", svdir + "\\world_backups\\" + worldx + "_nether");
            if (Directory.Exists(svdir + "\\" + worldn + "_the_end"))
                Directory.Move(svdir + "\\" + worldn + "_the_end", svdir + "\\world_backups\\" + worldx + "_the_end");
                */
            Directory.Move(svdir + "\\" + worldn, svdir + "\\world_backups\\" + worldx);
        }
        private void MoveFromBackups(string worldn)
        {
            var ovr = 0;
            if (!Directory.Exists(svdir + "\\world_backups"))
                Directory.CreateDirectory(svdir + "\\world_backups");

            if (Directory.Exists(svdir + "\\" + worldn))
            {
                DialogResult res = MessageBox.Show("World already exists, would you like to override it?", "World already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Program.DELETE_FILES(svdir + "\\" + worldn);
                    ovr = 2; // 2 is unused but it looks like i'm doing work :)
                }
                if (res == DialogResult.No)
                    ovr = 1;


                if (res == DialogResult.Cancel)
                    return;
            }

            var worldx = worldn;
            var worldi = 1;
            if (ovr == 1)
            {
                worldx = worldn + " (" + worldi + ")";
                while (Directory.Exists(svdir + "\\" + worldn + " (" + worldi + ")"))
                {
                    worldi++;
                    worldx = worldn + " (" + worldi + ")";
                }

            }

            /*
            if (Directory.Exists(svdir + "\\" + worldn + "_nether"))
                Directory.Move(svdir + "\\" + worldn + "_nether", svdir + "\\world_backups\\" + worldx + "_nether");
            if (Directory.Exists(svdir + "\\" + worldn + "_the_end"))
                Directory.Move(svdir + "\\" + worldn + "_the_end", svdir + "\\world_backups\\" + worldx + "_the_end");
            */
            Directory.Move(svdir + "\\world_backups\\" + worldn, svdir + "\\" + worldx);
        }
        private void CopyToBackups(string worldn)
        {
            var ovr = 0;
            if (!Directory.Exists(svdir + "\\world_backups"))
                Directory.CreateDirectory(svdir + "\\world_backups");

            if (Directory.Exists(svdir + "\\world_backups\\" + worldn))
            {
                DialogResult res = MessageBox.Show("World already exists in backups, would you like to override it?", "World already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Program.DELETE_FILES(svdir + "\\world_backups\\" + worldn);
                    ovr = 2; // 2 is unused but it looks like i'm doing work :)
                }
                if (res == DialogResult.No)
                    ovr = 1;


                if (res == DialogResult.Cancel)
                    return;
            }
            var worldx = worldn;
            var worldi = 1;
            if (ovr == 1)
            {
                worldx = worldn + " (" + worldi + ")";
                while (Directory.Exists(svdir + "\\world_backups\\" + worldn + " (" + worldi + ")"))
                {
                    worldi++;
                    worldx = worldn + " (" + worldi + ")";
                }

            }

            /*
            if (Directory.Exists(svdir + "\\" + worldn + "_nether"))
                CLONE_DIRECTORY(svdir + "\\" + worldn + "_nether", svdir + "\\world_backups\\" + worldx + "_nether");
            if (Directory.Exists(svdir + "\\" + worldn + "_the_end"))
                CLONE_DIRECTORY(svdir + "\\" + worldn + "_the_end", svdir + "\\world_backups\\" + worldx + "_the_end");
                */
            Program.CLONE_DIRECTORY(svdir + "\\" + worldn,svdir + "\\world_backups\\" + worldx);
        }
        private void CopyFromBackups(string worldn)
        {
            var ovr = 0;
            if (!Directory.Exists(svdir + "\\world_backups"))
                Directory.CreateDirectory(svdir + "\\world_backups");

            if (Directory.Exists(svdir + "\\" + worldn))
            {
                DialogResult res = MessageBox.Show("World already exists, would you like to override it?", "World already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Program.DELETE_FILES(svdir + "\\" + worldn);
                    ovr = 2; // 2 is unused but it looks like i'm doing work :)
                }
                if (res == DialogResult.No)
                    ovr = 1;


                if (res == DialogResult.Cancel)
                    return;
            }

            var worldx = worldn;
            var worldi = 1;
            if (ovr == 1)
            {
                worldx = worldn + " (" + worldi + ")";
                while (Directory.Exists(svdir + "\\" + worldn + " (" + worldi + ")"))
                {
                    worldi++;
                    worldx = worldn + " (" + worldi + ")";
                }

            }

            /*
            if (Directory.Exists(svdir + "\\" + worldn + "_nether"))
                CLONE_DIRECTORY(svdir + "\\" + worldn + "_nether", svdir + "\\world_backups\\" + worldx + "_nether");
            if (Directory.Exists(svdir + "\\" + worldn + "_the_end"))
                CLONE_DIRECTORY(svdir + "\\" + worldn + "_the_end", svdir + "\\world_backups\\" + worldx + "_the_end");
                */
            Program.CLONE_DIRECTORY(svdir + "\\world_backups\\" + worldn, svdir + "\\" + worldx);

        }
        private void DuplicateWorld(string worldn)
        {
            var ovr = 0;
            string xdir = BackupMode ? "\\world_backups" : "";
            var worldx = worldn + " - Copy";
            if (!Directory.Exists(svdir + xdir))
                Directory.CreateDirectory(svdir + xdir);

            if (Directory.Exists(svdir + xdir + "\\" + worldx))
            {
                DialogResult res = MessageBox.Show("World already exists, would you like to override it?", "World already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Program.DELETE_FILES(svdir + "\\" + worldx);
                    ovr = 2; // 2 is unused but it looks like i'm doing work :)
                }
                if (res == DialogResult.No)
                    ovr = 1;


                if (res == DialogResult.Cancel)
                    return;
            }

            
            var worldi = 1;
            if (ovr == 1)
            {
                worldx = worldn + " - Copy (" + worldi + ")";
                while (Directory.Exists(svdir + xdir + "\\" + worldn + " - Copy (" + worldi + ")"))
                {
                    worldi++;
                    worldx = worldn + " - Copy (" + worldi + ")";
                }

            }


            Program.CLONE_DIRECTORY(svdir + xdir + "\\" + worldn, svdir + xdir + "\\" + worldx);

        }


        #endregion


        private void BtnMoveBack_Click(object sender, EventArgs e)
        {
            if (!BackupMode)
                foreach(object obj in lstWorlds.SelectedItems)
                MoveToBackups(obj.ToString());
            else
                foreach (object obj in lstBackups.SelectedItems)
                    MoveFromBackups(obj.ToString());

            RefreshWorlds();
        }
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (!BackupMode)
                foreach (object obj in lstWorlds.SelectedItems)
                    CopyToBackups(obj.ToString());
            else
                foreach (object obj in lstBackups.SelectedItems)
                    CopyFromBackups(obj.ToString());
            
            RefreshWorlds();
        }

        static char[] invalidChars = Path.GetInvalidFileNameChars();
        private void BtnRename_Click(object sender, EventArgs e)
        {
            if (!BackupMode ? lstWorlds.SelectedIndex == -1 : lstBackups.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a world first!");
                return;
            }
            var dgr = new Diags.diagRename(!BackupMode ? lstWorlds.SelectedItems[0].ToString() : lstBackups.SelectedItems[0].ToString());
                    dgr.ShowDialog();
                    if (dgr.cancel)
                        return;
                if (dgr.name == (!BackupMode ? lstWorlds.SelectedItems[0].ToString() : lstBackups.SelectedItems[0].ToString()))
                    return;
                if(dgr.name.IndexOfAny(invalidChars) != -1)
                {
                    MessageBox.Show("Inavlid world name!");
                    return;
                }

                if (Directory.Exists(svdir + "/" + dgr.name))
                    MessageBox.Show("World with name \"" + dgr.name + "\" already exists!");
                
                Directory.Move(svdir + "/" + (BackupMode ? "world_backups//" + lstBackups.SelectedItems[0].ToString() : lstWorlds.SelectedItems[0].ToString()) , svdir + "/" + (BackupMode ? "world_backups//" : "") + dgr.name);
            RefreshWorlds();
        }


        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (!BackupMode)
                foreach (object obj in lstWorlds.SelectedItems)
                    DuplicateWorld(obj.ToString());
            else
                foreach (object obj in lstBackups.SelectedItems)
                    DuplicateWorld(obj.ToString());

            RefreshWorlds();
        }

        void create_empty_world(string folder_name)
        {
            Directory.CreateDirectory(folder_name);
            Directory.CreateDirectory(folder_name + "\\datapacks");
            Directory.CreateDirectory(folder_name + "\\region");
            Directory.CreateDirectory(folder_name + "\\playerdata");
            Directory.CreateDirectory(folder_name + "\\data");
        }
        private void btnNewWorld_Click(object sender, EventArgs e)
        {
            if (!BackupMode)
            {
                string st = "New World";
                int ind = 1;
                bool __world_exists = false;
                if (Directory.Exists(svdir + "/" + st))
                {
                    __world_exists = true;
                    while (Directory.Exists(svdir + "/" + st + " (" + ind + ")"))
                        ind++;
                }
                if (__world_exists)
                    st = st + " (" + ind + ")";

                Diags.diagRename dr = new Diags.diagRename(st);
                do
                {
                    dr.ShowDialog();
                    if (Directory.Exists(svdir + "/" + dr.name))
                        MessageBox.Show("World already exists!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } while (Directory.Exists(svdir + "/" + dr.name));
                if (!dr.cancel)
                {
                    create_empty_world(svdir + "/" + dr.name);
                    RefreshWorlds();
                }
                dr.Dispose();
            }
                
        }
    }
}
