using System.Windows;

namespace MEFExampleApp.Contracts
{
    /// <summary>
    /// Every plugin module must implement this interface and export it via MEF.
    /// The shell imports all IModule exports without knowing which concrete types exist.
    ///
    /// ViewModel-first pattern
    /// ───────────────────────
    /// Instead of returning a UIElement, a module exposes two things:
    ///   • GetViewModel()  – the plain data / logic object (no UI coupling).
    ///   • GetResources()  – a ResourceDictionary that contains an *implicit DataTemplate*
    ///                       mapping the ViewModel type → the View UserControl.
    ///
    /// The shell merges those dictionaries into Application.Current.Resources once at
    /// startup, then simply puts the ViewModel into a ContentPresenter.  WPF looks up
    /// the matching DataTemplate automatically and renders the correct View — the shell
    /// never references a View type directly and the View never needs a code-behind
    /// constructor that sets DataContext.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Returns the ViewModel that drives this module.
        /// The returned object is set as the Content of a ContentPresenter in the shell;
        /// WPF resolves the correct View via the DataTemplate registered in GetResources().
        /// </summary>
        object GetViewModel();

        /// <summary>
        /// Returns a ResourceDictionary that contains at least one implicit DataTemplate
        /// whose DataType is the ViewModel type returned by GetViewModel().
        /// The shell merges this dictionary into Application.Current.Resources before
        /// any content is rendered, so WPF can find the template automatically.
        /// </summary>
        ResourceDictionary GetResources();
    }
}
