namespace MauiMovies.Core.Interfaces;

public interface INavigationParametersAware
{
	void ApplyParameters(IDictionary<string, object> parameters);
}
