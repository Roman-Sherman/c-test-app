using System;
using System.Collections.Generic;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Models.Dtos;
using aspdotnet_managesys.Repositories;

namespace aspdotnet_managesys.Services
{
    public class BookService
    {
        private readonly BookRepository repo;

        public BookService(BookRepository rep)
        {
            repo = rep;
        }

        public List<Book> FindAllBook()
        {
            return repo.Transaction(() =>
            {
                return Book.FindAll(repo);
            });
        }

        public Book FindById(int id)
        {
            return repo.Transaction(() =>
            {
                return Book.FindById(repo, id);
            });
        }

        public PagedList<Book> FindBookByTitle(String keyword, int number, int size)
        {
            return repo.Transaction(() =>
            {
                return Book.FindByTitle(repo, keyword, number, size);
            });
        }

        public PagedList<Book> FindAllBook(int number, int size)
        {
            return repo.Transaction(() =>
            {
                return Book.FindAll(repo, number, size);
            });
        }

        public Book SaveBook(RegBook book)
        {
            return repo.Transaction(() =>
            {
                Category category = Category.FindById(repo, book.CategoryId);
                Format format = Format.FindById(repo, book.FormatId);

                Book entity = book.create();
                if (category != null && format != null)
                {
                    entity.Category = category;
                    entity.Format = format;
                    entity.Save(repo);
                    return entity;
                }
                return null;
            });
        }

        public Book UpdateBook(ChgBook book)
        {
            return repo.Transaction(() =>
            {
                Book entity = Book.FindById(repo, book.Id);

                if (entity != null)
                {
                    entity.Title = book.Title;
                    entity.Isbn = book.Isbn;

                    Category category = Category.FindById(repo, book.CategoryId);
                    Format format = Format.FindById(repo, book.FormatId);

                    if (category != null && format != null)
                    {
                        entity.Category = category;
                        entity.Format = format;
                        entity.Update(repo);
                        return entity;
                    }
                    return null;
                }
                return null;
            });
        }

        public Book DeleteBook(ChgBook book)
        {
            return repo.Transaction(() =>
            {
                Book entity = Book.FindById(repo, book.Id);
                if (entity != null)
                {
                    entity.Delete(repo);
                    return entity;
                }
                return null;
            });
        }

    }
}