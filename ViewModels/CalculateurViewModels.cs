using System.Collections.ObjectModel;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";

    private DateTime _dateNaissance =
        DateTime.Today.AddYears(-20);

    private string _resultat = "";

    private string _messageAge = "";

    private string _messageAnniversaire = "";

    private bool _resultatVisible;

    public ObservableCollection<string> HistoriqueCalculs { get; } = new();

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

        set
        {
            if (SetField(ref _dateNaissance, value))
                CalculerCommand.Rafraichir();
        }
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

    public string MessageAnniversaire
    {
        get => _messageAnniversaire;

        set => SetField(
            ref _messageAnniversaire,
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
            () => !string.IsNullOrWhiteSpace(Nom)
                && DateNaissance.Date <= DateTime.Today);

        EffacerCommand = new RelayCommand(Effacer);
    }

    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        MessageAge = "";
        MessageAnniversaire = "";
        ResultatVisible = false;
    }

    // Logique métier : aucun contrôle d'interface.
    private void Calculer()
    {
        if (DateNaissance.Date > DateTime.Today)
            return;

        int age = DateTime.Today.Year - DateNaissance.Year;

        if (DateNaissance.Date >
            DateTime.Today.AddYears(-age))
        {
            age--;
        }

        DateTime aujourdHui = DateTime.Today;
        DateTime prochainAnniversaire = DateNaissance
            .AddYears(aujourdHui.Year - DateNaissance.Year)
            .Date;

        if (prochainAnniversaire < aujourdHui)
            prochainAnniversaire = prochainAnniversaire.AddYears(1);

        int joursAvantAnniversaire =
            (prochainAnniversaire - aujourdHui).Days;

        MessageAge = age >= 18 ? "Majeur" : "Mineur";
        MessageAnniversaire =
            $"Prochain anniversaire dans {joursAvantAnniversaire} jours.";
        Resultat = $"{Nom}, vous avez {age} ans";
        HistoriqueCalculs.Add($"{Nom} — {age} ans");
        ResultatVisible = true;
    }
}