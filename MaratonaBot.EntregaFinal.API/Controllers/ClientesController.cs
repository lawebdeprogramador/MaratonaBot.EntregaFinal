using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpGet("{name}")]
        public Cliente GetByName(string name)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Cliente>("clientes").Where(x => x.Nome.ToLowerInvariant() == name.ToLowerInvariant()).SingleOrDefault();
            }
        }
        
        [HttpPost]
        public Cliente Post([FromBody]Cliente cliente)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                cliente.ClienteId = db.Insert<Cliente>(cliente, "clientes").AsInt32 ;

                return cliente;
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
