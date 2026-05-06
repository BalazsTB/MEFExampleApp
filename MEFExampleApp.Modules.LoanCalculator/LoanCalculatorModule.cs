using System;
using System.ComponentModel.Composition;
using System.Windows;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.LoanCalculator
{
    /// <summary>
    /// MEF plugin module that demonstrates the ViewModel-first pattern with mixed inputs.
    ///
    /// This module accepts both a free-text label and three numerical parameters (principal,
    /// annual rate, term in years).  All input parsing, validation, and calculation logic
    /// lives exclusively in LoanCalculatorViewModel — the View is a pure template.
    ///
    /// The shell discovers this class at runtime via DirectoryCatalog("Plugins") and
    /// never references LoanCalculatorView, LoanCalculatorViewModel, or this class by name.
    /// </summary>
    [Export(typeof(IModule))]
    [ExportMetadata("Name", "Loan Calculator")]
    [ExportMetadata("Description", "Enter a label, amount, rate, and term to compute monthly repayments.")]
    [ExportMetadata("Order", 3)]
    public class LoanCalculatorModule : IModule
    {
        /// <summary>
        /// Returns a fresh LoanCalculatorViewModel.
        /// The shell places this object into a ContentPresenter as a plain object;
        /// WPF resolves LoanCalculatorView automatically via the DataTemplate below.
        /// </summary>
        public object GetViewModel() => new LoanCalculatorViewModel();

        /// <summary>
        /// Loads LoanCalculatorResources.xaml (compiled to BAML inside this assembly).
        /// That file's implicit DataTemplate maps LoanCalculatorViewModel → LoanCalculatorView.
        /// The shell merges this into Application.Current.Resources before the window shows,
        /// so WPF can find the template for any LoanCalculatorViewModel it encounters.
        /// </summary>
        public ResourceDictionary GetResources() =>
            new ResourceDictionary
            {
                Source = new Uri(
                    "pack://application:,,,/MEFExampleApp.Modules.LoanCalculator;component/LoanCalculatorResources.xaml",
                    UriKind.Absolute)
            };
    }
}
