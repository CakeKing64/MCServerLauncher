using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Runtime.InteropServices;

namespace MCServerLauncher
{
    class Program
    {

        public static WebClient client = new WebClient(); // For downloading the json files


        public static int DownloadPos = 0;
        public static bool DownloadComplete = false;
        public static bool DownloadingServer = false;
        public static int GetDownloadPos()
        {
            return DownloadPos;
        }

        public static class NativeMethods
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            public const int SW_HIDE = 0;
            public const int SW_SHOW = 5;
        }

        // Settings
        public static bool bUseSingleDirectory = false;
        public static string sLatestVersion = "1.0";
        public static bool bReadEULA = false;
        public static string sLaunchArgs = "-Xmx1024M -Xms1024M -jar server.jar";
        public static string sQLaunchSType = "Vanilla";
        public static string sQLaunchSVersion = "1.0";
        public static string sJavaPath = null;
        public static bool bUseGUI = true;
        public static bool bFirstStart = true;
        public static bool bCloseOnServerLaunch = false;
        public static bool bUseGlobalLaunchArgs = false;
        public static bool bAskedGit = false;
        public static bool bForceEnable64Bit = false;
        public static bool bForceJavaPath = false;

        public static int iJavaType = 32; // 32 || 64 bit version of java being used
        static string GetLatest()
        {
            return sLatestVersion;
        }
        [STAThread]
        public static void launch(string stype, string vnumb)
        {
            bool has_jar = false;

            
            if (Directory.Exists("Servers/" + stype + "/" + vnumb))
                {
                
                string[] files = Directory.GetFiles(GetSDV() + vnumb);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) == ".jar")
                        has_jar = true;
                }
            }
            if (!Directory.Exists("Servers/" + stype + "/"  + vnumb) && has_jar)
            {
                Console.WriteLine("Unable to launch " + stype + " / " + vnumb);
                return;
            }

            if (!File.Exists("Servers/" + stype + "/" + vnumb + "/server.json"))
            {
                var set = new JObject();
                set["sLaunchArgs"] = "-Xmx1024M -Xms1024M " + (iJavaType == 64 ? "-d64 " : "") + "-jar server.jar";
                File.WriteAllText("Servers/" + stype + "/" + vnumb + "/server.json", set.ToString());
            }
            var job = JObject.Parse(File.ReadAllText("Servers/" + stype + "/" + vnumb + "/server.json"));


            sQLaunchSType = stype;
            sQLaunchSVersion = vnumb;
            SaveSettings();
            Directory.SetCurrentDirectory("Servers/" + stype + "/" + vnumb);

            // 64 bit java remover thingy
            string largs = job["sLaunchArgs"].ToString();
            if(iJavaType == 32 && !bForceEnable64Bit)
            if (largs.Contains("-d64"))
                largs = largs.Remove(largs.IndexOf("-d64"), 4);

            ProcessStartInfo a = new ProcessStartInfo(sJavaPath + "/java.exe",largs );

           // a.RedirectStandardOutput = true;
           //a.UseShellExecute = false;
            System.Diagnostics.Process.Start(a);
            //System.Threading.Thread.Sleep(10000);
            Directory.SetCurrentDirectory("../../../");

            if(bCloseOnServerLaunch)
            {
                Environment.Exit(1);
            }

        }

        static void ProgramExit(object sender, EventArgs ea)
        {
            SaveSettings();
        }
        public static void SaveSettings()
        {
            JObject job = new JObject();
            job["bUseSingleDirectory"] = bUseSingleDirectory;
            job["sLatestVersion"] = sLatestVersion;
            job["bReadEULA"] = bReadEULA;
            job["sLaunchArgs"] = sLaunchArgs;
            job["bUseGUI"] = true;
            job["sQLaunchSType"] = sQLaunchSType;
            job["sQLaunchSVersion"] = sQLaunchSVersion;
            job["sJavaPath"] = sJavaPath;
            job["bCloseOnServerLaunch"] = bCloseOnServerLaunch;
            job["bFirstStart"] = bFirstStart;
            job["bForceEnable64Bit"] = bForceEnable64Bit;
            job["bForceJavaPath"] = bForceJavaPath;
            //job["bAskedGit"] = bAskedGit;
            File.WriteAllText("settings.json", job.ToString());
        }

        public static string GetString(JObject job, string get, string def)
        {
            return job[get] != null ? job[get].ToString() : def;
        }

        public static void TryLaunch(string stype, string version)
        {

            if (stype.ToLower() == "latest")
                MessageBox.Show("Using 'latest' is no longer supported");

            switch (stype.ToLower())
            {
                case "vanilla":
                    start_server_vanilla(version, false);
                    break;
                case "spigot":
                    start_server_spigot(version, false);
                    break;
            }
        }

        public static void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
          // Console.WriteLine("Downloading... " + e.ProgressPercentage + "% (" + e.BytesReceived / 1000 + "MB / " + e.TotalBytesToReceive / 1000 + "MB)");
            //DownloadPos = e.ProgressPercentage;
        }

       [STAThread]
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProgramExit);

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
           

            if (!File.Exists("settings.json"))
            {
                SaveSettings();
            } else
            {
                var job = JObject.Parse(File.ReadAllText("settings.json"));
                sLatestVersion = GetString(job, "sLatestVersion", sLatestVersion);
                bUseSingleDirectory = job["bUseSingleDirectory"] != null ? bool.Parse(job["bUseSingleDirectory"].ToString()) : false;
                bReadEULA = job["bReadEULA"] != null ? bool.Parse(job["bReadEULA"].ToString()) : false;
                sLaunchArgs = GetString(job, "sLaunchArgs", sLaunchArgs);
                sQLaunchSType = GetString(job, "sQLaunchSType", sQLaunchSType);
                sQLaunchSVersion = GetString(job, "sQLaunchSVersion", sQLaunchSVersion);
                sJavaPath = GetString(job, "sJavapath", sJavaPath);
                bUseGUI = true;//job["bUseGUI"] != null ? bool.Parse(job["bUseGUI"].ToString()) : false;
                bCloseOnServerLaunch = job["bCloseOnServerLaunch"] != null ? bool.Parse(job["bCloseOnServerLaunch"].ToString()) : false;
                bFirstStart = job["bFirstStart"] != null ? bool.Parse(job["bFirstStart"].ToString()) : true;
                bForceEnable64Bit = job["bForceEnable64Bit"] != null ? bool.Parse(job["bForceEnable64Bit"].ToString()) : false;
                bForceJavaPath = job["bForceJavaPath"] != null ? bool.Parse(job["bForceJavaPath"].ToString()) : false;
                //bAskedGit = job["bAskedGit"] != null ? bool.Parse(job["bAskedGit"].ToString()) : false;
            }
          
            // imma just put this in here so i don't feel bad/what ever
            MessageBoxResult mbrResult = MessageBoxResult.No;


            if (!bReadEULA)
            {
                mbrResult = MessageBox.Show("Please read the Minecraft EULA before continuing: https://account.mojang.com/documents/minecraft_eula\nHit a button to continue", "Read before continuing (Or not, you probably won't)", MessageBoxButton.YesNo);

                if (mbrResult == MessageBoxResult.Yes)
                    bReadEULA = true;
                else
                {
                    Environment.Exit(0);
                }
            }


            // Java check
            if (bForceJavaPath)
                goto _java_check_end;
            if (!Directory.Exists(sJavaPath) || !File.Exists(sJavaPath + "java.exe"))
            {
                sJavaPath = null;
            }
            if (sJavaPath == null)
            {
                try
                {
                    if (Directory.Exists("C:\\Program Files (x86)\\Common Files\\Oracle\\Java\\javapath"))
                        if (File.Exists("C:\\Program Files (x86)\\Common Files\\Oracle\\Java\\javapath\\java.exe"))
                        {
                            sJavaPath = "C:\\Program Files (x86)\\Common Files\\Oracle\\Java\\javapath";
                            goto _java_check_end;
                        }

                }
                catch
                { }
                string path = Environment.GetEnvironmentVariable("Path");

                string[] paths = path.Split(';');
                foreach (string sDir in paths)
                {
                    if (sDir != "")
                    {
                        var add = sDir[sDir.Length - 1] == '/' ? "" : "/";


                        if (Directory.Exists(sDir))
                            if (Directory.Exists(sDir + add + "bin"))
                                if (File.Exists(sDir + add + "bin\\java.exe"))
                                {
                                    sJavaPath = sDir + add + "bin";
                                }
                        if (File.Exists(sDir + add + "java.exe"))
                        {
                            sJavaPath = sDir;
                        }
                    }
                }

                if (sJavaPath == null)
                {
                    string jpath = Environment.GetEnvironmentVariable("JAVA_HOME");

                    if (jpath == null)
                    {
                        while (true)
                        {
                            MessageBox.Show("Can't find a java installation, please select from the options below");
                            Console.WriteLine("1) I have a java installation\n2) I don't have a java installation\n3) Exit");
                            char key = Console.ReadKey().KeyChar;
                            if (key == '1')
                            {
                                var fbdDialog = new FolderBrowserDialog();
                                DialogResult fbdResult = fbdDialog.ShowDialog();

                                MessageBox.Show(fbdDialog.SelectedPath + "/bin/java.exe");
                                if (fbdResult == DialogResult.OK)
                                {
                                    if (Directory.Exists(fbdDialog.SelectedPath + "/bin"))
                                        if (File.Exists(fbdDialog.SelectedPath + "/bin/java.exe"))
                                        {
                                            sJavaPath = fbdDialog.SelectedPath + "\\bin";
                                            // Environment.SetEnvironmentVariable("Path", Environment.GetEnvironmentVariable("Path") + ";" + sJavaPath);
                                            break;
                                        }
                                    if (File.Exists(fbdDialog.SelectedPath + "java.exe"))
                                        sJavaPath = fbdDialog.SelectedPath;
                                }
                            }
                            if (key == '2')
                            {
                                Process.Start("https://www.java.com/en/download/");
                                Console.Clear();
                                Console.WriteLine("Download java and relaunch the program\nPress any key to exit");
                                Console.ReadKey();
                                Environment.Exit(1);
                            }
                            if (key == '3')
                                Environment.Exit(1);

                            Console.Clear();

                        }

                    }
                    else
                    {
                        sJavaPath = jpath + "\\";
                    }
                }

                SaveSettings();
            }

        _java_check_end:

            var f = File.Open(sJavaPath + "\\java.exe", FileMode.Open, FileAccess.Read);

            // 64 / 32 bit check
            byte[] buffer = new byte[2];
            while (true)
            {


                f.Read(buffer, 0, 2);

                if (buffer[0] == 'P' && buffer[1] == 'E')
                    break;

                if (f.Position + 2 >= f.Length)
                    break;

                f.Seek(2, SeekOrigin.Current);


            }
            f.Seek(2, SeekOrigin.Current);
            f.Read(buffer, 0, 2);
            if (buffer[0] == 100 && buffer[1] == 134)
                iJavaType = 64;
            else if (buffer[0] == 0x4c && buffer[1] == 1)
                iJavaType = 32;
            else
                iJavaType = 16; // Invalid java probably
            f.Close();

            Console.Clear();

            /*
            if(bFirstStart)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("This program has can use either console or a GUI , please select the one you'd like to use (this can be changed later)\n1) GUI (Recommended)\n2) Console");

                    var key = Console.ReadKey().KeyChar;
                    if (key == '1')
                    {
                        bUseGUI = true;
                        bFirstStart = false;
                        break;
                    }
                    else if (key == '2')
                    {
                        bUseGUI = false;
                        bFirstStart = false;
                        break;
                    }
                }
                Console.Clear();
                SaveSettings();

            }
            */

            var sLV = "";
            bool sLVWait = false;
            var sLT = "";
            bool sLTWait = false;
            bool args_ = false;
            foreach(string arg in args)
            {
                if (arg == "-help" || arg == "-?")
                {
                    Console.WriteLine("Commands:\n-help / -? : Shows this text\n-quicklaunch / -ql : Activates Quicklaunch on startup\n-version / -v : Sets version (must be combined with -stype)\n-stype / -st : Server type (Vanilla/Spigot)");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    args_ = true;
                }

                if (arg == "-quicklaunch" || arg == "-ql")
                {
                    launch(sQLaunchSType, sQLaunchSVersion);
                    args_ = true;
                }

                if(sLVWait)
                {
                    sLVWait = false;
                    sLV = arg;
                    Console.WriteLine(arg);
                    if (sLV != "" && sLT != "")
                    {
                        TryLaunch(sLT, sLV);
                    }
                    args_ = true;
                }
                if(arg == "-version" || arg == "-v")
                {
                    sLVWait = true;
                    args_ = true;
                }

                if (sLTWait)
                {
                    sLTWait = false;
                    sLT = arg;
                    if (sLV != "" && sLT != "")
                    {
                        TryLaunch(sLT,sLV);
                    }
                    args_ = true;
                }
                if (arg == "-stype" || arg == "-st")
                {
                    sLTWait = true;
                    args_ = true;
                }


            }
            if (args_)
            {
                //Console.WriteLine("Press any key to exit");
                //Console.ReadKey();
                Environment.Exit(1);
            }

            if (bUseGUI)
            {
                var conHandle = NativeMethods.GetConsoleWindow();
                NativeMethods.ShowWindow(conHandle, NativeMethods.SW_HIDE);

                var slLauncher = new ServerLauncher();

                slLauncher.ShowDialog();
                return;
            }

            Console.Title = "MCServerLauncher 1.0.1";
            while (true)
            {
                
                Console.WriteLine("1) Download / Launch Server\n2) Modify Properties\n3) List Versions\n4) Quick Launch (" + sQLaunchSType + " / " + sQLaunchSVersion + ")\n5) Switch to GUI mode\n6) Exit");
                var sel = Console.ReadKey();
                Console.Clear();
                switch (sel.KeyChar)
                {
                    case '1':
                        Console.WriteLine("1) Vanilla Server\n2) Spigot Server\nPress any other key to return");
                        var sel2 = Console.ReadKey();
                        Console.Clear();
                        switch (sel2.KeyChar)
                        {
                            case '1':
                                start_server_vanilla(null,false);
                                break;
                            case '2':
                                start_server_spigot(null,false);
                                break;
                            default:
                                break;
                        }
                       
                        break;
                    case '2':
                        modify_prop();
                        break;
                    case '3':
                        if (!Directory.Exists("Servers")) Directory.CreateDirectory("Servers");
                        if (!Directory.Exists("Servers/Spigot")) Directory.CreateDirectory("Servers/Spigot");
                        if (!Directory.Exists("Servers/Vanilla")) Directory.CreateDirectory("Servers/Vanilla");
                        var saDirs = Directory.GetDirectories("Servers/Vanilla");
                        Console.WriteLine("Avalable Server Versions:\nVanilla)");
                        foreach (string sDir in saDirs)
                        {
                            var str = sDir.Substring(16);
                            Console.WriteLine(str + (str == sLatestVersion ? " (Latest)" : ""));
                        }
                        saDirs = Directory.GetDirectories("Servers/Spigot");
                        Console.WriteLine("Spigot)");
                        foreach (string sDir in saDirs)
                        {
                            var str = sDir.Substring(15);
                            Console.WriteLine(str + (str == sLatestVersion ? " (Latest)" : ""));
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case '4':
                        launch(sQLaunchSType, sQLaunchSVersion);
                        break;
                    case '5':
                        bUseGUI = true;
                        var cp = Process.GetCurrentProcess();
                        string exe = cp.MainModule.FileName;
                        ProcessStartInfo a = new ProcessStartInfo(exe);
                        System.Diagnostics.Process.Start(a);
                        Environment.Exit(1);
                        break;
                    case '6':
                        Environment.Exit(1);
                        break;
                    default:
                        break;
                }



                Console.Clear();

            }

        }

        private static void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadComplete = true;
        }

        public static void CheckServerF()
        {
            if (!Directory.Exists("Servers"))
                Directory.CreateDirectory("Servers");
            if (!Directory.Exists("Servers/Vanilla"))
                Directory.CreateDirectory("Servers/Vanilla");
        }

        public static string GetSDV()
        {
            return "Servers/Vanilla/";
        }
        public static void modify_prop()
        {
            Console.WriteLine("Server Type:\n1) Vanilla\n2) Spigot\nPress any other key to return");
            var stype = Console.ReadKey().KeyChar;
            if (stype < '1' || stype > '2')
                return;
            Console.Clear();
            Console.SetCursorPosition(0, 1);
            Console.Write("Type 'back' to return");
            Console.SetCursorPosition(0, 0);
            Console.Write("Server Version: ");


            var str = Console.ReadLine();
            Console.Clear();
            if (str.ToLower() == "back")
                return;
            Console.WriteLine("Server Version: " + str);

            if (str.ToLower() == "latest")
                MessageBox.Show("Using 'latest' is no longer supported");
               // str = GetLatest();

            CheckServerF();

            string sStypeString = (stype == '1' ? "Vanilla/" : "Spigot/");
            if (Directory.Exists("Servers/" + sStypeString + str))
            {
                var pe = new PropertiesEditor("Servers/" + sStypeString + str);
                pe.Text = "Properties Editor - " + (stype == '1' ? "Vanilla" : "Spigot") + " " + str; 
                pe.ShowDialog();
                pe.Dispose();
            } else
            {
                if (!bUseGUI)
                {
                    Console.WriteLine("Couldn't find version!\nPress any button to continue");
                    Console.ReadKey();
                } else
                {
                    Console.WriteLine("Couldn't find version!");
                }
            }

        }


        // UNUSED, might delete later idk ¯\_(ツ)_/¯
       // static async void ShowDownloadBar()
        //{
            /*
            var dp = new MCServerLauncher.Diags.DownloadPos();
            dp.ShowDialog();
            while (!DownloadComplete)
            {
                dp.pgbDownloadBar.Value = DownloadPos;
            }
            dp.Close();
            dp.Dispose();
            */
       // }

        #region "Vanilla Server"
        public static void start_server_vanilla(string version, bool redownload)
        {
            bool bFoundVersion = false; // Found Version check
            bool bFoundServer = false;  // Found Server check
            
            var bUseLatest = false;
            string str = version;
            if (version == null)
            {
                CheckServerF();
                Console.SetCursorPosition(0, 1);
                Console.Write("Type 'back' to return");
                Console.SetCursorPosition(0, 0);
                Console.Write("Server Version: ");


                str = Console.ReadLine();
                Console.Clear();
                if (str.ToLower() == "back")
                    return;
                Console.WriteLine("Server Version: " + str);

            }
        _beginning:
            if (str.ToLower() == "latest")
                MessageBox.Show("Using 'latest' is no longer supported");


            bUseLatest = str == "latest" || str == "Latest";
            if(redownload)
            File.Delete((GetSDV() + str + "/server.jar"));
            bool has_jar = false;
            if (Directory.Exists(GetSDV() + str))
            {
                has_jar = false;
                string[] files = Directory.GetFiles(GetSDV() + str);
                foreach (string file in files)
                {
                    if (Path.GetExtension(file) == ".jar")
                        has_jar = true;
                }
            }
            if (has_jar) // (File.Exists(GetSDV() + str + "/server.jar" ) || File.Exists(GetSDV() + str +  "/minecraft_server." + str + ".jar")
            {
                launch("Vanilla",str);

            }
            else
            {

                // Start off by grabbing the version list.
                try
                {
                    client.DownloadFile("https://launchermeta.mojang.com/mc/game/version_manifest.json", "version_manifest.json");
                }
                catch
                {
                    if (str.ToLower() != "latest")
                    {
                        Console.WriteLine("Failed to download version list, quitting...");
                        System.Threading.Thread.Sleep(3000); // Wait 3 secs
                        Environment.Exit(-1);
                    } else
                    {
                        str = sLatestVersion;
                        goto _beginning;
                    }
                }


                // Read all the text then dispose of the file, we won't be needing it later.
                string s = File.ReadAllText("version_manifest.json");
                File.Delete("version_manifest.json");
                JObject o = JObject.Parse(s);

                if (bUseLatest)
                {
                    str = o["latest"]["release"].ToString();
                    sLatestVersion = str;
                    SaveSettings();
                    goto _beginning;
                }

                bool bSnapshot = false;
                // Parse thru all the version 'till we find the correct one (if we do)
                foreach (JToken token in o["versions"])
                {
                    if (token["id"].ToString() == str)
                    {
                        Console.WriteLine("Found version " + str);
                        bFoundVersion = true;
                        if(token["type"].ToString() == "snapshot")
                        {
                            bSnapshot = true;
                        }
                        client.DownloadFile(token["url"].ToString(), str + ".json");
                    }
                }


                JToken tServer = null;

                // If we found a valid version download the .json file for it, else just tell the user that they have small brain.
                if (bFoundVersion)
                {
                    s = File.ReadAllText(str + ".json");
                    File.Delete(str + ".json");
                    o = JObject.Parse(s);
                    tServer = o["downloads"]["server"];
                    if(bSnapshot)
                    {
                        if (!File.Exists("Servers/Vanilla/snapshot_parents.json"))
                            File.WriteAllText("Servers/Vanilla/snapshot_parents.json","{}");
                        var c = JObject.Parse(File.ReadAllText("Servers/Vanilla/snapshot_parents.json"));
                        c[str] = o["assets"].ToString();
                        File.WriteAllText("Servers/Vanilla/snapshot_parents.json", c.ToString());
                    }
                    if (tServer != null)
                    {
                        bFoundServer = true;
                    }
                }
                else
                {
                    if (!bUseGUI)
                    {
                        Console.WriteLine("Couldn't find version " + str + "\nPress any button to return to continue");
                        Console.ReadKey();
                    }
                    else
                        Console.WriteLine("Couldn't find version " + str);
                    return;
                }


                // Final steps.
                // Make a folder for the version
                // Copy the elua.txt and server.properties file into the folder
                // Download the client.jar file and move it into the folder
                // and then finally launch the server!
                if (bFoundServer)
                {

                    Directory.CreateDirectory(GetSDV() + str);
                    File.WriteAllText(GetSDV() + str + "/eula.txt", "eula = " + bReadEULA.ToString());
                    File.WriteAllText(GetSDV() + str + "/server.properties", new ServerProperties().ToString());

                    string check = tServer["url"].ToString();

                    Console.Write("Downloading server.jar ");

                    try
                    {
                        client.OpenRead(check);
                        Int64 bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                        Console.Write("(" + (bytes_total / 1000000) + " MB)...");
                        client.DownloadFile(new Uri(check), GetSDV() + str + "/server.jar");
                        /*
                         * 
                         * eh, idk about this one.
                         * seems kinda pointless
                        DownloadComplete = false;
                        DownloadingServer = true;
                        DownloadPos = 0;
                        
                        ShowDownloadBar();

                        while(!DownloadComplete)
                        { }
                        */


                    }
                    catch
                    {
                        if (!bUseGUI)
                        {
                            Console.WriteLine("Failed to download server.jar\nPress any button to continue.");
                            Console.ReadKey();
                        }
                        else
                            Console.WriteLine("Failed to download server.jar");


                        return;
                    }

                    Console.Write("DONE!");





                    launch("Vanilla",str);

                }
                else
                {
                    // oop.
                    Console.WriteLine("Found version, but couldn't find available server.");
                }
            }
        }
        #endregion


        #region "Spigot Server"
        public static void start_server_spigot(string version, bool redownload)
        {
            //var client = new WebClient(); // For downloading the json files
            var ver = "";

            ver = version;

            if (Directory.Exists("Servers/Spigot/" + ver) || File.Exists("Servers/Spigot/" + ver + "/server.jar"))
                goto post_buildtools;


                // Download build tools first
            if (!Directory.Exists("BuildTools"))
            {
                Directory.CreateDirectory("BuildTools");
            }
            if (!Directory.Exists("BuildTools/" + version))
                Directory.CreateDirectory("BuildTools/" + version);

            if (!File.Exists("BuildTools/" + version + "/BuildTools.jar"))
            {
                if (File.Exists("BuildTools/BuildTools.jar"))
                {
                    File.Copy("BuildTools/BuildTools.jar", "BuildTools/" + version + "/BuildTools.jar");
                    goto pb0;
                }
                Console.WriteLine("Downloading BuildTools...");
                try
                { 
                client.DownloadFile("https://hub.spigotmc.org/jenkins/job/BuildTools/lastSuccessfulBuild/artifact/target/BuildTools.jar", "BuildTools/" + version + "/BuildTools.jar");
                }
                catch (WebException e)
                {
                    MessageBox.Show("Failed to download BuildTools.jar:\n" + e.Message);
                    MessageBox.Show("Opening web browser to download BuildTools.jar\nPlace it in the BuildTools folder");
                    ProcessStartInfo a = new ProcessStartInfo("https://hub.spigotmc.org/jenkins/job/BuildTools/lastSuccessfulBuild/artifact/target/BuildTools.jar");
                    Process.Start(a);
                    return;
                }
                Console.Clear();
            }

            pb0:

            if (!bUseGUI)
            {
                Console.SetCursorPosition(0, 1);
                Console.Write("Type 'back' to return");
                Console.SetCursorPosition(0, 0);
                Console.Write("Server Version: ");


                ver = Console.ReadLine();
                Console.Clear();
                if (ver.ToLower() == "back")
                    return;
            }
            if(redownload)
            {
                File.Delete("Servers/Spigot/" + ver + "/server.jar");
            }
            Console.WriteLine("Server Version: " + ver);

            
            post_buildtools:
                if (!Directory.Exists("Servers/Spigot/" + ver) || !File.Exists("Servers/Spigot/" + ver + "/server.jar"))
            {
                Directory.SetCurrentDirectory("BuildTools/" + ver);
                if (Directory.Exists("BuildData"))
                {
                    DELETE_FILES("BuildData");
                }
                ProcessStartInfo a = new ProcessStartInfo(sJavaPath + "/java", "-jar BuildTools.jar --rev " + ver);
                
                Process prc = System.Diagnostics.Process.Start(a);
                Console.WriteLine("Please wait for the download/compilation to complete, this may take a while...");
                prc.WaitForExit();
                Directory.SetCurrentDirectory("../../");

                if (!Directory.Exists("Servers/Spigot"))
                    Directory.CreateDirectory("Servers/Spigot");

                Directory.CreateDirectory("Servers/Spigot/" + ver);
                try
                {
                    File.Move("BuildTools/" + ver + "/spigot-" + ver + ".jar", "Servers/Spigot/" + ver + "/server.jar");
                } catch
                {
                    MessageBox.Show("Failed to download file, please delete the \"BuildTools\\" + ver + "\" directory and try again");
                    return;
                }
            }

            if(!File.Exists("Servers/Spigot/" + ver + "/eula.txt")) File.WriteAllText("Servers/Spigot/" + ver + "/eula.txt", "eula=" + bReadEULA.ToString().ToLower());
            if (!File.Exists("Servers/Spigot/" + ver + "/server.properties")) File.WriteAllText("Servers/Spigot/" + ver + "/server.properties", new ServerProperties().ToString());
            launch("Spigot", ver);
        }
        #endregion






        public static void DELETE_FILES(string dir)
        {

            try
            {
                foreach (string file in Directory.GetFiles(dir))
                    File.Delete(file);
                foreach (string sdir in Directory.GetDirectories(dir))
                {
                    DELETE_FILES(sdir);
                    Directory.Delete(sdir);
                }
            }
            catch
            { }
        }
        public static void CLONE_DIRECTORY(string inDir, string toDir)
        {
            Directory.CreateDirectory(toDir);
            foreach (string file in Directory.GetFiles(inDir))
            {
                File.Copy(file, toDir + "\\" + Path.GetFileName(file));
            }
            foreach (string dir in Directory.GetDirectories(inDir))
            {
                string outdir = toDir + "\\" + Path.GetFileName(dir);
                CLONE_DIRECTORY(dir, outdir);
            }
        }
    }




    public class SPropSetting
    {

        public static int type_string = 0;
        public static int type_int = 1;
        public static int type_boolean = 2;

        public string value = "";
        public int type = 0;
        // Types:
        // 0 : String
        // 1 : Int <-- not really used atm, pretty much just string. Best to use it if just incase I find a use for it.
        // 2 : Boolean

        public SPropSetting(string value, int type)
        {
            this.value = value;
            this.type = type;
        }
    }

    public class ServerProperties
    {
        public Dictionary<string, SPropSetting> ServerVars = new Dictionary<string, SPropSetting>();



        public ServerProperties()
        {
            ServerVars["announce-player-achievements"] = new SPropSetting(null, 1);
            ServerVars["allow-flight"] = new SPropSetting("false",2);
            ServerVars["allow-nether"] = new SPropSetting("true",2);
            ServerVars["broadcast-console-to-ops"] = new SPropSetting("true", 2);
            ServerVars["broadcast-rcon-to-ops"] = new SPropSetting("true", 2);
            ServerVars["debug"] = new SPropSetting("false", 2);
            ServerVars["difficulty"] = new SPropSetting("0", 1);
            ServerVars["enable-command-block"] = new SPropSetting("false",2);
            ServerVars["enable-query"] = new SPropSetting("false",2);
            ServerVars["enable-rcon"] = new SPropSetting("false",2);
            ServerVars["force-gamemode"] = new SPropSetting("false",2);
            ServerVars["function-permission-level"] = new SPropSetting("4",1);
            ServerVars["gamemode"] = new SPropSetting("0",1);
            ServerVars["generate-structures"] = new SPropSetting("true",1);
            ServerVars["generator-settings"] = new SPropSetting("",0);
            ServerVars["hardcore"] = new SPropSetting("false",2);
            ServerVars["level-name"] = new SPropSetting("world",0);
            ServerVars["level-seed"] = new SPropSetting("",0);
            ServerVars["level-type"] = new SPropSetting("default",0);
            ServerVars["max-build-height"] = new SPropSetting("256",1);
            ServerVars["max-players"] = new SPropSetting("20",1);
            ServerVars["max-tick-time"] = new SPropSetting("60000",1);
            ServerVars["max-world-size"] = new SPropSetting("29999984",1);
            ServerVars["motd"] = new SPropSetting("A Minecraft Server",0);
            ServerVars["network-compression-threshold"] = new SPropSetting("256",1);
            ServerVars["online-mode"] = new SPropSetting("true",2);
            ServerVars["op-permission-level"] = new SPropSetting("4",1);
            ServerVars["player-idle-timeout"] = new SPropSetting("0",1);
            ServerVars["prevent-proxy-connections"] = new SPropSetting("false",2);
            ServerVars["pvp"] = new SPropSetting("true",2);
            ServerVars["query.port"] = new SPropSetting("25565",1);
            ServerVars["query.password"] = new SPropSetting("",1);
            ServerVars["rcon.port"] = new SPropSetting("25575", 1); 
            ServerVars["rcon.password"] = new SPropSetting("", 0);
            ServerVars["resource-pack"] = new SPropSetting(null,0);         // Set to null because some older servers use texture packs
            ServerVars["resource-pack-sha1"] = new SPropSetting(null,0);
            ServerVars["resource-pack-hash"] = new SPropSetting(null, 0);
            ServerVars["server-ip"] = new SPropSetting("",0);
            ServerVars["server-port"] = new SPropSetting("25565",1);
            ServerVars["snooper-enabled"] = new SPropSetting("true",2);
            ServerVars["spawn-animals"] = new SPropSetting("true",2);
            ServerVars["spawn-monsters"] = new SPropSetting("true",2);
            ServerVars["spawn-npcs"] = new SPropSetting("true",2);
            ServerVars["spawn-protection"] = new SPropSetting("16",1);
            ServerVars["texture-pack"] = new SPropSetting(null, 0);        // Legacy
            ServerVars["use-native-transport"] = new SPropSetting("true",2);
            ServerVars["view-distance"] = new SPropSetting("10",1);
            ServerVars["white-list"] = new SPropSetting("false",2);
            ServerVars["enforce-whitelist"] = new SPropSetting("false",2);
        }

        public static ServerProperties FromFile(string file)
        {
            var spProp = new ServerProperties();

            if (File.Exists(file))
            {
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    if (line[0] != '#')
                    {
                        var vs = line.Split('=');
                        spProp.ServerVars[vs[0]].value = vs[1];
                    }
                }
            }

            return spProp;
        }
        public override string ToString()
        {

            var str = "";
            foreach( KeyValuePair<string,SPropSetting> KVP in ServerVars)
            {
                str+=KVP.Key + "=" + KVP.Value.value + "\n";
            }

            return str;
        }
    }
}


// sub 2 SimpleFlips