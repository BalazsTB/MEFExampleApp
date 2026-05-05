using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MEFExampleApp.Modules.Calculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _left = "0";
        private string _right = "0";
        private string _result = string.Empty;

        public string Left
        {
            get => _left;
            set { _left = value; OnPropertyChanged(); }
        }

        public string Right
        {
            get => _right;
            set { _right = value; OnPropertyChanged(); }
        }

        public string Result
        {
            get => _result;
            private set { _result = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }
        public ICommand SubtractCommand { get; }
        public ICommand MultiplyCommand { get; }
        public ICommand DivideCommand { get; }

        public CalculatorViewModel()
        {
            AddCommand      = new RelayCommand(() => Calculate("+"));
            SubtractCommand = new RelayCommand(() => Calculate("-"));
            MultiplyCommand = new RelayCommand(() => Calculate("×"));
            DivideCommand   = new RelayCommand(() => Calculate("÷"));
        }

        private void Calculate(string op)
        {
            if (!double.TryParse(Left, out double a) || !double.TryParse(Right, out double b))
            {
                Result = "Invalid input";
                return;
            }

            double answer;
            switch (op)
            {
                case "+": answer = a + b; break;
                case "-": answer = a - b; break;
                case "×": answer = a * b; break;
                case "÷":
                    if (b == 0) { Result = "Cannot divide by zero"; return; }
                    answer = a / b;
                    break;
                default: answer = double.NaN; break;
            }

            Result = $"{a} {op} {b} = {answer}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        public RelayCommand(Action execute) => _execute = execute;
        public bool CanExecute(object p) => true;
        public void Execute(object p) => _execute();
        public event EventHandler CanExecuteChanged;
    }
}
