using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SolutionApi.Enums;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Models
{
    /// <summary>
    /// Representa um aviso de evento climático (como Alagamento, Calor, etc.).
    /// </summary>
    public class Aviso
    {
        /// <summary>
        /// Tipo do evento climático (ex: Alagamento, Calor, etc.).
        /// </summary>
        [Key]
        [Required(ErrorMessage = "O tipo de aviso é obrigatório.")]
        [Display(Name = "Tipo de Aviso", Description = "Tipo do evento climático (Ex: Alagamento, Calor, etc.).")]
        [SwaggerSchema(Description = "Tipo de aviso (ex: Alagamento, Calor, etc.).")]
        public required string TipoAviso { get; set; }

        /// <summary>
        /// Descrição detalhada da ocorrência do evento climático.
        /// </summary>
        [Required(ErrorMessage = "A ocorrência do evento climático é obrigatória.")]
        [MaxLength(500, ErrorMessage = "A descrição da ocorrência não pode ter mais de 500 caracteres.")]
        [Display(Name = "Ocorrência", Description = "Descrição detalhada do evento climático ocorrido.")]
        [SwaggerSchema(Description = "Descrição detalhada do evento climático ocorrido.")]
        public required string Ocorrencia { get; set; }

        /// <summary>
        /// A gravidade do evento climático (Alta, Média ou Baixa).
        /// </summary>
        [Required(ErrorMessage = "A gravidade é obrigatória.")]
        [EnumDataType(typeof(Gravidade))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Gravidade", Description = "Gravidade do evento climático (Alta, Media ou Baixa).")]
        [SwaggerSchema(Description = "Gravidade do evento (Alta, Média ou Baixa).")]
        public Gravidade Gravidade { get; set; }

        /// <summary>
        /// Bairro(s) afetado(s) pelo evento climático.
        /// </summary>
        [Required(ErrorMessage = "O bairro afetado é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro(s) afetado(s) pelo evento climático.")]
        [SwaggerSchema(Description = "Bairro(s) afetado(s) pelo evento climático.")]
        public required string Bairro { get; set; }
    }

}
