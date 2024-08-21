using HealthProject.Models;
using HealthProject.Services.LogbookService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(LogJson), "logJson")]
public partial class LogbookEditPage : ContentPage
{
    private string logJson;
    private ILogbookService logbookService;
    private LogbookEditViewModel viewModel;
    public string LogJson
    {
        get => logJson;
        set
        {
            logJson = value;
            var log = JsonConvert.DeserializeObject<LogAddModel>(Uri.UnescapeDataString(value));
            BindingContext = this.viewModel = new LogbookEditViewModel(log, logbookService);
        }
    }
    public LogbookEditPage(ILogbookService logbookService)
	{
		InitializeComponent();
        Title = "Редактиране на запис";
        this.logbookService = logbookService;
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
        var hId = int.Parse(HealthIssueId.Text);
        var notes = Notes.Text;

        var values = new List<int>();
        var factors = new List<string>();

        if (FrequencyPicker.SelectedIndex == 0)
        {
            values.Add(int.Parse(TemperatureInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 1)
        {
            values.Add(int.Parse(SystolicInput.Text));
            values.Add(int.Parse(DiastolicInput.Text));
            values.Add(int.Parse(PulseInput.Text));

            var posIndex = PositionPicker.SelectedIndex;

            switch (posIndex)
            {
                case 0:
                    factors.Add("Седнал");
                    break;
                case 1:
                    factors.Add("Легнал");
                    break;
                case 2:
                    factors.Add("Прав");
                    break;
            }

            var siteIndex = SitePicker.SelectedIndex;

            switch (siteIndex)
            {
                case 0:
                    factors.Add("Лява ръка");
                    break;
                case 1:
                    factors.Add("Дясна ръка");
                    break;
                case 2:
                    factors.Add("Лява китка");
                    break;
                case 3:
                    factors.Add("Дясна китка");
                    break;
                case 4:
                    factors.Add("Ляво бедро");
                    break;
                case 5:
                    factors.Add("Дясно бедро");
                    break;
                case 6:
                    factors.Add("Ляв прасец");
                    break;
                case 7:
                    factors.Add("Десен прасец");
                    break;
                case 8:
                    factors.Add("Ляв глезен");
                    break;
                case 9:
                    factors.Add("Десен глезен");
                    break;
                default:
                    break;
            }
        }
        else if (FrequencyPicker.SelectedIndex == 2)
        {
            values.Add(int.Parse(BloodSugarInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 3)
        {
            values.Add(int.Parse(WeightInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 4)
        {
            values.Add(int.Parse(HeightInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 5)
        {
            values.Add(int.Parse(KetonesInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 6)
        {
            values.Add(int.Parse(HydrationInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 7)
        {
            values.Add(int.Parse(UrinePhInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 8)
        {
            values.Add(int.Parse(OxygenSaturationInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 9)
        {
            var flowIndex = FlowPicker.SelectedIndex;

            switch (flowIndex)
            {
                case 0:
                    factors.Add("Лек");
                    break;
                case 1:
                    factors.Add("Среден");
                    break;
                case 2:
                    factors.Add("Тежък");
                    break;
                case 3:
                    factors.Add("Неочакван");
                    break;
            }

            var colorIndex = ColorPicker.SelectedIndex;

            switch (colorIndex)
            {
                case 0:
                    factors.Add("Черен");
                    break;
                case 1:
                    factors.Add("Кафяв");
                    break;
                case 2:
                    factors.Add("Тъмно червен");
                    break;
                case 3:
                    factors.Add("Светло червен");
                    break;
                case 4:
                    factors.Add("Розов");
                    break;
                case 5:
                    factors.Add("Оранжев");
                    break;
                case 6:
                    factors.Add("Сив");
                    break;
            }

            var consistencyIndex = ConsistencyPicker.SelectedIndex;

            switch (flowIndex)
            {
                case 0:
                    factors.Add("Воднист");
                    break;
                case 1:
                    factors.Add("Съсиреци");
                    break;
            }
        }
        else if (FrequencyPicker.SelectedIndex == 10)
        {
            values.Add(int.Parse(CholesterolInput.Text));
            values.Add(int.Parse(HDLInput.Text));
            values.Add(int.Parse(LDLInput.Text));
            values.Add(int.Parse(TRIInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 11)
        {
            values.Add(int.Parse(HbA1cInput.Text));
        }
        else if (FrequencyPicker.SelectedIndex == 12)
        {
            values.Add(int.Parse(PeakFlowInput.Text));
        }

        var typeIndex = FrequencyPicker.SelectedIndex;
        var type = string.Empty;

        switch (typeIndex)
        {
            case 0:
                type = "Температура";
                break;
            case 1:
                type = "Кръвно налягане";
                break;
            case 2:
                type = "Кръвна захар";
                break;
            case 3:
                type = "Тегло";
                break;
            case 4:
                type = "Височина";
                break;
            case 5:
                type = "Кетони";
                break;
            case 6:
                type = "Хидратация";
                break;
            case 7:
                type = "pH на урина";
                break;
            case 8:
                type = "Кислородно засищане";
                break;
            case 9:
                type = "Дихателна честота";
                break;
            case 10:
                type = "Менструация";
                break;
            case 11:
                type = "Холестерол";
                break;
            case 12:
                type = "HbA1c";
                break;
            case 13:
                type = "Пиков поток";
                break;
            default:
                type = "Unknown";
                break;
        }

        await viewModel.EditAsync(values, factors, notes, hId, type);
    }
}