﻿using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetHistoryDeleted
{
    public class GetXlsMonPreHistoryDelQuery :  IQuery<MemoryStream>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string token { get; private set; }
        public int unit { get; private set; }
        public DateTime? dateFrom { get; private set; }
        public DateTime? dateTo { get; private set; }
        public GetXlsMonPreHistoryDelQuery(string monType, DateTime? dateFrom, DateTime? dateTo)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            //this.token = token;
        }
    }
}
