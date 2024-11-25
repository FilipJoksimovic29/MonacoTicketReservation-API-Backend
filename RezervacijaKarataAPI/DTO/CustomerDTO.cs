using RezervacijaKarata.Domain.Model;

namespace RezervacijaKarataAPI.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public BookingDTO? Booking { get; set; } // Nullable Booking property
    }

    public static class CustomerDTOMapper
    {
        public static Customer ToCustomer(this CustomerDTO dto)
        {
           
            return new Customer
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Company = dto.Company,
                Address1 = dto.Address1,
                Address2 = dto.Address2,
                PostalCode = dto.PostalCode,
                City = dto.City,
                Country = dto.Country,
                Email = dto.Email,
               
            };
        }

        public static CustomerDTO ToDTO(Customer customer)
        {
           
            return new CustomerDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Company = customer.Company,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                PostalCode = customer.PostalCode,
                City = customer.City,
                Country = customer.Country,
                Email = customer.Email,
                Booking = customer.Booking != null ? BookingDTOMapper.ToDTO(customer.Booking) : null
            };
        }
    }
}
