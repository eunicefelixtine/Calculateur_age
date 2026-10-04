using Microsoft.Maui.Controls;
namespace CalculateurAge.Views;

// Relie les paramètres "nom" et "age" de l'URL
// aux propriétés publiques Nom et Age.
[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
public partial class ResultatPage : ContentPage
{
    // Ces propriétés sont remplies par la navigation,
    // APRÈS le constructeur.
    public string Nom { get; set; }
    public string Age { get; set; }

    public ResultatPage()
    {
        InitializeComponent();
    }

    // Appelé à CHAQUE affichage de la page.
    protected override void OnAppearing()
    {
        base.OnAppearing();

        lblMessage.Text = $"{Nom}, vous avez {Age} ans";
    }

    // ".." = revenir à la page précédente.
    private async void OnRetourClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}