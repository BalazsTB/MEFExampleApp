using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MEFExampleApp.Parameters;

namespace MEFExampleApp.Modules.Calculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _result = string.Empty;

        /// <summary>Numeric parameter for the left operand.</summary>
        public INumericParameterViewModel Left { get; } =
            new NumericParameterViewModel(new NumericParameter("Left operand", "0"));

        /// <summary>Numeric parameter for the right operand.</summary>
        public INumericParameterViewModel Right { get; } =
            new NumericParameterViewModel(new NumericParameter("Right operand", "0"));

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
            if (!Left.IsValid || !Right.IsValid)
            {
                Result = "Invalid input";
                return;
            }

            double a = Left.NumericValue.Value;
            double b = Right.NumericValue.Value;
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

        // Hook into WPF's command manager so buttons re-evaluate CanExecute automatically.
        public event EventHandler CanExecuteChanged
        {
            add    => System.Windows.Input.CommandManager.RequerySuggested += value;
            remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
        }
    }
}
