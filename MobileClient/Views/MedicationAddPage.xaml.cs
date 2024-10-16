using HealthProject.ViewModels;

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

    private MedicationAddViewModel viewModel;

    public MedicationAddPage(MedicationAddViewModel viewModel)
    {
        InitializeComponent();
        Title = "�������� �� ���������";
        BindingContext = this.viewModel = viewModel;
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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (FrequencyPicker.SelectedIndex == 1)
        {
            var stackLayout = this.FindByName<VerticalStackLayout>("TimesStackLayout");
            var times = new List<TimeSpan>();

            foreach (var child in stackLayout.Children)
            {
                if (child is HorizontalStackLayout horizontalLayout)
                {
                    foreach (var innerChild in horizontalLayout.Children)
                    {
                        if (innerChild is TimePicker timePicker)
                        {
                            times.Add(timePicker.Time);
                        }
                    }
                }
            }

            var days = new List<DayOfWeek>();
            days.Add(DayOfWeek.Monday);
            days.Add(DayOfWeek.Tuesday);
            days.Add(DayOfWeek.Wednesday);
            days.Add(DayOfWeek.Thursday);
            days.Add(DayOfWeek.Friday);
            days.Add(DayOfWeek.Saturday);
            days.Add(DayOfWeek.Sunday);


            await viewModel.AddAsync(times, 0, days, 0, 0);
        }
        else if (FrequencyPicker.SelectedIndex == 2)
        {
            var stackLayout = this.FindByName<VerticalStackLayout>("TimesPerDayStackLayout");
            var times = new List<TimeSpan>();

            foreach (var child in stackLayout.Children)
            {
                if (child is HorizontalStackLayout horizontalLayout)
                {
                    foreach (var innerChild in horizontalLayout.Children)
                    {
                        if (innerChild is TimePicker timePicker)
                        {
                            times.Add(timePicker.Time);
                        }
                    }
                }
            }

            var skipCount = int.Parse(DaysXLabel.Text);

            await viewModel.AddAsync(times, skipCount, new List<DayOfWeek>(), 0, 0);
        }
        else if (FrequencyPicker.SelectedIndex == 3)
        {
            var stackLayout = this.FindByName<VerticalStackLayout>("TimesPerDaySpecificStackLayout");
            var times = new List<TimeSpan>();

            foreach (var child in stackLayout.Children)
            {
                if (child is HorizontalStackLayout horizontalLayout)
                {
                    foreach (var innerChild in horizontalLayout.Children)
                    {
                        if (innerChild is TimePicker timePicker)
                        {
                            times.Add(timePicker.Time);
                        }
                    }
                }
            }

            var days = new List<DayOfWeek>();

            if (MondayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Monday);
            }
            if (TuesdayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Tuesday);
            }
            if (WednesdayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Wednesday);
            }
            if (ThursdayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Thursday);
            }
            if (FridayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Friday);
            }
            if (SaturdayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Saturday);
            }
            if (SundayCheckBox.IsChecked)
            {
                days.Add(DayOfWeek.Sunday);
            }

            await viewModel.AddAsync(times, 0, days, 0, 0);
        }
        else if (FrequencyPicker.SelectedIndex == 4)
        {
            var stackLayout = this.FindByName<VerticalStackLayout>("TimesPerDayXYStackLayout");
            var times = new List<TimeSpan>();

            foreach (var child in stackLayout.Children)
            {
                if (child is HorizontalStackLayout horizontalLayout)
                {
                    foreach (var innerChild in horizontalLayout.Children)
                    {
                        if (innerChild is TimePicker timePicker)
                        {
                            times.Add(timePicker.Time);
                        }
                    }
                }
            }

            var take = int.Parse(DaysXIntakeLabel.Text);
            var rest = int.Parse(DaysYRestLabel.Text);

            await viewModel.AddAsync(times, 0, new List<DayOfWeek>(), take, rest);
        }
        else
        {
            await viewModel.AddAsync(new List<TimeSpan>(), 0, new List<DayOfWeek>(), 0, 0);
        }
    }
}
