using Lab5.Application.Contracts.Users;
using Lab5.Presentation.Scenarios;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Providers;

public class AdminProvider : IScenarioProvider
{
    private readonly IUserService _service;
    private readonly ICurrentUserService _currentUser;

    public AdminProvider(IUserService service, ICurrentUserService currentAdmin)
    {
        _service = service;
        _currentUser = currentAdmin;
    }

    public bool TryGetScenario(
        [NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUser.User is not null)
        {
            scenario = null;
            return false;
        }

        scenario = new AdminScenario(_service);
        return true;
    }
}