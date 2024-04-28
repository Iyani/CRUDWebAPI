using CrudWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext dbContext;

        public BrandController(BrandContext context)
        {
            dbContext = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if(dbContext.Brands == null)
            {
                return NotFound();
            }
            return await dbContext.Brands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            if (dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await dbContext.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        {
            dbContext.Brands.Add(brand);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrandById), new {id = brand.ID}, brand);
        }

        [HttpPut]
        public async Task<ActionResult<Brand>> EditBrand(int id, Brand brand)
        {
            if(id != brand.ID)
            {
                return NotFound();
            }
            dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!isBrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool isBrandAvailable(int id)
        {
            return dbContext.Brands.Any(b => b.ID == id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            if(dbContext.Brands == null)
            {
                return NotFound();
            }

            var brand = await dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            dbContext.Remove(brand);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
