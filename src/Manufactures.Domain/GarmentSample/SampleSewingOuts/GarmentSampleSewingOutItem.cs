﻿using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingOuts
{
    public class GarmentSampleSewingOutItem : AggregateRoot<GarmentSampleSewingOutItem, GarmentSampleSewingOutItemReadModel>
    {
        public Guid SampleSewingOutId { get; private set; }
        public Guid SampleSewingInId { get; private set; }
        public Guid SampleSewingInItemId { get; private set; }
        public ProductId ProductId { get; private set; }
        public string ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public string DesignColor { get; private set; }
        public SizeId SizeId { get; private set; }
        public string SizeName { get; private set; }
        public double Quantity { get; private set; }
        public UomId UomId { get; private set; }
        public string UomUnit { get; private set; }
        public string Color { get; private set; }
        public double RemainingQuantity { get; private set; }
        public double BasicPrice { get; private set; }
        public double Price { get; private set; }

        public GarmentSampleSewingOutItem(Guid identity, Guid sewingOutId, Guid sewingInId, Guid sewingInItemId, ProductId productId, string productCode, string productName, string designColor, SizeId sizeId, string sizeName, double quantity, UomId uomId, string uomUnit, string color, double remainingQuantity, double basicPrice, double price) : base(identity)
        {
            //MarkTransient();

            Identity = identity;
            SampleSewingOutId = sewingOutId;
            SampleSewingInId = sewingInId;
            SampleSewingInItemId = sewingInItemId;
            ProductId = productId;
            ProductCode = productCode;
            ProductName = productName;
            DesignColor = designColor;
            SizeId = sizeId;
            SizeName = sizeName;
            Quantity = quantity;
            UomId = uomId;
            UomUnit = uomUnit;
            Color = color;
            RemainingQuantity = remainingQuantity;
            BasicPrice = basicPrice;
            Price = price;

            ReadModel = new GarmentSampleSewingOutItemReadModel(identity)
            {
                SampleSewingOutId = SampleSewingOutId,
                SampleSewingInId = SampleSewingInId,
                SampleSewingInItemId = SampleSewingInItemId,
                ProductId = ProductId.Value,
                ProductCode = ProductCode,
                ProductName = ProductName,
                DesignColor = DesignColor,
                SizeId = SizeId.Value,
                SizeName = SizeName,
                Quantity = Quantity,
                UomId = UomId.Value,
                UomUnit = UomUnit,
                Color = Color,
                RemainingQuantity = remainingQuantity,
                BasicPrice = basicPrice,
                Price = price
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleSewingOutPlaced(Identity));
        }

        public GarmentSampleSewingOutItem(GarmentSampleSewingOutItemReadModel readModel) : base(readModel)
        {
            SampleSewingOutId = readModel.SampleSewingOutId;
            SampleSewingInId = readModel.SampleSewingInId;
            SampleSewingInItemId = readModel.SampleSewingInItemId;
            ProductId = new ProductId(readModel.ProductId);
            ProductCode = readModel.ProductCode;
            ProductName = readModel.ProductName;
            DesignColor = readModel.DesignColor;
            SizeId = new SizeId(readModel.SizeId);
            SizeName = readModel.SizeName;
            Quantity = readModel.Quantity;
            UomId = new UomId(readModel.UomId);
            UomUnit = readModel.UomUnit;
            Color = readModel.Color;
            RemainingQuantity = readModel.RemainingQuantity;
            BasicPrice = readModel.BasicPrice;
            Price = readModel.Price;
        }

        public void SetPrice(double Price)
        {
            if (this.Price != Price)
            {
                this.Price = Price;
                ReadModel.Price = Price;
            }
        }

        public void SetQuantity(double Quantity)
        {
            if (this.Quantity != Quantity)
            {
                this.Quantity = Quantity;
                ReadModel.Quantity = Quantity;
            }
        }

        public void SetRemainingQuantity(double RemainingQuantity)
        {
            if (this.RemainingQuantity != RemainingQuantity)
            {
                this.RemainingQuantity = RemainingQuantity;
                ReadModel.RemainingQuantity = RemainingQuantity;
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleSewingOutItem GetEntity()
        {
            return this;
        }
    }
}
