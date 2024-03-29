﻿using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GermentReciptSubcon.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GermentReciptSubcon.GarmentSewingOuts.Repositories
{
    public class GarmentSubconSewingOutRepository : AggregateRepostory<GarmentSubconSewingOut, GarmentSubconSewingOutReadModel>, IGarmentSubconSewingOutRepository
    {
        public IQueryable<GarmentSubconSewingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query ;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingOutNo",
                "UnitCode",
                "UnitToCode",
                "RONo",
                "Article",
                "GarmentSewingOutItem.ProductCode",
                "GarmentSewingOutItem.Color"
            };

            data = QueryHelper<GarmentSubconSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconSewingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSubconSewingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };

            data = QueryHelper<GarmentSubconSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconSewingOutReadModel>.Order(data, OrderDictionary);

            //var data2 = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable ReadDynamic(string order, string search, string select, string keyword, string filter)
        {
            var data = Query ;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = JsonConvert.DeserializeObject<List<string>>(search);
            if (SearchAttributes.Count == 0)
            {
                SearchAttributes = new List<string>
                {
                    "SewingOutNo",
                    "UnitCode",
                    "UnitToCode",
                    "RONo",
                    "Article",
                    "GarmentSewingOutItem.ProductCode",
                    "GarmentSewingOutItem.Color"
                };
            }
            data = QueryHelper<GarmentSubconSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconSewingOutReadModel>.Order(data, OrderDictionary);

            var selectedData = QueryHelper<GarmentSubconSewingOutReadModel>.Select(data, select);

            return selectedData;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSubconSewingOutReadModel> query)
        {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                SewingOutNo = x.SewingOutNo,
                ProductOwner = new {
                    Id = x.ProductOwnerId,
                    Code = x.ProductOwnerCode,
                    Name = x.ProductOwnerName
                },
                Unit = new
                {
                    Id = x.UnitId,
                    Code = x.UnitCode,
                    Name = x.UnitName
                },
                SewingTo = x.SewingTo,
                UnitTo = new
                {
                    Id = x.UnitToId,
                    Code = x.UnitToCode,
                    Name = x.UnitToName
                },
                RONo = x.RONo, 
                Article = x.Article,
                Comodity = new {
                    Id = x.ComodityId,
                    Code = x.ComodityCode,
                    Name = x.ComodityName
                },
                SewingOutDate = x.SewingOutDate,
                IsDifferentSize = x.IsDifferentSize,
                Items = x.GarmentSewingOutItem.Select(garmentSewingOutItem => new {
                    Id = garmentSewingOutItem.Identity,
                    SewingOutId = garmentSewingOutItem.SewingOutId,
                    SewingInId = garmentSewingOutItem.SewingInId,
                    SewingInItemId = garmentSewingOutItem.SewingInItemId,
                    Product = new {
                        Id = garmentSewingOutItem.ProductId,
                        Code = garmentSewingOutItem.ProductCode,
                        Name = garmentSewingOutItem.ProductName
                    },
                    Size = new {
                        Id = garmentSewingOutItem.SizeId,
                        Size = garmentSewingOutItem.SizeName,
                    },
                    DesignColor = garmentSewingOutItem.DesignColor,
                    Quantity = garmentSewingOutItem.Quantity,
                    Uom = new {
                        Id = garmentSewingOutItem.UomId,
                        Unit = garmentSewingOutItem.UomUnit,
                    },
                    Color = garmentSewingOutItem.Color,
                    RealQtyOut = garmentSewingOutItem.RealQtyOut,
                    BasicPrice = garmentSewingOutItem.BasicPrice,
                    Price = garmentSewingOutItem.Price,

                    Details = garmentSewingOutItem.GarmentSewingOutDetail.Select(garmentSewingOutDetail => new {
                        Id = garmentSewingOutDetail.Identity,
                        SewingOutItemId = garmentSewingOutDetail.SewingOutItemId,
                        Size = new
                        {
                            Id = garmentSewingOutDetail.SizeId,
                            Size = garmentSewingOutDetail.SizeName,
                        },
                        Quantity = garmentSewingOutDetail.Quantity,
                        RealQtyOut = garmentSewingOutDetail.RealQtyOut,
                        Uom = new
                        {
                            Id = garmentSewingOutDetail.UomId,
                            Unit = garmentSewingOutDetail.UomUnit,
                        },
                    })

                })
            });

            return newQuery;
        }

        protected override GarmentSubconSewingOut Map(GarmentSubconSewingOutReadModel readModel)
        {
            return new GarmentSubconSewingOut(readModel);
        }
    }
}