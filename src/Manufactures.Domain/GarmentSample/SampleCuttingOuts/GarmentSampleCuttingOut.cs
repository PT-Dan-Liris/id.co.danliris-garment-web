﻿using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOut : AggregateRoot<GarmentSampleCuttingOut, GarmentSampleCuttingOutReadModel>
    {
        public string CutOutNo { get; private set; }
        public string CuttingOutType { get; private set; }

        public UnitDepartmentId UnitFromId { get; private set; }
        public string UnitFromCode { get; private set; }
        public string UnitFromName { get; private set; }
        public DateTimeOffset CuttingOutDate { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        public string UnitCode { get; private set; }
        public string UnitName { get; private set; }
        public GarmentComodityId ComodityId { get; private set; }
        public string ComodityCode { get; private set; }
        public string ComodityName { get; private set; }
        public bool IsUsed { get; private set; }

        public string UId { get; private set; }
        public GarmentSampleCuttingOut(Guid identity, string cutOutNo, string cuttingOutType, UnitDepartmentId unitFromId, string unitFromCode, string unitFromName, DateTimeOffset cuttingOutDate, string rONo, string article, UnitDepartmentId unitId, string unitCode, string unitName, GarmentComodityId comodityId, string comodityCode, string comodityName, bool isUsed) : base(identity)
        {
            Validator.ThrowIfNull(() => unitFromId);
            Validator.ThrowIfNull(() => unitId);
            Validator.ThrowIfNull(() => rONo);

            //MarkTransient();

            Identity = identity;
            CutOutNo = cutOutNo;
            CuttingOutType = cuttingOutType;
            UnitFromId = unitFromId;
            UnitFromCode = unitFromCode;
            UnitFromName = unitFromName;
            CuttingOutDate = cuttingOutDate;
            RONo = rONo;
            Article = article;
            UnitId = unitId;
            UnitCode = unitCode;
            UnitName = unitName;
            ComodityId = comodityId;
            ComodityCode = comodityCode;
            ComodityName = comodityName;
            IsUsed = isUsed;

            ReadModel = new GarmentSampleCuttingOutReadModel(Identity)
            {
                CutOutNo = CutOutNo,
                CuttingOutType = CuttingOutType,
                UnitFromId = UnitFromId.Value,
                UnitFromCode = UnitFromCode,
                UnitFromName = UnitFromName,
                CuttingOutDate = CuttingOutDate,
                RONo = RONo,
                Article = Article,
                UnitId = UnitId.Value,
                UnitCode = UnitCode,
                UnitName = UnitName,
                ComodityId = ComodityId.Value,
                ComodityCode = ComodityCode,
                ComodityName = ComodityName,
                IsUsed = IsUsed,
                GarmentSampleCuttingOutItem = new List<GarmentSampleCuttingOutItemReadModel>()
            };

            ReadModel.AddDomainEvent(new OnGarmentSampleCuttingOutPlaced(Identity));
        }

        public GarmentSampleCuttingOut(GarmentSampleCuttingOutReadModel readModel) : base(readModel)
        {
            CutOutNo = readModel.CutOutNo;
            CuttingOutType = readModel.CuttingOutType;
            UnitFromId = new UnitDepartmentId(readModel.UnitFromId);
            UnitFromCode = readModel.UnitFromCode;
            UnitFromName = readModel.UnitFromName;
            CuttingOutDate = readModel.CuttingOutDate;
            RONo = readModel.RONo;
            Article = readModel.Article;
            UnitId = new UnitDepartmentId(readModel.UnitId);
            UnitCode = readModel.UnitCode;
            UnitName = readModel.UnitName;
            ComodityId = new GarmentComodityId(readModel.ComodityId);
            ComodityCode = readModel.ComodityCode;
            ComodityName = readModel.ComodityName;
            IsUsed = readModel.IsUsed;
        }

        public void SetDate(DateTimeOffset cuttingOutDate)
        {
            if (cuttingOutDate != CuttingOutDate)
            {
                CuttingOutDate = cuttingOutDate;
                ReadModel.CuttingOutDate = cuttingOutDate;

                MarkModified();
            }
        }

        public void SetIsUsed(bool isUsed)
        {
            if (IsUsed != isUsed)
            {
                IsUsed = isUsed;
                ReadModel.IsUsed = isUsed;

                MarkModified();
            }
        }

        public void Modify()
        {
            MarkModified();
        }

        protected override GarmentSampleCuttingOut GetEntity()
        {
            return this;
        }
    }
}
