using System.Text.Json.Serialization;

namespace SolutionApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TamanhoAbrigo
    {
        Pequeno,
        Medio,
        Grande
    }
}
