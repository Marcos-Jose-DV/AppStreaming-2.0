using System.Windows.Input;

namespace AppAvaliacao.Components;

public class FormComponent
{
    public FormComponent()
    {

    }
    private static Frame _frame;
    public static ICommand SaveCommand { get; set; }
    public static ICommand CloseCommand { get; set; }

    public Frame AddFrameToGrid()
    {
        _frame = new Frame
        {
            ZIndex = 2,
            Margin = 10,
            MinimumWidthRequest = 600,
            MaximumWidthRequest = 1200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromRgba("#041218"),
            BorderColor = Colors.White,
            Content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width =  100},
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width =  100},
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Auto }, // row 0
                        new RowDefinition { Height = GridLength.Auto }, // row 1
                        new RowDefinition { Height = GridLength.Auto }, // row 2
                        new RowDefinition { Height = GridLength.Auto }, // row 3
                        new RowDefinition { Height = GridLength.Auto }, // row 4
                        new RowDefinition { Height = GridLength.Auto }, // row 5
                        new RowDefinition { Height = GridLength.Auto }, // row 6
                        new RowDefinition { Height = 270 }, // row 7
                        new RowDefinition { Height = GridLength.Auto }, // row 8
                    },
                ColumnSpacing = 11,
                RowSpacing = 11,
            }
        };

        Grid.SetRowSpan(_frame, 0);

        SetAssessment();

        return _frame;
    }
    private void SetAssessment()
    {
        var grid = GetTitle();

        var AddFrameEndInput = AddInput("Assessment.Name", new Entry());

        grid.AddWithSpan(AddFrameEndInput, 0, 1, 1, 3);

        AddFrameEndInput = AddInput("Assessment.Assessment", new Entry());
        grid.Add(AddFrameEndInput, 1, 1);

        AddFrameEndInput = AddInput("Assessment.Director", new Entry());
        grid.Add(AddFrameEndInput, 3, 1);


        var buttonPicker = new Button();
        buttonPicker.Text = "Selecione a capa";
        buttonPicker.Clicked += ButtonPicker_Clicked;
        AddFrameEndInput = AddInput("Assessment.ImagePath", new Entry());
        grid.AddWithSpan(new VerticalStackLayout
        {
            Spacing = 10,
            Children = { buttonPicker, AddFrameEndInput }

        }, 2, 1, 1, 3);


        buttonPicker = new Button();
        buttonPicker.Text = "Selecione o fundo";
        buttonPicker.Clicked += ButtonPicker_Clicked;
        AddFrameEndInput = AddInput("Assessment.ImagePathBackDrop", new Entry());
        grid.AddWithSpan(new VerticalStackLayout
        {
            Spacing = 10,
            Children = { buttonPicker, AddFrameEndInput }

        }, 3, 1, 1, 3);


        AddFrameEndInput = AddInput("Assessment.Gender", new Entry());
        grid.Add(AddFrameEndInput, 1, 4);

        AddFrameEndInput = AddInput("Assessment.Duration", new Entry(), BindingMode.OneWay);
        grid.Add(AddFrameEndInput, 3, 4);


        AddFrameEndInput = AddInput("Assessment.Launch", new DatePicker());
        grid.Add(AddFrameEndInput, 1, 5);
        AddFrameEndInput = AddInput("Assessment.Category", new Picker());
        grid.Add(AddFrameEndInput, 3, 5);

        var checkBox = new CheckBox();
        checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("Assessment.Concluded"));
        grid.Add(checkBox, 1, 6);


        AddFrameEndInput = AddInput("Assessment.Comments", new Editor());
        grid.AddWithSpan(AddFrameEndInput, 7, 1, 1, 3);


        grid.Add(new Button
        {
            Text = "Salvar",
            BackgroundColor = Colors.Blue,
            BorderWidth = 0,
            Command = SaveCommand,
        }, 0, 8);

        grid.AddWithSpan(new Button
        {
            Text = "Cancelar",
            BorderWidth = 0,
            HorizontalOptions = LayoutOptions.End,
            Command = CloseCommand
        }, 8, 1, 1, 3);
    }
    private async void ButtonPicker_Clicked(object? sender, EventArgs e)
    {
        var button = (Button)sender;
        var verticalStackLayout = (VerticalStackLayout)button.Parent;
        var frame = (Frame)verticalStackLayout.Children[1];
        var entry = (Entry)frame.Children[0];

        await PickerFileAsync(entry);
    }
    private static async Task PickerFileAsync(Entry entry)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Selecione a imagem",
            FileTypes = FilePickerFileType.Images
        });

        if (result is not null)
        {
            entry.Text = result.FullPath;
        }
    }
    private static Frame AddInput(string Assessment, dynamic input, BindingMode mode = BindingMode.Default)
    {
        if (input is Entry)
        {
            input.ReturnType = ReturnType.Next;
            input.Placeholder = Assessment.Split('.')[1];
            input.SetBinding(Entry.TextProperty, new Binding(Assessment, mode));
        }
        else if (input is Picker)
        {
            var categories = new string[] { "Book", "Série", "Movie", "Music" };
            input.ItemsSource = categories;
            input.SelectedIndex = 1;
            input.SetBinding(Microsoft.Maui.Controls.Picker.SelectedItemProperty, new Binding(Assessment));
        }
        else if (input is DatePicker)
        {
            input.Date = DateTime.UtcNow;
            input.SetBinding(DatePicker.DateProperty, new Binding(Assessment));
        }
        else if (input is Editor)
        {
            input.Placeholder = Assessment.Split('.')[1];
            input.PlaceholderColor = Colors.White;
            input.SetBinding(Editor.TextProperty, new Binding(Assessment));
        }

        return AddFrame(input);
    }
    private static Frame AddFrame(dynamic input)
    {
        var frame = new Frame
        {
            BorderColor = Colors.Gray,
            BackgroundColor = Colors.Transparent,
            CornerRadius = 0,
            Padding = new Thickness(0),
            HasShadow = true,
            Content = input
        };

        return frame;
    }
    private Grid GetTitle()
    {
        var grid = _frame.Content as Grid;

        grid.Add(new Label { Text = "Nome" }, 0, 0);
        grid.Add(new Label { Text = "Nota" }, 0, 1);
        grid.Add(new Label { Text = "Direção" }, 2, 1);
        grid.Add(new Label { Text = "Imagem capa" }, 0, 2);
        grid.Add(new Label { Text = "Imagem fundo" }, 0, 3);
        grid.Add(new Label { Text = "Genero" }, 0, 4);
        grid.Add(new Label { Text = "Duração" }, 2, 4);
        grid.Add(new Label { Text = "Lanchamento" }, 0, 5);
        grid.Add(new Label { Text = "Categoria" }, 2, 5);
        grid.Add(new Label { Text = "Concluido" }, 0, 6);
        grid.Add(new Label { Text = "Comentario", VerticalOptions = LayoutOptions.Start }, 0, 7);

        return grid;
    }
    public async Task CloseForm()
    {
        await Task.WhenAll
        (
            _frame.ScaleTo(0, 550, Easing.CubicInOut),
            _frame.TranslateTo(-30, -105, 550, Easing.CubicInOut)
        );

        _frame.IsVisible = false;
        _frame.Content = null;
    }
}
