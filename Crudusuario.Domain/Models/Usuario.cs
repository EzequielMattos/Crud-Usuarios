using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudUsuario.Domain.Models
{
    public class Usuario
    {
        public Usuario()
        {
        }

        public Usuario(int id, string nome, string email, string cpf, DateTime dataNascimento, string password)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Password = password;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Cpf { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
