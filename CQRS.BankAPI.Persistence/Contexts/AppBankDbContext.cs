using CQRS.BankAPI.Application.Exceptions;
using CQRS.BankAPI.Application.Interfaces;
using CQRS.BankAPI.Domain;
using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Common;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.BankAPI.Persistence.Contexts
{
    public class AppBankDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher _publisher;

        private readonly IDateTimeService _dateTimeService;
        public AppBankDbContext(DbContextOptions<AppBankDbContext> options, IPublisher publisher, IDateTimeService dateTimeService) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTimeService = dateTimeService;
            _publisher = publisher;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTimeService.NowUtc;
                        break;

                }
            }

            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);

                await PublishDomainEventsAsync();

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("La excepcion por concurrencia se disparo", ex);
            }

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(u => u.Address);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppBankDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<IEntity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                }).ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }

        }

    }
}
