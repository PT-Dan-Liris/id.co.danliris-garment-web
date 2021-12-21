﻿using Barebone.Controllers;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Dtos.GarmentSample.SampleCuttingOuts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-cutting-outs")]
    public class GarmentSampleCuttingOutController : ControllerApiBase
    {
        private readonly IGarmentSampleCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentSampleSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;

        public GarmentSampleCuttingOutController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentCuttingOutRepository = Storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            _garmentCuttingOutItemRepository = Storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = Storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
            _garmentSewingInRepository = Storage.GetRepository<IGarmentSampleSewingInRepository>();
            _garmentSewingInItemRepository = Storage.GetRepository<IGarmentSampleSewingInItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var cuttingOutQuery = new Application.GarmentSample.SampleCuttingOuts.Queries.GetAllSampleCuttingOutQuery(page, size, order, keyword, filter);
            var viewModel = await Mediator.Send(cuttingOutQuery);
            return Ok(viewModel.data, info: new
            {
                page,
                size,
                viewModel.total,
                count = viewModel.data.Count,
                viewModel.totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSampleCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentSampleCuttingOutDto(cutOut)
            {
                Items = _garmentCuttingOutItemRepository.Find(o => o.CuttingOutId == cutOut.Identity).Select(cutOutItem => new GarmentSampleCuttingOutItemDto(cutOutItem)
                {
                    Details = _garmentCuttingOutDetailRepository.Find(o => o.CuttingOutItemId == cutOutItem.Identity).OrderBy(s => s.Color).ThenBy(s => s.SizeName).Select(cutOutDetail => new GarmentSampleCuttingOutDetailDto(cutOutDetail)
                    {
                    }).ToList()
                }).ToList()
            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentCuttingOutDto);


        }

        //[HttpGet("{id}/{buyer}")]
        //public async Task<IActionResult> GetPdf(string id, string buyer)
        //{
        //    Guid guid = Guid.Parse(id);

        //    VerifyUser();

        //    int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());
        //    GarmentSampleCuttingOutDto garmentCuttingOutDto = _garmentCuttingOutRepository.Find(o => o.Identity == guid).Select(cutOut => new GarmentSampleCuttingOutDto(cutOut)
        //    {
        //        Items = _garmentCuttingOutItemRepository.Find(o => o.CuttingOutId == cutOut.Identity).Select(cutOutItem => new GarmentSampleCuttingOutItemDto(cutOutItem)
        //        {
        //            Details = _garmentCuttingOutDetailRepository.Find(o => o.CuttingOutItemId == cutOutItem.Identity).Select(cutOutDetail => new GarmentSampleCuttingOutDetailDto(cutOutDetail)
        //            {

        //            }).ToList()
        //        }).ToList()
        //    }
        //    ).FirstOrDefault();
        //    var stream = GarmentSampleCuttingOutPDFTemplate.Generate(garmentCuttingOutDto, buyer);

        //    return new FileStreamResult(stream, "application/pdf")
        //    {
        //        FileDownloadName = $"{garmentCuttingOutDto.CutOutNo}.pdf"
        //    };
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleCuttingOutCommand command)
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
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleCuttingOutCommand command)
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
            var usedData = false;
            var garmentSewingIn = _garmentSewingInRepository.Query.Where(o => o.CuttingOutId == guid).Select(o => new GarmentSampleSewingIn(o)).Single();

            _garmentSewingInItemRepository.Find(x => x.SewingInId == garmentSewingIn.Identity).ForEach(async sewingDOItem =>
            {
                if (sewingDOItem.RemainingQuantity < sewingDOItem.Quantity)
                {
                    usedData = true;
                }
            });

            if (usedData == true)
            {
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Data Sudah Digunakan di Sewing In"
                });
            }
            else
            {
                RemoveGarmentSampleCuttingOutCommand command = new RemoveGarmentSampleCuttingOutCommand(guid);
                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
        }


        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentCuttingOutRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var newQuery = _garmentCuttingOutRepository.ReadExecute(query);
            
            await Task.Yield();
            return Ok(newQuery, info: new
            {
                page,
                size,
                count
            });
        }
    }
}
