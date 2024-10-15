namespace HotelResevation.Services
{
    public interface INavigationService
    {
        void Navigate(params object[] parameters);

        Task AsyncNavigation(params object[] parameters);
    }
}
