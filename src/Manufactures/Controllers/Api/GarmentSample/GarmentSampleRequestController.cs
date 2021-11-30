﻿using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Dtos.GarmentSample.SampleRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-requests")]
    public class GarmentSampleRequestController: ControllerApiBase
    {
        
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository _GarmentSampleRequestProductRepository;
        private readonly IGarmentSampleRequestSpecificationRepository _GarmentSampleRequestSpecificationRepository;

        public GarmentSampleRequestController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _GarmentSampleRequestRepository = Storage.GetRepository<IGarmentSampleRequestRepository>();
            _GarmentSampleRequestProductRepository = Storage.GetRepository<IGarmentSampleRequestProductRepository>();
            _GarmentSampleRequestSpecificationRepository = Storage.GetRepository<IGarmentSampleRequestSpecificationRepository>();
            
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query =_GarmentSampleRequestRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSampleRequestListDto> garmentSampleRequestListDtos =_GarmentSampleRequestRepository
                .Find(query)
                .Select(subcon => new GarmentSampleRequestListDto(subcon))
                .ToList();

            var dtoIds = garmentSampleRequestListDtos.Select(s => s.Id).ToList();
            

            await Task.Yield();
            return Ok(garmentSampleRequestListDtos, info: new
            {
                page,
                size,
                total
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSampleRequestDto garmentSampleRequestDto =_GarmentSampleRequestRepository.Find(o => o.Identity == guid).Select(sample => new GarmentSampleRequestDto(sample)
            {
                SampleProducts = _GarmentSampleRequestProductRepository.Find(o => o.SampleRequestId == sample.Identity).Select(product => new GarmentSampleRequestProductDto(product)
                {
                   
                }).ToList(),
                SampleSpecifications= _GarmentSampleRequestSpecificationRepository.Find(o => o.SampleRequestId == sample.Identity).Select(specification => new GarmentSampleRequestSpecificationDto(specification)
                {

                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSampleRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleRequestCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query =_GarmentSampleRequestRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSampleRequestDto =_GarmentSampleRequestRepository.Find(query).Select(o => new GarmentSampleRequestDto(o)).ToArray();
            var GarmentSampleRequestProductDto = _GarmentSampleRequestProductRepository.Find(_GarmentSampleRequestProductRepository.Query).Select(o => new GarmentSampleRequestProductDto(o)).ToList();
            var GarmentSampleRequestSpecificationDto = _GarmentSampleRequestSpecificationRepository.Find(_GarmentSampleRequestSpecificationRepository.Query).Select(o => new GarmentSampleRequestSpecificationDto(o)).ToList();
            
            Parallel.ForEach(garmentSampleRequestDto, productDto =>
            {
                var GarmentSampleRequestProducts = GarmentSampleRequestProductDto.Where(x => x.SampleRequestId == productDto.Id).OrderBy(x => x.Id).ToList();
                var GarmentSampleSpecifications = GarmentSampleRequestSpecificationDto.Where(x => x.SampleRequestId == productDto.Id).OrderBy(x => x.Id).ToList();

                productDto.SampleProducts = GarmentSampleRequestProducts;
                productDto.SampleSpecifications = GarmentSampleSpecifications;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSampleRequestDto = QueryHelper<GarmentSampleRequestDto>.Order(garmentSampleRequestDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSampleRequestDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleRequestCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSampleRequestCommand command = new RemoveGarmentSampleRequestCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }
    }
}
