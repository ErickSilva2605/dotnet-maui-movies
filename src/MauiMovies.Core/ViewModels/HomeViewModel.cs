using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiMovies.Core.Entities;
using MauiMovies.Core.Enums;
using MauiMovies.Core.Interfaces.Services;
using MauiMovies.Core.Mapping;
using MauiMovies.Core.Models;
using MauiMovies.Core.UseCases;

namespace MauiMovies.Core.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
	readonly GetTrendingAllUseCase getTrendingAllUseCase;
	readonly INavigationService navigationService;

	IReadOnlyList<MediaItem> trendingDay = [];
	IReadOnlyList<MediaItem> trendingWeek = [];

	public HomeViewModel(GetTrendingAllUseCase getTrendingAllUseCase, INavigationService navigationService)
	{
		this.getTrendingAllUseCase = getTrendingAllUseCase;
		this.navigationService = navigationService;
	}

	[ObservableProperty] bool isTrendingLoading;
	[ObservableProperty] string? trendingError;
	[ObservableProperty] string? welcomeBackdropPath;
	[ObservableProperty] string? trendingBackdropPath;
	[ObservableProperty] TimeWindow selectedTimeWindow = TimeWindow.Day;
	[ObservableProperty] MediaItemModel? trendingSelectedItem;

	public ObservableCollection<MediaItemModel> Trending { get; } = [];

	public bool IsDaySelected => SelectedTimeWindow == TimeWindow.Day;
	public bool IsWeekSelected => SelectedTimeWindow == TimeWindow.Week;

	public override Task OnAppearingAsync() =>
		Trending.Count == 0 ? LoadTrendingAsync() : Task.CompletedTask;

	[RelayCommand]
	async Task LoadTrendingAsync()
	{
		IsTrendingLoading = true;
		TrendingError = null;

		var dayTask = getTrendingAllUseCase.ExecuteAsync(TimeWindow.Day);
		var weekTask = getTrendingAllUseCase.ExecuteAsync(TimeWindow.Week);

		await Task.WhenAll(dayTask, weekTask);

		var dayResult = dayTask.Result;
		var weekResult = weekTask.Result;

		if (dayResult.IsSuccess && dayResult.Value is { } dayItems)
			trendingDay = dayItems;

		if (weekResult.IsSuccess && weekResult.Value is { } weekItems)
			trendingWeek = weekItems;

		if (!dayResult.IsSuccess && !weekResult.IsSuccess)
			TrendingError = dayResult.Error ?? weekResult.Error;

		PopulateTrendingFromCache();
		WelcomeBackdropPath = PickRandomBackdrop(trendingDay);
		TrendingBackdropPath = PickHighestRatedBackdrop(CurrentItems);

		IsTrendingLoading = false;
	}

	[RelayCommand]
	void SelectDayWindow() => ApplyTimeWindow(TimeWindow.Day);

	[RelayCommand]
	void SelectWeekWindow() => ApplyTimeWindow(TimeWindow.Week);

	void ApplyTimeWindow(TimeWindow timeWindow)
	{
		if (SelectedTimeWindow == timeWindow)
			return;

		SelectedTimeWindow = timeWindow;
		PopulateTrendingFromCache();
		TrendingBackdropPath = PickHighestRatedBackdrop(CurrentItems);
	}

	[RelayCommand]
	async Task NavigateToMediaItemAsync(MediaItemModel? model)
	{
		if (model is null)
			return;

		var item = model;
		TrendingSelectedItem = null;

		await (item switch
		{
			MovieModel m => navigationService.NavigateToMovieDetailsAsync(m.Id),
			TvModel t => navigationService.NavigateToTvDetailsAsync(t.Id),
			PersonModel p => navigationService.NavigateToPersonDetailsAsync(p.Id),
			_ => Task.CompletedTask,
		});
	}

	partial void OnSelectedTimeWindowChanged(TimeWindow value)
	{
		OnPropertyChanged(nameof(IsDaySelected));
		OnPropertyChanged(nameof(IsWeekSelected));
	}

	IReadOnlyList<MediaItem> CurrentItems =>
		SelectedTimeWindow == TimeWindow.Week ? trendingWeek : trendingDay;

	void PopulateTrendingFromCache()
	{
		Trending.Clear();

		foreach (var item in CurrentItems)
			if (item.ToModel() is { } model)
				Trending.Add(model);
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

	static string? PickHighestRatedBackdrop(IReadOnlyList<MediaItem> items) =>
		items
			.Select(item => item switch
			{
				Movie m => (Path: m.BackdropPath, Rating: m.VoteAverage),
				Tv t => (Path: t.BackdropPath, Rating: t.VoteAverage),
				_ => (Path: null, Rating: 0d),
			})
			.Where(x => !string.IsNullOrEmpty(x.Path))
			.OrderByDescending(x => x.Rating)
			.Select(x => x.Path)
			.FirstOrDefault();
}
