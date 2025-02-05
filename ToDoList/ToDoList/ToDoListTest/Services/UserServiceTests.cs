using Moq;
using ToDoList.Models;
using ToDoList.Repositories.Interfaces;
using ToDoList.Services;
using Microsoft.Extensions.Configuration;
using Task = System.Threading.Tasks.Task;

namespace ToDoListTest.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();

            var jwtSectionMock = new Mock<IConfigurationSection>();
            jwtSectionMock.Setup(x => x["Secret"]).Returns("your-very-strong-256-bit-secret-key-here-256-bits-long");
            jwtSectionMock.Setup(x => x["Issuer"]).Returns("your-app");
            jwtSectionMock.Setup(x => x["Audience"]).Returns("your-app-users");
            jwtSectionMock.Setup(x => x["ExpirationMinutes"]).Returns("60");

            _configurationMock.Setup(x => x.GetSection("JwtSettings")).Returns(jwtSectionMock.Object);

            _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldRegisterUserSuccessfully()
        {
            string email = "test@example.com";
            string password = "StrongPass123!";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
                               .ReturnsAsync((User)null);

            await _userService.RegisterUserAsync(email, password);

            _userRepositoryMock.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == email)), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowExceptionWhenEmailIsInvalid()
        {
            string email = "invalid-email";
            string password = "StrongPass123!";

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegisterUserAsync(email, password));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowExceptionWhenPasswordIsTooShort()
        {
            string email = "test@example.com";
            string password = "short";

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegisterUserAsync(email, password));
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowExceptionWhenUserAlreadyExists()
        {
            string email = "test@example.com";
            string password = "StrongPass123!";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email))
                               .ReturnsAsync(new User { Email = email });

            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RegisterUserAsync(email, password));
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnTokenWhenCredentialsAreValid()
        {
            string email = "test@example.com";
            string password = "ValidPass123!";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { Id = 1, Email = email, PasswordHash = hashedPassword };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);

            var token = await _userService.LoginUserAsync(email, password);

            Assert.NotNull(token);
            Assert.IsType<string>(token);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldThrowUnauthorizedAccessExceptionWhenUserDoesNotExist()
        {
            string email = "test@example.com";
            string password = "ValidPass123!";
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginUserAsync(email, password));
        }

        [Fact]
        public async Task LoginUserAsync_ShouldThrowUnauthorizedAccessExceptionWhenPasswordIsIncorrect()
        {
            string email = "test@example.com";
            string correctPassword = "ValidPass123!";
            string wrongPassword = "WrongPass123!";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);
            var user = new User { Id = 1, Email = email, PasswordHash = hashedPassword };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _userService.LoginUserAsync(email, wrongPassword));
        }

        [Fact]
        public async Task ChangeNameAsync_ShouldThrowArgumentException_WhenNewNameIsNullOrWhitespace()
        {
            int userId = 1;
            string invalidName = "   ";

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.ChangeNameAsync(userId, invalidName));
        }

        [Fact]
        public async Task ChangeNameAsync_ShouldCallRepositoryChangeNameAsync_WhenNewNameIsValid()
        {
            int userId = 1;
            string validName = "NewName";

            _userRepositoryMock.Setup(repo => repo.ChangeNameAsync(userId, validName))
                               .Returns(Task.CompletedTask);

            await _userService.ChangeNameAsync(userId, validName);

            _userRepositoryMock.Verify(repo => repo.ChangeNameAsync(userId, validName), Times.Once);
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldThrowArgumentException_WhenPasswordIsNullOrTooShort()
        {
            int userId = 1;
            string invalidPassword = "short";

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.ChangePasswordAsync(userId, invalidPassword));
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldCallRepositoryChangePasswordAsync_WhenPasswordIsValid()
        {
            int userId = 1;
            string validPassword = "LongEnoughPassword";

            _userRepositoryMock.Setup(repo => repo.ChangePasswordAsync(userId, validPassword))
                               .Returns(Task.CompletedTask);

            await _userService.ChangePasswordAsync(userId, validPassword);

            _userRepositoryMock.Verify(repo => repo.ChangePasswordAsync(userId, validPassword), Times.Once);
        }
    }
}
