﻿using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentLoadingIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentLoadings.Repositories
{
    public class GarmentSubconLoadingInRepository : AggregateRepostory<GarmentSubconLoadingIn, GarmentSubconLoadingInReadModel>, IGarmentSubconLoadingInRepository
    {
        public IQueryable<GarmentSubconLoadingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconLoadingInReadModel>.Filter(data, FilterDictionary);
            
            List<string> SearchAttributes = new List<string>
            {
                //"LoadingNo",
                //"Article",
                "RONo",
                //"UnitCode",
                //"UnitName",
                //"SewingDONo",
                //"UnitFromCode",
                //"UnitFromName",
                //"Items.Color",
                //"Items.ProductName"
            };
            data = QueryHelper<GarmentSubconLoadingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconLoadingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSubconLoadingIn Map(GarmentSubconLoadingInReadModel readModel)
        {
            return new GarmentSubconLoadingIn(readModel);
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconLoadingInReadModel> query) {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                LoadingNo = x.LoadingNo,
                CuttingOutId = x.CuttingOutId,
                CuttingOutNo = x.CuttingOutNo,
                RONo = x.RONo,
                Article = x.Article,
                Unit = new {
                    Id = x.UnitId, 
                    Code = x.UnitCode, 
                    Name = x.UnitName
                },
                UnitFrom = new {
                    Id = x.UnitFromId, 
                    Code = x.UnitFromCode,
                    Name = x.UnitFromName
                },
                Comodity = new {
                    Id = x.ComodityId, 
                    Code = x.ComodityCode, 
                    Name = x.ComodityName
                },
                LoadingDate = x.LoadingDate,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastModifiedBy = x.ModifiedBy,
                LastModifiedDate = x.ModifiedDate,
                Items = x.Items.Select(y => new {
                    Id = y.Identity,
                    Product = new {
                        Id = y.ProductId, 
                        Code = y.ProductCode, 
                        Name = y.ProductName
                    },
                    DesignColor = y.DesignColor,
                    Size= new {
                        Id = y.SizeId, 
                        Size = y.SizeName
                    },
                    Quantity = y.Quantity,
                    Uom = new {
                        Id = y.UomId, 
                        Unit = y.UomUnit
                    },
                    Color = y.Color,
                    RemainingQuantity = y.RemainingQuantity,
                    BasicPrice = y.BasicPrice,
                    CuttingOutDetailId = y.CuttingOutDetailId,
                    LoadingId = y.LoadingId,
                    Price = y.Price,
                    CreatedBy = y.CreatedBy,
                    CreatedDate = y.CreatedDate,
                    LastModifiedBy = y.ModifiedBy,
                    LastModifiedDate = y.ModifiedDate,
                })
            });
            
            return newQuery;
        }
    }
}
