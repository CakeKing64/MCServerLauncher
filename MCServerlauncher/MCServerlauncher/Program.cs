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
            /*
            string[] jars = Directory.GetFiles("./", "*.jar");
            for (int i = 0; i < jars.Length; i++)
                Console.WriteLine(i + ") " + jars[i].Substring(2));

            string str = Console.ReadLine();
            Console.Clear();

            */

            Console.Write("Server Version: ");
            var str = Console.ReadLine();
            if (Directory.Exists(str))
            {
                launch(str);

            }
            else
            {


                /*
                Console.Write("Parsing sv_list.json...");
                string s = File.ReadAllText("sv_list.json");
                */
                bool bFoundVersion = false; // Found Version check
                bool bFoundServer = false;
                var client = new WebClient();

                client.DownloadFile("https://launchermeta.mojang.com/mc/game/version_manifest.json", "version_manifest.json");


                string s = File.ReadAllText("version_manifest.json");
                File.Delete("version_manifest.json");
                JObject o = JObject.Parse(s);


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
                    Console.WriteLine("Found version, but couldn't find available server.");
                }
            }
        }
    }
}
