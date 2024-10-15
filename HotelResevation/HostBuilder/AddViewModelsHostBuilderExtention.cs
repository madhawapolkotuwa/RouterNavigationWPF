using HotelResevation.Router;
using HotelResevation.Services;
using HotelResevation.Stores;
using HotelResevation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotelResevation.HostBuilder
{
    public static class AddViewModelsHostBuilderExtention
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {

                services.AddSingleton<NavigationStore>();

                services.AddTransient<RoomListViewModel>();
                services.AddTransient<HotelListViewModel>();
                services.AddTransient<RoomDetailsViewModel>();

                services.AddSingleton<ViewModelRouter>((s) =>
                new ViewModelRouter(new Dictionary<string, INavigationService>
                {
                    {
                        "HotelListView" ,
                        new NavigationService<HotelListViewModel>(
                            s.GetRequiredService<NavigationStore>(),
                            () => s.GetRequiredService<HotelListViewModel>())
                    },
                    {
                        "RoomListView" ,
                        new NavigationService<RoomListViewModel>(
                            s.GetRequiredService<NavigationStore>(),
                            () => s.GetRequiredService<RoomListViewModel>())
                    },
                    {
                        "RoomDetailsView" ,
                        new NavigationService<RoomDetailsViewModel>(
                            s.GetRequiredService<NavigationStore>(),
                            () => s.GetRequiredService<RoomDetailsViewModel>())
                    }

                }));

            });

            return hostBuilder;
        }
    }
}
