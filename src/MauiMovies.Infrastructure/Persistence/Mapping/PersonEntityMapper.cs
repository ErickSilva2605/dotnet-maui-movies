
namespace MauiMovies.Infrastructure.Persistence.Mapping;

public static class PersonEntityMapper
{
	public static Person ToDomain(this PersonEntity entity) => new()
	{
		Id = entity.Id,
		Name = entity.Name,
		OriginalName = entity.OriginalName,
		ProfilePath = entity.ProfilePath,
		KnownForDepartment = entity.KnownForDepartment,
		Gender = entity.Gender,
		Popularity = entity.Popularity,
		Adult = entity.Adult,
	};

	public static PersonEntity ToEntity(this Person domain) => new()
	{
		Id = domain.Id,
		Name = domain.Name,
		OriginalName = domain.OriginalName,
		ProfilePath = domain.ProfilePath,
		KnownForDepartment = domain.KnownForDepartment,
		Gender = domain.Gender,
		Popularity = domain.Popularity,
		Adult = domain.Adult,
	};
}
