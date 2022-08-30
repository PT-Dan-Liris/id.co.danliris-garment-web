﻿using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconContracts.Commands
{
    public class UpdateGarmentSubconContractCommand : ICommand<GarmentSubconContract>
    {
        public Guid Identity { get; private set; }
        public string ContractType { get; set; }
        public string ContractNo { get; set; }
        public string AgreementNo { get; set; }
        public Supplier Supplier { get; set; }
        public string JobType { get; set; }
        public string BPJNo { get; set; }
        public string FinishedGoodType { get; set; }
        public double Quantity { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset ContractDate { get; set; }
        public bool IsUsed { get; set; }
        //public Buyer Buyer { get; set; }

        public string SubconCategory { get; set; }
        public Uom Uom { get; set; }
        public string SKEPNo { get; set; }
        public DateTimeOffset AgreementDate { get; set; }
        public double CIF { get; set; }
        public double NettWeight { get; set; }
        public double GrossWeight { get; set; }
        public List<GarmentSubconContractItemValueObject> Items { get; set; }
        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentSubconContractCommandValidator : AbstractValidator<UpdateGarmentSubconContractCommand>
    {
        public UpdateGarmentSubconContractCommandValidator()
        {
            RuleFor(r => r.Supplier).NotNull();
            RuleFor(r => r.Supplier.Id).NotEmpty().OverridePropertyName("Supplier").When(w => w.Supplier != null);

            //RuleFor(r => r.Buyer).NotNull();
            //RuleFor(r => r.Buyer.Id).NotEmpty().OverridePropertyName("Buyer").When(w => w.Buyer != null);

            RuleFor(r => r.Quantity).GreaterThan(0).WithMessage("Quantity harus lebih dari 0");
            //RuleFor(r => r.ContractNo).NotNull();
            // RuleFor(r => r.AgreementNo).NotNull();
            RuleFor(r => r.JobType).NotNull();
            // RuleFor(r => r.BPJNo).NotNull();
            RuleFor(r => r.FinishedGoodType).NotNull();
            RuleFor(r => r.DueDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Jatuh Tempo Tidak Boleh Kosong");
            RuleFor(r => r.ContractDate).NotNull().GreaterThan(DateTimeOffset.MinValue).WithMessage("Tanggal Kontrak Tidak Boleh Kosong");
        }
    }
}
