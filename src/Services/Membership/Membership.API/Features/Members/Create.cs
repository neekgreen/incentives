﻿namespace Incentives.Services.Membership.API.Features.Members
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using Models;

    public class Create
    {
        public class Command : IRequest
        {
            public Guid? CommandId { get; set; }
            public Guid? MemberTypeId { get; set; }
            public string CompleteName { get; set; }
            public string SortableName { get; set; }
            public string MemberNumber { get; set; }
        }


        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly AppDbContext context;

            public CommandHandler(AppDbContext context)
            {
                this.context = context;
            }

            protected override async Task Handle(Command request, CancellationToken cancellationToken)
            {
                throw new InvalidOperationException();
            }
        }


        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(t => t.CompleteName).NotEmpty();
                RuleFor(t => t.SortableName);
                RuleFor(t => t.MemberTypeId).NotNull();
            }
        }
    }
}