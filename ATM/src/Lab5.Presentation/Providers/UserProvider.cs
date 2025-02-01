using Lab5.Application.Contracts.Users;
using Lab5.Presentation.Scenarios;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Providers;

public class UserProvider : IScenarioProvider
{
    private readonly IUserService _service;
    private readonly ICurrentUserService _currentUser;

    public UserProvider(
        IUserService service,
        ICurrentUserService currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    public bool TryGetScenario(
        [NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUser.User is not null)
        {
            scenario = null;
            return false;
        }

        scenario = new UserScenario(_service);
        return true;
    }
}