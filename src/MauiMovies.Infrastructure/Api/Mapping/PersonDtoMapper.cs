using MauiMovies.Core.Entities;

namespace MauiMovies.Infrastructure.Api.Mapping;

public static class PersonDtoMapper
{
	public static Person ToDomain(this PersonDto dto) => new()
	{
		Id = dto.Id,
		Name = dto.Name ?? string.Empty,
		OriginalName = dto.OriginalName ?? string.Empty,
		ProfilePath = dto.ProfilePath,
		KnownForDepartment = dto.KnownForDepartment,
		Gender = (Core.Enums.Gender)(byte)dto.Gender,
		Popularity = dto.Popularity,
		Adult = dto.Adult ?? false,
	};
}
