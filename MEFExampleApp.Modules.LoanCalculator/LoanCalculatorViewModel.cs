using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.LoanCalculator
{
    /// <summary>
    /// ViewModel for the Loan Calculator module.
    ///
    /// All inputs are expressed as typed parameter ViewModels from MEFExampleApp.Contracts:
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
        // ── Input parameter ViewModels ────────────────────────────────────────────

        /// <summary>Free-text label: what the loan is for (e.g. "Home renovation").</summary>
        public ITextParameterViewModel LoanLabel { get; } =
            new TextParameterViewModel(new TextParameter("Loan label"));

        /// <summary>Loan principal as a numeric parameter.</summary>
        public INumericParameterViewModel Principal { get; } =
            new NumericParameterViewModel(new NumericParameter("Loan amount (£)", "10000"));

        /// <summary>Annual interest rate (%) as a numeric parameter.</summary>
        public INumericParameterViewModel AnnualRate { get; } =
            new NumericParameterViewModel(new NumericParameter("Annual interest rate (%)", "5.0"));

        /// <summary>Loan term in whole years as a numeric parameter.</summary>
        public INumericParameterViewModel TermYears { get; } =
            new NumericParameterViewModel(new NumericParameter("Loan term (years)", "5"));

        // ── Computed / output fields ──────────────────────────────────────────────

        private string _monthlyPayment = string.Empty;
        private string _totalRepayment = string.Empty;
        private string _totalInterest = string.Empty;
        private string _validationMessage = string.Empty;

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
            // Re-evaluate CanExecute whenever any input parameter changes.
            LoanLabel.PropertyChanged  += (_, __) => RefreshCanCalculate();
            Principal.PropertyChanged  += (_, __) => RefreshCanCalculate();
            AnnualRate.PropertyChanged += (_, __) => RefreshCanCalculate();
            TermYears.PropertyChanged  += (_, __) => RefreshCanCalculate();

            CalculateCommand = new RelayCommand(Calculate, CanCalculate);
        }

        // ── Business logic ────────────────────────────────────────────────────────

        private bool CanCalculate()
        {
            return !string.IsNullOrWhiteSpace(LoanLabel.Value)
                && Principal.IsValid  && Principal.NumericValue  > 0
                && AnnualRate.IsValid && AnnualRate.NumericValue >= 0
                && TermYears.IsValid  && TermYears.NumericValue  > 0
                && TermYears.NumericValue == Math.Floor(TermYears.NumericValue.Value); // whole years
        }

        private void Calculate()
        {
            double principal  = Principal.NumericValue.Value;
            double annualRate = AnnualRate.NumericValue.Value;
            int    termYears  = (int)TermYears.NumericValue.Value;

            int    n           = termYears * 12;
            double monthlyRate = annualRate / 100.0 / 12.0;

            double monthly;
            if (monthlyRate == 0)
            {
                monthly = principal / n;
            }
            else
            {
                double factor = Math.Pow(1 + monthlyRate, n);
                monthly = principal * monthlyRate * factor / (factor - 1);
            }

            double total    = monthly * n;
            double interest = total - principal;

            string label = string.IsNullOrWhiteSpace(LoanLabel.Value) ? "Your loan" : LoanLabel.Value.Trim();

            MonthlyPayment    = $"£{monthly:N2} / month";
            TotalRepayment    = $"£{total:N2}  over {n} months";
            TotalInterest     = $"£{interest:N2}  total interest on \"{label}\"";
            ValidationMessage = string.Empty;
        }

        private void RefreshCanCalculate()
        {
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
