using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StreamNote.Database.Commons.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreamNote.Database.Commons.Database.EntityConfigurations
{
    internal class FcmUserTokensConfiguration : IEntityTypeConfiguration<FcmUserTokens>
    {
        public void Configure(EntityTypeBuilder<FcmUserTokens> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.UserId)
                .IsRequired();

        }
    }
}
