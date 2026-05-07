namespace MEFExampleApp.Contracts
{
    /// <summary>Plain data model for a free-text input parameter.</summary>
    public class TextParameter : ITextParameter
    {
        /// <inheritdoc/>
        public string Prompt { get; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public string Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="TextParameter"/>.
        /// </summary>
        /// <param name="prompt">Label shown next to the input.</param>
        /// <param name="initialValue">Starting value (default: empty string).</param>
        /// <param name="isEnabled">Whether the input is initially enabled (default: true).</param>
        public TextParameter(string prompt, string initialValue = "", bool isEnabled = true)
        {
            Prompt    = prompt;
            Value     = initialValue;
            IsEnabled = isEnabled;
        }
    }
}
