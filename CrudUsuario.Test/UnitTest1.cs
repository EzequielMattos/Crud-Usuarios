using CrudUsuario.Application.Services;
using CrudUsuario.Data;
using CrudUsuario.Domain.Models;
using CrudUsuario.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using Xunit;

namespace CrudUsuario.Test
{
    [TestClass]
    public class UnitTest1
    {
        Context? context; 
        Mock<UserManager<IdentityUser>> mockUserManager;
        Mock<SignInManager<IdentityUser>> mockSignInManager;
        Mock<IOptions<JwtOptions>> optionsMock;
        SigningCredentials signingCredentials;

        //public IdentityServiceTests()
        //{
        //    // Arrange
        //    mockUserManager = new Mock<UserManager<IdentityUser>>(
        //        Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

        //    mockSignInManager = new Mock<SignInManager<IdentityUser>>(
        //        mockUserManager.Object,
        //        Mock.Of<IHttpContextAccessor>(),
        //        Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);

        //    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-super-secreta"));
        //    signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        //    optionsMock = new Mock<IOptions<JwtOptions>>();
        //    optionsMock.Setup(x => x.Value).Returns(new JwtOptions("Issuer", "Audience", signingCredentials, 5000));
        //}

        [Fact]
        public async Task CadastrarUsuario_SucessoEsperado()
        {
            //Arrange
            mockUserManager
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var jwtOptions = optionsMock.Object;
            var service = new UsuarioService(context, mockSignInManager.Object, mockUserManager.Object, jwtOptions);

            var usuarioViewModel = new UsuarioViewModel("teste", "teste@email.com", "1234567891", DateTime.Parse("30/03/2023"), "Senha@123");

            // Act
            var result = await service.Create(usuarioViewModel);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(usuarioViewModel.Notifications.Count == 0);
        }

        [Fact]
        public async Task CadastrarUsuario_ErroEsperado_EmailInvalido()
        {
            var jwtOptions = optionsMock.Object;
            var service = new UsuarioService(context, mockSignInManager.Object, mockUserManager.Object, jwtOptions);

            var usuarioViewModel = new UsuarioViewModel("teste", "testeemail.com", "1234567891", DateTime.Parse("30/03/2023"), "Senha@123");

            mockUserManager
                .Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User creation failed" }));

            // Act
            var result = await service.Create(usuarioViewModel);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(usuarioViewModel.Notifications.Count > 0);
        }

        [Fact]
        public async Task Login_FalhaEsperada()
        {
            var jwtOptions = optionsMock.Object;
            var service = new UsuarioService(context, mockSignInManager.Object, mockUserManager.Object, jwtOptions);

            var usuarioLoginViewModel = new UsuarioLoginViewModel("teste@email.com", "Senha@123");

            mockSignInManager
                .Setup(um => um.PasswordSignInAsync(usuarioLoginViewModel.Email, usuarioLoginViewModel.Password, false, true))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await service.Login(usuarioLoginViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Sucesso);
            Assert.IsTrue(result.Erros.Count > 0);
            Assert.IsTrue(usuarioLoginViewModel.Notifications.Count == 0);
        }
    }
}