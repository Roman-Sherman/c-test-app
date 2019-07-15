using aspdotnet_managesys.Models;
using aspdotnet_managesys.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using aspdotnet_managesys.Logger;

namespace aspdotnet_managesys.Repositories
{
    public class BookRepository : AbstractRepository
    {
        public BookRepository(DbContextOptions options) : base(options) {
            // ロガーの設定
            var serviceProvider = this.GetInfrastructure();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new Log4NetProvider("log4net.config"));
        }

        public DbSet<Book> Books { get; set;}
        public DbSet<Format> Formats { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
    }
}