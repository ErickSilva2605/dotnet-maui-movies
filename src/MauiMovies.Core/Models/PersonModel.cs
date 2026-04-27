using CommunityToolkit.Mvvm.ComponentModel;
using MauiMovies.Core.Enums;

namespace MauiMovies.Core.Models;

public partial class PersonModel : MediaItemModel
{
	public override MediaType MediaType => MediaType.Person;

	[ObservableProperty] string name = string.Empty;
	[ObservableProperty] string originalName = string.Empty;
	[ObservableProperty] string? profilePath;
	[ObservableProperty] string? knownForDepartment;
	[ObservableProperty] Gender gender;
}
