using AppAvaliacao.Components;
using AppAvaliacao.Helpers.DownloadHelper;
using AppAvaliacao.Helpers.ExcelHerper;
using AppAvaliacao.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Models;
using Models.Movies;
using Repository.Interfaces;
using System.Threading;

namespace AppAvaliacao.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly RestService _restService;
    private readonly IAssessmentsRepository _assessmentsRepository;
    private readonly IFileSaver _fileSaver;

    [ObservableProperty]
    IEnumerable<CardHome> _cardsHome;

    [ObservableProperty]
    Assessments _assessment = new();

    string _filter;

    public HomeViewModel(IAssessmentsRepository assessmentsRepository, IFileSaver fileSaver, RestService restService)
    {
        _assessmentsRepository = assessmentsRepository;
        _fileSaver = fileSaver;
        _restService = restService;


        FormComponent.SaveCommand = new Command(async () =>
        {
            await Save();
        });

        FormComponent.CloseCommand = new Command(async () =>
        {
            var formComponent = new FormComponent();
            await formComponent.CloseForm();
        });

        Get();
    }
    async Task Save()
    {
        var cancellationToken = new CancellationToken();
        try
        {
            if (Assessment.Assessment is not null)
            {
                await _assessmentsRepository.PostAsync(Assessment);
                var formComponent = new FormComponent();
                await formComponent.CloseForm();
                Assessment = new();

                if (_filter != "Api")
                {
                    await Get();
                }

                await Toast.Make($"A avaliação foi salva com sucesso.").Show(cancellationToken);
            }
            else
            {
                await Shell.Current.DisplayAlert("Nota", "Nota não foi preenchida", "Ok");
            }
        }
        catch (Exception e)
        {
            await Toast.Make($"Error ao salvar a avaliação: {e.Message}.").Show(cancellationToken);
        }
    }

    private async Task Get()
    {
        _filter = "home";
        CardsHome = await _assessmentsRepository.GetCardsHome();
    }

    [RelayCommand]
    async Task Detail(int id)
    {
        if (_filter != "Api")
        {

            try
            {
                await Shell.Current.GoToAsync($"DetailsPage?Id={id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} - {ex.StackTrace} - {ex.InnerException.Message}");
            }

            return;
        }

        Assessment = await _restService.GetAssessmentsAsync(id);
        WeakReferenceMessenger.Default.Send<string>("Add");
    }
    [RelayCommand]
    async Task Filter(string filter)
    {
        _filter = filter;
        if (_filter == "All")
        {
            CardsHome = await _assessmentsRepository.GetCardsHome();

            return;
        }

        if (_filter == "Api")
        {
            var cards = await _restService.GetMovies();
            var carsHome = new List<CardHome>();

            foreach (var card in cards)
            {
                var cardHome = new CardHome(card.Id, $"{AppSettings.ImageBaseUrl}{card.poster_path}");

                carsHome.Add(cardHome);
            }

            CardsHome = carsHome;
            return;
        }

        CardsHome = await _assessmentsRepository.GetFilterAsync(filter);
    }
    [RelayCommand]
    async Task DowloandFile(string fileName)
    {
        var assessments = await _assessmentsRepository.GetAllAssessments();
        var stream = DownloadFileHerper.DownloadFile(fileName, assessments);

        var cancellationToken = new CancellationToken();
        try
        {
            var fileLocationResult = await _fileSaver.SaveAsync(fileName, stream, cancellationToken);
            fileLocationResult.EnsureSuccess();
            await stream.DisposeAsync();
            await Toast.Make($"Arquivo salvo em: {fileLocationResult.FilePath}").Show(cancellationToken);
        }
        catch (Exception ex)
        {
            await Toast.Make($"O arquivo não foi salvo: {ex.Message}").Show(cancellationToken);
        }
    }
    [RelayCommand]
    async Task UploadFile(CancellationToken cancellationToken)
    {
        try
        {
            var uploadExcelHelper = new UploadExcelHelper();
            IEnumerable<Assessments> assessments = await uploadExcelHelper.ReadExcel();
            await _assessmentsRepository.PostAll(assessments);
            await Toast.Make($"Arquivo foi carregado com sucesso.").Show(cancellationToken);
        }
        catch (Exception ex)
        {
            await Toast.Make($"Erro ao carregar arquivo: {ex.Message}").Show(cancellationToken);
        }
    }

    [RelayCommand]
    async Task SearchTmdb(string text)
    {
        if (_filter != "Api")
        {
            CardsHome = await _assessmentsRepository.GetNameAsync(text);
        }
    }
}
