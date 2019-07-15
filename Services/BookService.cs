using System;
using System.Collections.Generic;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Services
{
    public class BookService
    {
        private readonly BookRepository repo;

        public BookService(BookRepository rep) {
            repo = rep;
        }

        public List<Book> FindAllBook() {
            return repo.Transaction(() => {
                return Book.FindAll(repo);
            });
        }

        public Book FindById(int id) {
            return repo.Transaction(() => {
                return Book.FindById(repo, id);
            });
        }

        public PagedList<Book> FindBookByTitle(String keyword, int number, int size)
        {
            return repo.Transaction(() => {
                return Book.FindByTitle(repo, keyword, number, size);
            });
        }

        public PagedList<Book> FindAllBook(int number, int size)
        {
            return repo.Transaction(() => {
                return Book.FindAll(repo, number, size);
            });
        }

        public void SaveBook(Book book) {
             repo.Transaction(() => {
                 Category category = Category.FindById(repo, book.Category.Id);
                 book.Category = category;

                 Format format = Format.FindById(repo, book.Format.Id);
		         book.Format = format;

		         book.Save(repo);
             });
        }

        public void UpdateBook(Book book) {
            repo.Transaction(() => {
		        Book entity = Book.FindById(repo, book.Id);

		        if(entity != null) {
			        entity.Title = book.Title;
			        entity.Isbn = book.Isbn;

			        Category category = Category.FindById(repo, book.Category.Id);
		            entity.Category = category;

		            Format format = Format.FindById(repo, book.Format.Id);
		            entity.Format = format;

		            entity.Update(repo);
		        }
		    });
	    }
	
	    public void DeleteBook(Book book) {
		    repo.Transaction(() => book.Delete(repo));
	    }
	
     }
}