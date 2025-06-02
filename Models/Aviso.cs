using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SolutionApi.Enums;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Models
{
    public class Aviso
    {
        [Key]
        [Required(ErrorMessage = "O tipo de aviso é obrigatório.")]
        [Display(Name = "Tipo de Aviso", Description = "Tipo do evento climático (Ex: Alagamento, Calor, etc.).")]
        [SwaggerSchema(Description = "Tipo de aviso (ex: Alagamento, Calor, etc.).")]
        public string TipoAviso { get; set; }

        [Required(ErrorMessage = "A ocorrência do evento climático é obrigatória.")]
        [MaxLength(500, ErrorMessage = "A descrição da ocorrência não pode ter mais de 500 caracteres.")]
        [Display(Name = "Ocorrência", Description = "Descrição detalhada do evento climático ocorrido.")]
        [SwaggerSchema(Description = "Descrição detalhada do evento climático ocorrido.")]
        public string Ocorrencia { get; set; }

        [Required(ErrorMessage = "A gravidade é obrigatória.")]
        [EnumDataType(typeof(Gravidade))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Gravidade", Description = "Gravidade do evento climático (Alta, Média ou Baixa).")]
        [SwaggerSchema(Description = "Gravidade do evento (Alta, Média ou Baixa).")]
        public Gravidade Gravidade { get; set; }

        [Required(ErrorMessage = "O bairro afetado é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro(s) afetado(s) pelo evento climático.")]
        [SwaggerSchema(Description = "Bairro(s) afetado(s) pelo evento climático.")]
        public string Bairro { get; set; }

        // Mantendo o relacionamento com o Bairro de forma implícita
        // Aviso pode ter um Bairro, e o Bairro pode ter muitos Avisos (relacionamento 1:N) 
    }
}
