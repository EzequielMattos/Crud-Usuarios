using CrudUsuario.Domain.Models;
using CrudUsuario.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudUsuario.Domain.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioViewModel>> GetAll(int page, int pageSize);
        Task<UsuarioViewModel> GetById(int id);
        Task<UsuarioViewModel> Create(UsuarioViewModel uvm);
        Task<UsuarioViewModel> Update(int id, UsuarioViewModel uvm);
        Task<bool> Delete(int id);
        Task<UsuarioLoginResponseViewModel> Login(UsuarioLoginViewModel request);
    }
}
