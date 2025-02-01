using Lab5.Application.Contracts.Users;
using Lab5.Presentation.Scenarios;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Providers;

public class GetMoneyProvider : IScenarioProvider
{
    private readonly IUserService _service;
    private readonly ICurrentUserService _currentUser;

    public GetMoneyProvider(
        IUserService service,
        ICurrentUserService currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    public bool TryGetScenario(
        [NotNullWhen(true)] out IScenario? scenario)
    {
        if (_currentUser.User is null || _currentUser.User.Id == -1)
        {
            scenario = null;
            return false;
        }

        scenario = new GetMoneyScenario(_service);
        return true;
    }
}