    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.Annotations;

    namespace SolutionApi.Models
    {
        public class Voluntario
        {
            [Key]
            [Required(ErrorMessage = "O RG do voluntário é obrigatório.")]
            [Display(Name = "RG", Description = "RG único do voluntário.")]
            [SwaggerSchema(Description = "RG único do voluntário.")]
            public string RG { get; set; }

            [Required(ErrorMessage = "O CPF da pessoa é obrigatório.")]
            [ForeignKey("Pessoa")]
            [Display(Name = "CPF da Pessoa", Description = "CPF da pessoa associada ao voluntário.")]
            [SwaggerSchema(Description = "CPF da pessoa associada ao voluntário.")]
            public string CPF { get; set; }

            [Required(ErrorMessage = "A função é obrigatória.")]
            [MaxLength(100, ErrorMessage = "A função não pode ter mais de 100 caracteres.")]
            [Display(Name = "Função", Description = "Função ou cargo do voluntário.")]
            [SwaggerSchema(Description = "Função ou cargo do voluntário.")]
            public string Funcao { get; set; }

            [ForeignKey("Abrigo")]
            [Required(ErrorMessage = "O nome do abrigo é obrigatório.")]
            [MaxLength(100, ErrorMessage = "O nome do abrigo não pode ter mais de 100 caracteres.")]
            [Display(Name = "Nome do Abrigo", Description = "Nome do abrigo ao qual o voluntário está associado.")]
            [SwaggerSchema(Description = "Nome do abrigo ao qual o voluntário está associado.")]
            public string NomeAbrigo { get; set; }

            // Relacionamentos
            [SwaggerSchema(Description = "Pessoa associada ao voluntário.")]
            public Pessoa Pessoa { get; set; }

            [SwaggerSchema(Description = "Abrigo associado ao voluntário.")]
            public Abrigo Abrigo { get; set; }
        }
    }
