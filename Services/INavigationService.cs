namespace CalculateurAge.Services;

public interface INavigationService
{
    Task AfficherResultatAsync(string message);

    Task RetournerAsync();
}

public static class NavigationRoutes
{
    public const string Resultat = "ResultatPage";

    public const string MessageParameter = "message";
}