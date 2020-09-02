namespace Aviant.DDD.Domain.TransferObjects
{
    using Newtonsoft.Json;

    public abstract class Dto : IDto
    {
    #region IDto Members

        public string ToJson() => JsonConvert.SerializeObject(this);

        public string ToJson(Formatting formatting) => JsonConvert.SerializeObject(this, formatting);

        public string ToJson(JsonSerializerSettings settings) => JsonConvert.SerializeObject(this, settings);

        public string ToJson(Formatting formatting, JsonSerializerSettings settings) =>
            JsonConvert.SerializeObject(this, formatting, settings);

    #endregion
    }
}