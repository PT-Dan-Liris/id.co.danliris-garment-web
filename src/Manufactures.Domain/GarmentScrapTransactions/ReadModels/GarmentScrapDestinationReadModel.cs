﻿using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapTransactions.ReadModels
{
	public class GarmentScrapDestinationReadModel : ReadModelBase
	{
		public GarmentScrapDestinationReadModel(Guid identity) : base(identity)
		{

		}
		public string Code { get; internal set; }
		public string Name { get; internal set; }
		public string Description { get; internal set; }
	}
}