﻿using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects
{
    public class UnitDepartment : ValueObject
    {
        public UnitDepartment()
        {

        }

        public UnitDepartment(int departmentId, string name, string code)
        {
            Id = departmentId;
            Name = name;
            Code = code;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
