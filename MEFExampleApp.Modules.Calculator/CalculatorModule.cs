using System;
using System.ComponentModel.Composition;
using System.Windows;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.Calculator
{
    /// <summary>
    /// ViewModel-first module: exposes a CalculatorViewModel and the DataTemplate that
    /// maps it to CalculatorView.  The shell never instantiates or references CalculatorView.
    /// </summary>
    [Export(typeof(IModule))]
    [ExportMetadata("Name", "Calculator")]
    [ExportMetadata("Description", "A simple four-operation calculator.")]
    [ExportMetadata("Order", 2)]
    public class CalculatorModule : IModule
    {
        public object GetViewModel() => new CalculatorViewModel();

        public ResourceDictionary GetResources() =>
            new ResourceDictionary
            {
                Source = new Uri(
                    "pack://application:,,,/MEFExampleApp.Modules.Calculator;component/CalculatorResources.xaml",
                    UriKind.Absolute)
            };
    }
}
