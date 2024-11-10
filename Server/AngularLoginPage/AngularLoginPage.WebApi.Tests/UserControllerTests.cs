using AngularLoginPage.Context;
using AngularLoginPage.Domain;
using AngularLoginPage.WebApi.Controllers;
using AngularLoginPage.WebApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AngularLoginPage.WebApi.Tests
{
    public class UserControllerTests
    {

        private UserController CreateController(string? name = null)
        {
            DbContextOptions<AngularLoginPageDbContext> options = new DbContextOptionsBuilder<AngularLoginPageDbContext>()
                .UseInMemoryDatabase(databaseName: name ?? Guid.NewGuid().ToString())
                .Options;
            AngularLoginPageDbContext context = new AngularLoginPageDbContext(options);
            UserController userController = new UserController(context);

            return userController;
        }

        [Theory]
        [InlineData("test@gmail.com", "123qwe", "b5b16571-66d6-4d75-86de-cf8272392440")]
        [InlineData("test@gmail.com", "1q", "b5b16571-66d6-4d75-86de-cf8272392440")]
        [InlineData("test@gmail.com", "q1", "b5b16571-66d6-4d75-86de-cf8272392440")]
        [InlineData("test@gmail.com", "qwe123", "b5b16571-66d6-4d75-86de-cf8272392440")]
        public async Task Register_ValidUser(string login, string password, string provinceId)
        {
            // Arrange
            UserController userController = CreateController();
            UserRegister userRegister = new UserRegister()
            {
                Login = login,
                Password = password,
                ConfirmPassword = password,
                ProvinceId = Guid.Parse(provinceId)
            };

            // Act 
            IActionResult result = await userController.Register(userRegister);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_InvalidProvince()
        {
            // Arrange
            UserController userController = CreateController();
            UserRegister userRegister = new UserRegister()
            {
                Login = "test@gmail.com",
                Password = "123qwe",
                ConfirmPassword = "123qwe",
                ProvinceId = Guid.NewGuid()
            };

            // Act
            IActionResult result = await userController.Register(userRegister);

            // Asserts
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Register_DuplicateUser()
        {
            // Arrange
            UserController userController = CreateController();
            UserRegister userRegister = new UserRegister()
            {
                Login = "test@gmail.com",
                Password = "123qwe",
                ConfirmPassword = "123qwe",
                ProvinceId = Guid.Parse("b5b16571-66d6-4d75-86de-cf8272392440")
            };

            // Act
            IActionResult result1 = await userController.Register(userRegister);
            IActionResult result2 = await userController.Register(userRegister);

            // Asserts
            Assert.IsType<OkObjectResult>(result1);
            Assert.IsType<ConflictObjectResult>(result2);
        }


        [Fact]
        public async Task Register_InvalidLogin()
        {
            // Arrange
            UserController userController = CreateController();
            UserRegister userRegister = new UserRegister()
            {
                Login = "",
                Password = "123qwe",
                ConfirmPassword = "123qwe",
                ProvinceId = Guid.Parse("b5b16571-66d6-4d75-86de-cf8272392440")
            };

            Assert.ThrowsAsync<DbUpdateException>(async () => await userController.Register(userRegister));
        }
    }
}