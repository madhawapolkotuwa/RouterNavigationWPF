using CommunityToolkit.Mvvm.Input;
using HotelResevation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResevation.ViewModels.ModalViewModels
{
    public partial class MakeAReservationModalViewModel : BaseViewModel
    {
        private readonly ModalNavigationStore _modalNavigationStore;

        public MakeAReservationModalViewModel(ModalNavigationStore modalNavigationStore)
        {
            _modalNavigationStore = modalNavigationStore;
        }

        [RelayCommand]
        void Close()
        {
            _modalNavigationStore.Close();
        }

    }
}
