﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.BusinessExceptions;

namespace BusinessLogic
{
    public class User
    {
        private Order currentOrder;

        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public void PlaceOrder(Order newOrder)
        {
            if (currentOrder != null)
            {
                throw new OrderAlreadyInProgressException();
            }
            currentOrder = newOrder;
        }
        public void ChangeOrder(Order newOrder)
        {
            if (currentOrder.Id != newOrder.Id)
            {
                throw new OrderHistoryAccessException();
            }
            currentOrder = newOrder;
        }
        public void CancelOrder()
        {
            currentOrder = null;
        }
        public Order GetCurrentOrder()
        {
            var orderToReturn = currentOrder;
            if (orderToReturn == null)
            {
                throw new OrderInvalidIdException();
            }
            return orderToReturn;
        }
        public void CreateOrder(Store store, MenuItem menuItem)
        {
            currentOrder = new Order();
            currentOrder.Store = store;
            currentOrder.Price = menuItem.Price;
            currentOrder.Details = menuItem.Details;
            currentOrder.RecipientName = this.Name;
        }
    }
}
