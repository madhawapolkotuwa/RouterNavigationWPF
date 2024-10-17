using HotelResevation.Services;

namespace HotelResevation.Router
{
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
}
