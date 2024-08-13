using AppAvaliacao.Components;
using AppAvaliacao.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;

namespace AppAvaliacao.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

        WeakReferenceMessenger.Default.Register<string>(this, async (e, msg) =>
        {
            if (msg.Equals("Add"))
            {
                await ShowForm();
            }
        });
    }

    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        var image = sender as Image;
        if (image == null)
            return;

        AnimationScaleTo(image, 1.2);
    }

    private void OnPointerExited(object sender, PointerEventArgs e)
    {
        var image = sender as Image;
        if (image == null)
            return;

        AnimationScaleTo(image);
    }
    private async void AnimationScaleTo(Image image, double cale = 1)
    {
        uint duration = 550;
        await image.ScaleTo(cale, duration, Easing.CubicInOut);
    }

    private async void AddAssessment(object sender, EventArgs e)
    {
        await ShowForm();

       
    }

    private async Task ShowForm()
    {
        var formComponent = new FormComponent();
        var frame = formComponent.AddFrameToGrid();

        await frame.ScaleTo(0, 0, Easing.CubicInOut);
        await frame.TranslateTo(-30, -105, 0, Easing.CubicInOut);

        GridContainer.Add(frame, 0, 0);

        await Task.WhenAll
            (
                frame.ScaleTo(1, 550, Easing.CubicInOut),
                frame.TranslateTo(0, 0, 550, Easing.CubicInOut)
            );
    }

    private async void SelectPage(object sender, EventArgs e)
    {
        if (BindingContext is HomeViewModel viewModel)
        {
           await viewModel.SelectPage();
        }
    }
}