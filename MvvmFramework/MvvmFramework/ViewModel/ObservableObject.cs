using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmFramework;

public abstract class ObservableObject : INotifyPropertyChanged, INotifyDataErrorInfo
{
    #region PropertyChanged

    protected bool _propertyChangedEventEnabled = true;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
        if(!_propertyChangedEventEnabled) return;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void EnablePropertyChangedEventCallback() => _propertyChangedEventEnabled = true;
    protected void DisablePropertyChangedEventCallback() => _propertyChangedEventEnabled = false;

    #endregion

    #region NotifyErrors

    private readonly Dictionary<string, List<string>> _errors = new();

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName = default)
    {
        if(string.IsNullOrWhiteSpace(propertyName))
        {
            return _errors.Values;
        }

        return _errors.TryGetValue(propertyName, out var errors)
            ? errors
            : new List<string>();
    }

    private void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    protected void AddError(string propertyName, string error)
    {
        if(!_errors.ContainsKey(propertyName))
        {
            _errors[propertyName] = new List<string>();
        }

        if(!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            RaiseErrorsChanged(propertyName);
        }
    }

    protected void AddError<T>(T property, string error, [CallerArgumentExpression(nameof(property))] string? propertyName = default)
    {
        AddError(propertyName, error);
    }

    protected void ClearErrors(string? propertyName = default)
    {
        if(string.IsNullOrWhiteSpace(propertyName))
        {
            var propertyNames = _errors.Keys.ToList();
            _errors.Clear();

            propertyNames.ForEach(name => RaiseErrorsChanged(name));

            return;
        }

        if(_errors.ContainsKey(propertyName))
        {
            _errors.Remove(propertyName);
            RaiseErrorsChanged(propertyName);
        }
    }

    #endregion
}
