using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace LetsTalk.Model
{
    /// <summary>
    /// A class used to store messages in Mobile Services
    /// </summary>
    [DataTable("messages")]
    public class Message
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}