﻿using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries.GarmentSampleFinishingByColorMonitoring;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleFinishingOuts.MonitoringFinishingOutByColor
{
	public class MonitoringFinishingOutByColorCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
		private readonly Mock<IGarmentSampleFinishingOutRepository> _mockGarmentFinishingOutRepository;
		private readonly Mock<IGarmentSampleFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
		private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentPreparingRepository;
		private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentPreparingItemRepository;
		private readonly Mock<IGarmentSampleCuttingInRepository> _mockGarmentCuttingInRepository;
		private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
		private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
		private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

		public MonitoringFinishingOutByColorCommandHandlerTest()
		{
			_mockGarmentFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();
			_mockGarmentFinishingOutItemRepository = CreateMock<IGarmentSampleFinishingOutItemRepository>();
			_mockGarmentCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
			_mockGarmentCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
			_mockGarmentCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
			_mockGarmentSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
			_mockGarmentSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
			_mockGarmentPreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
			_mockGarmentPreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();
			_mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
			_mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();


			_MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentPreparingRepository);
			_MockStorage.SetupStorage(_mockGarmentPreparingItemRepository); 
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
			_MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
			_MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();

			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}
		private GetMonitoringSampleFinishingByColorQueryHandler CreateGetMonitoringSampleFinishingByColorQueryHandler()
		{
			return new GetMonitoringSampleFinishingByColorQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetMonitoringSampleFinishingByColorQueryHandler unitUnderTest = CreateGetMonitoringSampleFinishingByColorQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();
			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidCuttingInDetail = Guid.NewGuid();

			GetSampleFinishingByColorMonitoringQuery getMonitoring = new GetSampleFinishingByColorMonitoringQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

			_mockGarmentCuttingInItemRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentSampleCuttingInItemReadModel>
			   {
					new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"uENNo",Guid.Empty,"sewingOutNo").GetReadModel()
			   }.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					new GarmentSampleCuttingIn(guidCuttingIn,"cutInNo","Main Fabric","cuttingFrom","ro","article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,4.5).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInDetailReadModel>
				{
					new GarmentSampleCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","fabricType",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutItemReadModel>
				{
					new GarmentSampleFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutReadModel>
				{
					new GarmentSampleFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutItemReadModel>
				{
					new GarmentSampleSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutReadModel>
				{
					new GarmentSampleSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparing = Guid.NewGuid();
			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingReadModel>
				{
					new Manufactures.Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidGarmentPreparing,1,"uenNo",new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparingItem = Guid.NewGuid();
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingItemReadModel>
				{
					new GarmentSamplePreparingItem(guidGarmentPreparingItem,1,new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidGarmentPreparing,null).GetReadModel()
				}.AsQueryable());
			var garmentBalanceFinishing = Guid.NewGuid();
			_mockGarmentSampleRequestProductRepository
			 .Setup(s => s.Query)
			 .Returns(new List<GarmentSampleRequestProductReadModel>
			 {
					new Domain.GarmentSample.SampleRequests.GarmentSampleRequestProduct (guidGarmentPreparing,guidGarmentPreparing,"","",new Domain.Shared.ValueObjects.SizeId(1),"","",10,1).GetReadModel()
			 }.AsQueryable());
			_mockGarmentSampleRequestRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleRequestReadModel>
				{
					new Domain.GarmentSample.SampleRequests.GarmentSampleRequest (guidGarmentPreparing,"","","","",DateTimeOffset.Now,new Domain.Shared.ValueObjects.BuyerId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","","","",DateTimeOffset.Now,"","","",true,true,DateTimeOffset.Now,"",false,DateTimeOffset.Now,"","",false,DateTimeOffset.Now,"","","","","","",new Domain.Shared.ValueObjects.SectionId(1),"","").GetReadModel()
				}.AsQueryable());
			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
