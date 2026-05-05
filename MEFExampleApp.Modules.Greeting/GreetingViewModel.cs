using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MEFExampleApp.Modules.Greeting
{
    public class GreetingViewModel : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _greeting = string.Empty;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Greeting
        {
            get => _greeting;
            private set { _greeting = value; OnPropertyChanged(); }
        }

        public ICommand SayHelloCommand { get; }

        public GreetingViewModel()
        {
            SayHelloCommand = new RelayCommand(
                () => Greeting = string.IsNullOrWhiteSpace(Name)
                    ? "Hello, World!"
                    : $"Hello, {Name.Trim()}!",
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
        public event System.EventHandler CanExecuteChanged;
    }
}
