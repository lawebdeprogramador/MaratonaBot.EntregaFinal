using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using MaratonaBot.EntregaFinal.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaratonaBot.EntregaFinal.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientesController : Controller
    {

        //string dirBanco = Directory.GetCurrentDirectory() + "\\BD\\Banco.db";
        string dirBanco = @"D:\home\site\wwwroot\Banco.db";

        [HttpGet]
        public IEnumerable<Cliente> Get()
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Cliente>("clientes").ToList();
            }
        }

        [HttpGet("{id}", Name = "Get")]
        public Cliente Get(int id)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Cliente>("clientes").Where(x => x.ClienteId == id).Single();
            }
        }
        
        [HttpPost]
        public void Post([FromBody]Cliente cliente)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Insert<Cliente>(cliente, "clientes");
            }
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Cliente cliente)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Update<Cliente>(cliente, "clientes");
            }
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Delete<Cliente>(x => x.ClienteId == id, "clientes");
            }
        }
    }
}
