using System.ComponentModel.Composition;
using System.Windows;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.Greeting
{
    /// <summary>
    /// [Export] tells MEF "I implement IModule – add me to any [ImportMany] collection".
    /// [ExportMetadata] attaches values that satisfy IModuleMetadata without the shell
    /// ever needing to instantiate this class first (metadata is read from the lazy wrapper).
    /// </summary>
    [Export(typeof(IModule))]
    [ExportMetadata("Name", "Greeting")]
    [ExportMetadata("Description", "Enter a name and get a personalised hello.")]
    [ExportMetadata("Order", 1)]
    public class GreetingModule : IModule
    {
        public UIElement GetView()
        {
            var view = new GreetingView();
            view.DataContext = new GreetingViewModel();
            return view;
        }
    }
}
