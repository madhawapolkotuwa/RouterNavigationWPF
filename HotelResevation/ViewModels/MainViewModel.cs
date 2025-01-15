using CommunityToolkit.Mvvm.ComponentModel;
using HotelResevation.Stores;

namespace HotelResevation.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        [ObservableProperty]
        private BaseViewModel _currentViewModel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsOpen))]
        private BaseViewModel _currentModalViewModel;

        public bool IsOpen => _modalNavigationStore.IsOpen;

        public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;

            CurrentViewModel = _navigationStore.CurrentViewModel;
            CurrentModalViewModel = _modalNavigationStore.CurrentModalViewModel;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChnaged;
        }

        private void OnCurrentModalViewModelChnaged()
        {
            CurrentModalViewModel = _modalNavigationStore.CurrentModalViewModel;
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
        }

    }
}
