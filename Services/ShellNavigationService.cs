using CalculateurAge.Views;

namespace CalculateurAge.Services;

public class ShellNavigationService : INavigationService
{
    public Task AfficherResultatAsync(string message)
    {
        return Shell.Current.GoToAsync(
            NavigationRoutes.Resultat,
            new Dictionary<string, object>
            {
                [NavigationRoutes.MessageParameter] = message
            });
    }

    public Task RetournerAsync()
    {
        return Shell.Current.GoToAsync("..");
    }
}