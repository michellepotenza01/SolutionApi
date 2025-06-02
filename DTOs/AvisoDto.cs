using System.ComponentModel.DataAnnotations;
using SolutionApi.Enums;

namespace SolutionApi.DTOs
{
    public class AvisoDto
    {
        [Required(ErrorMessage = "O tipo de aviso é obrigatório.")]
        [Display(Name = "Tipo de Aviso", Description = "Tipo do evento climático (Ex: Alagamento, Calor, etc.).")]
        public string TipoAviso { get; set; }

        [Required(ErrorMessage = "A ocorrência do evento climático é obrigatória.")]
        [MaxLength(500, ErrorMessage = "A descrição da ocorrência não pode ter mais de 500 caracteres.")]
        [Display(Name = "Ocorrência", Description = "Descrição detalhada do evento climático ocorrido.")]
        public string Ocorrencia { get; set; }

        [Required(ErrorMessage = "A gravidade é obrigatória.")]
        [EnumDataType(typeof(Gravidade))]
        [Display(Name = "Gravidade", Description = "Gravidade do evento climático (Alta, Média ou Baixa).")]
        public Gravidade Gravidade { get; set; }

        [Required(ErrorMessage = "O bairro afetado é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro(s) afetado(s) pelo evento climático.")]
        public string Bairro { get; set; }
    }
}
