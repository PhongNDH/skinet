using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
   
        private readonly StoreContext _context; //Convention : use _ to represent private
        public ProductsController(StoreContext context) // Dependency Injection
        {
            _context = context;
        }

        // [HttpGet]
        // public ActionResult<List<Product>> GetProducts()
        // {
        //     var products = _context.Products.ToList();
        //     return products;
        // }

        //asynchronous
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product?>> GetProduct(int id)
        {

            return await _context.Products.FindAsync(id);
        }
    }
}