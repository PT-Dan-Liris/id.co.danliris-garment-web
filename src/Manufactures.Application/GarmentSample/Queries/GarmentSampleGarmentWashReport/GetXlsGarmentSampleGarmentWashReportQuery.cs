﻿using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentSample.Queries.GarmentSampleGarmentWashReport
{
    public class GetXlsGarmentSampleGarmentWashReporQuery : IQuery<MemoryStream>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }

        public GetXlsGarmentSampleGarmentWashReporQuery(int page, int size, string order, DateTime dateFrom, DateTime dateTo)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
        }
    }
}
