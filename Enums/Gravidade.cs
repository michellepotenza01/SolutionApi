using System.Text.Json.Serialization;

namespace SolutionApi.Enums
{
    /// <summary>
    /// Enum para representar a gravidade do evento climático.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gravidade
    {
        /// <summary>
        /// Evento climático com alta gravidade.
        /// </summary>
        Alta,

        /// <summary>
        /// Evento climático com gravidade média.
        /// </summary>
        Media,

        /// <summary>
        /// Evento climático com baixa gravidade.
        /// </summary>
        Baixa
    }
}
