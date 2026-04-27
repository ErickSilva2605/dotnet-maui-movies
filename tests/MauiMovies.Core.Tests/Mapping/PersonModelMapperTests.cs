using MauiMovies.Core.Entities;
using MauiMovies.Core.Enums;
using MauiMovies.Core.Mapping;

namespace MauiMovies.Core.Tests.Mapping;

public class PersonModelMapperTests
{
	[Fact]
	public void ToModel_WithFullPerson_MapsAllFields()
	{
		var person = new Person
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

		var model = person.ToModel();

		Assert.Equal(7, model.Id);
		Assert.Equal("Cillian Murphy", model.Name);
		Assert.Equal("Cillian Murphy", model.OriginalName);
		Assert.Equal("/profile.jpg", model.ProfilePath);
		Assert.Equal("Acting", model.KnownForDepartment);
		Assert.Equal(Gender.Male, model.Gender);
		Assert.Equal(50.0, model.Popularity);
		Assert.False(model.Adult);
	}
}
