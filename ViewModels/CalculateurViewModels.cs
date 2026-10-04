namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";

    private DateTime _dateNaissance =
        DateTime.Today.AddYears(-20);

    private string _resultat = "";

    private string _messageAge = "";

    private bool _resultatVisible;

    public string Nom
    {
        get => _nom;

        set
        {
            if (SetField(ref _nom, value))
                CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;

        set => SetField(
            ref _dateNaissance,
             value);
    }

    public string Resultat
    {
        get => _resultat;

        set => SetField(
            ref _resultat,
            value);
    }

    public string MessageAge
    {
        get => _messageAge;

        set => SetField(
            ref _messageAge,
            value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;

        set => SetField(
            ref _resultatVisible,
            value);
    }

    public RelayCommand CalculerCommand { get; }

    public RelayCommand EffacerCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));

        EffacerCommand = new RelayCommand(Effacer);
    }

    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        MessageAge = "";
        ResultatVisible = false;
    }

    // Logique métier : aucun contrôle d'interface.
    private void Calculer()
     {
        int age = DateTime.Today.Year - DateNaissance.Year;

        if (DateNaissance.Date >
            DateTime.Today.AddYears(-age))
        {
            age--;
        }

        MessageAge = age >= 18 ? "Majeur" : "Mineur";
        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
    }
}