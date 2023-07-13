using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
            paymentConfiguration.ToTable("paymentmethods", OrderDbContext.DEFAULT_SCHEMA);
            paymentConfiguration.Ignore(b => b.DomainEvents);
            paymentConfiguration.HasKey(o => o.Id);
            paymentConfiguration.Property(i => i.Id).HasColumnName("id").ValueGeneratedOnAdd();

            paymentConfiguration.Property<int>("BuyerId")
                .IsRequired();

            paymentConfiguration
                .Property(o => o.CardHolderName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CardHolderName")
                .HasMaxLength(200)
                .IsRequired();

            paymentConfiguration
               .Property(o => o.Alias)
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("Alias")
               .HasMaxLength(200)
               .IsRequired();

            paymentConfiguration
              .Property(o => o.CardNumber)
              .UsePropertyAccessMode(PropertyAccessMode.Field)
              .HasColumnName("CardNumber")
              .HasMaxLength(25)
              .IsRequired();

            paymentConfiguration
              .Property(o => o.Expiration)
              .UsePropertyAccessMode(PropertyAccessMode.Field)
              .HasColumnName("Expiration")
              .HasMaxLength(25)
              .IsRequired();

            paymentConfiguration
             .Property(o => o.CardTypeId)
             .UsePropertyAccessMode(PropertyAccessMode.Field)
             .HasColumnName("CardTypeId")
             .IsRequired();

            paymentConfiguration.HasOne(p => p.CardType)
                .WithMany()
                .HasForeignKey(i => i.CardTypeId);
        }
    }
}
