﻿using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns.ValueObjects;
using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentCuttingIns.Commands
{
    public class UpdateGarmentSubconCuttingInCommand : ICommand<GarmentSubconCuttingIn>
    {
        public Guid Identity { get; private set; }
        public string CutInNo { get; set; }
        public string CuttingType { get; set; }
        public string CuttingFrom { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public UnitDepartment Unit { get; set; }
        public DateTimeOffset? CuttingInDate { get; set; }
        public DateTimeOffset? PreparingDate { get; set; }
        public double FC { get; set; }
        public List<GarmentSubconCuttingInItemValueObject> Items { get; set; }

        public void SetIdentity(Guid id)
        {
            Identity = id;
        }
    }

    public class UpdateGarmentCuttingInCommandValidator : AbstractValidator<UpdateGarmentSubconCuttingInCommand>
    {
        public UpdateGarmentCuttingInCommandValidator()
        {
            RuleFor(r => r.Unit).NotNull();
            RuleFor(r => r.Unit.Id).NotEmpty().OverridePropertyName("Unit").When(w => w.Unit != null);
            RuleFor(r => r.Article).NotNull();
            RuleFor(r => r.RONo).NotNull();
            RuleFor(r => r.CuttingInDate).NotNull().GreaterThan(DateTimeOffset.MinValue);
            RuleFor(r => r.CuttingInDate).NotNull().LessThan(DateTimeOffset.Now).WithMessage("Tanggal Cutting In Tidak Boleh Lebih dari Hari Ini");
            RuleFor(r => r.CuttingInDate).NotNull().GreaterThan(r => r.PreparingDate.GetValueOrDefault().Date).WithMessage(r => $"Tanggal Cutting In Tidak Boleh Kurang dari tanggal {r.PreparingDate.GetValueOrDefault().ToOffset(new TimeSpan(7, 0, 0)).ToString("dd/MM/yyyy", new CultureInfo("id-ID"))}");
            RuleFor(r => r.FC).GreaterThan(0);
            RuleFor(r => r.Items).NotEmpty().OverridePropertyName("Item");
            RuleForEach(r => r.Items).SetValidator(new GarmentSubconCuttingInItemValueObjectValidator());
        }
    }

}
