﻿using Manufactures.Domain.GermentReciptSubcon.GarmentCuttingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GermentCuttingOut
{
    public class GarmentSubconCuttingOutDetailDto : BaseDto
    {
        public GarmentSubconCuttingOutDetailDto(GarmentSubconCuttingOutDetail garmentCuttingOutDetail)
        {
            Id = garmentCuttingOutDetail.Identity;
            CutOutItemId = garmentCuttingOutDetail.CutOutItemId;
            Size = new SizeValueObject(garmentCuttingOutDetail.SizeId.Value, garmentCuttingOutDetail.SizeName);
            CuttingOutQuantity = garmentCuttingOutDetail.CuttingOutQuantity;
            CuttingOutUom = new Uom(garmentCuttingOutDetail.CuttingOutUomId.Value, garmentCuttingOutDetail.CuttingOutUomUnit);
            Color = garmentCuttingOutDetail.Color;
            RealQtyOut = garmentCuttingOutDetail.RealQtyOut;
            BasicPrice = garmentCuttingOutDetail.BasicPrice;
            Price = garmentCuttingOutDetail.Price;
            
        }

        public Guid Id { get; set; }
        public Guid CutOutItemId { get; set; }
        public SizeValueObject Size { get; set; }
        public double CuttingOutQuantity { get; set; }
        public Uom CuttingOutUom { get; set; }
        public string Color { get; set; }
        public double RealQtyOut { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
       
    }
}