﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_Project.Interfaces;
using MVC_Project.ViewModel;

namespace MVC_Project.Repository
{
    public class OrderRepository : IOrder
    {
        private readonly BookStoreContext bookStoreContext;

        public OrderRepository(BookStoreContext _bookStoreContext)
        {
            bookStoreContext = _bookStoreContext;
        }
        
        public void DeleteOrder(int id)
        {
            Order order = bookStoreContext.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                bookStoreContext.Orders.Remove(order);
                bookStoreContext.SaveChanges();
            }
        }

        public List<OrderWithCustomerAndOrderListVM> GetAllOrdersWithCustomerAndOrderList()
        {
            var ordersVM = bookStoreContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Select(o => new OrderWithCustomerAndOrderListVM
                {
                    OrderId = o.OrderId,
                    Customer = o.Customer,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderItems = o.OrderItems.ToList()
                })
                .ToList();

            return ordersVM;
        }

        public OrderWithCustomerAndOrderListVM GetOrderByIdWithCustomerAndOrderList(int id)
        {
            var orderVM = bookStoreContext.Orders
                .Where(o => o.OrderId == id)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Select(o => new OrderWithCustomerAndOrderListVM
                {
                    OrderId = o.OrderId,
                    Customer = o.Customer,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderItems = o.OrderItems.ToList()
                })
                .FirstOrDefault();

            return orderVM;
        }

        public void InsertOrder(Order order)
        {
            //var order = new Order
            //{
            //    CustomerId = orderVM.Customer.CustomerId,
            //    OrderDate = orderVM.OrderDate,
            //    TotalAmount = orderVM.TotalAmount,
            //    OrderItems = orderVM.OrderItems
            //};
            //bookStoreContext.Orders.Add(order);
            //bookStoreContext.SaveChanges();
            bookStoreContext.Orders.Add(order);
        }


        public void Save()
        {
            bookStoreContext.SaveChanges();
        }

        public void UpdateOrder(int id)
        {
            throw new NotImplementedException();
        }

        public List<OrderWithCustomerAndOrderListVM> GetOrdersByCustomerName(string customername)
        {
            List<OrderWithCustomerAndOrderListVM> orders = bookStoreContext.Orders
        .Where(o => o.Customer.FullName == customername)
        .Include(o => o.Customer)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Book)
        .Select(o => new OrderWithCustomerAndOrderListVM
        {
            OrderId = o.OrderId,
            Customer = o.Customer,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            OrderItems = o.OrderItems.ToList()
        })
        .ToList();

            return orders;
        }
        public List<OrderWithCustomerAndOrderListVM> GetOrderByCustomerId(string customerId)
        {
            var orders = bookStoreContext.Orders
        .Where(o => o.CustomerId == customerId)
        .Include(o => o.Customer)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Book)
        .Select(o => new OrderWithCustomerAndOrderListVM
        {
            OrderId = o.OrderId,
            Customer = o.Customer,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            OrderItems = o.OrderItems.ToList()
        })
        .ToList();

            return orders;
        }

		public int getOrderID(string customerID, DateTime orderDate)
		{
			var order = bookStoreContext.Orders
				.Where(o => o.CustomerId == customerID && o.OrderDate == orderDate)
				.FirstOrDefault();

			if (order != null)
			{
				return order.OrderId;
			}
			else
			{
				
				throw new Exception("Order not found");
			}
		}
	}
}
