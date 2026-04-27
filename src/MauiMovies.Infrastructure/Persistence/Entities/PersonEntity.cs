using MauiMovies.Core.Enums;

namespace MauiMovies.Infrastructure.Persistence.Entities;

public class PersonEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string OriginalName { get; set; } = string.Empty;
	public string? ProfilePath { get; set; }
	public string? KnownForDepartment { get; set; }
	public Gender Gender { get; set; }
	public double Popularity { get; set; }
	public bool Adult { get; set; }
}
