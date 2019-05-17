
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Abstractions;
using BusinessLogic;
using System.Collections.Generic;
using System.Threading.Tasks;
using EF.DataAccess.DataModel;
using AutoMapper;
using System.Linq;
using System;
using BusinessLogic.Business.Exceptions;

namespace EF.DataAccess
{
    public class UserRepositoryEF : IUsersRepository
    {

        private readonly ApplicationDbContext dbContext;
        private readonly EntitiesMapper mapper;

        public User GetById(int id)
        {

            var userWithId = dbContext.Users.SingleOrDefault(user => user.Id == id);

            if (userWithId == null)
            {
                throw new UserNotFoundException("User not found");
            }

            return mapper.MapData<User, UserDO>(userWithId);
            //mapper.MapData<User, UserDO>();
        }

        public String GetEmail(int id)
        {
            return GetById(id).Email;
        }

        public String GetName(int id)
        {
            return GetById(id).Name;
        }

        public UserRepositoryEF(ApplicationDbContext dbContext, EntitiesMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

    }
}
