using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using aspdotnet_managesys.Repositories;
using Microsoft.AspNetCore.Identity;

namespace aspdotnet_managesys.Models
{
    [Table("ACCOUNT_ROLE")]
    public class AccountRole : IdentityRole
    {
        public AccountRole() : base()
        {
        }

        public AccountRole(string roleName) : base(roleName)
        {
        }

        public static AccountRole FindById(AbstractRepository repo, Guid id)
        {
            return repo.FindOne<AccountRole>(a => a.Id.Equals(id));
        }

        public static AccountRole FindByName(AbstractRepository repo, string name)
        {
            return repo.FindOne<AccountRole>(a => a.NormalizedName.Contains(name));
        }

        public void Save(AbstractRepository repo)
        {
            repo.Save<AccountRole>(this);
        }

        public void Update(AbstractRepository repo)
        {
            repo.Change<AccountRole>(this);
        }

        public void Delete(AbstractRepository repo)
        {
            repo.Delete<AccountRole>(this);
        }
    }
}