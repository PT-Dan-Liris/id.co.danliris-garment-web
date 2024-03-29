﻿using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SamplePreparings.Commands
{
    public class RemoveGarmentSamplePreparingCommand : ICommand<GarmentSamplePreparing>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
