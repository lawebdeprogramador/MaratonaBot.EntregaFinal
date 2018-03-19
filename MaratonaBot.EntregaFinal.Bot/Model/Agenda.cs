using System;

namespace MaratonaBot.EntregaFinal.API.Model
{
    public class Agenda
    {
        public int AgendaId { get; set; }
        public DateTime Horario { get; set; }
        public Cliente Cliente { get; set; }
        public Servico Servico { get; set; }
        public bool Ocupado { get; set; }

    }
}
