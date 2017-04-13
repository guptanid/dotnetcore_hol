﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpyStore_HOL.DAL.EF;
using SpyStore_HOL.DAL.Repos.Base;
using SpyStore_HOL.DAL.Repos.Interfaces;
using SpyStore_HOL.Models.Entities;
using SpyStore_HOL.Models.ViewModels;

namespace SpyStore_HOL.DAL.Repos
{
    public class OrderDetailRepo : RepoBase<OrderDetail>, IOrderDetailRepo
    {
        public OrderDetailRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public OrderDetailRepo()
        {
        }

        public override IEnumerable<OrderDetail> GetAll()
            => Table.OrderBy(x => x.Id);

        public override IEnumerable<OrderDetail> GetRange(int skip, int take)
            => GetRange(Table.OrderBy(x => x.Id), skip, take);

        internal IEnumerable<OrderDetailWithProductInfo> GetRecords(IQueryable<OrderDetail> query) 
            => query
                .Include(x => x.Product)
                .ThenInclude(p => p.Category)
                .Select(x => new OrderDetailWithProductInfo
                {
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitCost = x.UnitCost,
                    LineItemTotal = x.LineItemTotal,
                    Description = x.Product.Description,
                    ModelName = x.Product.ModelName,
                    ProductImage = x.Product.ProductImage,
                    ProductImageLarge = x.Product.ProductImageLarge,
                    ProductImageThumb = x.Product.ProductImageThumb,
                    ModelNumber = x.Product.ModelNumber,
                    CategoryName = x.Product.Category.CategoryName
                })
                .OrderBy(x => x.ModelName);

        public IEnumerable<OrderDetailWithProductInfo> GetCustomersOrdersWithDetails(int customerId) 
            => GetRecords(Table.Where(x => x.Order.CustomerId == customerId));

        public IEnumerable<OrderDetailWithProductInfo> GetSingleOrderWithDetails(int orderId)
            => GetRecords(Table.Where(x => x.Order.Id == orderId));

    }
}

