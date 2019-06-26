using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasConsultorioHorario
    {
        public CqasConsultorioHorario()
        {
            CqasCita = new HashSet<CqasCita>();
        }

        public int Codigo { get; set; }
        public int CodigoConsultorioAsignado { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Dia { get; set; }
        public int Estado { get; set; }

        public CqasAsignacionConsultorio CodigoConsultorioAsignadoNavigation { get; set; }
        public ICollection<CqasCita> CqasCita { get; set; }
    }
}
