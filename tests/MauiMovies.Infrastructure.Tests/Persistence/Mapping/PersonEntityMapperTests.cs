using MauiMovies.Core.Entities;
using MauiMovies.Core.Enums;
using MauiMovies.Infrastructure.Persistence.Mapping;

namespace MauiMovies.Infrastructure.Tests.Persistence.Mapping;

public class PersonEntityMapperTests
{
	[Fact]
	public void RoundTrip_PreservesAllFields()
	{
		var original = new Person
		{
			Id = 7,
			Name = "Cillian Murphy",
			OriginalName = "Cillian Murphy",
			ProfilePath = "/profile.jpg",
			KnownForDepartment = "Acting",
			Gender = Gender.Male,
			Popularity = 50.0,
			Adult = false,
		};

		var roundTripped = original.ToEntity().ToDomain();

		Assert.Equal(original.Id, roundTripped.Id);
		Assert.Equal(original.Name, roundTripped.Name);
		Assert.Equal(original.OriginalName, roundTripped.OriginalName);
		Assert.Equal(original.ProfilePath, roundTripped.ProfilePath);
		Assert.Equal(original.KnownForDepartment, roundTripped.KnownForDepartment);
		Assert.Equal(original.Gender, roundTripped.Gender);
		Assert.Equal(original.Popularity, roundTripped.Popularity);
		Assert.Equal(original.Adult, roundTripped.Adult);
	}
}
