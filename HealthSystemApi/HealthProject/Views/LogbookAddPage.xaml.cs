using HealthProject.ViewModels;
using System.Security.Cryptography;

namespace HealthProject.Views;

public partial class LogbookAddPage : ContentPage
{
    private LogbookAddViewModel viewModel;

    public LogbookAddPage(LogbookAddViewModel viewModel)
    {
        InitializeComponent();
        Title = "Добавяне на запис";
        BindingContext = this.viewModel = viewModel;
    }

    private void FrequencyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Temperature.IsVisible = FrequencyPicker.SelectedIndex == 0;
        BloodPressure.IsVisible = FrequencyPicker.SelectedIndex == 1;
        BloodSugar.IsVisible = FrequencyPicker.SelectedIndex == 2;
        Weight.IsVisible = FrequencyPicker.SelectedIndex == 3;
        Height.IsVisible = FrequencyPicker.SelectedIndex == 4;
        Ketones.IsVisible = FrequencyPicker.SelectedIndex == 5;
        Hydration.IsVisible = FrequencyPicker.SelectedIndex == 6;
        UrinePh.IsVisible = FrequencyPicker.SelectedIndex == 7;
        OxygenSaturation.IsVisible = FrequencyPicker.SelectedIndex == 8;
        RespiratoryRate.IsVisible = FrequencyPicker.SelectedIndex == 9;
        Menstruation.IsVisible = FrequencyPicker.SelectedIndex == 10;
        Cholesterol.IsVisible = FrequencyPicker.SelectedIndex == 11;
        HbA1c.IsVisible = FrequencyPicker.SelectedIndex == 12;
        PeakFlow.IsVisible = FrequencyPicker.SelectedIndex == 13;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (!int.TryParse(HealthIssueId.Text, out var hId))
        {
            await DisplayAlert("Error", "Please enter a valid health issue ID.", "OK");
            return;
        }

        var notes = Notes.Text;
        var values = new List<int>();
        var factors = new List<string>();

        try
        {
            switch (FrequencyPicker.SelectedIndex)
            {
                case 0:
                    values.Add(int.Parse(TemperatureInput.Text));
                    break;
                case 1:
                    values.Add(int.Parse(SystolicInput.Text));
                    values.Add(int.Parse(DiastolicInput.Text));
                    values.Add(int.Parse(PulseInput.Text));
                    factors.Add(GetPickerSelection(PositionPicker));
                    factors.Add(GetPickerSelection(SitePicker));
                    break;
                case 2:
                    values.Add(int.Parse(BloodSugarInput.Text));
                    break;
                case 3:
                    values.Add(int.Parse(WeightInput.Text));
                    break;
                case 4:
                    values.Add(int.Parse(HeightInput.Text));
                    break;
                case 5:
                    values.Add(int.Parse(KetonesInput.Text));
                    break;
                case 6:
                    values.Add(int.Parse(HydrationInput.Text));
                    break;
                case 7:
                    values.Add(int.Parse(UrinePhInput.Text));
                    break;
                case 8:
                    values.Add(int.Parse(OxygenSaturationInput.Text));
                    break;
                case 9:
                    values.Add(int.Parse(RespiratoryRateInput.Text));
                    break;
                case 10:
                    factors.Add(GetPickerSelection(FlowPicker));
                    factors.Add(GetPickerSelection(ColorPicker));
                    factors.Add(GetPickerSelection(ConsistencyPicker));
                    break;
                case 11:
                    values.Add(int.Parse(CholesterolInput.Text));
                    values.Add(int.Parse(HDLInput.Text));
                    values.Add(int.Parse(LDLInput.Text));
                    values.Add(int.Parse(TRIInput.Text));
                    break;
                case 12:
                    values.Add(int.Parse(HbA1cInput.Text));
                    break;
                case 13:
                    values.Add(int.Parse(PeakFlowInput.Text));
                    break;
            }
        }
        catch (FormatException)
        {
            await DisplayAlert("Error", "Please enter valid numeric values.", "OK");
            return;
        }

        var type = GetMeasurementType(FrequencyPicker.SelectedIndex);

        await viewModel.AddAsync(values, factors, notes, hId, type);
    }

    private string GetPickerSelection(Picker picker)
    {
        return picker.SelectedIndex >= 0 ? picker.Items[picker.SelectedIndex] : string.Empty;
    }

    private string GetMeasurementType(int index)
    {
        return index switch
        {
            0 => "Температура",
            1 => "Кръвно налягане",
            2 => "Кръвна захар",
            3 => "Тегло",
            4 => "Височина",
            5 => "Кетони",
            6 => "Хидратация",
            7 => "pH на урина",
            8 => "Кислородно засищане",
            9 => "Дихателна честота",
            10 => "Менструация",
            11 => "Холестерол",
            12 => "HbA1c",
            13 => "Пиков поток",
            _ => "Unknown",
        };
    }
}
