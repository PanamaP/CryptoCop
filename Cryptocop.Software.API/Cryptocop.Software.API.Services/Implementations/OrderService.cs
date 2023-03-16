using System;
using System.Collections.Generic;
using System.Net;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IQueueService _queueService;

        public OrderService(IOrderRepository orderRepository, IShoppingCartService shoppingCartService, IQueueService queueService)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _queueService = queueService;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            return _orderRepository.GetOrders(email);
        }

        public bool CreateNewOrder(string email, OrderInputModel order)
        {
            // Create a new order using the appropriate repository class
            // Delete the current shopping cart
            // Publish a message to RabbitMQ with the routing key create-order and include the newly created order
            var newOrder = _orderRepository.CreateNewOrder(email, order);
            if (newOrder == null){return false;}
            _shoppingCartService.ClearCart(email);
            _queueService.PublishMessage("create-order", newOrder);
            return true;
        }
    }
}