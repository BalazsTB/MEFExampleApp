using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MEFExampleApp.Contracts;

namespace MEFExampleApp.Shell
{
    /// <summary>
    /// Shell view-model.
    ///
    /// [ImportMany] asks MEF to inject every exported IModule it found, wrapped in
    /// Lazy&lt;IModule, IModuleMetadata&gt; so we can read metadata (name, order…)
    /// before the module object is even constructed.
    ///
    /// [Export] makes this class itself discoverable by the container (see Bootstrapper).
    /// </summary>
    [Export]
    public class MainViewModel : INotifyPropertyChanged, IPartImportsSatisfiedNotification
    {
        // MEF fills this collection with every [Export(typeof(IModule))] it discovers.
        [ImportMany]
        private IEnumerable<Lazy<IModule, IModuleMetadata>> _lazyModules = null;

        private readonly ObservableCollection<ModuleEntry> _modules = new ObservableCollection<ModuleEntry>();
        public ObservableCollection<ModuleEntry> Modules => _modules;

        private ModuleEntry _selectedModule;
        public ModuleEntry SelectedModule
        {
            get => _selectedModule;
            set
            {
                _selectedModule = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedView));
            }
        }

        public UIElement SelectedView => _selectedModule?.Module.GetView();

        /// <summary>Called by MEF after all imports have been satisfied.</summary>
        public void OnImportsSatisfied()
        {
            // Sort by Order metadata so the list is predictable.
            foreach (var lazy in _lazyModules.OrderBy(m => m.Metadata.Order))
            {
                _modules.Add(new ModuleEntry(lazy.Value, lazy.Metadata));
            }

            SelectedModule = _modules.FirstOrDefault();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
