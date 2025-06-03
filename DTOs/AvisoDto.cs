using System.ComponentModel.DataAnnotations;
using SolutionApi.Enums;

namespace SolutionApi.DTOs
{
    /// <summary>
    /// DTO para representar um aviso de evento climático.
    /// </summary>
    public class AvisoDto
    {
        /// <summary>
        /// Tipo do evento climático (Ex: Alagamento, Calor, etc.).
        /// </summary>
        [Required(ErrorMessage = "O tipo de aviso é obrigatório.")]
        [Display(Name = "Tipo de Aviso", Description = "Tipo do evento climático (Ex: Alagamento, Calor, etc.).")]
        public string TipoAviso { get; set; }

        /// <summary>
        /// Descrição detalhada do evento climático ocorrido.
        /// </summary>
        [Required(ErrorMessage = "A ocorrência do evento climático é obrigatória.")]
        [MaxLength(500, ErrorMessage = "A descrição da ocorrência não pode ter mais de 500 caracteres.")]
        [Display(Name = "Ocorrência", Description = "Descrição detalhada do evento climático ocorrido.")]
        public string Ocorrencia { get; set; }

        /// <summary>
        /// Gravidade do evento climático (Alta, Média ou Baixa).
        /// </summary>
        [Required(ErrorMessage = "A gravidade é obrigatória.")]
        [EnumDataType(typeof(Gravidade))]
        [Display(Name = "Gravidade", Description = "Gravidade do evento climático (Alta, Média ou Baixa).")]
        public Gravidade Gravidade { get; set; }

        /// <summary>
        /// Bairro(s) afetado(s) pelo evento climático.
        /// </summary>
        [Required(ErrorMessage = "O bairro afetado é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro(s) afetado(s) pelo evento climático.")]
        public string Bairro { get; set; }
    }
}
