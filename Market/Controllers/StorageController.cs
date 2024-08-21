using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{

    public class StorageModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;    

    }

    [ApiController]
    [Route("[controller]")]
    public class StorageController: ControllerBase
    {
        [HttpPost(template: "post_storage")]
        public ActionResult AddStorage(string name, string description, int productId)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    if (!context.Storages.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Storages.Add(new Storage()
                        {
                            Name = name,
                            Description = description,
                            ProductId = productId
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



        [HttpGet(template: "get_srorage")]
        public ActionResult<IEnumerable<StorageModel>> GetStorages()
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var storages = context.Storages.Select(s => new StorageModel
                    {
                        Name = s.Name,
                        Description = s.Description,
                        Id = s.Id,

                    }).ToList();
                    if (storages.Count == 0)
                    {
                        return Ok("Список пуст");
                    }
                    return Ok(storages);
                }
            }
            catch
            {
                return StatusCode(500);
            }           
        }
    }
}
