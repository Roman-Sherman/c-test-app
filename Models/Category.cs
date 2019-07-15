using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Models
{
    [Table("CATEGORY_MASTER")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MinLength(1)]
        public string Name { get; set; }

        public static Category FindById(AbstractRepository repo, int id)
        {
            return repo.FindOne<Category>(c => c.Id == id);
        }

        public static Category FindByName(AbstractRepository repo, string name)
        {
            return repo.FindOne<Category>(c => c.Name == name);
        }

        public static List<Category> FindAll(AbstractRepository repo)
        {
            return repo.FindAll<Category>();
        }

        public void Save(AbstractRepository repo)
        {
            repo.Save<Category>(this);
        }
    }
}