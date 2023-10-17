using CrudUsuario.Domain.Interfaces;
using CrudUsuario.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace CrudUsuario.Endpoints
{
    public static class UsuarioEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            app.MapGet("/v1/usuarios", async (int page, int pageSize, IUsuarioService _service) =>
            {
                if (page <= 0 || pageSize <= 0)
                    return Results.BadRequest("Parametros de paginação inválidos");

                var result = await _service.GetAll(page, pageSize);

                return Results.Ok(result);
            });

            app.MapGet("/v1/usuarios/{id}", [Authorize] async ([FromQuery] int id, IUsuarioService _service) => 
            {
                var usuario = await _service.GetById(id);

                if (usuario == null)
                    return Results.NotFound("Nenhum usuário foi encontrado!");

                return Results.Ok(usuario);
            });

            app.MapPost("/v1/usuarios", [Authorize] async (UsuarioViewModel uvm, IUsuarioService _service) => {
                if (uvm.IsValid)
                {
                    var result = await _service.Create(uvm);
                    if (result == null)
                        return Results.BadRequest();

                    return Results.Ok(result);
                }
                return Results.BadRequest(uvm.Notifications);
            });

            app.MapPut("/v1/usuarios/{id}", [Authorize] async ([FromQuery] int id, UsuarioViewModel uvm, IUsuarioService _service) =>
            {
                if (uvm.IsValid)
                {
                    var result = await _service.Update(id, uvm);
                    if (result == null)
                        return Results.BadRequest();

                    return Results.Ok(result);
                }
                return Results.BadRequest(uvm.Notifications);
            });

            app.MapDelete("/v1/usuarios/{id}", [Authorize] async (int id, IUsuarioService _service) =>
            {
                var result = await _service.Delete(id);
                if (result)
                    return Results.Ok(result);

                return Results.BadRequest();
            });

            app.MapPost("/v1/usuario/login", async (IUsuarioService _service, UsuarioLoginViewModel uvm) =>
            {
                if (uvm.IsValid)
                {
                    var result = await _service.Login(uvm);

                    if (result.Sucesso)
                        return Results.Ok(result);
                    else if (result.Erros.Count > 0)
                        return Results.BadRequest(result);
                }

                return Results.BadRequest(uvm.Notifications);
            });
        }
    }
}
