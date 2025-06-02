using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolutionApi.DTOs
{
    public class VoluntarioDto
    {
        [Required(ErrorMessage = "O RG do voluntário é obrigatório.")]
        [Display(Name = "RG", Description = "RG único do voluntário.")]
        public string RG { get; set; }

        [Required(ErrorMessage = "O CPF da pessoa é obrigatório.")]
        [Display(Name = "CPF da Pessoa", Description = "CPF da pessoa associada ao voluntário.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "A função é obrigatória.")]
        [MaxLength(100, ErrorMessage = "A função não pode ter mais de 100 caracteres.")]
        [Display(Name = "Função", Description = "Função ou cargo do voluntário.")]
        public string Funcao { get; set; }

        [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome do abrigo não pode ter mais de 100 caracteres.")]
        [Display(Name = "Nome do Abrigo", Description = "Nome do abrigo ao qual o voluntário está associado.")]
        public string NomeAbrigo { get; set; }
    }
}
