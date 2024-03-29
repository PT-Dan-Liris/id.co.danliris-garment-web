﻿using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleCuttingOuts
{
    public class PlaceGarmentSampleCuttingOutCommandValidatorTest
    {
        private PlaceGarmentSampleCuttingOutCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSampleCuttingOutCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSampleCuttingOutCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSampleCuttingOutCommand();
            unitUnderTest.CuttingOutDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.CuttingInDate = DateTimeOffset.Now;

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
            var unitUnderTest = new PlaceGarmentSampleCuttingOutCommand()
            {
                Article = "Article",
                RONo = "RONo",
                Unit = new Domain.Shared.ValueObjects.UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                CutOutNo = "CutOutNo",
                CuttingOutDate = DateTimeOffset.Now,
                CuttingInDate = DateTimeOffset.Now,
                CuttingOutType = "CuttingOutType",
                Price = 1,
                PriceSewing = 1,
                UnitFrom = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleCuttingOutItemValueObject>()
                {
                    new GarmentSampleCuttingOutItemValueObject()
                    {

                        Id =id,
                        Details=new List<GarmentSampleCuttingOutDetailValueObject>()
                        {
                            new GarmentSampleCuttingOutDetailValueObject()
                            {

                               BasicPrice =1,
                               Color ="Color",
                               CutOutItemId =id,
                               CuttingOutQuantity =1,
                                RemainingQuantity =2,
                                CuttingOutUom =new Uom()
                                {
                                    Id =1,
                                    Unit ="Unit"
                                },
                                Id =id,
                                Price =0,
                                Size =new SizeValueObject()
                                {
                                    Id =1,
                                    Size ="Size"
                                },

                            }
                        },
                        CutOutId =id,
                        CuttingInDetailId =id,
                        CuttingInId =id,
                        DesignColor ="DesignColor",
                        IsSave =true,
                        Product =new Product()
                        {
                             Id = 1,
                             Code = "Code",
                             Name = "Name"
                        },

                        TotalCuttingOut =1,
                        TotalCuttingOutQuantity =1,
                        TotalRemainingQuantityCuttingInItem=2
                    }
                }
            };

            var validator = GetValidationRules();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}

