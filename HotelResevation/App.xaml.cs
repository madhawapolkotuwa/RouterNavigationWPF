using HotelResevation.HostBuilder;
using HotelResevation.Router;
using HotelResevation.Stores;
using HotelResevation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace HotelResevation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices(services => {

                    services.AddSingleton<HotelStore>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>((s) => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });

            
            }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var router = _host.Services.GetRequiredService<ViewModelRouter>();
            await router.AsyncNavigation("HotelListView");

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

    }


}
