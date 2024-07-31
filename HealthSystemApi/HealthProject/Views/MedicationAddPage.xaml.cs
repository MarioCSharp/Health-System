namespace HealthProject.Views;

public partial class MedicationAddPage : ContentPage
{
    int timesDaily = 1;
    int daysX = 1;
    int timesPerDay = 1;
    int daysXIntake = 1;
    int daysYRest = 1;
    int timesPerDayXY = 1;
    int timesPerDaySpecific = 1;

    public MedicationAddPage()
    {
        InitializeComponent();
        FrequencyPicker.SelectedIndex = 0;
    }

    private void FrequencyPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        DailyIntakeView.IsVisible = FrequencyPicker.SelectedIndex == 1;
        XDayIntakeView.IsVisible = FrequencyPicker.SelectedIndex == 2;
        XYDaysIntakeView.IsVisible = FrequencyPicker.SelectedIndex == 4;
        SpecificDaysView.IsVisible = FrequencyPicker.SelectedIndex == 3;
    }

    private void IncreaseTimes_Clicked(object sender, EventArgs e)
    {
        timesDaily++;
        UpdateTimes();
    }

    private void DecreaseTimes_Clicked(object sender, EventArgs e)
    {
        if (timesDaily > 1)
        {
            timesDaily--;
            UpdateTimes();
        }
    }

    private void IncreaseDaysX_Clicked(object sender, EventArgs e)
    {
        daysX++;
        DaysXLabel.Text = daysX.ToString();
    }

    private void DecreaseDaysX_Clicked(object sender, EventArgs e)
    {
        if (daysX > 1)
        {
            daysX--;
            DaysXLabel.Text = daysX.ToString();
        }
    }

    private void IncreaseTimesPerDay_Clicked(object sender, EventArgs e)
    {
        timesPerDay++;
        UpdateTimesPerDay();
    }

    private void DecreaseTimesPerDay_Clicked(object sender, EventArgs e)
    {
        if (timesPerDay > 1)
        {
            timesPerDay--;
            UpdateTimesPerDay();
        }
    }

    private void IncreaseDaysXIntake_Clicked(object sender, EventArgs e)
    {
        daysXIntake++;
        DaysXIntakeLabel.Text = daysXIntake.ToString();
    }

    private void DecreaseDaysXIntake_Clicked(object sender, EventArgs e)
    {
        if (daysXIntake > 1)
        {
            daysXIntake--;
            DaysXIntakeLabel.Text = daysXIntake.ToString();
        }
    }

    private void IncreaseDaysYRest_Clicked(object sender, EventArgs e)
    {
        daysYRest++;
        DaysYRestLabel.Text = daysYRest.ToString();
    }

    private void DecreaseDaysYRest_Clicked(object sender, EventArgs e)
    {
        if (daysYRest > 1)
        {
            daysYRest--;
            DaysYRestLabel.Text = daysYRest.ToString();
        }
    }

    private void IncreaseTimesPerDayXY_Clicked(object sender, EventArgs e)
    {
        timesPerDayXY++;
        UpdateTimesPerDayXY();
    }

    private void DecreaseTimesPerDayXY_Clicked(object sender, EventArgs e)
    {
        if (timesPerDayXY > 1)
        {
            timesPerDayXY--;
            UpdateTimesPerDayXY();
        }
    }

    private void IncreaseTimesPerDaySpecific_Clicked(object sender, EventArgs e)
    {
        timesPerDaySpecific++;
        UpdateTimesPerDaySpecific();
    }

    private void DecreaseTimesPerDaySpecific_Clicked(object sender, EventArgs e)
    {
        if (timesPerDaySpecific > 1)
        {
            timesPerDaySpecific--;
            UpdateTimesPerDaySpecific();
        }
    }

    private void UpdateTimes()
    {
        TimesLabel.Text = timesDaily.ToString();
        TimesStackLayout.Children.Clear();
        for (int i = 0; i < timesDaily; i++)
        {
            TimesStackLayout.Children.Add(CreateTimePicker(i));
        }
    }

    private void UpdateTimesPerDay()
    {
        TimesPerDayLabel.Text = timesPerDay.ToString();
        TimesPerDayStackLayout.Children.Clear();
        for (int i = 0; i < timesPerDay; i++)
        {
            TimesPerDayStackLayout.Children.Add(CreateTimePicker(i));
        }
    }

    private void UpdateTimesPerDayXY()
    {
        TimesPerDayXYLabel.Text = timesPerDayXY.ToString();
        TimesPerDayXYStackLayout.Children.Clear();
        for (int i = 0; i < timesPerDayXY; i++)
        {
            TimesPerDayXYStackLayout.Children.Add(CreateTimePicker(i));
        }
    }

    private void UpdateTimesPerDaySpecific()
    {
        TimesPerDaySpecificLabel.Text = timesPerDaySpecific.ToString();
        TimesPerDaySpecificStackLayout.Children.Clear();
        for (int i = 0; i < timesPerDaySpecific; i++)
        {
            TimesPerDaySpecificStackLayout.Children.Add(CreateTimePicker(i));
        }
    }

    private HorizontalStackLayout CreateTimePicker(int index)
    {
        var timePicker = new TimePicker
        {
            Time = new TimeSpan(7 + index * 1, 0, 0) // Example times
        };

        var stackLayout = new HorizontalStackLayout();
        stackLayout.Children.Add(timePicker);

        return stackLayout;
    }
}
