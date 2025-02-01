using Lab5.Application.Models.Users;

namespace Lab5.Application.Contracts.Users;

public interface ICurrentUserService
{
    User? User { get; }
}