using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SolutionApi.Enums;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Models
{
    public class Abrigo
    {
        [Key]
        [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
        [Display(Name = "Nome do Abrigo", Description = "Nome único do abrigo.")]
        [SwaggerSchema(Description = "Nome único do abrigo.")]
        public string NomeAbrigo { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [Display(Name = "Bairro", Description = "Bairro onde o abrigo está localizado.")]
        [SwaggerSchema(Description = "Bairro onde o abrigo está localizado.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O tamanho do abrigo é obrigatório.")]
        [EnumDataType(typeof(TamanhoAbrigo))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Tamanho do Abrigo", Description = "Tamanho do abrigo (Pequeno, Médio ou Grande).")]
        [SwaggerSchema(Description = "Tamanho do abrigo: Pequeno, Médio ou Grande.")]
        public TamanhoAbrigo Tamanho { get; set; }

        // Relacionamento entre Abrigo e Voluntários
        [SwaggerSchema(Description = "Lista de voluntários associados ao abrigo.")]
        public ICollection<Voluntario> Voluntarios { get; set; }
    }
}
