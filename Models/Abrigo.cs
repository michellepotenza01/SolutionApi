using System.ComponentModel.DataAnnotations;
using SolutionApi.Enums; // Enum TamanhoAbrigo
using System.Text.Json.Serialization;

namespace SolutionApi.Models
{
    public class Abrigo
    {
        [Key]
        [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
        [Display(Name = "Nome do Abrigo", Description = "Nome único do abrigo.")]
        public string NomeAbrigo { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [Display(Name = "Bairro", Description = "Bairro onde o abrigo está localizado.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O tamanho do abrigo é obrigatório.")]
        [EnumDataType(typeof(TamanhoAbrigo))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Display(Name = "Tamanho do Abrigo", Description = "Tamanho do abrigo (Pequeno, Médio ou Grande).")]
        public TamanhoAbrigo Tamanho { get; set; }

        public ICollection<Voluntario> Voluntarios { get; set; }
    }
}
