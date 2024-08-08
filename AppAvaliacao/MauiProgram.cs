using AppAvaliacao.Pages;
using AppAvaliacao.Services;
using AppAvaliacao.ViewModels;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using DataBase.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Repository.Interfaces;
using Repository.Repositoreis;


namespace AppAvaliacao;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkitMediaElement()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });


        builder.Services.AddSingleton<IAssessmentsRepository, AssessmentsRepository>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<DetailsPage>();
        builder.Services.AddSingleton<DetailsViewModel>();
        builder.Services.AddScoped<AppDbContext>();
        builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
        builder.Services.AddTransient<HttpClient>();
        builder.Services.AddSingleton<RestService>();

#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(windowsLifecycleBuilder =>
            {
                windowsLifecycleBuilder.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = false;
                    var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                    switch (appWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.SetBorderAndTitleBar(true, true);
                            overlappedPresenter.Maximize();
                            overlappedPresenter.IsMaximizable = false;
                            break;
                    }
                });
            });
        });
#endif

        Routing.RegisterRoute("DetailsPage", typeof(DetailsPage));
        Routing.RegisterRoute("PlayPage", typeof(PlayPage));

#if DEBUG
        builder.Logging.AddDebug();
#endif


        return builder.Build();
    }
}
