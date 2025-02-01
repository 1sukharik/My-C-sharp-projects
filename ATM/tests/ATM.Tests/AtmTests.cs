using Lab5.Application.Abstractions.Repositories;
using Lab5.Application.Models.Users;
using Lab5.Application.Users;
using Moq;
using Xunit;

namespace Lab5.Tests;

public class AtmTests
{
    [Fact]
    public void ShouldPutAmountOnAccountAndSaveCorrectBalance_WhenAllOk_ReturnsTrue()
    {
        // Arrange
        var repoMok = new Mock<IUserRepository>();
        repoMok.Setup(m => m.FindUser("Bob", "12")).Returns(new User(1, "Bob", 52, "12"));
        repoMok.Setup(m => m.SetMoney(
            It.Is<User>(user =>
            user.Id == 1),
            "Bob",
            "12",
            53)).Returns(new User(1, "Bob", 53, "12"));
        var manager = new CurrentUserManager();

        // Act
        manager.User = new User(1, "Bob", 52, "12");
        var service = new UserService(repoMok.Object, manager);

        // Assert
        Assert.Equal(53, service.SetMoney("Bob", "12", 1).Balance);
    }

    [Fact]
    public void ShouldGetAmountOnAccountAndSaveCorrectBalance_WhenAllOk_ReturnsTrue()
    {
        // Arrange
        var repoMok = new Mock<IUserRepository>();
        repoMok.Setup(m => m.FindUser("Bob", "12")).Returns(new User(1, "Bob", 52, "12"));
        repoMok.Setup(m => m.GetMoney(
            It.Is<User>(user =>
                user.Id == 1),
            "Bob",
            "12",
            51)).Returns(new User(1, "Bob", 51, "12"));
        var manager = new CurrentUserManager();

        // Act
        manager.User = new User(1, "Bob", 52, "12");
        var service = new UserService(repoMok.Object, manager);

        // Assert
        Assert.Equal(51, service.GetMoney("Bob", "12", 1).Balance);
    }

    [Fact]
    public void ShouldGetAmountOnAccountAndSaveCorrectBalance_WhenAccountDoesntHaveEnoughMoney_ReturnsError()
    {
        // Arrange
        var repoMok = new Mock<IUserRepository>();
        repoMok.Setup(m => m.FindUser("Bob", "12")).Returns(new User(1, "Bob", 52, "12"));
        repoMok.Setup(m => m.GetMoney(
            It.Is<User>(user =>
                user.Id == 1),
            "Bob",
            "12",
            100)).Throws(new Exception("You can't get more than your balance."));

        var service = new UserService(repoMok.Object, new CurrentUserManager());

        // Act
        Exception exception = Assert.Throws<Exception>(() => service.GetMoney("Bob", "12", 100));

        // Assert
        Assert.Equal("You can't get more than your balance.", exception.Message);
    }
}