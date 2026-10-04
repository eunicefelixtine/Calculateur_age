
using CalculateurAge.Views;
namespace CalculateurAge;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCalculerClicked(object sender, EventArgs e)
{
    // Validation : on refuse un nom vide.
    if (string.IsNullOrWhiteSpace(entryNom.Text))
    {
        DisplayAlert("Erreur", "Entrez un nom", "OK");
        return;
    }

    DateTime d = pickerDate.Date;

    int age = DateTime.Today.Year - d.Year;

    // Si l'anniversaire n'est pas encore passé cette année,
    // on retire une année.
    if (d.Date > DateTime.Today.AddYears(-age))
        age--;

    await Shell.Current.GoToAsync(
    $"{nameof(ResultatPage)}?nom={entryNom.Text}&age={age}");
}
}