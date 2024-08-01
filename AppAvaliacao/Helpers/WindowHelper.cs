using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System.ComponentModel;

namespace AppAvaliacao.Helpers;

public class WindowHelper
{
    public void OnToggleFullscreenClicked(bool isPlayPage = true)
    {
        var window = App.Current.MainPage.GetParentWindow().Handler.PlatformView as MauiWinUIWindow;

        var appWindow = GetAppWindow(window);

        switch (appWindow.Presenter)
        {
            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                if (isPlayPage)
                {
                    overlappedPresenter.Restore();
                    overlappedPresenter.SetBorderAndTitleBar(false, false);
                    overlappedPresenter.Maximize();
                }
                else
                {
                    overlappedPresenter.Restore();
                    overlappedPresenter.SetBorderAndTitleBar(true, true);
                    overlappedPresenter.Maximize();
                }

                break;
        }
    }
    private Microsoft.UI.Windowing.AppWindow GetAppWindow(MauiWinUIWindow window)
    {
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
        return appWindow;
    }
}
