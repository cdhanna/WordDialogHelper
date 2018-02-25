using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogAddin.Host
{
    public class DevManifest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("files")]
        public string[] Files { get; set; }

    }
}
