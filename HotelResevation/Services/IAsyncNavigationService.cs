using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelResevation.Services
{
    public interface IAsyncNavigationService
    {
        Task InitialzeAsync();
    }
}
