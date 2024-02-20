
namespace CQRS.BankAPI.Application.DTOS.Request
{
    public class MeTokenRequest
    {
        public string AuthToken { get; set; }

        public MeTokenRequest(string authToken)
        {
            AuthToken = authToken;
        }
    }
}