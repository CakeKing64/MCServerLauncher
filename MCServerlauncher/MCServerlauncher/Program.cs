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

namespace MCServerLauncher
{
    class Program
    {

        // Settings
        static bool bUseSingleDirectory = false;
        static string sLatestVersion = "1.0";
        static bool bReadEULA = false;
        static string sLaunchArgs = "-Xmx1024M -Xms1024M -jar server.jar nogui";




        static string GetLatest()
        {
            return sLatestVersion;
        }

        static void launch(string vnumb)
        {
            Directory.SetCurrentDirectory("Servers/" + vnumb);
            ProcessStartInfo a = new ProcessStartInfo("java", sLaunchArgs);

            System.Diagnostics.Process.Start(a);
            Directory.SetCurrentDirectory("../../");

        }

        static void ProgramExit(object sender, EventArgs ea)
        {
            JObject job = new JObject();
            job["bUseSingleDirectory"] = bUseSingleDirectory;
            job["sLatestVersion"] = sLatestVersion;
            job["bReadEULA"] = bReadEULA;
            job["sLaunchArgs"] = sLaunchArgs;
            File.WriteAllText("settings.json", job.ToString());
        }
        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProgramExit);


            if (!File.Exists("settings.json"))
            {
                var job = new JObject();
                job["bUseSingleDirectory"] = false;
                job["sLatestVersion"] = "1.0";
                job["bReadEULA"] = false;
                job["sLaunchArgs"] = "-Xmx1024M -Xms1024M -jar server.jar nogui";
                File.WriteAllText("settings.json", job.ToString());
            } else
            {
                var job = JObject.Parse(File.ReadAllText("settings.json"));
                sLatestVersion = job["sLatestVersion"] != null ? job["sLatestVersion"].ToString() : "1.0";
                bUseSingleDirectory = job["bUseSingleDirectory"] != null ? bool.Parse(job["bUseSingleDirectory"].ToString()) : false;
                bReadEULA = job["bReadEULA"] != null ? bool.Parse(job["bReadEULA"].ToString()) : false;
                sLatestVersion = job["sLaunchArgs"] != null ? job["sLaunchArgs"].ToString() : sLaunchArgs;
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

            Console.Title = "MCServerLauncher 1.0";
            while (true)
            {
                Console.WriteLine("1) Download / Launch Server\n2) Modify Properties\n3) List Versions\n4) Exit");
                var sel = Console.ReadKey();
                Console.Clear();
                switch (sel.KeyChar)
                {
                    case '1':
                        start_server();
                        break;
                    case '2':
                        modify_prop();
                        break;
                    case '3':
                        var saDirs = Directory.GetDirectories("Servers");
                        Console.WriteLine("Avalable Server Versions:");
                        foreach (string sDir in saDirs)
                        {
                            var str = sDir.Substring(8);
                            Console.WriteLine(str + (str == sLatestVersion ? " (Latest)" : ""));
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case '4':
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
        }

        static void modify_prop()
        {
            Console.Write("Server Version: ");
            var str = Console.ReadLine();

            if (str == "latest")
                str = GetLatest();

            CheckServerF();


            if (Directory.Exists("Servers/" + str))
            {
                var process = System.Diagnostics.Process.Start("notepad", "Servers/" + str + "/server.properties");
                process.WaitForExit();
            } else
            {
                Console.WriteLine("Couldn't find version!\nPress any button to continue");
                Console.ReadKey();
            }

        }
        static void start_server()
        {
            bool bFoundVersion = false; // Found Version check
            bool bFoundServer = false;  // Found Server check
            var client = new WebClient(); // For downloading the json files
            var bUseLatest = false;


            CheckServerF();

            Console.Write("Server Version: ");
            var str = Console.ReadLine();
        _beginning:

            bUseLatest = str == "latest";
            if (Directory.Exists("Servers/" + str))
            {
                launch(str);

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

                    Directory.CreateDirectory("Servers/" + str);
                    File.WriteAllText("Servers/" + str + "/eula.txt", "eula = " + bReadEULA.ToString());
                    File.WriteAllText("Servers/" + str + "/server.properties", new ServerProperties().ToString());

                    string check = tServer["url"].ToString();

                    Console.Write("Downloading server.jar ");

                    try
                    {
                        client.OpenRead(check);
                        Int64 bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                        Console.Write("(" + (bytes_total / 1000000) + " MB)...");
                        client.DownloadFile(check, "Servers/" + str + "/server.jar");
                    }
                    catch
                    {
                        Console.WriteLine("Failed to download server.jar\nPress any button to continue.");
                        Console.ReadKey();
                        return;
                    }

                    Console.Write("DONE!");





                    launch(str);

                }
                else
                {
                    // oop.
                    Console.WriteLine("Found version, but couldn't find available server.");
                }
            }
        }
    }


    class ServerProperties
    {
        public Dictionary<string, string> ServerVars = new Dictionary<string, string>();


        public ServerProperties()
        {
            ServerVars["allow-flight"] = "false";
            ServerVars["allow-nether"] = "true";
            ServerVars["difficulty"] = "0";
            ServerVars["enable-command-block"] = "false";
            ServerVars["enable-query"] = "false";
            ServerVars["enable-rcon"] = "false";
            ServerVars["force-gamemode"] = "false";
            ServerVars["function-permission-level"] = "4";
            ServerVars["gamemode"] = "0";
            ServerVars["generate-structures"] = "true";
            ServerVars["generator-settings"] = "";
            ServerVars["hardcore"] = "false";
            ServerVars["level-name"] = "world";
            ServerVars["level-seed"] = "";
            ServerVars["level-type"] = "default";
            ServerVars["max-build-height"] = "256";
            ServerVars["max-players"] = "20";
            ServerVars["max-tick-time"] = "60000";
            ServerVars["max-world-size"] = "29999984";
            ServerVars["motd"] = "A Minecraft Server";
            ServerVars["network-compression-threshold"] = "256";
            ServerVars["online-mode"] = "true";
            ServerVars["op-permission-level"] = "4";
            ServerVars["player-idle-timeout"] = "0";
            ServerVars["prevent-proxy-connections"] = "false";
            ServerVars["pvp"] = "true";
            ServerVars["query.port"] = "25565";
            ServerVars["query.password"] = "25575";
            ServerVars["resource-pack"] = "";
            ServerVars["resource-pack-sha1"] = "";
            ServerVars["server-ip"] = "";
            ServerVars["server-port"] = "25565";
            ServerVars["snooper-enabled"] = "true";
            ServerVars["spawn-animals"] = "true";
            ServerVars["spawn-monsters"] = "true";
            ServerVars["spawn-npcs"] = "true";
            ServerVars["spawn-protection"] = "16";
            ServerVars["use-native-transport"] = "true";
            ServerVars["view-distance"] = "10";
            ServerVars["white-list"] = "false";
            ServerVars["enforce-whitelist"] = "false";
        }

        public static ServerProperties FromFile(string file)
        {
            var spProp = new ServerProperties();

            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines)
            {
                if(line[0] != '#')
                {
                    var vs = line.Split('=');
                    spProp.ServerVars[vs[0]] = vs[1];
                }
            }

            return spProp;
        }
        public override string ToString()
        {

            var str = "";
            foreach( KeyValuePair<string,string> KVP in ServerVars)
            {
                str+=KVP.Key + "=" + KVP.Value + "\n";
            }

            return str;
        }
    }
}


// subscribe to jacksfilms on YTYouTube