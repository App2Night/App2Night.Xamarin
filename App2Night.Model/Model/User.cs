using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using App2Night.Model.Enum;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace App2Night.Model.Model
{
    public class User 
    {
        [JsonProperty(PropertyName = "sub")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
	}
}