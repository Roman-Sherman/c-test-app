
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Services
{
    public class AccountService
    {
        private readonly BookRepository repo;

        public AccountService(BookRepository rep) {
            repo = rep;
        }
     }
}