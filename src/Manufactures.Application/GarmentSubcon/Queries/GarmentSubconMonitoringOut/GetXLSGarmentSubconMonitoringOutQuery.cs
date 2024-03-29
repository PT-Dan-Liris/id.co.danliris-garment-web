﻿using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconMonitoringOut
{
    public class GetXLSGarmentSubconMonitoringOutQuery : IQuery<MemoryStream>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string token { get; private set; }
        public string subconcontractNo { get; private set; }
        public string subconContractType { get; private set; }
        public string subconCategory { get; private set; }



        public GetXLSGarmentSubconMonitoringOutQuery(int page, int size, string order, string subconcontractNo, string subconContractType, string subconCategory, string token)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.subconcontractNo = subconcontractNo;
            this.subconContractType = subconContractType;
            this.subconCategory = subconCategory;
            this.token = token;
        }

    }
}
