﻿using System;
using System.Collections.Generic;
using Infrastructure.Domain.ReadModels;
using Manufactures.Domain.GarmentSewingIns.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.ReadModels
{
    public class GarmentServiceSubconExpenditureGoodReadModel : ReadModelBase
    {
        public GarmentServiceSubconExpenditureGoodReadModel(Guid identity) : base(identity)
        {
        }

        public string ServiceSubconExpenditureGoodNo { get; internal set; }
        public DateTimeOffset ServiceSubconExpenditureGoodDate { get; internal set; }
        public bool IsUsed { get; internal set; }
        public int BuyerId { get; internal set; }
        public string BuyerCode { get; internal set; }
        public string BuyerName { get; internal set; }
        public int QtyPacking { get; internal set; }
        public string UomUnit { get; internal set; }
        public double NettWeight { get; internal set; }
        public double GrossWeight { get; internal set; }
        public virtual List<GarmentServiceSubconExpenditureGoodItemReadModel> GarmentServiceSubconExpenditureGoodItem { get; internal set; }

    }
}
