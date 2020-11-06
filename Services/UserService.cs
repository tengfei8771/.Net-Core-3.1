using IRepository;
using IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class UserService :IUserService
    {
        private IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public object GetUserList()
        {
            return userRepository.GetInfo(t=>t.Account!="");
        }
    }
}
