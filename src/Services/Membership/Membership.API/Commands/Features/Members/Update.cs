﻿namespace Incentives.Services.Membership.API.Commands.Features.Members
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CQRSlite.Domain;
    using FluentValidation;
    using MediatR;
    using Models;

    public class Update
    {
        public class Request : IRequest
        {
            public Guid? Id { get; set; }
            public Guid? CommandId { get; set; }
            public Guid? MemberTypeId { get; set; }

            public string CompleteName { get; set; }
            public string SortableName { get; set; }
            public string MemberNumber { get; set; }
            public bool? IsActive { get; set; }
        }


        public class RequestHandler : AsyncRequestHandler<Request>
        {
            private readonly ISession session;

            public RequestHandler(ISession session)
            {
                this.session = session;
            }

            protected override async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var memberType = await
                    session.Get<MemberType>(
                        request.MemberTypeId.Value, cancellationToken: cancellationToken);

                var member = await
                    session.Get<Member>(request.Id.Value, cancellationToken: cancellationToken);

                member.Update(
                    request.MemberTypeId.Value, 
                    request.CompleteName, 
                    request.SortableName, 
                    request.MemberNumber, 
                    request.IsActive.Value);

                await session.Commit();
            }
        }


        public class CommandValidator : AbstractValidator<Request>
        {
            public CommandValidator()
            {
                RuleFor(t => t.CompleteName).NotEmpty();
                RuleFor(t => t.SortableName);
                RuleFor(t => t.MemberTypeId).NotNull();
                RuleFor(t => t.IsActive).NotNull();
            }
        }
    }
}