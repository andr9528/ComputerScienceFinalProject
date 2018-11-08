using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Repository.EntityFramework.Config
{

    
    public class ClientAdConfig : IEntityTypeConfiguration<ClientAd>
    {
        public ClientAdConfig()
        {
        }

        public void Configure(EntityTypeBuilder<ClientAd> builder)
        {
            // Defining Primary Key -->
            builder.HasKey(x => new {x.FK_AdId, x.FK_ClientId});

            // Defining Relations -->
            builder.HasOne(x => (Client) x.Client).WithMany(x => (ICollection<ClientAd>) x.Ads).HasForeignKey(x => x.FK_AdId);
            builder.HasOne(x => (Ad) x.Ad).WithMany(x => (ICollection<ClientAd>) x.Clients).HasForeignKey(x => x.FK_ClientId);
        }
    }
    
}