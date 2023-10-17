using CrudUsuario.Data;
using CrudUsuario.Domain.Interfaces;
using CrudUsuario.Domain.Models;
using CrudUsuario.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SecureIdentity.Password;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrudUsuario.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly Context _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        public UsuarioService(Context context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAll(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            return await _context.Usuarios.AsNoTracking()
                .Skip(skip)
                .Take(take)
                .Select(u => new UsuarioViewModel(u.Nome, u.Email, u.Cpf, u.DataNascimento, u.Password))
                .ToListAsync();
                ;
        }

        public async Task<UsuarioViewModel> GetById(int id)
        {
            return await _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new UsuarioViewModel(u.Nome, u.Email, u.Cpf, u.DataNascimento, u.Password))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<UsuarioViewModel> Create(UsuarioViewModel uvm)
        {
            try
            {
                var password = PasswordHasher.Hash(uvm.Password);
                Usuario usuario = new Usuario
                {
                    Nome = uvm.Nome,
                    Email = uvm.Email,
                    Cpf = uvm.Cpf,
                    DataNascimento = uvm.DataNascimento,
                    Password = password
                };

                await _context.Usuarios.AddAsync(usuario);
                int rows = await _context.SaveChangesAsync();

                if (rows > 0)
                    return uvm;

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UsuarioViewModel> Update(int id, UsuarioViewModel uvm)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                    return null;

                var password = PasswordHasher.Hash(uvm.Password);

                usuario.Nome = uvm.Nome;
                usuario.Email = uvm.Email;
                usuario.Cpf = uvm.Cpf;
                usuario.DataNascimento = uvm.DataNascimento;
                usuario.Password = password;

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                return uvm;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                    return false;

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return true;
            }
            catch 
            {
                return false;
            } 
        }

        public async Task<UsuarioLoginResponseViewModel> Login(UsuarioLoginViewModel request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

            if (result.Succeeded)
                return await GerarToken(request.Email);

            UsuarioLoginResponseViewModel response = new(result.Succeeded);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    response.AdicionarErro("Conta bloqueada");
                else if (result.IsNotAllowed)
                    response.AdicionarErro("Conta não autorizada");
                else
                    response.AdicionarErro("Erro ao fazer login. Revise os campos");
            };

            return response;
        }

        private async Task<UsuarioLoginResponseViewModel> GerarToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new NullReferenceException(nameof(user));

            var tokenClaims = await ObterClaims(user);
            var dataExpiracao = DateTime.Now.AddHours(_jwtOptions.Expiration);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: tokenClaims,
                notBefore: DateTime.Now,
                expires: dataExpiracao,
                signingCredentials: _jwtOptions.SigningCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new UsuarioLoginResponseViewModel(
                sucesso: true,
                token: token,
                dataExpiracao: dataExpiracao);
        }

        private async Task<IList<Claim>> ObterClaims(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            foreach (var role in roles)
                claims.Add(new Claim("role", role));

            return claims;
        }
    }
}
