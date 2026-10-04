# TP1 --- Calculateur d'âge : Routing et MVVM avec .NET MAUI

> **Atelier de développement mobile --- Travail individuel**
>
> **Objectif :** construire une application de calcul d'âge en
> **code-behind**, ajouter une seconde page et la navigation
> (**Routing**), puis réécrire l'application en **MVVM**.

Ce document reprend la fiche de TP fournie en images et la transforme en
un guide de réalisation pas à pas. Les extraits de code sont remis en
forme pour être directement exploitables dans un projet .NET MAUI.

------------------------------------------------------------------------

# 1. Résultat attendu

L'application finale doit permettre de :

1.  Saisir un **nom**.
2.  Sélectionner une **date de naissance**.
3.  Calculer l'âge réel de la personne.
4.  Afficher le résultat sur une **seconde page** grâce au routing.
5.  Réaliser ensuite la même application selon l'architecture **MVVM**.
6.  Désactiver automatiquement le bouton **Calculer** lorsque le nom est
    vide.
7.  Ajouter au moins **3 fonctionnalités supplémentaires** pour
    l'Activité 6.
8.  Publier le projet sur GitHub avec un historique de commits organisé
    par phase.
9.  Fournir une vidéo de démonstration de **30 secondes maximum**.

------------------------------------------------------------------------

# 2. Étape 0 --- Créer le projet

## 2.1 Créer le projet dans Visual Studio

Dans Visual Studio :

**Créer un projet → .NET MAUI App**

Nom du projet :

``` text
CalculateurAge
```

Lors de l'écran suivant :

> **DÉCOCHER** `Include sample content`

La fiche précise que, si cette option reste cochée, Visual Studio génère
une application complète de gestion de tâches qui ne correspond pas au
TP.

## 2.2 Vérifier la structure initiale

La solution doit notamment contenir :

``` text
App.xaml
App.xaml.cs
AppShell.xaml
AppShell.xaml.cs
MainPage.xaml
MainPage.xaml.cs
MauiProgram.cs
Platforms/
Resources/
```

## 2.3 Premier lancement

Avant de continuer :

-   lancer l'application ;
-   choisir l'émulateur Android ;
-   vérifier que l'application démarre correctement.

------------------------------------------------------------------------

# 3. Phase A --- Version code-behind

L'objectif de cette phase est de construire volontairement l'application
avec la logique directement dans `MainPage.xaml.cs`.

## A1. `MainPage.xaml`

Remplacer le contenu de `<ContentPage>` par :

``` xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="CalculateurAge.MainPage"
    Title="Calculateur">

    <ScrollView>
        <VerticalStackLayout Padding="20" Spacing="12">

            <Label
                Text="Nom"
                FontSize="16" />

            <Entry
                x:Name="entryNom"
                Placeholder="Votre nom" />

            <Label
                Text="Date de naissance"
                FontSize="16" />

            <DatePicker
                x:Name="pickerDate" />

            <Button
                x:Name="btnCalculer"
                Text="Calculer"
                Clicked="OnCalculerClicked" />

            <Label
                x:Name="lblResultat"
                FontSize="20"
                IsVisible="False" />

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
```

Ne pas oublier `Title="Calculateur"` sur `ContentPage`.

## A2. `MainPage.xaml.cs`

Ajouter le gestionnaire du bouton :

``` csharp
private void OnCalculerClicked(object sender, EventArgs e)
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

    // On écrit DIRECTEMENT dans les contrôles.
    lblResultat.Text = $"{entryNom.Text}, vous avez {age} ans";
    lblResultat.IsVisible = true;
}
```

Tester l'application.

Cette version montre que la logique est fortement liée à l'interface :
le code manipule directement `entryNom`, `lblResultat` et `pickerDate`.

------------------------------------------------------------------------

# 4. Phase B --- Seconde page et navigation

## B1. Créer le dossier `Views`

Dans l'Explorateur de solutions :

``` text
Clic droit sur le projet
→ Ajouter
→ Nouveau dossier
→ Views
```

Puis :

``` text
Clic droit sur Views
→ Ajouter
→ Nouvel élément
→ .NET MAUI
→ .NET MAUI ContentPage (XAML)
```

Nom :

``` text
ResultatPage.xaml
```

Vérifier que le `x:Class` est :

``` text
CalculateurAge.Views.ResultatPage
```

## B2. `Views/ResultatPage.xaml`

``` xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="CalculateurAge.Views.ResultatPage"
    Title="Résultat">

    <VerticalStackLayout Padding="20" Spacing="16">

        <Label
            x:Name="lblMessage"
            FontSize="22" />

        <Button
            Text="Retour"
            Clicked="OnRetourClicked" />

    </VerticalStackLayout>

</ContentPage>
```

## B3. `Views/ResultatPage.xaml.cs`

``` csharp
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
```

### Pourquoi `OnAppearing()` ?

Les propriétés liées à `[QueryProperty]` sont remplies après le
constructeur. L'affichage doit donc être réalisé dans `OnAppearing()`.

## B4. `AppShell.xaml.cs` --- enregistrer la route

Ajouter :

``` csharp
using CalculateurAge.Views;
```

Puis :

``` csharp
public AppShell()
{
    InitializeComponent();

    // Déclare la route.
    Routing.RegisterRoute(
        nameof(ResultatPage),
        typeof(ResultatPage));
}
```

## B5. Modifier `MainPage.xaml.cs`

Ajouter :

``` csharp
using CalculateurAge.Views;
```

La méthode devient `async` :

``` csharp
private async void OnCalculerClicked(object sender, EventArgs e)
```

Puis, après le calcul de `age` :

``` csharp
await Shell.Current.GoToAsync(
    $"{nameof(ResultatPage)}?nom={entryNom.Text}&age={age}");
```

Version complète :

``` csharp
private async void OnCalculerClicked(object sender, EventArgs e)
{
    if (string.IsNullOrWhiteSpace(entryNom.Text))
    {
        await DisplayAlert("Erreur", "Entrez un nom", "OK");
        return;
    }

    DateTime d = pickerDate.Date;

    int age = DateTime.Today.Year - d.Year;

    if (d.Date > DateTime.Today.AddYears(-age))
        age--;

    await Shell.Current.GoToAsync(
        $"{nameof(ResultatPage)}?nom={entryNom.Text}&age={age}");
}
```

> Pour une application réelle, les valeurs placées dans une URL
> devraient être correctement encodées si elles peuvent contenir des
> caractères spéciaux. Pour le TP, on conserve la forme demandée par la
> fiche.

## À retenir sur le Routing

``` csharp
GoToAsync(nameof(Page))
```

→ aller vers une route enregistrée.

``` csharp
GoToAsync("..")
```

→ revenir à la page précédente.

``` csharp
GoToAsync("//MainPage")
```

→ retourner à la racine.

Avec paramètres :

``` csharp
GoToAsync($"{nameof(Page)}?cle=valeur")
```

Les paramètres sont placés après `?` et séparés par `&`.

------------------------------------------------------------------------

# 5. Phase C --- Réécriture en MVVM

Créer le dossier :

``` text
ViewModels/
```

L'objectif est de séparer :

``` text
View (XAML)
    ↓ Binding / Command
ViewModel
    ↓
Logique métier
```

## C1. `ViewModels/BaseViewModel.cs`

``` csharp
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalculateurAge.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string nom = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nom));
    }

    protected bool SetField<T>(
        ref T champ,
        T valeur,
        [CallerMemberName] string nom = null)
    {
        if (EqualityComparer<T>.Default.Equals(champ, valeur))
            return false;

        champ = valeur;
        OnPropertyChanged(nom);

        return true;
    }
}
```

`INotifyPropertyChanged` permet au ViewModel de signaler au moteur de
binding qu'une propriété a changé.

## C2. `ViewModels/RelayCommand.cs`

``` csharp
using System;
using System.Windows.Input;

namespace CalculateurAge.ViewModels;

public class RelayCommand : ICommand
{
    private readonly Action _executer;
    private readonly Func<bool> _peutExecuter;

    public RelayCommand(
        Action executer,
        Func<bool> peutExecuter = null)
    {
        _executer = executer;
        _peutExecuter = peutExecuter;
    }

    public bool CanExecute(object p)
        => _peutExecuter?.Invoke() ?? true;

    public void Execute(object p)
        => _executer();

    public event EventHandler CanExecuteChanged;

    public void Rafraichir()
        => CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
}
```

`CanExecute()` indique si l'action est possible. Le bouton peut alors se
désactiver automatiquement.

## C3. `ViewModels/CalculateurViewModel.cs`

``` csharp
namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";

    private DateTime _dateNaissance =
        DateTime.Today.AddYears(-20);

    private string _resultat = "";

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

    public bool ResultatVisible
    {
        get => _resultatVisible;

        set => SetField(
            ref _resultatVisible,
            value);
    }

    public RelayCommand CalculerCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));
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

        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
    }
}
```

Ce fichier ne contient aucun `Label`, `Entry`, `Button` ou
`DisplayAlert`.

## C4. `MainPage.xaml` en MVVM

``` xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="CalculateurAge.MainPage"
    Title="Calculateur">

    <ScrollView>
        <VerticalStackLayout Padding="20" Spacing="12">

            <Label
                Text="Nom"
                FontSize="16" />

            <Entry
                Text="{Binding Nom, Mode=TwoWay}"
                Placeholder="Votre nom" />

            <Label
                Text="Date de naissance"
                FontSize="16" />

            <DatePicker
                Date="{Binding DateNaissance}" />

            <Button
                Text="Calculer"
                Command="{Binding CalculerCommand}" />

            <Label
                Text="{Binding Resultat}"
                FontSize="20"
                IsVisible="{Binding ResultatVisible}" />

        </VerticalStackLayout>
    </ScrollView>

</ContentPage>
```

Il n'y a plus de `x:Name` pour les contrôles métier et plus de
`Clicked`.

## C5. `MainPage.xaml.cs` --- fichier complet

``` csharp
using CalculateurAge.ViewModels;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        BindingContext = new CalculateurViewModel();
    }
}
```

Tester :

-   champ `Nom` vide → bouton **Calculer** grisé ;
-   première lettre saisie → bouton activé ;
-   clic sur **Calculer** → calcul effectué par le ViewModel.

------------------------------------------------------------------------

# 6. Erreurs fréquentes

  -----------------------------------------------------------------------
  Symptôme                            Cause
  ----------------------------------- -----------------------------------
  Écran vide, aucune erreur           Nom de propriété mal orthographié
                                      dans `{Binding}`

  La valeur ne se met pas à jour      Setter sans `SetField` ou mauvais
                                      nom de propriété

  Route inconnue à l'exécution        `Routing.RegisterRoute` oublié

  Page de détail vide                 Affichage dans le constructeur au
                                      lieu de `OnAppearing()`

  Bouton toujours grisé               `Rafraichir()` non appelé depuis le
                                      setter de `Nom`

  Erreur sur `x:Class`                Namespace/classe ne correspondant
                                      pas au fichier
  -----------------------------------------------------------------------

------------------------------------------------------------------------

# 7. Mots-clés à connaître

  -----------------------------------------------------------------------
  Mot-clé                             Rôle
  ----------------------------------- -----------------------------------
  `{Binding X}`                       Relie une propriété du contrôle à
                                      une propriété du ViewModel

  `BindingContext`                    Objet dans lequel les bindings
                                      cherchent leurs valeurs

  `INotifyPropertyChanged`            Contrat de notification des
                                      changements

  `PropertyChanged`                   Événement déclenché lors d'une
                                      modification

  `[CallerMemberName]`                Récupère automatiquement le nom de
                                      la propriété appelante

  `ICommand`                          Action transformée en objet
                                      utilisable par un Button

  `CanExecute`                        Indique si l'action est possible

  `Mode=TwoWay`                       Binding dans les deux sens

  `x:Name`                            Nom permettant d'accéder à un
                                      contrôle depuis le code-behind

  `partial`                           Classe répartie sur plusieurs
                                      fichiers

  `nameof(X)`                         Renvoie le nom du symbole `X`

  `[QueryProperty]`                   Relie un paramètre de navigation à
                                      une propriété publique

  `Shell`                             Système de navigation de .NET MAUI
  -----------------------------------------------------------------------

------------------------------------------------------------------------

# 8. Mémo --- trajet d'une donnée

1.  L'utilisateur tape une lettre dans l'`Entry`.
2.  Le binding `TwoWay` écrit dans la propriété `Nom` du ViewModel.
3.  Le setter détecte le changement et appelle `SetField`.
4.  `SetField` affecte le champ privé puis déclenche
    `PropertyChanged("Nom")`.
5.  Les contrôles liés à `Nom` sont notifiés et se redessinent.

``` text
Utilisateur
    ↓
Entry
    ↓ Binding TwoWay
ViewModel.Nom
    ↓
SetField()
    ↓
PropertyChanged
    ↓
Binding
    ↓
Interface mise à jour
```

------------------------------------------------------------------------

# 9. Mémo --- les trois modes de Binding

  Mode        Sens                     Utilisation
  ----------- ------------------------ ----------------------------
  `OneWay`    ViewModel → écran        Mode par défaut d'un Label
  `TwoWay`    ViewModel ↔ écran        Mode adapté à un Entry
  `OneTime`   Lecture une seule fois   Au démarrage

Exemple :

``` xml
<Entry Text="{Binding Nom, Mode=TwoWay}" />
```

------------------------------------------------------------------------

# 10. Mémo --- Routing

``` csharp
await Shell.Current.GoToAsync(nameof(ResultatPage)); // aller vers
await Shell.Current.GoToAsync("..");                  // revenir en arrière
await Shell.Current.GoToAsync("//MainPage");          // retour à la racine
await Shell.Current.GoToAsync($"{nameof(ResultatPage)}?cle=valeur"); // paramètre
```

------------------------------------------------------------------------

# 11. Organisation finale du projet

``` text
CalculateurAge/
│
├── Views/
│   ├── ResultatPage.xaml
│   └── ResultatPage.xaml.cs
│
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── RelayCommand.cs
│   └── CalculateurViewModel.cs
│
├── MainPage.xaml
├── MainPage.xaml.cs
├── AppShell.xaml
├── AppShell.xaml.cs
├── App.xaml
├── App.xaml.cs
└── MauiProgram.cs
```

> **Règle finale :** si un ViewModel contient le mot `Label`, `Entry`,
> `Button` ou `DisplayAlert`, ce n'est pas du MVVM correct.

------------------------------------------------------------------------

# 12. Activité 6 --- Travail à rendre

Reprendre le projet terminé et l'enrichir.

## 12.1 Ajouter au moins trois fonctionnalités

La fiche demande **au moins trois fonctionnalités supplémentaires**, en
respectant le MVVM.

Exemples donnés dans la fiche :

### A. Message selon l'âge

Afficher :

``` text
Majeur
```

ou :

``` text
Mineur
```

### B. Commande « Effacer »

Remettre les champs à zéro :

``` text
Nom → vide
Date de naissance → valeur par défaut
Résultat → vide
RésultatVisible → false
```

### C. Refuser une date future

Empêcher le calcul si la date de naissance est dans le futur.

### D. Nombre de jours avant le prochain anniversaire

Afficher par exemple :

``` text
Prochain anniversaire dans 125 jours.
```

### E. Historique des calculs

Conserver les calculs réalisés :

``` text
Klein — 24 ans
Paul — 19 ans
Marie — 31 ans
```

### F. Navigation vers `ResultatPage` par le ViewModel

La fiche propose également une amélioration consistant à faire remonter
le résultat vers `ResultatPage` par le ViewModel plutôt que de
transmettre uniquement les valeurs par l'URL.

------------------------------------------------------------------------

# 13. GitHub

Publier le projet dans un dépôt GitHub public.

Exemple d'organisation des commits :

``` bash
git init

git add .
git commit -m "Phase A - Version code-behind"

git add .
git commit -m "Phase B - Routing et seconde page"

git add .
git commit -m "Phase C - Réécriture en MVVM"

git add .
git commit -m "Activite 6 - Ajout des fonctionnalites"
```

L'objectif est de montrer clairement l'évolution du projet.

------------------------------------------------------------------------

# 14. Vidéo de démonstration

Enregistrer une vidéo de :

> **30 secondes maximum**

Montrer :

1.  l'application en fonctionnement ;
2.  la saisie du nom ;
3.  la sélection de la date ;
4.  le calcul ;
5.  la navigation ;
6.  les fonctionnalités supplémentaires.

------------------------------------------------------------------------

# 15. Dépôt du travail

Déposer dans l'espace du cours **Activité 6** :

-   le lien du dépôt GitHub ;
-   la vidéo de démonstration.

------------------------------------------------------------------------

# 16. Critères d'évaluation

L'évaluation porte notamment sur :

-   le respect du **MVVM** ;
-   la pertinence des fonctionnalités ajoutées ;
-   la qualité du dépôt ;
-   l'historique des commits ;
-   la clarté de la démonstration.

------------------------------------------------------------------------

# 17. Checklist finale

## Projet

-   [ ] Projet nommé `CalculateurAge`
-   [ ] `Include sample content` désactivé
-   [ ] Premier lancement Android effectué
-   [ ] `Views/ResultatPage.xaml` présent
-   [ ] `ViewModels/` présent

## Phase A

-   [ ] Calcul de l'âge fonctionnel
-   [ ] Validation du nom
-   [ ] Résultat affiché

## Phase B

-   [ ] Route `ResultatPage` enregistrée
-   [ ] Navigation avec `GoToAsync`
-   [ ] Paramètres `nom` et `age`
-   [ ] `[QueryProperty]`
-   [ ] Retour avec `GoToAsync("..")`

## Phase C

-   [ ] `BaseViewModel`
-   [ ] `INotifyPropertyChanged`
-   [ ] `SetField`
-   [ ] `RelayCommand`
-   [ ] `CalculateurViewModel`
-   [ ] `BindingContext`
-   [ ] Binding `TwoWay`
-   [ ] `Command="{Binding CalculerCommand}"`
-   [ ] Bouton désactivé lorsque le nom est vide
-   [ ] Aucun contrôle UI dans le ViewModel

## Activité 6

-   [ ] Au moins 3 fonctionnalités supplémentaires
-   [ ] Fonctionnalités réalisées en MVVM
-   [ ] Dépôt GitHub public
-   [ ] Commits organisés par phase
-   [ ] Vidéo ≤ 30 secondes
-   [ ] Lien GitHub déposé
-   [ ] Vidéo déposée

------------------------------------------------------------------------

# 18. Architecture finale à retenir

``` text
                 ┌─────────────────────┐
                 │        View         │
                 │   XAML / Interface  │
                 └──────────┬──────────┘
                            │
                      Binding / Command
                            │
                            ▼
                 ┌─────────────────────┐
                 │      ViewModel      │
                 │                     │
                 │  Propriétés         │
                 │  Commands           │
                 │  Logique métier     │
                 └──────────┬──────────┘
                            │
                            ▼
                 ┌─────────────────────┐
                 │       Model         │
                 │  Données / logique  │
                 └─────────────────────┘
```

Le TP fait donc évoluer progressivement l'application :

``` text
Code-behind
     ↓
Code-behind + Routing
     ↓
MVVM + Binding + Commands + Routing
```

pour obtenir une application .NET MAUI mieux structurée, maintenable et
extensible.
