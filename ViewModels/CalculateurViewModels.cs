using System.Collections.ObjectModel;
using CalculateurAge.Services;

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

    private readonly INavigationService _navigationService;

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

        set
        {
            if (SetField(ref _resultatVisible, value))
                AfficherResultatCommand.Rafraichir();
        }
    }

    public RelayCommand CalculerCommand { get; }

    public RelayCommand EffacerCommand { get; }

    public AsyncRelayCommand AfficherResultatCommand { get; }

    public CalculateurViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom)
                && DateNaissance.Date <= DateTime.Today);

        EffacerCommand = new RelayCommand(Effacer);
        AfficherResultatCommand = new AsyncRelayCommand(
            () => _navigationService.AfficherResultatAsync(Resultat),
            () => ResultatVisible);
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