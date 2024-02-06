namespace CQRS.BankAPI.Domain.Entities.Users
{
    public partial record Address
    {
        public Address(string city, string state, string district)
        {
            City = city;
            State = state;
            District = district;
        }

        public string City { get; init; }
        public string State { get; init; }
        public string District { get; init; }

        public static Address? Create(string city, string state, string district)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(state) || string.IsNullOrEmpty(district))
            {
                return null;
            }

            return new Address(city, state, district);
        }
    }
}
