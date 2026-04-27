using CommunityToolkit.Mvvm.ComponentModel;
using MauiMovies.Core.Enums;

namespace MauiMovies.Core.Models;

public partial class MovieModel : MediaItemModel
{
	public override MediaType MediaType => MediaType.Movie;

	[ObservableProperty] string title = string.Empty;
	[ObservableProperty] string originalTitle = string.Empty;
	[ObservableProperty] string overview = string.Empty;
	[ObservableProperty] string? posterPath;
	[ObservableProperty] string? backdropPath;
	[ObservableProperty] string? releaseDate;
	[ObservableProperty] double voteAverage;
	[ObservableProperty] int voteCount;
	[ObservableProperty] string? originalLanguage;
	[ObservableProperty] IReadOnlyList<int> genreIds = [];
	[ObservableProperty] bool video;
}
