using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using aspdotnet_managesys.Repositories;
using Microsoft.AspNetCore.Identity;

namespace aspdotnet_managesys.Models
{
    [Table("ACCOUNT")]
    public class Account : IdentityUser {

        public string name { get; set; }

        public Account() : base() {
        }

        public Account(string userName) : base(userName) {
            this.name = userName;
        }

        public static Account FindById(AbstractRepository repo, Guid id)
        {
            return repo.FindOne<Account>(a => a.Id.Equals(id));
        }

        public static Account FindByName(AbstractRepository repo, string name)
        {
            return repo.FindOne<Account>(a => a.NormalizedUserName.Contains(name));
        }

        public void Save(AbstractRepository repo)
        {
            repo.Save<Account>(this);
        }
        
        public void Update(AbstractRepository repo)
        {
            repo.Change<Account>(this);
        }

        public void Delete(AbstractRepository repo)
        {
            repo.Delete<Account>(this);
        }
    }
}