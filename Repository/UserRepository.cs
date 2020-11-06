using Entity.Models;
using IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class UserRepository: BaseRepository<User>,IUserRepository
    {
        private readonly AppDBContext _appDBContext;
        public UserRepository(AppDBContext _appDBContext) : base(_appDBContext)
        {
            this._appDBContext = _appDBContext;
        }
    }
}
