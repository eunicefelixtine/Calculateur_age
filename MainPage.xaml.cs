using CalculateurAge.ViewModels;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage(CalculateurViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}