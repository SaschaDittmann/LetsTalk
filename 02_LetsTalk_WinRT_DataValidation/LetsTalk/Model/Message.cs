using System;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.MobileServices;

namespace LetsTalk.Model
{
    /// <summary>
    /// A class used to store messages in Mobile Services
    /// </summary>
    [DataTable(Name = "messages")]
    public class Message
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "body")]
        public string Body { get; set; }
        
        [DataMember(Name = "createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}