using MauiMovies.Core.Enums;

namespace MauiMovies.Core.Entities;

public class Person : MediaItem
{
	public override MediaType MediaType => MediaType.Person;

	public required string Name { get; init; }
	public string OriginalName { get; init; } = string.Empty;
	public string? ProfilePath { get; init; }
	public string? KnownForDepartment { get; init; }
	public Gender Gender { get; init; }
}
