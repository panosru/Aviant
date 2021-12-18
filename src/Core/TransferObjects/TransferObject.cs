namespace Aviant.DDD.Core.TransferObjects;

using Newtonsoft.Json;

public abstract record TransferObject : ITransferObject
{
    #region ITransferObject Members

    public string ToJson() => JsonConvert.SerializeObject(this);

    public string ToJson(Formatting formatting) => JsonConvert.SerializeObject(this, formatting);

    public string ToJson(JsonSerializerSettings settings) => JsonConvert.SerializeObject(this, settings);

    public string ToJson(Formatting formatting, JsonSerializerSettings settings) =>
        JsonConvert.SerializeObject(this, formatting, settings);

    #endregion
}
