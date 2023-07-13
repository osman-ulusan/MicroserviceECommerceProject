using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", OrderDbContext.DEFAULT_SCHEMA);

            builder.HasKey(o=>o.Id);
            builder.Property(i=>i.Id).ValueGeneratedNever();
            builder.Ignore(i => i.DomainEvents);

            builder
                .OwnsOne(o => o.Address, a =>
                {
                    a.WithOwner();
                });

            builder
                .Property<int>("orderStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field) //bu field sayesinde private olan ordertatusıd ye entityframework içininden set edebiliyoruz
                .HasColumnName("OrderStatusId")
                .IsRequired();

            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);//aynı şekilde IReadOnlyCollection orderitemse findnavigation ile ulaşığ set edebiliyoruz.

            builder.HasOne(o => o.Buyer)
                .WithMany()
                .HasForeignKey(i => i.BuyerId);

            builder.HasOne(o => o.OrderStatus)
                .WithMany()
                .HasForeignKey("orderStatusId");
        }
    }
}
