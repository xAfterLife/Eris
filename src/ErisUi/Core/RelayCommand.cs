using System;
using System.Windows.Input;

namespace ErisUi.Core;

/// <summary>
///     Wrapper for ICommand creations
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    ///     Action to be executed
    /// </summary>
    private readonly Action<object?> _action;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="action">Action to be executed</param>
    public RelayCommand(Action<object?> action)
    {
        _action = action;
    }

    /// <summary>
    ///     Executes the Action
    /// </summary>
    /// <param name="parameter">Optional Parameter that gets passed to the Action</param>
    public void Execute(object? parameter)
    {
        _action(parameter);
    }

    /// <summary>
    ///     Can this Action be executed?
    /// </summary>
    /// <param name="parameter">Optional Parameter that gets passed to the Action</param>
    /// <returns></returns>
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    /// <summary>
    ///     Empty EventHandler for CanExecuteChanged
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add {}
        remove {}
    }
}
