﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class DoctorDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private DoctorDetailsModel doctor;

        public ICommand EditDoctorInfoRedirect { get; }

        public DoctorDetailsPageViewModel(DoctorDetailsModel doctor)
        {
            Doctor = doctor;
            this.EditDoctorInfoRedirect = new AsyncRelayCommand<object>(RedirectToEditInfo);
        }

        public async Task RedirectToEditInfo(object parameter)
        {
            if (parameter is int id)
            {
                var doctorJson = JsonConvert.SerializeObject(doctor);
                var encodedDoctorJson = Uri.EscapeDataString(doctorJson);
                await Shell.Current.GoToAsync($"///EditDoctorInfo?hospitalJson={encodedDoctorJson}");
            }
        }
    }
}
