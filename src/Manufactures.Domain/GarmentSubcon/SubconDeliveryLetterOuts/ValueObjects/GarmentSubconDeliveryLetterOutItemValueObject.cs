﻿using Moonlay.Domain;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects
{
    public class GarmentSubconDeliveryLetterOutItemValueObject : ValueObject
    {
        public Guid Id { get; set; }
        public Guid SubconDeliveryLetterOutId { get; set; }
        public int UENItemId { get; set; }

        public Product Product { get; set; }
        public string ProductRemark { get; set; }

        public string DesignColor { get; set; }
        public double Quantity { get; set; }

        public Uom Uom { get; set; }
        public Uom UomOut { get; set; }

        public string FabricType { get; set; }
        public double ContractQuantity { get; set; }

        #region Cutting
        public string RONo { get; set; }
        public Guid SubconId { get; set; }
        public string POSerialNumber { get; set; }
        public string SubconNo { get; set; }

        #endregion
        //public Uom UomSatuan { get; set; }
        public string UomSatuanUnit { get; set; }
        public int QtyPacking { get; set; }
        public double SmallQuantity { get;  set; }
        public string SmallUomUnit { get;  set; }
        public int UENId { get; set; }
        public string UENNo { get; set; }
        public List<GarmentSubconDeliveryLetterOutDetailValueObject> Details { get; set; }
        
        public GarmentSubconDeliveryLetterOutItemValueObject()
        {
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
