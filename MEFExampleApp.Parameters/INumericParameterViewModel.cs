namespace MEFExampleApp.Parameters
{
    /// <summary>
    /// Observable ViewModel for a numeric parameter.
    /// Extends <see cref="IParameterViewModel"/> with validity information.
    /// </summary>
    public interface INumericParameterViewModel : IParameterViewModel
    {
        /// <summary>Parsed numeric value, or <c>null</c> when the text is not a valid number.</summary>
        double? NumericValue { get; }

        /// <summary><c>true</c> when <see cref="IParameterViewModel.Value"/> parses to a number.</summary>
        bool IsValid { get; }
    }
}
