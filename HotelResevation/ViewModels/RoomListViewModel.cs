using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelResevation.Models;
using HotelResevation.Router;
using HotelResevation.Services;
using HotelResevation.Stores;
using System.Collections.ObjectModel;

namespace HotelResevation.ViewModels
{
    public partial class RoomListViewModel : BaseViewModel, IParameterNavigationService, IAsyncNavigationService
    {

        private readonly ViewModelRouter _viewModelRouter;
        private readonly HotelStore _hotelStore;
        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private Hotel _itemHotel;


        public RoomListViewModel(ViewModelRouter viewModelRouter, HotelStore hotelStore)
        {
            _viewModelRouter = viewModelRouter;
            _hotelStore = hotelStore;
            _itemHotel = _hotelStore.SelectedHotel;
        }

        public void ParameterInitialize(params object[] parameters)
        {
            // paramaters from Hotel List View model
            if (parameters != null && parameters.Length > 0)
            {
                if(parameters[0].GetType() == typeof(Hotel))
                {
                    ItemHotel = (Hotel)parameters[0];
                    _hotelStore.SelectedHotel = ItemHotel;
                }
            }
        }

        public async Task InitialzeAsync()
        {
            await Task.Delay(1000);
        }

        [RelayCommand]
        void ToRoomDetails(int roomId)
        {
            Room room = ItemHotel?.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            if(room != null)
            {
                _viewModelRouter.NavigateTo("RoomDetailsView", room, ItemHotel?.HotelName);
            }
        }

        [RelayCommand]
        void ToHotelList()
        {
            _viewModelRouter.NavigateTo("HotelListView");
        }

        
    }
}
