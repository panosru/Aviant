namespace Aviant.DDD.Domain.TransferObjects
{
    using Newtonsoft.Json;

    public interface IEto 
    {
        public string ToJson();

        public string ToJson(Formatting formatting);

        public string ToJson(JsonSerializerSettings settings);

        public string ToJson(Formatting formatting, JsonSerializerSettings settings);
    }
}