using System.ComponentModel;

namespace MEFExampleApp.Parameters
{
    /// <summary>
    /// Observable wrapper around an <see cref="IParameter"/>.
    /// Exposes the same three fields with <see cref="INotifyPropertyChanged"/> support
    /// so WPF bindings update automatically.
    /// </summary>
    public interface IParameterViewModel : INotifyPropertyChanged
    {
        /// <summary>Label shown to the user.</summary>
        string Prompt { get; }

        /// <summary>Enables or disables the input control.</summary>
        bool IsEnabled { get; set; }

        /// <summary>The current raw string value of the parameter.</summary>
        string Value { get; set; }
    }
}
