using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Entities.Users;

namespace CQRS.BankAPI.Domain.Users.Events;


public sealed record UserCreatedDomainEvent(UserId UserId) : IDomainEvent;