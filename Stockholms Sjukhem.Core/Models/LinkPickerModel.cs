using Newtonsoft.Json;

namespace Boilerplate.Core.Models
{
    public class LinkPickerModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }
}
