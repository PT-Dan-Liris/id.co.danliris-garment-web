﻿using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api
{
    [ApiController]
    [Authorize]
    [Route("expenditure-goods")]
    public class GarmentExpenditureGoodController : ControllerApiBase
    {
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public GarmentExpenditureGoodController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentExpenditureGoodRepository = Storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = Storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentExpenditureGoodListDto> garmentExpenditureGoodListDtos = _garmentExpenditureGoodRepository
                .Find(query)
                .Select(ExGood => new GarmentExpenditureGoodListDto(ExGood))
                .ToList();

            var dtoIds = garmentExpenditureGoodListDtos.Select(s => s.Id).ToList();
            var items = _garmentExpenditureGoodItemRepository.Query
                .Where(o => dtoIds.Contains(o.ExpenditureGoodId))
                .Select(s => new { s.Identity, s.ExpenditureGoodId, s.Quantity })
                .ToList();

            var itemIds = items.Select(s => s.Identity).ToList();
            Parallel.ForEach(garmentExpenditureGoodListDtos, dto =>
            {
                var currentItems = items.Where(w => w.ExpenditureGoodId == dto.Id);
                dto.TotalQuantity = currentItems.Sum(i => i.Quantity);
            });

            await Task.Yield();
            return Ok(garmentExpenditureGoodListDtos, info: new
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

            GarmentExpenditureGoodDto garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(o => o.Identity == guid).Select(finishOut => new GarmentExpenditureGoodDto(finishOut)
            {
                Items = _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == finishOut.Identity).Select(finishOutItem => new GarmentExpenditureGoodItemDto(finishOutItem)
                {
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentExpenditureGoodCommand command)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentExpenditureGoodCommand command)
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

            RemoveGarmentExpenditureGoodCommand command = new RemoveGarmentExpenditureGoodCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentExpenditureGoodRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentExpenditureGoodDto = _garmentExpenditureGoodRepository.Find(query).Select(o => new GarmentExpenditureGoodDto(o)).ToArray();
            var garmentExpenditureGoodItemDto = _garmentExpenditureGoodItemRepository.Find(_garmentExpenditureGoodItemRepository.Query).Select(o => new GarmentExpenditureGoodItemDto(o)).ToList();
            
            Parallel.ForEach(garmentExpenditureGoodDto, itemDto =>
            {
                var garmentExpenditureGoodItems = garmentExpenditureGoodItemDto.Where(x => x.ExpenditureGoodId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentExpenditureGoodItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentExpenditureGoodDto = QueryHelper<GarmentExpenditureGoodDto>.Order(garmentExpenditureGoodDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentExpenditureGoodDto, info: new
            {
                page,
                size,
                count
            });
        }
    }
}
