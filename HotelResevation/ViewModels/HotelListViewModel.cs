using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelResevation.Models;
using HotelResevation.Router;
using HotelResevation.Services;
using HotelResevation.Stores;
using System.Collections.ObjectModel;

namespace HotelResevation.ViewModels
{
    public partial class HotelListViewModel : BaseViewModel, IAsyncNavigationService
    {
        private readonly HotelStore _hotelStore;
        private readonly ViewModelRouter _viewModelRouter;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<Hotel> _hotels;

        public HotelListViewModel(HotelStore hotelStore,ViewModelRouter viewModelRouter)
        {
            _hotels = new ObservableCollection<Hotel>();
            _hotelStore = hotelStore;
            _viewModelRouter = viewModelRouter;
            init();
        }

        void init()
        {
            if(_hotelStore.HotelList != null && _hotelStore.HotelList.Count() > 0)
            {
                foreach (var hotel in _hotelStore.HotelList)
                {
                    Hotels.Add(hotel);
                }
            }
        }

        [RelayCommand]
        async Task AsyncNavigation(string view)
        {
            IsLoading = true;
            await _viewModelRouter.AsyncNavigation(view, "Hotel ABCD", DateTime.Now);
            IsLoading = false;
        }

        [RelayCommand]
        async Task HotelClicked(int id)
        {
            Hotel hotel = _hotels.FirstOrDefault(h => h.HotelId == id);
            if(hotel != null)
            {
                try
                {
                    IsLoading = true;
                    await _viewModelRouter.AsyncNavigation("RoomListView", hotel);
                    IsLoading = false;
                }
                catch (Exception ex)
                {
                    IsLoading = false;
                    throw new Exception(ex.Message);
                }
            } 
        }


        [RelayCommand]
        void Navigation(string view)
        {
            _viewModelRouter.NavigateTo(view);
        }

        public async Task InitialzeAsync()
        {
            await LoadHotels();
        }

        private async Task LoadHotels()
        {
            try
            {
                await _hotelStore.Load();
                foreach (var hotel in _hotelStore.HotelList)
                {
                    Hotels.Add(hotel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
