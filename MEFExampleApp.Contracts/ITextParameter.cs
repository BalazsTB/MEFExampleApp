namespace MEFExampleApp.Contracts
{
    /// <summary>
    /// A parameter whose value is free-form text.
    /// Extends <see cref="IParameter"/> with no additional members — the distinction
    /// is used to select the correct ViewModel / View template at compile and design time.
    /// </summary>
    public interface ITextParameter : IParameter { }
}
