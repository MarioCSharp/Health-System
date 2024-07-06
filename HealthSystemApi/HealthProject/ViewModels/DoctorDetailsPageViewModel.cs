﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.ServiceService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class DoctorDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private DoctorDetailsModel doctor;

        public ICommand EditDoctorInfoRedirect { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand AddServiceRedirect { get; }
        private IServiceService serviceService;

        [ObservableProperty]
        private ObservableCollection<ServiceModel> services;

        public DoctorDetailsPageViewModel(DoctorDetailsModel doctor,
                                          IServiceService serviceService)
        {
            Doctor = doctor;
            this.EditDoctorInfoRedirect = new AsyncRelayCommand<object>(RedirectToEditInfo);
            NavigateBackCommand = new AsyncRelayCommand(OnNavigateBack);
            AddServiceRedirect = new AsyncRelayCommand<object>(RedirectToAddService);
            LoadServices();
        }

        private async void LoadServices()
        {
            var servicesList = await serviceService.AllByIdAsync(doctor.Id);
            Services = new ObservableCollection<ServiceModel>(servicesList);
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

        private async Task OnNavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task RedirectToAddService(object parameter)
        {
            if (parameter is int id)
            {
                var doctorJson = JsonConvert.SerializeObject(new DoctorPassModel { Id = id });
                var encodedDoctorJson = Uri.EscapeDataString(doctorJson);
                await Shell.Current.GoToAsync($"///AddServicePage?doctorJson={encodedDoctorJson}");
            }
        }
    }
}
