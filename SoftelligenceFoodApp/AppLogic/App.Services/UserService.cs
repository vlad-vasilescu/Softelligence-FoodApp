﻿using BusinessLogic;
using BusinessLogic.Abstractions;
using EF.DataAccess;

namespace Logic.Implementations
{
    public class UserService
    {
        private User user;
        private readonly IPersistenceContext dataContext;
        private ApplicationDbContext dbContext;

        public UserService(IPersistenceContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void SelectCurrentUser(int userId)
        {
            this.user = dataContext.GetUsersRepository().GetById(userId);
        }

        private Session GetActiveSession()
        {
            return dataContext.GetSessionsRepository().GetActiveSession();
        }

        public void PlaceOrder(Store store, MenuItem menuItem)
        {
            Session currentSession = GetActiveSession();

            Order newOrder = new Order();
            newOrder.Store = store;
            // newOrder.RecipientName = user.Name;
            newOrder.User = user;
            newOrder.Price = menuItem.Price;
            newOrder.Details = menuItem.Details;

            currentSession.AddOrder(newOrder);
            dataContext.GetSessionsRepository().Update(currentSession);
            /*
            dbContext.Add(newOrder);
            dbContext.SaveChanges();
            */

        }
        //order info to change
        public void ChangeOrder(Store store, MenuItem menuItem)
        {
            Order newOrder = new Order();
            newOrder.Store = store;
            // newOrder.RecipientName = user.Name;
            newOrder.User = user;
            newOrder.Price = menuItem.Price;
            newOrder.Details = menuItem.Details;

            user.ChangeOrder(newOrder);
        }
        public void CancelOrder()
        {
            user.CancelOrder();
        }
        public Order GetCurrentOrder()
        {
            return user.GetCurrentOrder();
        }
    }
}
