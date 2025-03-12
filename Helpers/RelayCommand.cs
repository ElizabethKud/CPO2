using System.Windows.Input;

namespace CPO2.Helpers;

public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    private readonly Func<bool> _canExecute = canExecute ?? (() => true);

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute();

    public void Execute(object? parameter) => execute();
}

public class RelayCommand<T>(Action<T> execute, Predicate<T>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => canExecute == null || (parameter is T t && canExecute(t));

    public void Execute(object? parameter)
    {
        if (parameter is T t)
            execute(t);
    }
}