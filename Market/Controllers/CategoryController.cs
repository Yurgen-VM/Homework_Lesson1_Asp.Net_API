using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpPost(template: "post_category")]
        public ActionResult AddСategory(string name, string description)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    if (!context.Categories.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Categories.Add(new Category()
                        {
                            Name = name,
                            Description = description,
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

        [HttpGet(template: "get_category")]
        public ActionResult<IEnumerable<CategoryModel>> GetCategory()
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var listCategory = context.Categories.Select(x => new CategoryModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,

                    }).ToList();
                    return Ok(listCategory);
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete(template: "delete_category")]
        public ActionResult DeleteCategory(string name)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var category = context.Categories.SingleOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

                    if (category == null)
                    {
                        return NotFound("Категории не существует");
                    }
                    context.Categories.Remove(category);
                    context.SaveChanges();
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut(template: "put_category")]
        public ActionResult PutCategory(string name, string newName, string newDescription)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var category = context.Categories.SingleOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));

                    if (category == null)
                    {
                        return NotFound("Категории не существует");
                    }
                    category.Name = newName;
                    if (!newDescription.Equals("old"))
                    {
                        category.Description = newDescription;
                        context.SaveChanges();
                        return Ok();
                    }
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
