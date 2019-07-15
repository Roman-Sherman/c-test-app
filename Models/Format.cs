using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Models
{
    [Table("FORMAT_MASTER")]
    public class Format
    {
        [Key]
        public int Id { get; set; }

        [MinLength(1)]
        public string Name { get; set; }

        public static Format FindById(AbstractRepository repo, int id)
        {
            return repo.FindOne<Format>(c => c.Id == id);
        }

        public static Format FindByName(AbstractRepository repo, string name)
        {
            return repo.FindOne<Format>(c => c.Name == name);
        }

        public static List<Format> FindAll(AbstractRepository repo)
        {
            return repo.FindAll<Format>();
        }

        public void Save(AbstractRepository repo)
        {
            repo.Save<Format>(this);
        }
    }
}