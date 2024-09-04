using HealthProject.Models;
using HealthProject.ViewModels;
using Microsoft.Maui.Controls.Compatibility;

namespace HealthProject.Views;

[QueryProperty(nameof(PharmacyId), "pharmacyId")]
public partial class PharmacyProductsPage : ContentPage, IQueryAttributable
{
    private string pharmacyId;

    public string PharmacyId
    {
        get => pharmacyId;
        set
        {
            pharmacyId = value;
            if (viewModel != null && int.TryParse(pharmacyId, out int id))
            {
                viewModel.PharmacyId = id;
                LoadData(); 
            }
        }
    }

    private readonly PharmacyProductsViewModel viewModel;

    public PharmacyProductsPage(PharmacyProductsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("pharmacyId", out var pharmacyIdObj))
        {
            PharmacyId = pharmacyIdObj as string;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrEmpty(PharmacyId) && int.TryParse(PharmacyId, out int id))
        {
            viewModel.PharmacyId = id;
            LoadData(); 
        }
    }

    private void LoadData()
    {
        viewModel.GetPharmacyMedications();
        viewModel.GetUserCart();
    }

    private void OnAddToCartClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button == null)
            return;

        // Use FindByName to locate the Entry within the current view
        var entry = this.FindByName<Entry>("quantityEntry");

        if (entry != null && int.TryParse(entry.Text, out int quantity))
        {
            var product = button.BindingContext as PharmacyProductDisplayModel;

            if (product != null)
            {
                var parameter = (product, quantity);
                var viewModel = BindingContext as PharmacyProductsViewModel;
                viewModel?.AddToCartCommand.Execute(parameter);
            }
        }
    }
}
