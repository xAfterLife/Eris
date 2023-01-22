using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ErisUi.Core;

public class ObservableObject : INotifyPropertyChanged
{
    /// <summary>
    ///     Event from INotifyPropertyChanged
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     Easy access to updating a value and instantly calling the PropertyChanged Event
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    /// <param name="field">Field to update</param>
    /// <param name="value">Value to set the Field to</param>
    /// <param name="propertyName">Automaticly pulled by CallerMemberName Attribute</param>
    protected void Update<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        //Return if old and new value are the same
        if ( EqualityComparer<T>.Default.Equals(field, value) )
            return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    /// <summary>
    ///     Raises the PropertyChanged Event
    /// </summary>
    /// <param name="propertyName">If externaly called required to send the correct PropertyChanged Event</param>
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
