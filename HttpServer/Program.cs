using System;
using Newtonsoft.Json;
using System.IO;
using HttpServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string FilePath = "C:/LicenseServer/License.json";
            var license = new License();
            if (File.Exists(FilePath) == false)
            {
                FileStream fs = new FileStream(FilePath, FileMode.Create);
                fs.Close();
                SaveDefault(license, FilePath);
                SendConsole("Filepath created. Restart the program.", ConsoleColor.Cyan);
                Console.ReadKey();
                return;
            }
            if (File.ReadAllText(FilePath).Length == 0)
            {
                SaveDefault(license, FilePath);
                SendConsole("Default license created. Restart the program.", ConsoleColor.Cyan);
                Console.ReadKey();
                return;
            }
            try
            {
                license = JsonConvert.DeserializeObject<License>(File.ReadAllText(FilePath));

                HttpListener httpListener = new HttpListener();
                httpListener.Prefixes.Add(license.HttpUrl);
                httpListener.Start();

                while (true)
                {

                    SendConsole("Connection waiting..", ConsoleColor.Yellow);
                    var context = httpListener.GetContext();

                    SendConsole("Connected.", ConsoleColor.Yellow);
                    var response = context.Response;
                    var request = context.Request;

                    Console.Write("Connected address: ");
                    SendConsole(request.RawUrl, ConsoleColor.Yellow);
                    
                    var rawValue = "";
                    var plugin = license.Plugins.FirstOrDefault(p => p.Name == request.RawUrl);
                    if (plugin != null)
                    {
                        foreach (var member in plugin.Members)
                        {
                            rawValue += $"{member.IpAddress}  -  {member.Name} <br />";
                        }
                    }
                    else
                    {
                        rawValue = "Page not found.";
                    }
                    byte[] value = Encoding.UTF8.GetBytes(rawValue);
                    response.AddHeader("Content-Type", "text/html; charset=utf-8");
                    response.OutputStream.Write(value, 0, value.Length);
                    response.Close();
                }
            }
            catch (JsonException)
            {
                SendConsole("The json file is wrong. Delete file and restart program.", ConsoleColor.Red);
                Console.ReadKey();
            }
        }
        static void SendConsole(string message, ConsoleColor color)
        {
            Console.WriteLine(message, Console.ForegroundColor = color);
            Console.ResetColor();
        }
        static void SaveDefault(License license, string filepath)
        {
            license.HttpUrl = "http://localhost:3090/";
            license.Plugins = new List<Plugin>()
                {
                    new Plugin
                    {
                        Name = "/Test",
                        Members = new List<Member>
                        {
                            new Member
                            {
                                Name = "Mixy",
                                IpAddress = "89.252.171.101",
                                DiscordName = "`Mixy#1232",
                                DiscordID = "",
                                Steam64ID = "",
                            }
                        }
                    }
                };
            Json.Save(license, filepath);
        }
    }
}
