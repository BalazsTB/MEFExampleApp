using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MEFExampleApp.Modules.LoanCalculator
{
    /// <summary>
    /// ViewModel for the Loan Calculator module.
    ///
    /// Demonstrates the ViewModel-first pattern with both text and numerical inputs:
    ///
    ///   Text input    → LoanLabel   (what the loan is for, e.g. "New car")
    ///   Numeric input → Principal   (loan amount in £)
    ///   Numeric input → AnnualRate  (annual interest rate, % per year)
    ///   Numeric input → TermYears   (loan duration in whole years)
    ///
    /// All business logic — parsing, validation, and the monthly-payment formula — lives
    /// here.  The View is a pure template: it binds to these properties and commands and
    /// contains zero code-behind logic.
    ///
    /// Standard annuity formula:
    ///   M = P × [r(1+r)ⁿ] / [(1+r)ⁿ − 1]
    ///   where r = monthly rate, n = total months
    ///
    /// For a 0 % rate the formula degenerates to M = P / n.
    /// </summary>
    public class LoanCalculatorViewModel : INotifyPropertyChanged
    {
        // ── Raw string fields (exactly what the user typed) ──────────────────────

        private string _loanLabel = string.Empty;
        private string _principal = "10000";
        private string _annualRate = "5.0";
        private string _termYears = "5";

        // ── Computed / output fields ──────────────────────────────────────────────

        private string _monthlyPayment = string.Empty;
        private string _totalRepayment = string.Empty;
        private string _totalInterest = string.Empty;
        private string _validationMessage = string.Empty;

        // ── Properties ───────────────────────────────────────────────────────────

        /// <summary>Free-text label: what the loan is for (e.g. "Home renovation").</summary>
        public string LoanLabel
        {
            get => _loanLabel;
            set { _loanLabel = value; OnPropertyChanged(); RefreshCanCalculate(); }
        }

        /// <summary>Loan principal as a text string; user may type freely.</summary>
        public string Principal
        {
            get => _principal;
            set { _principal = value; OnPropertyChanged(); RefreshCanCalculate(); }
        }

        /// <summary>Annual interest rate (%) as a text string.</summary>
        public string AnnualRate
        {
            get => _annualRate;
            set { _annualRate = value; OnPropertyChanged(); RefreshCanCalculate(); }
        }

        /// <summary>Loan term in whole years as a text string.</summary>
        public string TermYears
        {
            get => _termYears;
            set { _termYears = value; OnPropertyChanged(); RefreshCanCalculate(); }
        }

        /// <summary>Calculated monthly payment; empty until Calculate is run.</summary>
        public string MonthlyPayment
        {
            get => _monthlyPayment;
            private set { _monthlyPayment = value; OnPropertyChanged(); }
        }

        /// <summary>Total amount repaid over the full term.</summary>
        public string TotalRepayment
        {
            get => _totalRepayment;
            private set { _totalRepayment = value; OnPropertyChanged(); }
        }

        /// <summary>Total interest charged over the full term.</summary>
        public string TotalInterest
        {
            get => _totalInterest;
            private set { _totalInterest = value; OnPropertyChanged(); }
        }

        /// <summary>Shown when inputs are invalid; empty when inputs are valid.</summary>
        public string ValidationMessage
        {
            get => _validationMessage;
            private set { _validationMessage = value; OnPropertyChanged(); }
        }

        // ── Command ───────────────────────────────────────────────────────────────

        public ICommand CalculateCommand { get; }

        public LoanCalculatorViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate, CanCalculate);
        }

        // ── Business logic ────────────────────────────────────────────────────────

        private bool CanCalculate()
        {
            return !string.IsNullOrWhiteSpace(LoanLabel)
                && double.TryParse(Principal, out double p)  && p > 0
                && double.TryParse(AnnualRate, out double r) && r >= 0
                && int.TryParse(TermYears, out int y)        && y > 0;
        }

        private void Calculate()
        {
            if (!TryParse(out double principal, out double annualRate, out int termYears))
            {
                MonthlyPayment = string.Empty;
                TotalRepayment = string.Empty;
                TotalInterest  = string.Empty;
                return;
            }

            int n = termYears * 12;           // total months
            double monthlyRate = annualRate / 100.0 / 12.0;

            double monthly;
            if (monthlyRate == 0)
            {
                // 0 % interest — simple division
                monthly = principal / n;
            }
            else
            {
                // Standard annuity formula
                double factor = Math.Pow(1 + monthlyRate, n);
                monthly = principal * monthlyRate * factor / (factor - 1);
            }

            double total    = monthly * n;
            double interest = total - principal;

            string label = string.IsNullOrWhiteSpace(LoanLabel) ? "Your loan" : LoanLabel.Trim();

            MonthlyPayment = $"£{monthly:N2} / month";
            TotalRepayment = $"£{total:N2}  over {n} months";
            TotalInterest  = $"£{interest:N2}  total interest on \"{label}\"";
            ValidationMessage = string.Empty;
        }

        private bool TryParse(out double principal, out double annualRate, out int termYears)
        {
            principal  = 0;
            annualRate = 0;
            termYears  = 0;

            if (!double.TryParse(Principal, out principal) || principal <= 0)
            {
                ValidationMessage = "Principal must be a positive number.";
                return false;
            }
            if (!double.TryParse(AnnualRate, out annualRate) || annualRate < 0)
            {
                ValidationMessage = "Annual rate must be 0 or greater.";
                return false;
            }
            if (!int.TryParse(TermYears, out termYears) || termYears <= 0)
            {
                ValidationMessage = "Term must be a whole number of years greater than 0.";
                return false;
            }

            ValidationMessage = string.Empty;
            return true;
        }

        private void RefreshCanCalculate()
        {
            // Trigger WPF's command manager so the button's IsEnabled re-evaluates.
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        // ── INotifyPropertyChanged ─────────────────────────────────────────────────

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    /// <summary>Minimal ICommand — no framework dependency required.</summary>
    internal class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute    = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute();
        public void Execute(object parameter)    => _execute();

        public event EventHandler CanExecuteChanged
        {
            add    => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
