using AppAvaliacao.Components;
using AppAvaliacao.ViewModels;
using Models.Movies;

namespace AppAvaliacao.Pages;

public partial class DetailsPage : ContentPage
{
    private readonly DetailsViewModel _viewModel;
    public DetailsPage(DetailsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    private async void Edit(object sender, EventArgs e)
    {
        var formComponent = new FormComponent();
        var frame = formComponent.AddFrameToGrid();

        await frame.ScaleTo(0, 0, Easing.CubicInOut);
        await frame.TranslateTo(-30, -105, 0, Easing.CubicInOut);

        DetailPage.Add(frame, 0, 0);

        await Task.WhenAll
            (
                frame.ScaleTo(1, 550, Easing.CubicInOut),
                frame.TranslateTo(0, 0, 550, Easing.CubicInOut)
            );
    }

    private void IsVibleComments(object sender, EventArgs e)
    {
        CommentsDisplay.IsVisible = !CommentsDisplay.IsVisible;
    }
}
