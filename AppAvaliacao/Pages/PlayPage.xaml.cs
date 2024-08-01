using AppAvaliacao.Helpers;
using AppAvaliacao.ViewModels;
using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace AppAvaliacao.Pages;

public partial class PlayPage : ContentPage
{
    public PlayPage()
    {
        InitializeComponent();
        BindingContext = new PlayViewModel();

#if WINDOWS
        var windowHelper = new WindowHelper();
        windowHelper.OnToggleFullscreenClicked();
#endif

        mediaElement.PropertyChanged += MediaElement_PropertyChanged;
        mediaElement.PositionChanged += MediaElement_PositionChanged;

        SliderVolume.Value = mediaElement.Volume * 100;
    }
    void ContentPage_Unloaded(object? sender, EventArgs e)
    {
#if WINDOWS
         var windowHelper = new WindowHelper();
        windowHelper.OnToggleFullscreenClicked(false);
#endif

        // Stop and cleanup MediaElement when we navigate away
        mediaElement.Handler?.DisconnectHandler();
    }
    private void MediaElement_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
        {
            PositionSlider.Maximum = mediaElement.Duration.TotalSeconds;
        }
    }
    private void MediaElement_PositionChanged(object? sender, CommunityToolkit.Maui.Core.Primitives.MediaPositionChangedEventArgs e)
    {
        PositionSlider.Value = e.Position.TotalSeconds;
    }
    private void OnStartAndPause(object sender, EventArgs e)
    {
        if (mediaElement.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
            mediaElement.Pause();
        else if (mediaElement.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Paused)
            mediaElement.Play();
    }
    private void Slider_DragStarted(object sender, EventArgs e)
    {
        mediaElement.Pause();
    }
    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        mediaElement.Play();
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        double newValue = e.NewValue;
        mediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);
        (displayLabelPosition.Text, displayLabelDurantion.Text) = StringFormatTime();
    }
    private (string, string) StringFormatTime()
    {
        var position = String.Format("{0}:{1}:{2}",
                   mediaElement.Position.Hours > 9 ? mediaElement.Position.Hours : "0" + mediaElement.Position.Hours
                 , mediaElement.Position.Minutes > 9 ? mediaElement.Position.Minutes : "0" + mediaElement.Position.Minutes
                 , mediaElement.Position.Seconds > 9 ? mediaElement.Position.Seconds : "0" + mediaElement.Position.Seconds);

        var remainingTime = mediaElement.Duration - mediaElement.Position;
        var duration = String.Format("{0}:{1}:{2}",
               remainingTime.Hours > 9 ? remainingTime.Hours : "0" + remainingTime.Hours
             , remainingTime.Minutes > 9 ? remainingTime.Minutes : "0" + remainingTime.Minutes
             , remainingTime.Seconds > 9 ? remainingTime.Seconds : "0" + remainingTime.Seconds);

        return (position, duration);
    }

    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        GridDisplay.IsVisible = true;
    }

    private void OnPointerExited(object sender, PointerEventArgs e)
    {

        GridDisplay.IsVisible = false;
    }

    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        GridDisplay.IsVisible = true;
    }
    private void Slider_Volume(object sender, ValueChangedEventArgs e)
    {
        mediaElement.Volume = e.NewValue / 100;
    }
}
