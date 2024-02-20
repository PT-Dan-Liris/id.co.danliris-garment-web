﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers
{
    public class PlaceGarmentSubconCustomsOutCommandHandler : ICommandHandler<PlaceGarmentSubconCustomsOutCommand, GarmentSubconCustomsOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconCustomsOutDetailRepository _garmentSubconCustomsOutDetailRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public PlaceGarmentSubconCustomsOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
            _garmentSubconCustomsOutDetailRepository = storage.GetRepository<IGarmentSubconCustomsOutDetailRepository>();
        }

        public async Task<GarmentSubconCustomsOut> Handle(PlaceGarmentSubconCustomsOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconCustomsOut garmentSubconCustomsOut = new GarmentSubconCustomsOut(
                Guid.NewGuid(),
                request.CustomsOutNo,
                request.CustomsOutDate,
                request.CustomsOutType,
                request.SubconType,
                request.SubconContractId,
                request.SubconContractNo,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.Remark,
                request.SubconCategory,
                request.BuyerStaff
            );

            foreach (var item in request.Items)
            {
                GarmentSubconCustomsOutItem garmentSubconCustomsOutItem = new GarmentSubconCustomsOutItem(
                    Guid.NewGuid(),
                    garmentSubconCustomsOut.Identity,
                    item.SubconDLOutNo,
                    item.SubconDLOutId,
                    item.Quantity
                );

                foreach(var detail in item.Details)
                {
                    GarmentSubconCustomsOutDetail garmentSubconCustomsOutDetail = new GarmentSubconCustomsOutDetail(
                        Guid.NewGuid(),
                        garmentSubconCustomsOutItem.Identity,
                        new ProductId(detail.Product.Id),
                        detail.Product.Code,
                        detail.Product.Name,
                        detail.Product.Remark,
                        detail.Quantity,
                        new UomId(detail.Uom.Id),
                        detail.Uom.Unit
                        );

                    await _garmentSubconCustomsOutDetailRepository.Update(garmentSubconCustomsOutDetail);
                }

                var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == item.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                subconDLOut.SetIsUsed(true);
                subconDLOut.Modify();
                await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);
                await _garmentSubconCustomsOutItemRepository.Update(garmentSubconCustomsOutItem);
            }

            var subconContract = _garmentSubconContractRepository.Query.Where(x => x.Identity == request.SubconContractId).Select(n => new GarmentSubconContract(n)).Single();
            subconContract.SetIsCustoms(true);
            subconContract.Modify();
            await _garmentSubconContractRepository.Update(subconContract);

            await _garmentSubconCustomsOutRepository.Update(garmentSubconCustomsOut);

            ////Add Log History
            //LogHistory logHistory = new LogHistory(new Guid(), "EXIM", "Create BC Keluar Subcon - " + garmentSubconCustomsOut.CustomsOutNo, DateTime.Now);
            //await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return garmentSubconCustomsOut;
        }
    }
}
