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

namespace MCServerLauncher
{
    class Program
    {

        static void launch(string vnumb)
        {
            Directory.SetCurrentDirectory("Servers/" + vnumb);
            ProcessStartInfo a = new ProcessStartInfo("java", "-Xmx1024M -Xms1024M -jar server.jar nogui");

            System.Diagnostics.Process.Start(a);
            Directory.SetCurrentDirectory("../../");

        }
        static void Main(string[] args)
        {
            Console.Title = "MCServerLauncher 1.0";
            while (true)
            {
                Console.WriteLine("1) Download / Launch Server\n2) Modify Properties\n3) List Versions\n4) Exit");
                var sel = Console.ReadKey();
                Console.Clear();
                switch(sel.KeyChar)
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
                            Console.WriteLine(sDir.Substring(8));
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

            CheckServerF();

            Console.Write("Server Version: ");
            var str = Console.ReadLine();
            if (Directory.Exists("Servers/" + str))
            {
                launch(str);

            }
            else
            {



                bool bFoundVersion = false; // Found Version check
                bool bFoundServer = false;  // Found Server check
                var client = new WebClient(); // For downloading the json files


                // Start off by grabbing the version list.
                try
                {
                    client.DownloadFile("https://launchermeta.mojang.com/mc/game/version_manifest.json", "version_manifest.json");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to download version list, quitting...");
                    System.Threading.Thread.Sleep(3000); // Wait 3 secs
                    Environment.Exit(-1);
                }


                // Read all the text then dispose of the file, we won't be needing it later.
                string s = File.ReadAllText("version_manifest.json");
                File.Delete("version_manifest.json");
                JObject o = JObject.Parse(s);

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
                    File.Copy("eula.txt", "Servers/" + str + "/eula.txt");
                    File.Copy("server.properties", "Servers/" + str + "/server.properties");

                    string check = tServer["url"].ToString();

                    Console.Write("Downloading server.jar ");

                    try
                    {
                        client.OpenRead(check);
                        Int64 bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                        Console.Write("(" + (bytes_total / 1000000) + " MB)...");
                        client.DownloadFile(check, "Servers/" +  str + "/server.jar");
                    }
                    catch (Exception e)
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
}


// subscribe to jacksfilms on YTYouTube