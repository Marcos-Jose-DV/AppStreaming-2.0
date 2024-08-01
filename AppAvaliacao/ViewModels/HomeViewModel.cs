using AppAvaliacao.Components;
using AppAvaliacao.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;
using Models.Movies;
using Repository.Interfaces;
using System.Collections.ObjectModel;

namespace AppAvaliacao.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IAssessmentsRepository _assessmentsRepository;
    [ObservableProperty]
    IEnumerable<CardHome> _cardsHome;

    [ObservableProperty]
    Assessments _assessment = new();

    public HomeViewModel(IAssessmentsRepository assessmentsRepository)
    {
        _assessmentsRepository = assessmentsRepository;
        FormComponent.SaveCommand = new Command(async () =>
        {
            await Save();
        });

        FormComponent.CloseCommand = new Command(async () =>
        {
            var formComponent = new FormComponent();
            await formComponent.CloseForm();
        });

        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Get();
        });


        Get();
    }
    async Task Save()
    {
        await _assessmentsRepository.PostAsync(Assessment);
        var formComponent = new FormComponent();
        await formComponent.CloseForm();

        Assessment = new();
        await Get();
    }

    private async Task Get()
    {
        CardsHome = await _assessmentsRepository.GetAllAsync();
    }

    [RelayCommand]
    async Task Detail(int id)
    {
        try
        {
            await Shell.Current.GoToAsync($"DetailsPage?Id={id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message} - {ex.StackTrace} - {ex.InnerException.Message}");
        }
    }
    [RelayCommand]
    async Task Filter(string filter)
    {
        if (filter != "All")
        {
            CardsHome = await _assessmentsRepository.GetFilterAsync(filter);

            return;
        }

        CardsHome = await _assessmentsRepository.GetAllAsync();
    }

}
