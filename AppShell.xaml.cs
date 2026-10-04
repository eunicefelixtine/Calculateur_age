using CalculateurAge.Views;
namespace CalculateurAge;

public partial class AppShell : Shell
{
	public AppShell()
{
    InitializeComponent();

    // Déclare la route.
    Routing.RegisterRoute(
        nameof(ResultatPage),
        typeof(ResultatPage));
}
}
