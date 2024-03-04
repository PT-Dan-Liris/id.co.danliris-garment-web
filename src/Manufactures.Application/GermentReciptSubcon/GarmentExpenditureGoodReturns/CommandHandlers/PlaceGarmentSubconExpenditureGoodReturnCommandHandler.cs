﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentPackingOut;
using Manufactures.Domain.GarmentPackingOut.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Commands;
using Manufactures.Domain.GermentReciptSubcon.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks;
using Manufactures.Domain.GermentReciptSubcon.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentReturGoodReturns.Commands;

namespace Manufactures.Application.GermentReciptSubcon.GarmentExpenditureGoodReturns.CommandHandlers
{
    public class PlaceGarmentSubconExpenditureGoodReturnCommandHandler : ICommandHandler<PlaceSubconGarmentExpenditureGoodReturnCommand, GarmentSubconExpenditureGoodReturn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconExpenditureGoodReturnRepository _garmentExpenditureGoodReturnRepository;
        private readonly IGarmentSubconExpenditureGoodReturnItemRepository _garmentExpenditureGoodReturnItemRepository;
        private readonly IGarmentSubconFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentSubconPackingOutItemRepository _garmentExpenditureGoodItemRepository;
        private readonly IGarmentSubconPackingOutRepository _garmentExpenditureGoodRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public PlaceGarmentSubconExpenditureGoodReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentSubconExpenditureGoodReturnRepository>();
            _garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentSubconExpenditureGoodReturnItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSubconFinishedGoodStockRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentSubconPackingOutItemRepository>();
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentSubconPackingOutRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentSubconExpenditureGoodReturn> Handle(PlaceSubconGarmentExpenditureGoodReturnCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

            GarmentSubconExpenditureGoodReturn garmentExpenditureGoodReturn = new GarmentSubconExpenditureGoodReturn(
                Guid.NewGuid(),
                GenerateExpenditureGoodReturnNo(request),
                request.ReturType,
                request.PackingOutNo,
                request.DONo,
                request.URNNo,
                request.BCNo,
                request.BCType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                request.ReturDate.GetValueOrDefault(),
                request.Invoice,
                request.ReturDesc
            );

            Dictionary<Guid, double> finstockQty = new Dictionary<Guid, double>();
            Dictionary<string, double> finStockToBeUpdated = new Dictionary<string, double>();
            Dictionary<Guid, double> exGoodToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                if (item.isSave)
                {
                    //var garmentexGood = _garmentExpenditureGoodRepository.Query.Where(x=>x.UnitId==request.Unit.Id && x.RONo==request.RONo).OrderBy(a => a.CreatedDate).ToList();
                    var garmentexGood = _garmentExpenditureGoodRepository.Query.Where(x => x.UnitId == request.Unit.Id && x.PackingOutNo == request.PackingOutNo).OrderBy(a => a.CreatedDate).ToList();
                    var garmentexGoodItem = _garmentExpenditureGoodItemRepository.Query.Where(x => x.SizeId == item.Size.Id && x.UomId == item.Uom.Id && (x.Quantity- x.ReturQuantity) > 0 && x.Description==item.Description).OrderBy(a => a.CreatedDate).ToList();
                    var join = (from a in garmentexGoodItem join b in garmentexGood on a.PackingOutId equals b.Identity select a).ToList();
                    double qty = item.Quantity;
                    foreach (var exGood in join)
                    {
                        string key = exGood.FinishedGoodStockId.ToString() + "~" + item.Description;
                        if (qty > 0)
                        {
                            double exGoodQty = exGood.Quantity - exGood.ReturQuantity;
                            double remainQty = exGoodQty - qty;

                            if (!finstockQty.ContainsKey(exGood.FinishedGoodStockId))
                            {
                                finstockQty.Add(exGood.FinishedGoodStockId, 0);
                            }
                            //double stockQty = exGoodQty - qty;
                            if (remainQty < 0)
                            {
                                qty -= exGood.Quantity;
                                exGoodToBeUpdated.Add(exGood.Identity, exGoodQty);
                                finstockQty[exGood.FinishedGoodStockId] += exGoodQty;
                                //finStockToBeUpdated.Add(key, exGoodQty);
                            }
                            else if (remainQty == 0)
                            {
                                exGoodToBeUpdated.Add(exGood.Identity, exGoodQty);
                                finstockQty[exGood.FinishedGoodStockId] += exGoodQty;
                                //finStockToBeUpdated.Add(key, exGoodQty);
                                break;
                            }
                            else if (remainQty > 0)
                            {
                                exGoodToBeUpdated.Add(exGood.Identity, qty);
                                finstockQty[exGood.FinishedGoodStockId] += qty;
                                //finStockToBeUpdated.Add(key, qty);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var exGood in exGoodToBeUpdated)
            {
                var garmentExpenditureGoodItem = _garmentExpenditureGoodItemRepository.Query.Where(x => x.Identity == exGood.Key).Select(s => new GarmentSubconPackingOutItem(s)).Single();

                //var dup= request.Items.Where(a =>  new SizeId(a.Size.Id) == garmentExpenditureGoodItem.SizeId && new UomId(a.Uom.Id) == garmentExpenditureGoodItem.UomId && a.isSave == true).FirstOrDefault();

                var item = request.Items.Where(a => a.Description.Trim()==garmentExpenditureGoodItem.Description.Trim() && new SizeId(a.Size.Id) == garmentExpenditureGoodItem.SizeId && new UomId(a.Uom.Id) == garmentExpenditureGoodItem.UomId && a.isSave==true ).Single();

                var quantityRetur = garmentExpenditureGoodItem.ReturQuantity + exGood.Value;
                double price = (garmentExpenditureGoodItem.BasicPrice + ((double)garmentComodityPrice.Price * 1)) * quantityRetur;

                GarmentSubconExpenditureGoodReturnItem garmentExpenditureGoodReturnItem = new GarmentSubconExpenditureGoodReturnItem(
                    Guid.NewGuid(),
                    garmentExpenditureGoodReturn.Identity,
                    garmentExpenditureGoodItem.PackingOutId,
                    garmentExpenditureGoodItem.Identity,
                    garmentExpenditureGoodItem.FinishedGoodStockId,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    exGood.Value,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Description,
                    garmentExpenditureGoodItem.BasicPrice,
                    price
                );

                await _garmentExpenditureGoodReturnItemRepository.Update(garmentExpenditureGoodReturnItem);

                var qty = garmentExpenditureGoodItem.ReturQuantity + exGood.Value;
                garmentExpenditureGoodItem.SetReturQuantity(qty);
                garmentExpenditureGoodItem.Modify();

                await _garmentExpenditureGoodItemRepository.Update(garmentExpenditureGoodItem);

            }

            foreach (var finStock in finstockQty)
            {
                //var keyString = finStock.Key.Split("~");
                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentSubconFinishedGoodStock(s)).Single();
                var qty = garmentFinishingGoodStockItem.Quantity + finStock.Value;
                garmentFinishingGoodStockItem.SetQuantity(qty);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
            }

            await _garmentExpenditureGoodReturnRepository.Update(garmentExpenditureGoodReturn);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI RETUR BARANG JADI - TERIMA SUBCON", "Create Retur Barang Jadi - Terima Subcon - " + garmentExpenditureGoodReturn.ReturNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return garmentExpenditureGoodReturn;
        }

        private string GenerateExpenditureGoodReturnNo(PlaceSubconGarmentExpenditureGoodReturnCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"RL{unitcode}{year}{month}";

            var lastReturNo = _garmentExpenditureGoodReturnRepository.Query.Where(w => w.ReturNo.StartsWith(prefix))
                .OrderByDescending(o => o.ReturNo)
                .Select(s => int.Parse(s.ReturNo.Replace(prefix, "")))
                .FirstOrDefault();
            var returNo = $"{prefix}{(lastReturNo + 1).ToString("D4")}";

            return returNo;
        }
    }
}
