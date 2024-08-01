#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Platform;
#endif

namespace ConfigureScreenComponent.FullScreen;

public class FulScreenWindowsComponent : MauiWinUIWindow
{
    public static void OnToggleFullscreenClicked(Microsoft.Maui.Controls.Page page, bool isPlayPage = true)
    {
        var window = page.GetParentWindow().Handler.PlatformView as MauiWinUIWindow;

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
                    overlappedPresenter.SetBorderAndTitleBar(true, true);
                    //overlappedPresenter.Restore();
                }

                break;
        }
    }
#if WINDOWS
    private Microsoft.UI.Windowing.AppWindow GetAppWindow(MauiWinUIWindow window)
    {
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
        return appWindow;
    }
#endif
}
