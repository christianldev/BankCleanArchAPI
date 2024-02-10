

using System.Data;

namespace CQRS.BankAPI.Application.Abstractions.Data;

public interface ISqlConnectionFactory
{

    IDbConnection CreateConnection();

}