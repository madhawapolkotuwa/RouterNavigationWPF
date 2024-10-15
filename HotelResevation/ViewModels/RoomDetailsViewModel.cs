using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HotelResevation.Models;
using HotelResevation.Router;
using HotelResevation.Services;
using HotelResevation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResevation.ViewModels
{
    public partial class RoomDetailsViewModel : BaseViewModel, IParameterNavigationService
    {
        private readonly ViewModelRouter _viewModelRouter;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private Room _itemRoom;

        [ObservableProperty]
        private string _hotelName;

        public RoomDetailsViewModel(ViewModelRouter viewModelRouter)
        {
            _viewModelRouter = viewModelRouter;
        }

        public void ParameterInitialize(params object[] parameters)
        {
            if(parameters != null && parameters.Length > 0)
            {
                if (parameters[0].GetType() == typeof(Room))
                {
                    ItemRoom = (Room)parameters[0];
                }

                if (parameters[1].GetType() == typeof(string))
                {
                    HotelName = (string)parameters[1];
                }
            } 
        }


        [RelayCommand]
        void Navigation(string view)
        {
            _viewModelRouter.NavigateTo(view);
        }
    }
}
