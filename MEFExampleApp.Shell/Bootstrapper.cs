using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Windows;

namespace MEFExampleApp.Shell
{
    /// <summary>
    /// Composition root: creates the MEF catalog and container, then shows the shell.
    ///
    /// AggregateCatalog lets us combine multiple sources:
    ///   • AssemblyCatalog(Shell) – picks up MainViewModel and other shell parts.
    ///   • DirectoryCatalog(Plugins) – discovers any module DLL dropped into the folder.
    ///
    /// Nothing in the shell references module types directly; MEF resolves them at runtime.
    /// </summary>
    public class Bootstrapper
    {
        public void Run()
        {
            // ── 1. Build the catalog ──────────────────────────────────────────────
            var catalog = new AggregateCatalog();

            // Shell assembly (MainViewModel is exported from here)
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));

            // Plugins folder – any DLL placed here is automatically discovered
            string pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            Directory.CreateDirectory(pluginsPath);
            catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath));

            // ── 2. Create the container ───────────────────────────────────────────
            var container = new CompositionContainer(catalog);

            // ── 3. Resolve the main view-model (satisfies its [ImportMany]) ───────
            var viewModel = container.GetExportedValue<MainViewModel>();

            // ── 4. Show the shell ─────────────────────────────────────────────────
            var window = new MainWindow { DataContext = viewModel };
            Application.Current.MainWindow = window;
            window.Show();
        }
    }
}
