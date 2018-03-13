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
    public class AgendasController : Controller
    {
        //string dirBanco = Directory.GetCurrentDirectory() + "\\BD\\Banco.db";
        string dirBanco = @"D:\home\site\wwwroot\Banco.db";

        [HttpGet]
        public IEnumerable<Agenda> Get()
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Agenda>("agendas").ToList();
            }
        }

        [HttpGet("{id}", Name = "Get")]
        public Agenda Get(int id)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                return db.Query<Agenda>("agendas").Where(x => x.AgendaId == id).Single();
            }
        }

        [HttpPost]
        public void Post([FromBody]Agenda agenda)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Insert<Agenda>(agenda, "agendas");
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Agenda agenda)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Update<Agenda>(agenda, "agendas");
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var db = new LiteDB.LiteRepository(new LiteDatabase(dirBanco)))
            {
                db.Delete<Agenda>(x => x.AgendaId == id, "agendas");
            }
        }
    }
}