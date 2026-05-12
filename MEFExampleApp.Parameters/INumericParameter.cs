namespace MEFExampleApp.Parameters
{
    /// <summary>
    /// A parameter whose value is expected to be a valid number.
    /// Extends <see cref="IParameter"/> with a parsed numeric representation.
    /// </summary>
    public interface INumericParameter : IParameter
    {
        /// <summary>
        /// The parsed numeric value, or <c>null</c> when <see cref="IParameter.Value"/>
        /// cannot be converted to a <see cref="double"/>.
        /// </summary>
        double? NumericValue { get; }

        /// <summary><c>true</c> when <see cref="IParameter.Value"/> is a valid number.</summary>
        bool IsValid { get; }
    }
}
