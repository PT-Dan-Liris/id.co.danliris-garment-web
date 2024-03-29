﻿using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.GermentReciptSubcon.GarmentFinishingIn
{
    public class GarmentFinishingInItemDto : BaseDto
    {
        public GarmentFinishingInItemDto(GarmentSubconFinishingInItem garmentFinishingInItem)
        {
            Id = garmentFinishingInItem.Identity;
            Product = new Product(garmentFinishingInItem.ProductId.Value, garmentFinishingInItem.ProductCode, garmentFinishingInItem.ProductName);
            DesignColor = garmentFinishingInItem.DesignColor;
            Size = new SizeValueObject(garmentFinishingInItem.SizeId.Value, garmentFinishingInItem.SizeName);
            Quantity = garmentFinishingInItem.Quantity;
            Uom = new Uom(garmentFinishingInItem.UomId.Value, garmentFinishingInItem.UomUnit);
            Color = garmentFinishingInItem.Color;
            RemainingQuantity = garmentFinishingInItem.RemainingQuantity;
            SewingOutItemId = garmentFinishingInItem.SewingOutItemId;
            FinishingInId = garmentFinishingInItem.FinishingInId;
            BasicPrice = garmentFinishingInItem.BasicPrice;
            Price = garmentFinishingInItem.Price;
        }

        public GarmentFinishingInItemDto(GarmentSubconFinishingInItemReadModel garmentFinishingInItemReadModel)
        {
            Id = garmentFinishingInItemReadModel.Identity;
            Product = new Product(garmentFinishingInItemReadModel.ProductId, garmentFinishingInItemReadModel.ProductCode, garmentFinishingInItemReadModel.ProductName);
            DesignColor = garmentFinishingInItemReadModel.DesignColor;
            Size = new SizeValueObject(garmentFinishingInItemReadModel.SizeId, garmentFinishingInItemReadModel.SizeName);
            Quantity = garmentFinishingInItemReadModel.Quantity;
            Uom = new Uom(garmentFinishingInItemReadModel.UomId, garmentFinishingInItemReadModel.UomUnit);
            Color = garmentFinishingInItemReadModel.Color;
            RemainingQuantity = garmentFinishingInItemReadModel.RemainingQuantity;
            SewingOutItemId = garmentFinishingInItemReadModel.SewingOutItemId;
            FinishingInId = garmentFinishingInItemReadModel.FinishingInId;
            BasicPrice = garmentFinishingInItemReadModel.BasicPrice;
            Price = garmentFinishingInItemReadModel.Price;
        }

        public Guid Id { get; set; }
        public Guid SewingOutItemId { get; set; }

        public Product Product { get; set; }
        public string DesignColor { get; set; }
        public SizeValueObject Size { get; set; }
        public double Quantity { get; set; }
        public Uom Uom { get; set; }
        public string Color { get; set; }
        public double RemainingQuantity { get; set; }
        public Guid FinishingInId { get; set; }
        public double BasicPrice { get; set; }
        public double Price { get; set; }
    }
}
