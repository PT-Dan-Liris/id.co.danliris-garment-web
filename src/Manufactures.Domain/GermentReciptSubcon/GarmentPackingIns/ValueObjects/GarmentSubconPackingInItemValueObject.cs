﻿using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.GermentReciptSubcon.GarmentPackingIns.ValueObjects
{
    public class GarmentSubconPackingInItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid CuttingOutItemId { get;  set; }
        public Guid CuttingOutDetailId { get;  set; }
        public Guid SewingOutItemId { get;  set; }
        public Guid SewingOutDetailId { get;  set; }
        public Guid FinishingOutItemId { get;  set; }
        public Guid FinishingOutDetailId { get;  set; }
        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double DataFromRemainingQuantity { get; set; }
        public double RemainingQuantity { get; set; }
        public bool IsSave { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }

        public GarmentSubconPackingInItemValueObject()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
