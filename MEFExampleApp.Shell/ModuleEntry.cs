using MEFExampleApp.Contracts;

namespace MEFExampleApp.Shell
{
    /// <summary>
    /// View-facing wrapper that pairs a resolved IModule instance with its metadata
    /// and the ViewModel the module provides.
    ///
    /// The shell binds to <see cref="ViewModel"/> via ContentPresenter; WPF picks the
    /// matching DataTemplate automatically from Application.Current.Resources — no
    /// code in the shell ever references a View type.
    /// </summary>
    public class ModuleEntry
    {
        public IModule Module { get; }
        public IModuleMetadata Metadata { get; }

        /// <summary>
        /// The plain ViewModel object returned by the module.
        /// WPF uses the implicit DataTemplate registered by the module to render it.
        /// </summary>
        public object ViewModel { get; }

        public string Name => Metadata.Name;
        public string Description => Metadata.Description;
        public int Order => Metadata.Order;

        public ModuleEntry(IModule module, IModuleMetadata metadata)
        {
            Module = module;
            Metadata = metadata;
            ViewModel = module.GetViewModel();
        }
    }
}
