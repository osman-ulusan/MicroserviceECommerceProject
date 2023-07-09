using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public record Address //: Value Object
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }

        public Address()
        {

        }

        public Address(string street, string city, string state, string country, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }


        //protected override IEnumerable<object> GetEqualityComponents()
        //{
        //    // using a yield return statement to return each element one at a time
        //    yield return Street;
        //    yield return City;
        //    yield return State;
        //    yield return Country;
        //    yield return ZipCode;
        //}

    }
}
