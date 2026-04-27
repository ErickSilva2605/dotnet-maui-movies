using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Entities;
using MauiMovies.Core.Mapping;
using MauiMovies.Core.Models;
using MauiMovies.Core.UseCases;

namespace MauiMovies.Core.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
	readonly GetTrendingAllUseCase getTrendingAllUseCase;

	public HomeViewModel(GetTrendingAllUseCase getTrendingAllUseCase)
	{
		this.getTrendingAllUseCase = getTrendingAllUseCase;
	}

	[ObservableProperty] bool isTrendingLoading;
	[ObservableProperty] string? trendingError;
	[ObservableProperty] string? welcomeBackdropPath;

	public ObservableCollection<MediaItemModel> Trending { get; } = [];

	public override Task OnAppearingAsync() =>
		Trending.Count == 0 ? LoadTrendingAsync() : Task.CompletedTask;

	[RelayCommand]
	async Task LoadTrendingAsync()
	{
		IsTrendingLoading = true;
		TrendingError = null;

		var result = await getTrendingAllUseCase.ExecuteAsync();

		if (result.IsSuccess && result.Value is { } items)
		{
			Trending.Clear();

			foreach (var item in items)
				if (item.ToModel() is { } model)
					Trending.Add(model);

			WelcomeBackdropPath = PickRandomBackdrop(items);
		}
		else
		{
			TrendingError = result.Error;
		}

		IsTrendingLoading = false;
	}

	static string? PickRandomBackdrop(IReadOnlyList<MediaItem> items)
	{
		var candidates = items
			.Select(item => item switch
			{
				Movie m => m.BackdropPath,
				Tv t => t.BackdropPath,
				_ => null,
			})
			.Where(path => !string.IsNullOrEmpty(path))
			.ToList();

		return candidates.Count == 0
			? null
			: candidates[Random.Shared.Next(candidates.Count)];
	}
}
