using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.PharmacyService;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class PharmacyProductsViewModel : ObservableObject
    {
        [ObservableProperty]
        private int pharmacyId;

        [ObservableProperty]
        private CartModel userCart;

        [ObservableProperty]
        private ObservableCollection<PharmacyProductDisplayModel> pharmacyProducts;

        private readonly IPharmacyService pharmacyService;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string userLocation;

        [ObservableProperty]
        private string userPhoneNumber;

        [ObservableProperty]
        private bool isOrderFormVisible;

        public ICommand AddToCartCommand { get; }
        public ICommand ToggleOrderFormCommand { get; }
        public ICommand SubmitOrderCommand { get; }
        public ICommand RemoveFromCartCommand { get; }

        public PharmacyProductsViewModel(IPharmacyService pharmacyService)
        {
            this.pharmacyService = pharmacyService;

            AddToCartCommand = new AsyncRelayCommand<(PharmacyProductDisplayModel product, int quantity)>(async (param) => await AddToCart(param.product, param.quantity));
            ToggleOrderFormCommand = new RelayCommand(ToggleOrderForm);
            SubmitOrderCommand = new AsyncRelayCommand(SubmitOrder);
            RemoveFromCartCommand = new AsyncRelayCommand<int>(async (cartItemId) => await RemoveFromCart(cartItemId));
        }

        partial void OnPharmacyIdChanged(int value)
        {
            if (value > 0)
            {
                GetUserCart();
                GetPharmacyMedications();
            }
        }

        public async void GetUserCart()
        {
            var cart = await pharmacyService.GetUserCart(PharmacyId);
            UserCart = cart;
        }

        public async void GetPharmacyMedications()
        {
            var medications = await pharmacyService.GetAllProducts(PharmacyId);
            PharmacyProducts = new ObservableCollection<PharmacyProductDisplayModel>(medications);
        }

        private async Task AddToCart(PharmacyProductDisplayModel product, int quantity)
        {
            var success = await pharmacyService.AddToCart(product.Id, UserCart.Id, product.UserEnteredQuantity);

            if (success)
            {
                GetUserCart();
            }
        }

        private void ToggleOrderForm()
        {
            IsOrderFormVisible = !IsOrderFormVisible;
        }

        private async Task SubmitOrder()
        {
            var order = new SubmitOrderModel
            {
                Name = UserName,
                Location = UserLocation,
                PhoneNumber = UserPhoneNumber,
                CartId = UserCart.Id
            };

            var success = await pharmacyService.SubmitOrder(order);

            if (success)
            {
                IsOrderFormVisible = false;
                GetUserCart();
            }
        }
        private async Task RemoveFromCart(int cartItemId)
        {
            var success = await pharmacyService.RemoveFromCart(cartItemId);

            if (success)
            {
                GetUserCart(); 
            }
        }
    }
}
