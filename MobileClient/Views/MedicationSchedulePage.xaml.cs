using System.Globalization;
using HealthProject.ViewModels;

namespace HealthProject.Views
{
    public partial class MedicationSchedulePage : ContentPage
    {
        private MedicationScheduleViewModel viewModel;
        private DateTime currentDate;
        private DateTime selectedDate;

        public MedicationSchedulePage(MedicationScheduleViewModel viewModel)
        {
            InitializeComponent();
            Title = "Лекарства";
            BindingContext = this.viewModel = viewModel;

            currentDate = DateTime.Today;
            selectedDate = DateTime.Today;  // Default selected date is today
            DisplayCalendar(currentDate);
        }

        private void DisplayCalendar(DateTime date)
        {
            // Update the month-year label
            monthYearLabel.Text = date.ToString("MMMM yyyy", CultureInfo.InvariantCulture);

            // Clear previous calendar days
            calendarGrid.Children.Clear();

            // Add day labels
            string[] daysOfWeek = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                var dayLabel = new Label
                {
                    Text = daysOfWeek[i],
                    HorizontalOptions = LayoutOptions.Center
                };
                calendarGrid.Children.Add(dayLabel);
                Grid.SetColumn(dayLabel, i);
                Grid.SetRow(dayLabel, 0);
            }

            // Get the first day of the month
            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            // Add day buttons to the grid
            for (int day = 1; day <= daysInMonth; day++)
            {
                int row = (day + startDayOfWeek - 1) / 7 + 1;
                int column = (day + startDayOfWeek - 1) % 7;

                DateTime currentDay = new DateTime(date.Year, date.Month, day);
                Button dayButton = new Button
                {
                    Text = day.ToString(),
                    BackgroundColor = currentDay == selectedDate ? Colors.LightBlue : Colors.LightGray,
                    CommandParameter = currentDay
                };

                dayButton.Clicked += OnDateSelected;

                calendarGrid.Children.Add(dayButton);
                Grid.SetColumn(dayButton, column);
                Grid.SetRow(dayButton, row);
            }
        }

        private void OnPreviousMonthClicked(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(-1);
            DisplayCalendar(currentDate);
        }

        private void OnNextMonthClicked(object sender, EventArgs e)
        {
            currentDate = currentDate.AddMonths(1);
            DisplayCalendar(currentDate);
        }

        private void OnDateSelected(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is DateTime date)
            {
                selectedDate = date;
                DisplayCalendar(currentDate); // Refresh the calendar to update selected date color
                viewModel.OnDateSelected(date);
            }
        }
    }
}
