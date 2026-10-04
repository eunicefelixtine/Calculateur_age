using System.Windows.Input;

namespace CalculateurAge.ViewModels;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _executer;
    private readonly Func<bool>? _peutExecuter;
    private bool _enExecution;

    public AsyncRelayCommand(
        Func<Task> executer,
        Func<bool>? peutExecuter = null)
    {
        _executer = executer;
        _peutExecuter = peutExecuter;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
        => !_enExecution && (_peutExecuter?.Invoke() ?? true);

    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
            return;

        _enExecution = true;
        Rafraichir();

        try
        {
            await _executer();
        }
        finally
        {
            _enExecution = false;
            Rafraichir();
        }
    }

    public void Rafraichir()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}