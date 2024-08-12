using AppAvaliacao.Components;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models.Movies;
using Repository.Interfaces;
using Repository.Repositoreis;

namespace AppAvaliacao.ViewModels;

public partial class DetailsViewModel : ObservableObject, IQueryAttributable
{
    private readonly IAssessmentsRepository _assessmentsRepository;
    [ObservableProperty]
    Assessments _assessment;

    [ObservableProperty]
    string[] _categories;

    public DetailsViewModel(IAssessmentsRepository assessmentsRepository)
    {
        FormComponent.SaveCommand = new Command(async () =>
        {
            await Save();
        });

        FormComponent.CloseCommand = new Command(async () =>
        {
            var formComponent = new FormComponent();
            await formComponent.CloseForm();
        });

        _assessmentsRepository = assessmentsRepository;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Assessment = new();
        var id = query["Id"];

        Assessment = await _assessmentsRepository.GetByIdAsync(Convert.ToInt32(id));
    }
    async Task Save()
    {
        var assessment = await _assessmentsRepository.UpdateAsync(Assessment);
        Assessment = new();
        Assessment = assessment;
        var formComponent = new FormComponent();
        await formComponent.CloseForm();
    }

    [RelayCommand]
    async Task Play()
    {
        var data = new Dictionary<string, object>()
        {
            {"Data", Assessment},
        };
        await Shell.Current.GoToAsync($"PlayPage", data);
    }
}
