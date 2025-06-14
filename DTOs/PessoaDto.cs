﻿using System.ComponentModel.DataAnnotations;

namespace SolutionApi.DTOs
{
    /// <summary>
    /// DTO para representar uma pessoa.
    /// </summary>
    public class PessoaDto
    {
        /// <summary>
        /// Nome completo da pessoa.
        /// </summary>
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome completo deve conter pelo menos dois nomes.")]
        [Display(Name = "Nome", Description = "Nome completo da pessoa.")]
        public required string Nome { get; set; }

        /// <summary>
        /// Idade da pessoa.
        /// </summary>
        [Required(ErrorMessage = "A idade é obrigatória.")]
        [Range(16, 99, ErrorMessage = "A idade deve ser maior que 15 e ter exatamente 2 dígitos.")]
        [Display(Name = "Idade", Description = "Idade da pessoa.")]
        public int Idade { get; set; }

        /// <summary>
        /// Bairro de residência da pessoa.
        /// </summary>
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O bairro não pode ter mais de 100 caracteres.")]
        [Display(Name = "Bairro", Description = "Bairro de residência da pessoa.")]
        public required string Bairro { get; set; }

        /// <summary>
        /// Indica se a pessoa possui deficiência (PCD).
        /// </summary>
        [Required(ErrorMessage = "O campo PCD é obrigatório.")]
        [RegularExpression(@"^(Sim|Não)$", ErrorMessage = "O campo PCD deve ser 'Sim' ou 'Não'.")]
        [Display(Name = "PCD", Description = "Indica se a pessoa possui deficiência (PCD).")]
        public required string PCD { get; set; }

        /// <summary>
        /// Senha da pessoa.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [Display(Name = "Senha", Description = "Senha da pessoa.")]
        public required string Senha { get; set; }

        /// <summary>
        /// CPF único da pessoa.
        /// </summary>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 dígitos.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "O CPF deve conter apenas números.")]
        [Display(Name = "CPF", Description = "CPF único da pessoa.")]
        public required string CPF { get; set; }

        /// <summary>
        /// Carreira ou profissão da pessoa.
        /// </summary>
        [Required(ErrorMessage = "A carreira é obrigatória.")]
        [Display(Name = "Carreira", Description = "Carreira ou profissão da pessoa.")]
        public required string Carreira { get; set; }
    }
}
