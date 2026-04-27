using CommunityToolkit.Mvvm.ComponentModel;
using MauiMovies.Core.Enums;

namespace MauiMovies.Core.Models;

public partial class TvModel : MediaItemModel
{
	public override MediaType MediaType => MediaType.Tv;

	[ObservableProperty] string name = string.Empty;
	[ObservableProperty] string originalName = string.Empty;
	[ObservableProperty] string overview = string.Empty;
	[ObservableProperty] string? posterPath;
	[ObservableProperty] string? backdropPath;
	[ObservableProperty] string? firstAirDate;
	[ObservableProperty] double voteAverage;
	[ObservableProperty] int voteCount;
	[ObservableProperty] string? originalLanguage;
	[ObservableProperty] IReadOnlyList<int> genreIds = [];
	[ObservableProperty] IReadOnlyList<string> originCountry = [];
}
