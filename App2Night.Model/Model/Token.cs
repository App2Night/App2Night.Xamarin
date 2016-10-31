using Newtonsoft.Json;

namespace App2Night.Model.Model
{
    public class Token
    {
        [JsonProperty(PropertyName = "access_token")] 
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; } 
    }
}