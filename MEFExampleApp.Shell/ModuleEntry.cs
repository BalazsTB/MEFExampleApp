using MEFExampleApp.Contracts;

namespace MEFExampleApp.Shell
{
    /// <summary>
    /// View-facing wrapper that pairs a resolved IModule instance with its metadata.
    /// The list on the left binds to a collection of these.
    /// </summary>
    public class ModuleEntry
    {
        public IModule Module { get; }
        public IModuleMetadata Metadata { get; }

        public string Name => Metadata.Name;
        public string Description => Metadata.Description;
        public int Order => Metadata.Order;

        public ModuleEntry(IModule module, IModuleMetadata metadata)
        {
            Module = module;
            Metadata = metadata;
        }
    }
}
