using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace VSOClientData.Entities.Vso
{
    [JsonObjectAttribute]
    public class VsoAccount : MongoEntity
    {
        [JsonIgnore]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "accountUri")]
        public string AccountUrl { get; set; }

        [JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        [JsonProperty(PropertyName = "organizationName")]
        public string OrganizationName { get; set; }

        [JsonProperty(PropertyName = "accountType")]
        public string AccountType { get; set; }

        [JsonProperty(PropertyName = "accountOwner")]
        public string AccountOwner { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "accountStatus")]
        public string AccountStatus { get; set; }

        [JsonProperty(PropertyName = "lastUpdatedBy")]
        public Guid LastUpdatedBy { get; set; }

        [JsonProperty(PropertyName = "lastUpdatedDate")]
        public DateTime LastUpdatedDate { get; set; }
    }
}