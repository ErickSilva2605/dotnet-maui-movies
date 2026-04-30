
namespace MauiMovies.Core.Mapping;

public static class PersonModelMapper
{
	public static PersonModel ToModel(this Person person) => new()
	{
		Id = person.Id,
		Name = person.Name,
		OriginalName = person.OriginalName,
		ProfilePath = person.ProfilePath,
		KnownForDepartment = person.KnownForDepartment,
		Gender = person.Gender,
		Popularity = person.Popularity,
		Adult = person.Adult,
	};
}
