using System.Collections.Generic;
using aspdotnet_managesys.Models;
using aspdotnet_managesys.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspdotnet_managesys.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : Controller
    {
        private readonly MasterService service;

        public MasterController(MasterService service)
        {
            this.service = service;
        }

        [HttpGet("category")]
        public IEnumerable<Category> ListCategory()
        {
            return service.FindAllCategories();
        }
        
        [HttpGet("format")]
        public IEnumerable<Format> ListFormat()
        {
            return service.FindAllFormats();
        }
    }
}