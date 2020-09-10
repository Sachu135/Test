using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinuxInstaller
{
    [JsonObject("application")]
    class Application
    {
        [JsonProperty("name")]
        public List<string> commands { get; set; }
    }
}
