
/* Alteração não mesclada do projeto 'MauiMovies.UI(net9.0-ios)'
Antes:
namespace MauiMovies.UI
{
	public partial class MainPage : ContentPage
	{
		int count = 0;

		public MainPage()
		{
			InitializeComponent();
		}

		private void OnCounterClicked(object sender, EventArgs e)
		{
			count++;

			if (count == 1)
				CounterBtn.Text = $"Clicked {count} time";
			else
				CounterBtn.Text = $"Clicked {count} times";

			SemanticScreenReader.Announce(CounterBtn.Text);
		}
	}
Após:
namespace MauiMovies.UI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
*/
namespace MauiMovies.UI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}