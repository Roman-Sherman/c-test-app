using System;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace aspdotnet_managesys.Common
{
    public static class MasterDataGenerator
    {
        public static void InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                BookRepository rep = serviceScope.ServiceProvider.GetService<BookRepository>();
                UserManager<Account> userManager = serviceScope.ServiceProvider.GetService<UserManager<Account>>();
                MasterDataGenerator.Initialize(rep, userManager);
            }
        }

        public static void Initialize(BookRepository rep, UserManager<Account> userManager)
        {
            // データベースの初期化
            rep.Database.EnsureDeleted();
            // テーブル生成
            rep.Database.EnsureCreated();
            // アカウント作成
            CreateUserAsync(userManager);
            // 初期データ挿入
            InsertTestData(rep);
        }

        private static void InsertTestData(BookRepository rep)
        {
            Format f1 = new Format { Name = "書籍" };
            f1.Save(rep);

            Format f2 = new Format { Name = "電子書籍" };
            f2.Save(rep);

            Category c1 = new Category { Name = "技術書" };
            c1.Save(rep);

            Category c2 = new Category { Name = "小説" };
            c2.Save(rep);

            Category c3 = new Category { Name = "雑誌" };
            c3.Save(rep);

            for (int i = 0; i < 7; i++)
            {
                Category c = Category.FindById(rep, (i + 1) % 3 + 1);
                Format f = Format.FindById(rep, (i + 1) % 2 + 1);
                Book b = new Book
                {
                    Title = "Test_" + i,
                    Isbn = "123-234-567-" + i,
                    Category = c,
                    Format = f
                };
                b.Save(rep);
            }

            rep.SaveChanges();
        }

        private static async void CreateUserAsync(UserManager<Account> userManager)
        {
            var user = new Account("test");
            var password = "Pa$$w0rd";
            await userManager.CreateAsync(user, password);
        }
    }
}
