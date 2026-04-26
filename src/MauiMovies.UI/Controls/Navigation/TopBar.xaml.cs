using System.Windows.Input;

namespace MauiMovies.UI.Controls.Navigation;

public partial class TopBar : ContentView
{
	public static readonly BindableProperty UserCommandProperty =
		BindableProperty.Create(nameof(UserCommand), typeof(ICommand), typeof(TopBar));

	public static readonly BindableProperty SearchCommandProperty =
		BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(TopBar));

	public ICommand? UserCommand
	{
		get => (ICommand?)GetValue(UserCommandProperty);
		set => SetValue(UserCommandProperty, value);
	}

	public ICommand? SearchCommand
	{
		get => (ICommand?)GetValue(SearchCommandProperty);
		set => SetValue(SearchCommandProperty, value);
	}

	public TopBar()
	{
		InitializeComponent();
	}
}
