using System.Globalization;

namespace MEFExampleApp.Parameters
{
    /// <summary>Plain data model for a numeric input parameter.</summary>
    public class NumericParameter : INumericParameter
    {
        private string _value;

        /// <inheritdoc/>
        public string Prompt { get; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                // Re-parse on every set so NumericValue / IsValid stay consistent.
                if (double.TryParse(_value, NumberStyles.Any, CultureInfo.CurrentCulture, out double d))
                {
                    NumericValue = d;
                    IsValid      = true;
                }
                else
                {
                    NumericValue = null;
                    IsValid      = false;
                }
            }
        }

        /// <inheritdoc/>
        public double? NumericValue { get; private set; }

        /// <inheritdoc/>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Creates a new <see cref="NumericParameter"/>.
        /// </summary>
        /// <param name="prompt">Label shown next to the input.</param>
        /// <param name="initialValue">Starting value as a string (default: "0").</param>
        /// <param name="isEnabled">Whether the input is initially enabled (default: true).</param>
        public NumericParameter(string prompt, string initialValue = "0", bool isEnabled = true)
        {
            Prompt    = prompt;
            IsEnabled = isEnabled;
            Value     = initialValue;   // triggers parse via the property setter
        }
    }
}
