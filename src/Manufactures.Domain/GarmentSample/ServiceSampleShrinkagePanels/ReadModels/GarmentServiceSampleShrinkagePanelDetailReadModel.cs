﻿using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.ReadModels
{
    public class GarmentServiceSampleShrinkagePanelDetailReadModel : ReadModelBase
    {
        public GarmentServiceSampleShrinkagePanelDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid ServiceSampleShrinkagePanelItemId { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductRemark { get; internal set; }

        public string DesignColor { get; internal set; }
        public decimal Quantity { get; internal set; }

        public int UomId { get; internal set; }
        public string UomUnit { get; internal set; }

        public virtual GarmentServiceSampleShrinkagePanelItemReadModel GarmentServiceSampleShrinkagePanelItemIdentity { get; internal set; }
    }
}
