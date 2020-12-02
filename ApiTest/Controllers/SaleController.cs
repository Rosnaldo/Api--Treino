using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTest.Context;
using ApiTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiTest.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ApiTestContext _context;
        public SaleController(ApiTestContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SaleOrder>>> GetSales()
        {
            var SaleOrderProduct = _context.SaleOrderProduct.AsQueryable();
            var SaleOrder = _context.SaleOrder.ToList();
            var Product = _context.Product.ToList();
            var sales = await SaleOrderProduct
                .Join(SaleOrder,
                    sop => sop.SaleOrderId,
                    so => so.Id,
                    (sop, so) => new { sop, so })
                .Join(Product,
                    sopso => sopso.sop.ProductId,
                    p => p.Id,
                    (sopso, p) => new { sopso, p })
                .Select(x => new
                {
                    Buyer = x.sopso.so.Buyer,
                    Quantity = x.sopso.sop.Quantity,
                    Name = x.p.Name,
                }).ToListAsync();

            return Ok(sales);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleOrder>> GetSale(
            [FromRoute] Guid id)
        {
            var sale = await _context.SaleOrder
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        [HttpPost]
        public async Task<ActionResult<SaleOrder>> AddSale(
            [FromBody] List<SaleOrderProductDto> list)
        {
            var Id = Guid.NewGuid();
            _context.SaleOrder.Add(
            new SaleOrder
            {
                Id = Id,
                Buyer = "José",
            }
            );
            list.ForEach(o =>
            {
                _context.SaleOrderProduct.Add(
                new SaleOrderProduct
                    {
                        Quantity = o.Quantity,
                        ProductId = o.ProductId,
                        SaleOrderId = Id,
                    }
                );
            });

            await _context.SaveChangesAsync();

            return Ok();
            //if (ModelState.IsValid)
            //{
            //    _context.SaleOrder.Add(sale);
            //    await _context.SaveChangesAsync();
            //    return Ok(sale);
            //}
            //return BadRequest(ModelState);
        }
    }
}
