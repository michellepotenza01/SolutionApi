using System.Text.Json.Serialization;

namespace SolutionApi.Enums
{
    /// <summary>
    /// Enum para representar o tamanho do abrigo.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TamanhoAbrigo
    {
        /// <summary>
        /// Tamanho pequeno do abrigo.
        /// </summary>
        Pequeno,

        /// <summary>
        /// Tamanho médio do abrigo.
        /// </summary>
        Medio,

        /// <summary>
        /// Tamanho grande do abrigo.
        /// </summary>
        Grande
    }
}
