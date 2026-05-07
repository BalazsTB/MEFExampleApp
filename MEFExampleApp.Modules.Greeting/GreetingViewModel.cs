using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.Greeting
{
    public class GreetingViewModel : INotifyPropertyChanged
    {
        private string _greeting = string.Empty;

        /// <summary>Text parameter that holds the name the user enters.</summary>
        public ITextParameterViewModel NameParam { get; } =
            new TextParameterViewModel(new TextParameter("Your name"));

        public string Greeting
        {
            get => _greeting;
            private set { _greeting = value; OnPropertyChanged(); }
        }

        public ICommand SayHelloCommand { get; }

        public GreetingViewModel()
        {
            SayHelloCommand = new RelayCommand(
                () => Greeting = string.IsNullOrWhiteSpace(NameParam.Value)
                    ? "Hello, World!"
                    : $"Hello, {NameParam.Value.Trim()}!",
                () => true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    /// <summary>Minimal ICommand implementation – no framework dependency needed.</summary>
    internal class RelayCommand : ICommand
    {
        private readonly System.Action _execute;
        private readonly System.Func<bool> _canExecute;

        public RelayCommand(System.Action execute, System.Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute();
        public void Execute(object parameter) => _execute();

        // Hook into WPF's command manager so the button re-evaluates CanExecute automatically.
        public event System.EventHandler CanExecuteChanged
        {
            add    => System.Windows.Input.CommandManager.RequerySuggested += value;
            remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
        }
    }
}
