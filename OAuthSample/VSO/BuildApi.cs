using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace VSOOAuthClientSample.VSO.BuildApi
{

    public class Build
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "drop")]
        public Drop BuildDrop { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; }
    }

    public class Drop
    {
        [JsonProperty(PropertyName = "location")]
        public String Location { get; set; }

        [JsonProperty(PropertyName = "type")]
        public String type { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; }
    }

    public class BuildDefinition
    {

        [JsonProperty(PropertyName = "uri")]
        public String Uri { get; set; }

        [JsonProperty(PropertyName = "queue")]
        public Queue Queue { get; set; }

        [JsonProperty(PropertyName = "triggerType")]
        public String TriggerType { get; set; }

        [JsonProperty(PropertyName = "defaultDropLocation")]
        public String DefaultDropLocation { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "definitionType")]
        public String DefinitionType { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; }
    }

    public class BuildRequest
    {
        [JsonProperty(PropertyName = "definition")]
        public Definition Definition { get; set; }

        [JsonProperty(PropertyName = "reason")]
        public String Reason { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public String Priority { get; set; }

        [JsonProperty(PropertyName = "queuePosition")]
        public int QueuePosition { get; set; }

        [JsonProperty(PropertyName = "queueTime")]
        public DateTime QueueTime { get; set; }

        [JsonProperty(PropertyName = "requestedBy")]
        public RequestedBy RequestedBy { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String Url { get; set; }

        [JsonProperty(PropertyName = "builds")]
        public IEnumerable<Build> Builds { get; set; }

    }

    public class RequestedBy
    {
        [JsonProperty(PropertyName = "displayName")]
        public String DisplayName { get; set; }

        [JsonProperty(PropertyName = "uniqueName")]
        public String UniqueName { get; set; }

    }

    public class Definition
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }

    

    public class Queue
    {
        [JsonProperty(PropertyName = "queueType")]
        public String QueueType { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "url")]
        public String Url;

    }

    public static class Priority
    {
        public const String Normal = "Normal";
        public const String AboveNormal = "AboveNormal";
        public const String BelowNormal = "BelowNormal";
        public const String High = "High";
        public const String Low = "Low";
    }

    public static class Reason
    {
        public const String BatchedCI = "BatchedCI";
        public const String CheckInShelveset = "CheckInShelveset";
        public const String IndividualCI = "IndividualCI";
        public const String Manual = "Manual";
        public const String None = "None";
        public const String Schedule = "Schedule";
        public const String ScheduleForced = "ScheduleForced";
        public const String Triggered = "Triggered";
        public const String UserCreated = "UserCreated";
        public const String ValidateShelveset = "ValidateShelveset";
        public const String HasFlag = "HasFlag";
    }

    public static class BuildStatus
    {
        public const String NotStarted = "notStarted";
        public const String InProgress = "inProgress";
        public const String Succeeded = "succeeded";
    }
}