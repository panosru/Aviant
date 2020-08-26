namespace Aviant.DDD.Domain.TransferObjects
{
    using Newtonsoft.Json;

    public abstract class Dto : IDto
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToJson(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public string ToJson(JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(this, settings);
        }

        public string ToJson(Formatting formatting, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(this, formatting, settings);
        }
    }
}