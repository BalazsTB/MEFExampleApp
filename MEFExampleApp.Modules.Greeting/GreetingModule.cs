using System;
using System.ComponentModel.Composition;
using System.Windows;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Modules.Greeting
{
    /// <summary>
    /// [Export] tells MEF "I implement IModule – add me to any [ImportMany] collection".
    /// [ExportMetadata] attaches values that satisfy IModuleMetadata without the shell
    /// ever needing to instantiate this class first (metadata is read from the lazy wrapper).
    ///
    /// ViewModel-first: this class returns a plain GreetingViewModel from GetViewModel()
    /// and a ResourceDictionary containing the implicit DataTemplate from GetResources().
    /// The shell never touches GreetingView directly.
    /// </summary>
    [Export(typeof(IModule))]
    [ExportMetadata("Name", "Greeting")]
    [ExportMetadata("Description", "Enter a name and get a personalized hello.")]
    [ExportMetadata("Order", 1)]
    public class GreetingModule : IModule
    {
        /// <summary>
        /// Returns the ViewModel.  The shell puts this object into a ContentPresenter;
        /// WPF resolves the View automatically via the DataTemplate in GetResources().
        /// </summary>
        public object GetViewModel() => new GreetingViewModel();

        /// <summary>
        /// Loads GreetingResources.xaml (compiled into this assembly as BAML).
        /// That file contains the implicit DataTemplate: GreetingViewModel → GreetingView.
        /// The shell merges this dictionary into Application.Current.Resources once at
        /// startup so WPF can find the template for any GreetingViewModel it encounters.
        /// </summary>
        public ResourceDictionary GetResources() =>
            new ResourceDictionary
            {
                Source = new Uri(
                    "pack://application:,,,/MEFExampleApp.Modules.Greeting;component/GreetingResources.xaml",
                    UriKind.Absolute)
            };
    }
}
