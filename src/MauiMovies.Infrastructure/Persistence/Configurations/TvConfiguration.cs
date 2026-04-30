using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MauiMovies.Infrastructure.Persistence.Configurations;

public class TvConfiguration : IEntityTypeConfiguration<TvEntity>
{
	public void Configure(EntityTypeBuilder<TvEntity> builder)
	{
		builder.ToTable("TvShows");

		builder.HasKey(t => t.Id);
		builder.Property(t => t.Id).ValueGeneratedNever();

		builder.Property(t => t.Name).IsRequired();
		builder.Property(t => t.OriginalName).IsRequired();
		builder.Property(t => t.Overview).IsRequired();
		builder.Property(t => t.GenreIds).IsRequired();
		builder.Property(t => t.OriginCountry).IsRequired();
	}
}
