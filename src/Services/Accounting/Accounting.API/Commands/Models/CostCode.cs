namespace Incentives.Services.Accounting.API.Commands.Models
{
    using System;
    using Incentives.Services.Accounting.API.Commands.Events;
    using CQRSlite.Domain;


    public class CostCode : AggregateRoot
    {
        private CostCode() { }

        public CostCode(Guid id, string commonName, string uniqueIdentifier)
        {
            this.Id = id;
            ApplyChange(new CostCodeCreated(id, commonName, uniqueIdentifier));
        }


        public string CommonName { get; private set; }
        public string UniqueIdentifier { get; private set; }
        public bool IsActive { get; private set; } 


        public void Update(string commonName, string uniqueIdentifier, bool isActive)
        {
            ApplyChange(new CostCodeUpdated(this.Id, commonName, uniqueIdentifier, IsActive));
        }

        private void Apply(CostCodeCreated e)
        {
            this.CommonName = e.CommonName;
            this.UniqueIdentifier = e.UniqueIdentifier;
            this.IsActive = true;
        }

        private void Apply(CostCodeUpdated e)
        {
            this.CommonName = e.CommonName;
            this.UniqueIdentifier = e.UniqueIdentifier;
            this.IsActive = e.IsActive;
        }
    }
}