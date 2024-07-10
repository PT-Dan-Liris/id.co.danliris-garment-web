﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood;
using Manufactures.Domain.GarmentSample.ServiceSampleExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes;
using Manufactures.Domain.GarmentSample.ServiceSampleFabricWashes.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings;
using Manufactures.Domain.GarmentSample.ServiceSampleSewings.Repositories;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels;
using Manufactures.Domain.GarmentSample.ServiceSampleShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood;
using Manufactures.Domain.GarmentSubcon.ServiceSubconExpenditureGood.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class RemoveGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<RemoveGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutDetailRepository _garmentSubconDeliveryLetterOutDetailRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentServiceSubconCuttingRepository _garmentSubconCuttingRepository;
        private readonly IGarmentServiceSubconSewingRepository _garmentSubconSewingRepository;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly IGarmentServiceSubconExpenditureGoodRepository _garmentServiceSubconExpenditureGoodRepository;
        //SampleSubcon
        private readonly IGarmentServiceSampleCuttingRepository _garmentSubconSampleCuttingRepository;
        private readonly IGarmentServiceSampleSewingRepository _garmentSubconSampleSewingRepository;
        private readonly IGarmentServiceSampleShrinkagePanelRepository _garmentServiceSubconSampleShrinkagePanelRepository;
        private readonly IGarmentServiceSampleFabricWashRepository _garmentServiceSubconSampleFabricWashRepository;
        private readonly IGarmentServiceSampleExpenditureGoodRepository _garmentServiceSubconSampleExpenditureGoodRepository;

        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentSubconDeliveryLetterOutDetailRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutDetailRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            _garmentServiceSubconExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSubconExpenditureGoodRepository>();
            //Sample
            _garmentSubconSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentSubconSampleSewingRepository = storage.GetRepository<IGarmentServiceSampleSewingRepository>();
            _garmentServiceSubconSampleShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSampleShrinkagePanelRepository>();
            _garmentServiceSubconSampleFabricWashRepository = storage.GetRepository<IGarmentServiceSampleFabricWashRepository>();
            _garmentServiceSubconSampleExpenditureGoodRepository = storage.GetRepository<IGarmentServiceSampleExpenditureGoodRepository>();

            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }


        public async Task<GarmentSubconDeliveryLetterOut> Handle(RemoveGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
            {
                //Detail
                _garmentSubconDeliveryLetterOutDetailRepository.Find(s => s.SubconDeliveryLetterOutItemId == subconDeliveryLetterOutItem.Identity).ForEach(async subconDeliveryLetterOutDetail =>
                {
                    subconDeliveryLetterOutDetail.Remove();
                    await _garmentSubconDeliveryLetterOutDetailRepository.Update(subconDeliveryLetterOutDetail);
                });

                subconDeliveryLetterOutItem.Remove();
                //New Query
                if (subconDeliveryLetterOut.OrderType == "SAMPLE")
                {
                    switch (subconDeliveryLetterOut.SubconCategory)
                    {
                        case "SUBCON JASA KOMPONEN":
                            var subconCutting = _garmentSubconSampleCuttingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSampleCutting(s)).Single();
                            subconCutting.SetIsUsed(false);
                            subconCutting.Modify();

                            await _garmentSubconSampleCuttingRepository.Update(subconCutting);
                            break;
                        case "SUBCON JASA GARMENT WASH":
                            var subconSewing = _garmentSubconSampleSewingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSampleSewing(s)).Single();
                            subconSewing.SetIsUsed(false);
                            subconSewing.Modify();

                            await _garmentSubconSampleSewingRepository.Update(subconSewing);
                            break;
                        case "SUBCON BB SHRINKAGE/PANEL":
                            var subconPanel = _garmentServiceSubconSampleShrinkagePanelRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSampleShrinkagePanel(s)).Single();
                            subconPanel.SetIsUsed(false);
                            subconPanel.Modify();

                            await _garmentServiceSubconSampleShrinkagePanelRepository.Update(subconPanel);
                            break;
                        case "SUBCON BB FABRIC WASH/PRINT":
                            var subconFabric = _garmentServiceSubconSampleFabricWashRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSampleFabricWash(s)).Single();
                            subconFabric.SetIsUsed(false);
                            subconFabric.Modify();

                            await _garmentServiceSubconSampleFabricWashRepository.Update(subconFabric);
                            break;
                        case "SUBCON JASA BARANG JADI":
                            var subconExpenditureGood = _garmentServiceSubconSampleExpenditureGoodRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSampleExpenditureGood(s)).Single();
                            subconExpenditureGood.SetIsUsed(false);
                            subconExpenditureGood.Modify();

                            await _garmentServiceSubconSampleExpenditureGoodRepository.Update(subconExpenditureGood);
                            break;
                    }
                }
                else
                {
                    switch (subconDeliveryLetterOut.SubconCategory)
                    {
                        case "SUBCON SEWING":
                            if (subconDeliveryLetterOutItem.SubconId != Guid.Empty)
                            {
                                var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                                subconCuttingOut.SetIsUsed(false);
                                subconCuttingOut.Modify();

                                await _garmentCuttingOutRepository.Update(subconCuttingOut);
                                
                            }
                            break;
                        case "SUBCON JASA KOMPONEN":
                            var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                            subconCutting.SetIsUsed(false);
                            subconCutting.Modify();

                            await _garmentSubconCuttingRepository.Update(subconCutting);
                            break;
                        case "SUBCON JASA GARMENT WASH":
                            var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                            subconSewing.SetIsUsed(false);
                            subconSewing.Modify();

                            await _garmentSubconSewingRepository.Update(subconSewing);
                            break;
                        case "SUBCON BB SHRINKAGE/PANEL":
                            var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                            subconPanel.SetIsUsed(false);
                            subconPanel.Modify();

                            await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                            break;
                        case "SUBCON BB FABRIC WASH/PRINT":
                            var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                            subconFabric.SetIsUsed(false);
                            subconFabric.Modify();

                            await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                            break;
                        case "SUBCON JASA BARANG JADI":
                            var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconExpenditureGood(s)).Single();
                            subconExpenditureGood.SetIsUsed(false);
                            subconExpenditureGood.Modify();

                            await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);
                            break;
                    }
                }
                #region OldQuery
                //if (subconDeliveryLetterOut.SubconCategory == "SUBCON SEWING")
                //{
                //    var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                //    subconCuttingOut.SetIsUsed(false);
                //    subconCuttingOut.Modify();

                //    await _garmentCuttingOutRepository.Update(subconCuttingOut);
                //}
                //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA KOMPONEN")
                //{
                //    var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                //    subconCutting.SetIsUsed(false);
                //    subconCutting.Modify();

                //    await _garmentSubconCuttingRepository.Update(subconCutting);
                //}
                //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA GARMENT WASH")
                //{
                //    var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                //    subconSewing.SetIsUsed(false);
                //    subconSewing.Modify();

                //    await _garmentSubconSewingRepository.Update(subconSewing);
                //}
                //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                //{
                //    var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                //    subconPanel.SetIsUsed(false);
                //    subconPanel.Modify();

                //    await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                //}
                //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT")
                //{
                //    var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                //    subconFabric.SetIsUsed(false);
                //    subconFabric.Modify();

                //    await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                //}
                //else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA BARANG JADI")
                //{
                //    var subconExpenditureGood = _garmentServiceSubconExpenditureGoodRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconExpenditureGood(s)).Single();
                //    subconExpenditureGood.SetIsUsed(false);
                //    subconExpenditureGood.Modify();

                //    await _garmentServiceSubconExpenditureGoodRepository.Update(subconExpenditureGood);
                //}
                #endregion
                await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
            });

            var subconDLOuts = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.SubconContractId == subconDeliveryLetterOut.SubconContractId && o.Identity!=request.Identity).FirstOrDefault();
            if (subconDLOuts == null)
            {
                var subconContract = _garmentSubconContractRepository.Query.Where(x => x.Identity == subconDeliveryLetterOut.SubconContractId).Select(s => new GarmentSubconContract(s)).Single();
                subconContract.SetIsUsed(false);
                subconContract.Modify();

                await _garmentSubconContractRepository.Update(subconContract);
            }

            subconDeliveryLetterOut.Remove();
            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PEMBELIAN", "Delete Surat Jalan Subcon - " + subconDeliveryLetterOut.DLNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
    }
}
