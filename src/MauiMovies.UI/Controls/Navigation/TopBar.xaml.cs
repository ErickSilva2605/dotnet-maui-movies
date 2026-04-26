namespace MauiMovies.UI.Controls.Navigation;

public partial class TopBar : ContentView
{
	public event EventHandler? UserTapped;
	public event EventHandler? SearchTapped;

	public TopBar()
	{
		InitializeComponent();
	}

	void OnUserTapped(object sender, EventArgs e) =>
		UserTapped?.Invoke(this, EventArgs.Empty);

	void OnSearchTapped(object sender, EventArgs e) =>
		SearchTapped?.Invoke(this, EventArgs.Empty);
}
