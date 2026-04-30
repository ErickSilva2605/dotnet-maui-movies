using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MauiMovies.Infrastructure.Persistence.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<MovieEntity>
{
	public void Configure(EntityTypeBuilder<MovieEntity> builder)
	{
		builder.ToTable("Movies");

		builder.HasKey(m => m.Id);
		builder.Property(m => m.Id).ValueGeneratedNever();

		builder.Property(m => m.Title).IsRequired();
		builder.Property(m => m.OriginalTitle).IsRequired();
		builder.Property(m => m.Overview).IsRequired();
		builder.Property(m => m.GenreIds).IsRequired();
	}
}
