using Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleFinishingOuts
{
    public class GarmentSampleMonitoringFinishingDtoTest
    {
        [Fact]
        public void Should_Success_Intantiate()
        {
            GarmentSampleFinishingMonitoringDto garmentMonitoring = new GarmentSampleFinishingMonitoringDto();
            GarmentSampleFinishingMonitoringDto dto = new GarmentSampleFinishingMonitoringDto(garmentMonitoring);

            Assert.NotNull(dto);

        }
    }
}
