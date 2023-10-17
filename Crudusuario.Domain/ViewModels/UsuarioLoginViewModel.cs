using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrudUsuario.Domain.ViewModels
{
    public class UsuarioLoginViewModel : Notifiable<Notification>
    {
        public UsuarioLoginViewModel(string email, string senha)
        {
            Email = email;
            Password = senha;
        }

        [Required(ErrorMessage = "O campo E-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "O e-mail inserido não é válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório!")]
        public string Password { get; set; }
    }
}
