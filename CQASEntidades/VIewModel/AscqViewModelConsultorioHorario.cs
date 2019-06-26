using CQASEntidades.Negocio;
using System;
using System.Collections.Generic;
namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelConsultorioHorario
    {
        public int Codigo { get; set; }
        public int CodigoConsultorioAsignado { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Dia { get; set; }
        public int Estado { get; set; }

        public List<CqasConsultorioHorario> ListaConsultorioHorario{ get; set; }
    }
}
