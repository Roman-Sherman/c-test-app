using aspdotnet_managesys.Models;
using aspdotnet_managesys.Models.Dtos;
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
        public Book SaveBook([FromBody] RegBook book)
        {
            return service.SaveBook(book);
        }

        [HttpPost("edit")]
        public Book EditBook([FromBody] ChgBook book)
        {
            return service.UpdateBook(book);
        }

        [HttpPost("delete")]
        public Book DeleteBook([FromBody] ChgBook book)
        {
            return service.DeleteBook(book);
        }
    }
}