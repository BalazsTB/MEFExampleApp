using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MEFExampleApp.Contracts
{
    /// <summary>
    /// Observable ViewModel for a free-text input parameter.
    /// Wraps an <see cref="ITextParameter"/> model and raises
    /// <see cref="INotifyPropertyChanged"/> events so WPF bindings stay current.
    /// Implements <see cref="ITextParameterViewModel"/> without inheriting from any base class —
    /// interface composition is preferred over inheritance.
    /// </summary>
    public class TextParameterViewModel : ITextParameterViewModel
    {
        private readonly ITextParameter _model;

        /// <inheritdoc/>
        public string Prompt => _model.Prompt;

        /// <inheritdoc/>
        public bool IsEnabled
        {
            get => _model.IsEnabled;
            set { _model.IsEnabled = value; OnPropertyChanged(); }
        }

        /// <inheritdoc/>
        public string Value
        {
            get => _model.Value;
            set { _model.Value = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Creates a new <see cref="TextParameterViewModel"/> that wraps <paramref name="model"/>.
        /// </summary>
        public TextParameterViewModel(ITextParameter model)
        {
            _model = model;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
