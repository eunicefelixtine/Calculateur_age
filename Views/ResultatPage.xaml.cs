using CalculateurAge.ViewModels;

namespace CalculateurAge.Views;

public partial class ResultatPage : ContentPage
{
    public ResultatPage(ResultatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}