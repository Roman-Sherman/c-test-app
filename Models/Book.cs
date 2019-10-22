using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Models
{
    [Table("BOOK")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "書籍タイトルは、{1}文字以内で入力してください。")]
        [MinLength(3, ErrorMessage = "書籍タイトルは、{1}文字以上で入力してください。")]
        public string Title { get; set; }

        [MaxLength(20, ErrorMessage = "ISBNは、{1}文字以内で入力してください。")]
        [MinLength(10, ErrorMessage = "ISBNは、{1}文字以上で入力してください。")]
        public string Isbn { get; set; }

        [Required]
        public virtual Format Format { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        public static Book FindById(AbstractRepository repo, int id)
        {
            return BookEagerFetch(repo).Where(b => b.Id == id).FirstOrDefault();
        }

        public static PagedList<Book> FindByTitle(AbstractRepository repo, string title, int page, int pageSize)
        {
            return repo.Find<Book>(BookEagerFetch(repo).Where(b => b.Title.Contains(title)), page, pageSize);
        }

        public static List<Book> FindAll(AbstractRepository repo)
        {
            return BookEagerFetch(repo).ToList();
        }

        public static PagedList<Book> FindAll(AbstractRepository repo, int page, int pageSize)
        {
            return repo.Find<Book>(BookEagerFetch(repo), page, pageSize);
        }

        private static IQueryable<Book> BookEagerFetch(AbstractRepository repo)
        {
            var books = repo.EntitySet<Book>();
            var categories = repo.EntitySet<Category>();
            var formats = repo.EntitySet<Format>();

            var outerJoin = from b in books
                            join c in categories on b.Category.Id equals c.Id into gs
                            from g in gs.DefaultIfEmpty()
                            join f in formats on b.Format.Id equals f.Id into gj
                            from h in gj.DefaultIfEmpty()
                            select new Book
                            {
                                Id = b.Id,
                                Title = b.Title,
                                Isbn = b.Isbn,
                                Category = g,
                                Format = h
                            };

            return outerJoin;
        }

        public void Save(AbstractRepository repo)
        {
            repo.Save<Book>(this);
        }

        public void Update(AbstractRepository repo)
        {
            repo.Change<Book>(this);
        }

        public void Delete(AbstractRepository repo)
        {
            repo.Delete<Book>(this);
        }
    }
}