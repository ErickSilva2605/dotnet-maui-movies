namespace MauiMovies.UI.Navigation;

static class Routes
{
	// Shell navigation primitives
	public const string Up = "..";

	// Visual hierarchy (registered as ShellContent in AppShell.xaml)
	public const string Main = "main";
	public const string Root = "//" + Main;

	// Modal / pushed routes (registered via Routing.RegisterRoute)
	public const string PreLogin = "preLogin";
	public const string Login = "login";
	public const string Profile = "profile";

	public const string MovieDetails = "movieDetails";
	public const string TvDetails = "tvDetails";
	public const string PersonDetails = "personDetails";

	// Query parameters
	public const string IdParameter = "id";
}
