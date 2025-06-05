using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SolutionApi.Enums;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Models
{

    /// <summary>
    /// Representa um abrigo onde as pessoas podem se abrigar durante eventos climáticos extremos.
    /// </summary>
    public class Abrigo
    {
        /// <summary>
        /// Nome único do abrigo.
        /// </summary>
        [Key]
        [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
        [Display(Name = "Nome do Abrigo", Description = "Nome único do abrigo.")]
        [SwaggerSchema(Description = "Nome único do abrigo.")]
        public required string NomeAbrigo { get; set; }

        /// <summary>
        /// Bairro onde o abrigo está localizado.
        /// </summary>
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [Display(Name = "Bairro", Description = "Bairro onde o abrigo está localizado.")]
        [SwaggerSchema(Description = "Bairro onde o abrigo está localizado.")]
        public required string Bairro { get; set; }

        /// <summary>
        /// Tamanho do abrigo (Pequeno, Médio ou Grande).
        /// </summary>
        [Required(ErrorMessage = "O tamanho do abrigo é obrigatório.")]
        [EnumDataType(typeof(TamanhoAbrigo))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Tamanho do Abrigo", Description = "Tamanho do abrigo (Pequeno, Medio ou Grande).")]
        [SwaggerSchema(Description = "Tamanho do abrigo: Pequeno, Medio ou Grande.")]
        public TamanhoAbrigo Tamanho { get; set; }

        /// <summary>
        /// Lista de voluntários associados ao abrigo.
        /// </summary>
        [SwaggerSchema(Description = "Lista de voluntários associados ao abrigo.")]
        public ICollection<Voluntario> Voluntarios { get; set; }
    }

}
