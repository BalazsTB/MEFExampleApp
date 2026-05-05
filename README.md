# MEFExampleApp

A clean **MEF + WPF** reference application on **.NET Framework 4.8** that shows how to build a loosely-coupled, plugin-style architecture without any third-party frameworks.

---

## What MEF does in this sample

MEF (Managed Extensibility Framework, `System.ComponentModel.Composition`) discovers and wires up parts of an application at runtime using **exports** and **imports**.

- A class decorated with `[Export(typeof(IModule))]` says *"I am available as an IModule"*.
- A property or field decorated with `[ImportMany]` says *"give me everything exported as IModule"*.
- The **Shell never references the module assemblies** directly. Modules are simply DLLs placed in a `Plugins` folder; MEF discovers and instantiates them.

---

## Project structure

```
MEFExampleApp.sln
│
├── MEFExampleApp.Contracts          # Shared interfaces only – no UI, no logic
│   ├── IModule.cs                   # Contract every plugin must implement
│   └── IModuleMetadata.cs           # Typed metadata interface (Name, Description, Order)
│
├── MEFExampleApp.Shell              # WPF host – knows nothing about concrete modules
│   ├── Bootstrapper.cs              # MEF composition root
│   ├── MainViewModel.cs             # [ImportMany] all IModule exports
│   ├── ModuleEntry.cs               # View-friendly wrapper for module + metadata
│   ├── MainWindow.xaml/.cs          # Two-pane shell UI
│   └── App.xaml/.cs                 # Starts the bootstrapper
│
├── MEFExampleApp.Modules.Greeting   # Plugin #1 – name → greeting
│   ├── GreetingModule.cs            # [Export(typeof(IModule))] with metadata
│   ├── GreetingViewModel.cs         # Simple MVVM view-model
│   └── GreetingView.xaml/.cs        # WPF UserControl
│
└── MEFExampleApp.Modules.Calculator # Plugin #2 – four-operation calculator
    ├── CalculatorModule.cs
    ├── CalculatorViewModel.cs
    └── CalculatorView.xaml/.cs
```

**Dependency rule:** `Contracts ← Shell`, `Contracts ← Modules`. The Shell has **no reference** to any module project.

---

## How composition works

### 1 — Catalog

`Bootstrapper.cs` builds an `AggregateCatalog` from two sources:

```csharp
var catalog = new AggregateCatalog();
catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));   // shell parts
catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath));           // all DLLs in Plugins\
```

`DirectoryCatalog` scans every DLL in the `Plugins` folder and registers anything it finds that is decorated with MEF attributes.

### 2 — Export (module side)

Each module class declares itself with a few attributes:

```csharp
[Export(typeof(IModule))]
[ExportMetadata("Name",        "Greeting")]
[ExportMetadata("Description", "Enter a name and get a personalized hello.")]
[ExportMetadata("Order",       1)]
public class GreetingModule : IModule { ... }
```

`[ExportMetadata]` values are stored in the MEF catalog without constructing the object — the shell can read them cheaply via `Lazy<IModule, IModuleMetadata>`.

### 3 — Import (shell side)

`MainViewModel` asks MEF for every `IModule` export:

```csharp
[ImportMany]
private IEnumerable<Lazy<IModule, IModuleMetadata>> _lazyModules;
```

`Lazy<IModule, IModuleMetadata>` gives:
- `.Metadata` — read `Name`, `Description`, and `Order` **before** the module is instantiated.
- `.Value` — access the real module instance (created on first access).

After MEF fills the imports (`IPartImportsSatisfiedNotification.OnImportsSatisfied`), the view-model sorts by `Order` and exposes the list to the UI.

### 4 — Container

```csharp
var container = new CompositionContainer(catalog);
var viewModel  = container.GetExportedValue<MainViewModel>();
```

`GetExportedValue` resolves `MainViewModel` (itself exported with `[Export]`) and satisfies all its `[ImportMany]` fields in one call.

---

## Why this is advantageous

| Benefit | How it shows up here |
|---|---|
| **Loose coupling** | Shell has zero compile-time knowledge of modules |
| **Plugin architecture** | Drop a new DLL in `Plugins\` and restart — it appears automatically |
| **Independent deployment** | Modules can be versioned and shipped separately |
| **Metadata without instantiation** | Shell reads Name/Order before creating any module object |
| **Testability** | Contracts project is a plain interface library; modules and shell can be tested independently |

---

## How to run

### Prerequisites
- Windows
- [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net48)
- [.NET 6+ SDK](https://dotnet.microsoft.com/download) (for the `dotnet build` CLI) **or** Visual Studio 2019/2022

### Build and run

```bash
# Clone the repository
git clone https://github.com/BalazsTB/MEFExampleApp.git
cd MEFExampleApp

# Build (copies module DLLs to Plugins\ automatically)
dotnet build MEFExampleApp.sln --configuration Debug

# Launch the shell
MEFExampleApp.Shell\bin\Debug\net48\MEFExampleApp.Shell.exe
```

Or open `MEFExampleApp.sln` in Visual Studio and press **F5**.

The `CopyToPlugins` MSBuild target in each module `.csproj` copies the module DLL to  
`MEFExampleApp.Shell\bin\<Configuration>\net48\Plugins\` after every build.

---

## How to add a new module

1. **Create a new Class Library project** targeting `net48` with `<UseWPF>true</UseWPF>`.
2. **Add a project reference** to `MEFExampleApp.Contracts`.
3. **Create a `UserControl`** for your UI.
4. **Implement `IModule`** and decorate with export attributes:

```csharp
[Export(typeof(IModule))]
[ExportMetadata("Name",        "MyModule")]
[ExportMetadata("Description", "What my module does.")]
[ExportMetadata("Order",       3)]
public class MyModule : IModule
{
    public UIElement GetView()
    {
        var view = new MyView();
        view.DataContext = new MyViewModel();
        return view;
    }
}
```

5. **Add the `CopyToPlugins` build target** (copy from an existing module `.csproj`).
6. Build — your module appears in the shell's left-hand list automatically.

> **No changes to the Shell or Contracts projects are required.**
