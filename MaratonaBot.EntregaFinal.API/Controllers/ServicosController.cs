using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using MaratonaBot.EntregaFinal.API.Model;
using Microsoft.AspNetCore.Mvc;

namespace MaratonaBot.EntregaFinal.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ServicosController : Controller
    {
        //string dirBanco = Directory.GetCurrentDirectory() + "\\BD\\Banco.db";
        string dirBanco = @"D:\home\site\wwwroot\Banco.db";


        [HttpGet]
        public IEnumerable<Servico> Get()
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Servico>("servicos").ToList();
            }
        }

        [HttpGet("{id}")]
        public Servico Get(int id)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Servico>("servicos").Where( x => x.ServicoId == id).Single();
            }
        }

        [HttpPost]
        public void Post([FromBody]Servico servico)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Insert<Servico>(servico,"servicos");
            }
        }

        // Por convenção vou deixar mas não é necessário.
        [HttpPut("{id}")]
        public void Put(int id,[FromBody]Servico servico)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Update<Servico>(servico, "servicos");
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Delete<Servico>( x => x.ServicoId == id,"servicos");
            }
        }
    }
}