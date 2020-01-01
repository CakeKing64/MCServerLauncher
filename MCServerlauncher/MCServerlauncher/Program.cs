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

namespace MCServerLauncher
{
    class Program
    {

        // Settings
        static bool bUseSingleDirectory = false;
        static string sLatestVersion = "1.0";
        static bool bReadEULA = false;
        static string sLaunchArgs = "-Xmx1024M -Xms1024M -jar server.jar nogui";
        static string sQLaunchSType = "Vanilla";
        static string sQLaunchSVersion = "1.0";
        static string sJavaPath = null;


        static string GetLatest()
        {
            return sLatestVersion;
        }

        static void launch(string stype, string vnumb)
        {
            if (!Directory.Exists("Servers/" + stype + "/"  + vnumb) || !File.Exists("Servers/" + stype + "/" + vnumb + "/server.jar"))
            {
                Console.WriteLine("Unable to launch " + stype + " / " + vnumb);
                return;
            }
            sQLaunchSType = stype;
            sQLaunchSVersion = vnumb;
            SaveSettings();
            Directory.SetCurrentDirectory("Servers/" + stype + "/" + vnumb);
            ProcessStartInfo a = new ProcessStartInfo(sJavaPath + "bin/java", sLaunchArgs);

            System.Diagnostics.Process.Start(a);
            Directory.SetCurrentDirectory("../../../");

        }

        static void ProgramExit(object sender, EventArgs ea)
        {
            SaveSettings();
        }
        static void SaveSettings()
        {
            JObject job = new JObject();
            job["bUseSingleDirectory"] = bUseSingleDirectory;
            job["sLatestVersion"] = sLatestVersion;
            job["bReadEULA"] = bReadEULA;
            job["sLaunchArgs"] = sLaunchArgs;
            job["bUseGUI"] = false;
            job["sQLaunchSType"] = sQLaunchSType;
            job["sQLaunchSVersion"] = sQLaunchSVersion;
            job["sJavaPath"] = sJavaPath;
            File.WriteAllText("settings.json", job.ToString());
        }

        static string GetString(JObject job, string get, string def)
        {
            return job[get] != null ? job[get].ToString() : def;
        }

        [STAThreadAttribute]
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProgramExit);


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
            if(!Directory.Exists(sJavaPath) || !File.Exists(sJavaPath + "java.exe"))
            {
                sJavaPath = null;
            }
            if (sJavaPath == null)
            {
                bool foundJpath = false;
                string path = Environment.GetEnvironmentVariable("Path");
                string[] paths = path.Split(';');
                foreach(string sDir in paths)
                {
                    if (sDir != "")
                    {
                        var add = sDir[sDir.Length - 1] == '/' ? "" : "//";

                        if (Directory.Exists(sDir))
                            if (Directory.Exists(sDir + add + "bin"))
                                if (File.Exists(sDir + add + "bin/java.exe"))
                                {
                                    sJavaPath = sDir + add + "bin";
                                }
                        if (File.Exists(sDir + add + "java.exe"))
                        {
                            sJavaPath = sDir;
                        }
                    }
                }
                if (!foundJpath)
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


                                if (fbdResult == DialogResult.OK)
                                {
                                    if (Directory.Exists(fbdDialog.SelectedPath + "/bin"))
                                        if (File.Exists(fbdDialog.SelectedPath + "/bin/java.exe"))
                                        {
                                            sJavaPath = fbdDialog.SelectedPath + "bin";
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
            Console.Clear();

            

            Console.Title = "MCServerLauncher 1.0";
            while (true)
            {
                Console.WriteLine("1) Download / Launch Server\n2) Modify Properties\n3) List Versions\n4) Quick Launch (" + sQLaunchSType + " / " + sQLaunchSVersion + ")\n5) Exit");
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
                                start_server_vanilla();
                                break;
                            case '2':
                                start_server_spigot();
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
                        Environment.Exit(1);
                        break;
                    default:
                        break;
                }



                Console.Clear();

            }

        }

        static void CheckServerF()
        {
            if (!Directory.Exists("Servers"))
                Directory.CreateDirectory("Servers");
            if (!Directory.Exists("Servers/Vanilla"))
                Directory.CreateDirectory("Servers/Vanilla");
        }

        static string GetSDV()
        {
            return "Servers/Vanilla/";
        }
        static void modify_prop()
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
            if (str.ToLower() == "back")
                return;

            if (str == "latest")
                str = GetLatest();

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
                Console.WriteLine("Couldn't find version!\nPress any button to continue");
                Console.ReadKey();
            }

        }

    #region "Vanilla Server"
        static void start_server_vanilla()
        {
            bool bFoundVersion = false; // Found Version check
            bool bFoundServer = false;  // Found Server check
            var client = new WebClient(); // For downloading the json files
            var bUseLatest = false;


            CheckServerF();
            Console.SetCursorPosition(0, 1);
            Console.Write("Type 'back' to return");
            Console.SetCursorPosition(0, 0);
            Console.Write("Server Version: ");
            

            var str = Console.ReadLine();
            if (str.ToLower() == "back")
                return;
        _beginning:

            bUseLatest = str == "latest";
            if (Directory.Exists(GetSDV() + str))
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
                    if (str != "latest")
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
                // Parse thru all the version 'till we find the correct one (if we do)
                foreach (JToken token in o["versions"])
                {
                    if (token["id"].ToString() == str)
                    {
                        Console.WriteLine("Found version " + str);
                        bFoundVersion = true;
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

                    if (tServer != null)
                    {
                        bFoundServer = true;
                    }
                }
                else
                {
                    Console.WriteLine("Couldn't find version " + str + "\nPress any button to return to continue");
                    Console.ReadKey();
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
                        client.DownloadFile(check, GetSDV() + str + "/server.jar");
                    }
                    catch
                    {
                        Console.WriteLine("Failed to download server.jar\nPress any button to continue.");
                        Console.ReadKey();
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
        static void start_server_spigot()
        {
            var client = new WebClient(); // For downloading the json files

            
            // Download build tools first
            if (!Directory.Exists("BuildTools"))
            {
                Directory.CreateDirectory("BuildTools");
            }
            if (!File.Exists("BuildTools/BuildTools.jar")) 
            {
                Console.WriteLine("Downloading BuildTools...");
                client.DownloadFile("https://hub.spigotmc.org/jenkins/job/BuildTools/lastSuccessfulBuild/artifact/target/BuildTools.jar", "BuildTools/BuildTools.jar");
                Console.Clear();
            }


            Console.SetCursorPosition(0, 1);
            Console.Write("Type 'back' to return");
            Console.SetCursorPosition(0, 0);
            Console.Write("Server Version: ");


            var ver = Console.ReadLine();
            if (ver.ToLower() == "back")
                return;

            ;
                if (!Directory.Exists("Servers/Spigot/" + ver) || !File.Exists("Servers/Spigot/" + ver + "/server.jar"))
            {
                Directory.SetCurrentDirectory("BuildTools");

                ProcessStartInfo a = new ProcessStartInfo(sJavaPath + "bin/java", "-jar BuildTools.jar --rev " + ver);
                
                Process prc = System.Diagnostics.Process.Start(a);
                Console.WriteLine("Please wait for the download/compilation to complete, this may take a while...");
                prc.WaitForExit();
                Directory.SetCurrentDirectory("../");

                if (!Directory.Exists("Servers/Spigot"))
                    Directory.CreateDirectory("Servers/Spigot");

                Directory.CreateDirectory("Servers/Spigot/" + ver);
                File.Move("BuildTools/spigot-" + ver + ".jar", "Servers/Spigot/" + ver + "/server.jar");
            }

            if(!File.Exists("Servers/Spigot/" + ver + "/eula.txt")) File.WriteAllText("Servers/Spigot/" + ver + "/eula.txt", "eula=" + bReadEULA.ToString().ToLower());
            if (!File.Exists("Servers/Spigot/" + ver + "/server.properties")) File.WriteAllText("Servers/Spigot/" + ver + "/server.properties", new ServerProperties().ToString());
            launch("Spigot", ver);
        }
    #endregion
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
        // 1 : Int
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
            ServerVars["resource-pack"] = new SPropSetting("",0);
            ServerVars["resource-pack-sha1"] = new SPropSetting("",0);
            ServerVars["server-ip"] = new SPropSetting("",0);
            ServerVars["server-port"] = new SPropSetting("25565",1);
            ServerVars["snooper-enabled"] = new SPropSetting("true",2);
            ServerVars["spawn-animals"] = new SPropSetting("true",2);
            ServerVars["spawn-monsters"] = new SPropSetting("true",2);
            ServerVars["spawn-npcs"] = new SPropSetting("true",2);
            ServerVars["spawn-protection"] = new SPropSetting("16",1);
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


// subscribe to jacksfilms on YTYouTube