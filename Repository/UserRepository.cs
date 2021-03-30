using Entity.Models;
using IRepository;
using System.Data;
using System.Linq;

namespace Repository
{
    public class UserRepository: BaseRepository<User>,IUserRepository
    {
        private readonly AppDBContext _appDBContext;
        public UserRepository(AppDBContext _appDBContext) : base(_appDBContext)
        {
            this._appDBContext = _appDBContext;
        }
        //public DataTable GetUserInfo()
        //{
        //    var list= _appDBContext.Set<User>().Join()
        //}
    }
}
