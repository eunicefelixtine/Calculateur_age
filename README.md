# Calculateur d'âge

Application mobile .NET MAUI permettant de calculer l'âge d'une personne à partir de son nom et de sa date de naissance. Le projet met en pratique le binding, les commandes et la séparation des responsabilités avec l'architecture MVVM.

## Fonctionnalités

- Calcul de l'âge exact en années révolues.
- Indication du statut **Majeur** ou **Mineur** (seuil : 18 ans).
- Affichage du nombre de jours avant le prochain anniversaire.
- Refus des dates de naissance futures : la commande de calcul est désactivée et le ViewModel protège également le calcul.
- Commande **Effacer** pour réinitialiser le nom, la date (valeur par défaut : aujourd'hui moins 20 ans) et les résultats.
- Historique des calculs au format `Nom — âge ans`. L'historique est conservé lors de l'effacement du formulaire, mais reste en mémoire et n'est pas sauvegardé après la fermeture de l'application.

## Technologies

- C# et .NET MAUI
- .NET 10
- XAML
- Architecture MVVM avec `INotifyPropertyChanged`, bindings et `ICommand`

## Prérequis

- SDK .NET 10
- Charge de travail .NET MAUI
- SDK Android et émulateur Android, ou appareil Android configuré

Sous Linux, le projet cible Android (`net10.0-android`). Les cibles iOS et Mac Catalyst sont ajoutées sur macOS; la cible Windows est ajoutée sous Windows.

## Compiler

Depuis le dossier contenant `CalculateurAge.csproj` :

```bash
dotnet restore
dotnet build -f net10.0-android
```

Pour lancer l'application, ouvrez le projet dans Visual Studio ou VS Code, sélectionnez un émulateur ou appareil Android, puis démarrez le débogage.

## Architecture

```text
CalculateurAge/
├── ViewModels/
│   ├── BaseViewModels.cs
│   ├── CalculateurViewModels.cs
│   └── RelayCommand.cs
├── Views/
│   ├── ResultatPage.xaml
│   └── ResultatPage.xaml.cs
├── MainPage.xaml
├── MainPage.xaml.cs
├── AppShell.xaml
└── CalculateurAge.csproj
```

`MainPage.xaml` présente les champs et les résultats via des bindings. `CalculateurViewModel` porte l'état, les règles de validation, le calcul de l'âge et les commandes. `ObservableCollection` permet de mettre à jour l'historique sans logique métier dans le code-behind.

Projet réalisé par NZEUTEM DOMMOE Eunice Felixtine - 22G00347