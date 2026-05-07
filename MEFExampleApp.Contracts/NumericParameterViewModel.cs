using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MEFExampleApp.Contracts
{
    /// <summary>
    /// Observable ViewModel for a numeric input parameter.
    /// Wraps an <see cref="INumericParameter"/> model and raises
    /// <see cref="INotifyPropertyChanged"/> events so WPF bindings stay current.
    /// Implements <see cref="INumericParameterViewModel"/> without inheriting from any base class —
    /// interface composition is preferred over inheritance.
    /// </summary>
    public class NumericParameterViewModel : INumericParameterViewModel
    {
        private readonly INumericParameter _model;

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
            set
            {
                _model.Value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NumericValue));
                OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <inheritdoc/>
        public double? NumericValue => _model.NumericValue;

        /// <inheritdoc/>
        public bool IsValid => _model.IsValid;

        /// <summary>
        /// Creates a new <see cref="NumericParameterViewModel"/> that wraps <paramref name="model"/>.
        /// </summary>
        public NumericParameterViewModel(INumericParameter model)
        {
            _model = model;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
