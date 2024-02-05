using System.Reflection;

namespace CQRS.BankAPI.Application;

public class ApplicationAssemblyReference
{
    internal static readonly Assembly Reference = typeof(ApplicationAssemblyReference).Assembly;

}
