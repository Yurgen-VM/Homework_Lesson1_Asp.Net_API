using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    public class ProductModel
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string CategoryName { get; set; } = String.Empty;
        public double Price { get; set; }

    }


    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet(template: "get_products")]
        public ActionResult<IEnumerable<ProductModel>> GetProducts()
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var products = context.Products.Select(x => new ProductModel
                    {
                        Name = x.Name,
                        CategoryName = x.Category!.Name, 
                        Description = x.Description,
                        Price = x.Price,
                    }).ToList();

                    return Ok(products);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpPost(template: "post_product")]
        public ActionResult AddProduct(string name, string description, int categoryId, double price)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Products.Add(new Product()
                        {
                            Name = name,
                            Description = description,
                            Price = price,
                            CategoryId = categoryId
                        });
                        context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(409);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpPut(template: "put_product")]
        public ActionResult PutProduct(string name, string newName, string newDescription, int newCategoryId, double newPrice)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var product = context.Products.SingleOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

                    if (product == null)
                    {
                        return NotFound("Такого продукта не существует");
                    }
                    product.Name = newName;
                    product.Description = newDescription;
                    product.Price = newPrice;
                    product.Category!.Id = newCategoryId;

                    context.SaveChanges();
                    return Ok();                        
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPatch(template: "patch_product")]
        public ActionResult PatchProduct(string name, double newPrice)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var product = context.Products.SingleOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

                    if (product == null)
                    {
                        return NotFound("Такого продукта не существует");
                    }
                    product.Price = newPrice;     
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete(template: "delete_product")]
        public ActionResult DeleteProduct(string name)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var product = context.Products.SingleOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

                    if (product == null)
                    {
                        return NotFound("Такого продукта не существует");
                    }
                    context.Products.Remove(product);
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
