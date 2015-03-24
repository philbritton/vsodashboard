using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VSOClientData.Entities.Vso
{
    public class VsoWorkItem
    {
        [JsonProperty(PropertyName="id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "rev")]
        public int Revision { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public Dictionary<string, string> Fields { get; set; }

        [JsonProperty(PropertyName = "url")]
        public int Url { get; set; }

    }
}
