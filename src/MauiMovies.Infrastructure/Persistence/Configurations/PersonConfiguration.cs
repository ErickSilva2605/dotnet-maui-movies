using MauiMovies.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MauiMovies.Infrastructure.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<PersonEntity>
{
	public void Configure(EntityTypeBuilder<PersonEntity> builder)
	{
		builder.ToTable("Persons");

		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id).ValueGeneratedNever();

		builder.Property(p => p.Name).IsRequired();
		builder.Property(p => p.OriginalName).IsRequired();
	}
}
