using aspdotnet_managesys.Models;
using aspdotnet_managesys.Repositories;
using aspdotnet_managesys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet_managesys.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly BookService service;

        public BookController(BookService service)
        {
            this.service = service;
        }

        [HttpGet("list")]
        public PagedList<Book> GetAll([FromQuery] PagedList<Book> list)
        {
            return service.FindAllBook(list.Page, list.Size);
        }

        [HttpGet("search")]
        public PagedList<Book> Search([FromQuery] string query, [FromQuery] PagedList<Book> list)
        {
            return service.FindBookByTitle(query, list.Page, list.Size);
        }

        [HttpPost("new")]
        public void SaveBook([FromBody] Book book)
        {
            service.SaveBook(book);
        }

        [HttpPost("edit")]
        public void EditBook([FromBody] Book book)
        {
            service.UpdateBook(book);
        }

        [HttpPost("delete")]
        public void DeleteBook([FromBody] Book book)
        {
            Book b = service.FindById(book.Id);
            service.DeleteBook(b);
        }
    }
}