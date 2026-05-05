namespace MEFExampleApp.Contracts
{
    /// <summary>
    /// Typed metadata interface for MEF exports.
    /// Decorating an export with [ExportMetadata] and using Lazy&lt;IModule, IModuleMetadata&gt;
    /// lets the shell read Name/Description/Order without instantiating the module.
    /// </summary>
    public interface IModuleMetadata
    {
        string Name { get; }
        string Description { get; }
        int Order { get; }
    }
}
