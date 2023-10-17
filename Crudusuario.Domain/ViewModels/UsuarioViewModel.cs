using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudUsuario.Domain.ViewModels
{
    public class UsuarioViewModel : Notifiable<Notification>
    {
        public UsuarioViewModel(string nome, string email, string cpf, DateTime dataNascimento, string password)
        {
            Nome = nome;
            Email = email;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Password = password;
        }

        [Required(ErrorMessage = "O campo nome é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Limite de caracteres ultrapassados(100)!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório!")]
        [MaxLength(100, ErrorMessage = "Limite de caracteres ultrapassados(100)!")]
        [EmailAddress(ErrorMessage = "O e-mail inserido não é válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório!")]
        [MaxLength(20, ErrorMessage = "Limite de caracteres ultrapassados(20)!")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo Data de nascimento é obrigatório!")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        [MinLength(8, ErrorMessage = "A senha deve ter mais de 8 caracteres!")]
        public string Password { get; set; }
    }
}
