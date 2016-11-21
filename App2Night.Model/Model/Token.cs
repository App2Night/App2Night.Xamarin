using System;
using Newtonsoft.Json;

namespace App2Night.Model.Model
{
    public class Token
    {
        [JsonProperty(PropertyName = "access_token")] 
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; } 

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpirationLength { get; set; }

        [JsonIgnore]
        public DateTime LastRefresh { get; set; }

        [JsonIgnore]
        public DateTime ExpirationDate => LastRefresh.AddMinutes(ExpirationLength);

        [JsonIgnore]
        public TimeSpan ExpiresIn => ExpirationDate - DateTime.Now;
    }
}