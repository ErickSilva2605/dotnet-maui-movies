using MauiMovies.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MauiMovies.Infrastructure.Persistence.Configurations;

public class MediaListItemConfiguration : IEntityTypeConfiguration<MediaListItemEntity>
{
	public void Configure(EntityTypeBuilder<MediaListItemEntity> builder)
	{
		builder.ToTable("MediaListItems");

		builder.HasKey(e => e.Id);

		builder.HasIndex(e => new { e.ListType, e.Position });
		builder.HasIndex(e => new { e.ListType, e.MediaType, e.MediaId }).IsUnique();
	}
}
