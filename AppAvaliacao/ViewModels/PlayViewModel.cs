using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.Movies;
using System.Net;
using System.Web;

namespace AppAvaliacao.ViewModels;

public partial class PlayViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    MediaElement _video = new();

    [ObservableProperty]
    Assessments _assessment;

    [ObservableProperty]
    string _isBook;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        IsBook = null;
        Video.Source = null;

        Assessments assessment = (Assessments)query["Data"];
        if (assessment.Category != "Book")
        {
            string pathFile = Load(assessment.Name, AppSettings.PathMovie);
            Video.Source = MediaSource.FromFile(pathFile);
        }
        else
        {
            string pathFile = Load(assessment.Name, AppSettings.PathBook);
            IsBook = "file:///" + pathFile;
        }
    }

    private string Load(string name, string path)
    {
        string[] fileEntries = Directory.GetFiles(path);
        foreach (string pathFile in fileEntries)
        {
            string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathFile);
            if (FileNameWithoutExtension.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                if (FileNameWithoutExtension.Contains('#'))
                {
                    var fileName = Path.GetFileName(pathFile);
                    fileName = WebUtility.UrlEncode(fileName);
                    path = Path.Combine(path, fileName);

                    return path;
                }
                return pathFile;
            }
        }
        return null;
    }

    [RelayCommand]
    async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }
}

