

namespace CQRS.BankAPI.Application.DTOS.Response
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IpUser { get; set; }
        public string UserStatus { get; set; }

    }
}