using System.Text.Json.Serialization;

namespace SolutionApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gravidade
    {
        Alta,
        Media,
        Baixa
    }
}
