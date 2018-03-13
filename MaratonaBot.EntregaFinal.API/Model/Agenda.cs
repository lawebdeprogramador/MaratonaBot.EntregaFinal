using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaratonaBot.EntregaFinal.API.Model
{
    public class Agenda
    {
        public int AgendaId { get; set; }
        public DateTime Horario { get; set; }
        [BsonRef("clientes")]
        public Cliente Cliente { get; set; }
        [BsonRef("servicos")]
        public Servico Servico { get; set; }
        public bool Ocupado { get; set; }

    }
}
