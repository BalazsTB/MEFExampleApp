namespace MEFExampleApp.Parameters
{
    /// <summary>
    /// Common interface for a single input parameter.
    /// Carries a prompt label, an enabled/disabled state, and the current value as a string.
    /// </summary>
    public interface IParameter
    {
        /// <summary>Label shown to the user (e.g. "Your name", "Loan amount (£)").</summary>
        string Prompt { get; }

        /// <summary>When false the input should be read-only / grayed out.</summary>
        bool IsEnabled { get; set; }

        /// <summary>Raw string value exactly as entered or set.</summary>
        string Value { get; set; }
    }
}
