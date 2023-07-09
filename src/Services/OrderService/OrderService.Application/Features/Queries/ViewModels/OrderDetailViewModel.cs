using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Queries.ViewModels
{
    public class OrderDetailViewModel
    {
        public string OrderNumber { get; init; } //sadece nesneyi oluştururken değeri verebiliyoruz init o yüzden
        public DateTime Date { get; init; }
        public string Status { get; init; }
        public string Description { get; init; }
        public string Street { get; init; }
        public string City { get; init; }
        public string ZipCode { get; init; }
        public string Country { get; init; }
        public List<Orderitem> Orderitems { get; set; }
        public decimal Total { get; init; }
    }

    public class Orderitem
    {
        public string Productname { get; init; }
        public int Units { get; init; }
        public string Unitprice { get; init; }
        public string Pictureurl { get; init; }
    }

}
