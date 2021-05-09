using System;
using System.Collections.Generic;

namespace HttpServer.Models
{
    [Serializable]
    public class License
    {
        public string HttpUrl;
        public List<Plugin> Plugins = new List<Plugin>();
    }
    [Serializable]
    public class Plugin
    {
        public string Name;
        public List<Member> Members = new List<Member>();
    }
    [Serializable]
    public class Member
    {
        public string IpAddress;
        public string Name;
        public string Steam64ID;
        public string DiscordName;
        public string DiscordID;
    }
}
