namespace Aviant.DDD.Domain.TransferObjects
{
    #region

    using Newtonsoft.Json;

    #endregion

    public interface ITransferObject
    {
        public string ToJson();

        public string ToJson(Formatting formatting);

        public string ToJson(JsonSerializerSettings settings);

        public string ToJson(Formatting formatting, JsonSerializerSettings settings);
    }
}