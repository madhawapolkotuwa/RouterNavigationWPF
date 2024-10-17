# Building a Router-Based Navigation System in WPF Using ViewModel Routing

## Introduction
- In this post, we’ll walk through building a router-based navigation system in WPF using ViewModel routing. This method will help you manage navigation between views using ViewModels and also allow for asynchronous navigation and parameter passing between ViewModels.
-------------

## 1. Overview of the Navigation System

We will create a router-based navigation system that will:

* Route between multiple views using ViewModels.
* Pass parameters to ViewModels during navigation.
* Support asynchronous initialization of ViewModels.

The system includes:

* A `ViewModelRouter` class to handle navigation.
* `INavigationService` and supporting interfaces for synchronous and asynchronous navigation.
* A `NavigationService<TViewModel>` to register ViewModels and manage the routing process.
* A `NavigationStore` to hold the current ViewModel for display in the view.

## 2. Creating the Core Interfaces
We start by defining two core interfaces that will manage parameter passing and asynchronous initialization in our ViewModels.

* **IParameterNavigationService**
    
    This interface allows a ViewModel to accept parameters when it is being navigated to.
```csharp
public interface IParameterNavigationService
{
    void ParameterInitialize(params object[] parameters);
}
```
* **IAsyncNavigationService**
    
    This interface supports asynchronous initialization for any ViewModel that requires time-consuming setup, such as fetching data from a server.
```csharp
public interface IAsyncNavigationService
{
    Task InitialzeAsync();
}
```
* **INavigationService**

    The navigation system is driven by this interface, which defines the methods for synchronous and asynchronous navigation.
```csharp
public interface INavigationService
{
    void Navigate(params object[] parameters);
    Task AsyncNavigation(params object[] parameters);
}
```

## 3. Building the `NavigationService<TViewModel>`

Next, we define the `NavigationService<TViewModel>`, which will handle the creation of a new ViewModel and manage the initialization of parameters or asynchronous tasks when navigating.
```csharp
public class NavigationService<TViewModel> : INavigationService where TViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate(params object[] parameters)
    {
        var viewModel = _createViewModel();

        if (parameters.Length > 0 && viewModel is IParameterNavigationService paramViewModel)
        {
            paramViewModel.ParameterInitialize(parameters);
        }

        _navigationStore.CurrentViewModel = viewModel;
    }

    public async Task AsyncNavigation(params object[] parameters)
    {
        var viewModel = _createViewModel();

        if (parameters.Length > 0 && viewModel is IParameterNavigationService paramViewModel)
        {
            paramViewModel.ParameterInitialize(parameters);
        }

        if (viewModel is IAsyncNavigationService asyncViewModel)
        {
            await asyncViewModel.InitialzeAsync();
        }

        _navigationStore.CurrentViewModel = viewModel;
    }
}
```
## 4. Setting Up the `ViewModelRouter`
The `ViewModelRouter` acts as the central piece, routing to the correct ViewModel based on a provided URL and handling both synchronous and asynchronous navigation.
```csharp
public class ViewModelRouter
{
    private readonly IDictionary<string, INavigationService> _routes;

    public ViewModelRouter(IDictionary<string, INavigationService> routes)
    {
        _routes = routes;
    }

    public void NavigateTo(string url, params object[] parameters)
    {
        if (_routes.ContainsKey(url))
        {
            _routes[url].Navigate(parameters);
        }
    }

    public async Task AsyncNavigation(string url, params object[] parameters)
    {
        if (_routes.ContainsKey(url))
        {
            await _routes[url].AsyncNavigation(parameters);
        }
    }
}
```

## 5. The Navigation Store
To manage which ViewModel is currently active, we use a `NavigationStore` class that holds the current ViewModel and triggers a view update when changed.
```csharp
public class NavigationStore
{
    private BaseViewModel _currentViewModel;

    public BaseViewModel CurrentViewModel
    {
        get { return _currentViewModel; }
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    public event Action CurrentViewModelChanged;

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}
```
6. Creating the ViewModels
Each ViewModel that participates in navigation should implement the appropriate interfaces as needed. For example, the SomeViewModel implements both `IParameterNavigationService` and `IAsyncNavigationService` to handle parameters and asynchronous initialization.
```csharp
public partial class SomeViewModel : BaseViewModel, IParameterNavigationService, IAsyncNavigationService
{
    public async Task InitialzeAsync()
    {
        await Task.Delay(1000);  // Simulate async work.
    }

    public void ParameterInitialize(params object[] parameters)
    {
        // Use parameters as needed.
    }
}
```
## 7. Dependency Injection Setup
Next, we configure Dependency Injection using a `HostBuilder`. This registers the ViewModels and the router in the service collection.
```csharp
services.AddSingleton<NavigationStore>();

services.AddTransient<SomeViewModel1>();
services.AddTransient<SomeViewModel2>();
services.AddTransient<SomeViewModel3>();

services.AddSingleton<ViewModelRouter>((s) =>
    new ViewModelRouter(new Dictionary<string, INavigationService>
    {
        { 
            "SomeView1", 
            new NavigationService<SomeViewModel1>(
                s.GetRequiredService<NavigationStore>(), 
                () => s.GetRequiredService<SomeViewModel1>()) 
        },
        { 
            "SomeView2", 
            new NavigationService<SomeViewModel2>(
                s.GetRequiredService<NavigationStore>(), 
                () => s.GetRequiredService<SomeViewModel2>()) 
        },
        { 
            "SomeView3", 
            new NavigationService<SomeViewModel3>(
                s.GetRequiredService<NavigationStore>(), 
                () => s.GetRequiredService<SomeViewModel3>()) 
        }
    }));
```
## 8. Binding the ViewModels to the Views
We bind the current ViewModel from the `NavigationStore` to the view in the XAML using `ContentControl` and DataTemplates for each ViewModel.
- MainWindow XAML
```xml
<Grid>
    <Grid.Resources>
        <DataTemplate DataType="{x:Type vm:SomeViewModel1}">
            <view:SomeView1/>
        </DataTemplate>
        <DataTemplate DataType="{x:Type vm:SomeViewModel2}">
            <view:SomeView2/>
        </DataTemplate>
        <DataTemplate DataType="{x:Type vm:SomeViewModel3}">
            <view:SomeView3/>
        </DataTemplate>
    </Grid.Resources>

    <ContentControl Content="{Binding CurrentViewModel}" />
</Grid>
```
- Main View Model
```csharp
public partial class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    [ObservableProperty] // from CommunityToolkit MVVM
    private BaseViewModel _currentViewModel;

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        CurrentViewModel = _navigationStore.CurrentViewModel;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModel = _navigationStore.CurrentViewModel;
    }
}
```
## Conclusion
By implementing a `ViewModelRouter`, `INavigationService`, and `NavigationStore`, you’ve created a powerful, flexible, and asynchronous navigation system for WPF applications. This system allows for easy parameter passing and async initialization, making it ideal for modern applications that require dynamic view management.



