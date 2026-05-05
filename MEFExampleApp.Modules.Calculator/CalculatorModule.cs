using System.ComponentModel.Composition;
using System.Windows;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.Calculator
{
    [Export(typeof(IModule))]
    [ExportMetadata("Name", "Calculator")]
    [ExportMetadata("Description", "A simple four-operation calculator.")]
    [ExportMetadata("Order", 2)]
    public class CalculatorModule : IModule
    {
        public UIElement GetView()
        {
            var view = new CalculatorView();
            view.DataContext = new CalculatorViewModel();
            return view;
        }
    }
}
