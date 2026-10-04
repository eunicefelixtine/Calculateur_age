using CalculateurAge.Services;
using CalculateurAge.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CalculateurAge;

public partial class AppShell : Shell
{
    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPageShellContent.ContentTemplate = new DataTemplate(
            () => serviceProvider.GetRequiredService<MainPage>());

        Routing.RegisterRoute(
            NavigationRoutes.Resultat,
            typeof(ResultatPage));
    }
}
