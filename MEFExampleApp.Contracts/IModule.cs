using System.Windows;

namespace MEFExampleApp.Contracts
{
    /// <summary>
    /// Every plugin module must implement this interface and export it via MEF.
    /// The shell imports all IModule exports without knowing which concrete types exist.
    /// </summary>
    public interface IModule
    {
        /// <summary>Returns the WPF UIElement that represents this module's view.</summary>
        UIElement GetView();
    }
}
