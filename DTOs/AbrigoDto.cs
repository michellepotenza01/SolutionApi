using System.ComponentModel.DataAnnotations;
using SolutionApi.Enums;

namespace SolutionApi.DTOs
{
    public class AbrigoDto
    {
        [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do abrigo não pode ter mais de 100 caracteres.")]
        [Display(Name = "Nome do Abrigo", Description = "Nome único do abrigo.")]
        public string NomeAbrigo { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro onde o abrigo está localizado.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O tamanho do abrigo é obrigatório.")]
        [EnumDataType(typeof(TamanhoAbrigo))]
        [Display(Name = "Tamanho do Abrigo", Description = "Tamanho do abrigo (Pequeno, Médio ou Grande).")]
        public TamanhoAbrigo Tamanho { get; set; }
    }
}
