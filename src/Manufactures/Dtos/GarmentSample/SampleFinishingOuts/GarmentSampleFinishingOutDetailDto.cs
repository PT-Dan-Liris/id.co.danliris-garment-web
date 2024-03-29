﻿using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GarmentSample.SampleFinishingOuts
{
    public class GarmentSampleFinishingOutDetailDto : BaseDto
    {
        public GarmentSampleFinishingOutDetailDto(GarmentSampleFinishingOutDetail garmentFinishingOutDetail)
        {
            Id = garmentFinishingOutDetail.Identity;
            FinishingOutItemId = garmentFinishingOutDetail.FinishingOutItemId;
            Size = new SizeValueObject(garmentFinishingOutDetail.SizeId.Value, garmentFinishingOutDetail.SizeName);
            Quantity = garmentFinishingOutDetail.Quantity;
            Uom = new Uom(garmentFinishingOutDetail.UomId.Value, garmentFinishingOutDetail.UomUnit);

        }

        public Guid Id { get; set; }
        public Guid FinishingOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
    }
}
