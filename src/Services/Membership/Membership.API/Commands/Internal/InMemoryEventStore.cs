﻿namespace Incentives.Services.Membership.API.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CQRSlite.Events;
    using MediatR;

    public class InMemoryEventStore : IEventStore
    {
        private readonly IMediator mediator;
        private readonly IEventStreamService eventStreamService;

        public InMemoryEventStore(IMediator mediator, IEventStreamService eventStreamService)
        {
            this.mediator = mediator;
            this.eventStreamService = eventStreamService;
        }

        Task<IEnumerable<IEvent>> IEventStore.Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken)
        {
            var events = eventStreamService.GetOrCreateStream(aggregateId);

            return Task.FromResult(events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>());
        }

        async Task IEventStore.Save(IEnumerable<IEvent> events, CancellationToken cancellationToken)
        {
            foreach (var e in events)
            {
                var eventStream = eventStreamService.GetOrCreateStream(e.Id);
                eventStream.Add(e);

                var notification = e as INotification;
                if (notification != null)
                {
                    await mediator.Publish(notification, cancellationToken);
                }
            }
        }
    }
}