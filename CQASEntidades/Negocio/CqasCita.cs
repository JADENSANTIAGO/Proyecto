using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasCita
    {
        public CqasCita()
        {
            CqasHistoriaClinica = new HashSet<CqasHistoriaClinica>();
        }

        public int Codigo { get; set; }
        public int CodigoPaciente { get; set; }
        public int CodigoConsultorioHorario { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraIncio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Estado { get; set; }

        public CqasConsultorioHorario CodigoConsultorioHorarioNavigation { get; set; }
        public CqasPaciente CodigoPacienteNavigation { get; set; }
        public ICollection<CqasHistoriaClinica> CqasHistoriaClinica { get; set; }
    }
}
