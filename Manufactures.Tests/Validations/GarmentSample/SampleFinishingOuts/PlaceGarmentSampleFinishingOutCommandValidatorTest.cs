﻿using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleFinishingOuts
{
    public class PlaceGarmentSampleFinishingOutCommandValidatorTest
    {
        private PlaceGarmentSampleFinishingOutCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSampleFinishingOutCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentSampleFinishingOutCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSampleFinishingOutCommand();
            unitUnderTest.FinishingOutDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.FinishingInDate = DateTimeOffset.Now;

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSampleFinishingOutCommand()
            {
                Article = "Article",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                FinishingOutDate = DateTimeOffset.Now,
                FinishingInDate = DateTimeOffset.Now,
                FinishingOutNo = "FinishingOutNo",
                FinishingTo = "FinishingTo",
                IsDifferentSize = true,
                IsSave = true,
                IsUsed = true,
                Price = 1,
                RONo = "RONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UnitTo = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleFinishingOutItemValueObject>()
                {
                    new GarmentSampleFinishingOutItemValueObject()
                    {
                        BasicPrice=1,
                        Color ="Color",
                        DesignColor ="DesignColor",
                        FinishingInId =id,
                        FinishingInItemId =id,
                        FinishingInQuantity =2,
                        FinishingOutId =id,
                        Id =id,
                        IsDifferentSize =true,
                        IsSave =true,
                        Price =1,
                        Product =new Product()
                        {
                             Id = 1,
                             Code = "Code",
                             Name = "Name"
                        },
                        Quantity =1,
                        RemainingQuantity =2,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size="Size"
                        },
                        TotalQuantity=1,
                        Uom =new Uom()
                        {
                            Id=1,
                            Unit="Unit"
                        },
                        Details =new List<GarmentSampleFinishingOutDetailValueObject>()
                        {
                            new GarmentSampleFinishingOutDetailValueObject()
                            {
                                Id =id,
                                FinishingOutItemId =id,
                                Quantity =1,
                                Size =new SizeValueObject()
                                {
                                    Id =1,
                                    Size ="Size"
                                },
                                Uom =new Uom()
                                {
                                    Id=1,
                                    Unit="Unit"
                                }
                            }
                        }
                    }
                },

            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
