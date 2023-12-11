﻿using Microsoft.AspNetCore.Mvc;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using Pustok.Services.Concretes;
using System;
using System.Drawing;
using System.Linq;

namespace Pustok.Controllers.Client
{
    [Route("order")]
    public class OrderController : Controller
    {
        private readonly PustokDbContext _pustokDbContext;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public OrderController(PustokDbContext pustokDbContext, IUserService userService, IOrderService orderService)
        {
            _pustokDbContext = pustokDbContext;
            _userService = userService;
            _orderService = orderService;
        }


        [HttpGet("execute")]
        public IActionResult Execute()
        {

            var order = new Order
            {
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.New,
                User = _userService.CurrentUser,
                TrackingCode = _orderService.GenerateTrackingCode()
            };


            var basketProduct = _pustokDbContext.BasketProducts.Where(x=> x.User == _userService.CurrentUser).ToList();




            if (basketProduct.Count == 0)
            {
                return RedirectToAction("cart", "basket");
            }
            
                order.OrderProducts = basketProduct
                .Select(bp => new OrderProduct
                {
                    ColorId = bp.ColorId,
                    SizeId = bp.SizeId,
                    ProductId = bp.ProductId,
                    Quantity = bp.Quantity,
                    Order = order
                })
                .ToList();

            _pustokDbContext.Orders.Add(order);

            _pustokDbContext.BasketProducts.RemoveRange(basketProduct);


            var notification = _orderService.CreatOrderNotifications(order);


            _pustokDbContext.SaveChanges();

            return RedirectToAction("Order","Dashboard");
            
                
            
        }
    }
}
