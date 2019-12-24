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
            Directory.SetCurrentDirectory(vnumb);
            ProcessStartInfo a = new ProcessStartInfo("java", "-Xmx1024M -Xms1024M -jar server.jar nogui");

            System.Diagnostics.Process.Start(a);

        }
        static void Main(string[] args)
        {


            Console.Write("Server Version: ");
            var str = Console.ReadLine();
            if (Directory.Exists(str))
            {
                launch(str);

            }
            else
            {



                bool bFoundVersion = false; // Found Version check
                bool bFoundServer = false;  // Found Server check
                var client = new WebClient(); // For downloading the json files


                // Start off by grabbing the version list.
                client.DownloadFile("https://launchermeta.mojang.com/mc/game/version_manifest.json", "version_manifest.json");

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
                    Console.WriteLine("Couldn't find version " + str);
                    Environment.Exit(2);
                }


                // Final steps.
                // Make a folder for the version
                // Copy the elua.txt and server.properties file into the folder
                // Download the client.jar file and move it into the folder
                // and then finally launch the server!
                if (bFoundServer)
                {

                    Directory.CreateDirectory(str);
                    File.Copy("eula.txt", str + "/eula.txt");
                    File.Copy("server.properties", str + "/server.properties");

                    string check = tServer["url"].ToString();

                    Console.Write("Downloading server.jar ");

                    client.OpenRead(check);
                    Int64 bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
                    Console.Write("(" + (bytes_total / 1000000) + " MB)...");
                    client.DownloadFile(check, str + "/server.jar");
                    Console.Write("DONE!");



                    /* Clean up */

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