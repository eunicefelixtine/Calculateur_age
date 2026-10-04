using CalculateurAge.Services;
using Microsoft.Maui.Controls;

namespace CalculateurAge.ViewModels;

public class ResultatViewModel : BaseViewModel, IQueryAttributable
{
    private readonly INavigationService _navigationService;
    private string _message = "";

    public string Message
    {
        get => _message;

        set => SetField(ref _message, value);
    }

    public AsyncRelayCommand RetourCommand { get; }

    public ResultatViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        RetourCommand = new AsyncRelayCommand(
            _navigationService.RetournerAsync);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(NavigationRoutes.MessageParameter, out object? message))
            Message = message?.ToString() ?? "";
    }
}