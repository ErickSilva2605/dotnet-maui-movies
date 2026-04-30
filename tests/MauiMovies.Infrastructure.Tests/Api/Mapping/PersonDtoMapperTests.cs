using MauiMovies.Infrastructure.Api.Dtos.Enums;

namespace MauiMovies.Infrastructure.Tests.Api.Mapping;

public class PersonDtoMapperTests
{
	[Fact]
	public void ToDomain_WithFullDto_MapsAllFields()
	{
		var dto = new PersonDto
		{
			Id = 7,
			Name = "Cillian Murphy",
			OriginalName = "Cillian Murphy",
			ProfilePath = "/profile.jpg",
			KnownForDepartment = "Acting",
			Gender = (Gender)2,
			Popularity = 50.0,
			Adult = false,
		};

		var person = dto.ToDomain();

		Assert.Equal(7, person.Id);
		Assert.Equal("Cillian Murphy", person.Name);
		Assert.Equal("/profile.jpg", person.ProfilePath);
		Assert.Equal(MauiMovies.Core.Enums.Gender.Male, person.Gender);
	}

	[Theory]
	[InlineData(0, MauiMovies.Core.Enums.Gender.Unknown)]
	[InlineData(1, MauiMovies.Core.Enums.Gender.Female)]
	[InlineData(2, MauiMovies.Core.Enums.Gender.Male)]
	[InlineData(3, MauiMovies.Core.Enums.Gender.NonBinary)]
	public void ToDomain_GenderConvertsByValue(byte dtoValue, MauiMovies.Core.Enums.Gender expected)
	{
		var dto = new PersonDto { Id = 1, Gender = (Gender)dtoValue };

		var person = dto.ToDomain();

		Assert.Equal(expected, person.Gender);
	}
}
