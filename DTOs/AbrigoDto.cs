using System.ComponentModel.DataAnnotations;
using SolutionApi.Enums;

namespace SolutionApi.DTOs
{
    /// <summary>
    /// DTO para representar o abrigo.
    /// </summary>
    public class AbrigoDto
    {
        /// <summary>
        /// Nome único do abrigo.
        /// </summary>
        [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do abrigo não pode ter mais de 100 caracteres.")]
        [Display(Name = "Nome do Abrigo", Description = "Nome único do abrigo.")]
        public required string NomeAbrigo { get; set; }

        /// <summary>
        /// Bairro onde o abrigo está localizado.
        /// </summary>
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro onde o abrigo está localizado.")]
        public required string Bairro { get; set; }

        /// <summary>
        /// Tamanho do abrigo (Pequeno, Médio ou Grande).
        /// </summary>
        [Required(ErrorMessage = "O tamanho do abrigo é obrigatório.")]
        [EnumDataType(typeof(TamanhoAbrigo))]
        [Display(Name = "Tamanho do Abrigo", Description = "Tamanho do abrigo (Pequeno, Médio ou Grande).")]
        public TamanhoAbrigo Tamanho { get; set; }

        /// <summary>
        /// Lista de voluntarios associados ao abrigo
        /// </summary>
        public List<VoluntarioDto> Voluntarios { get; internal set; }
    }
}
