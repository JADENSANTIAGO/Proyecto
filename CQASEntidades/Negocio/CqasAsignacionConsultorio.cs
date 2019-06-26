using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasAsignacionConsultorio
    {
        public CqasAsignacionConsultorio()
        {
            CqasConsultorioHorario = new HashSet<CqasConsultorioHorario>();
            CqasMedicoCitas = new HashSet<CqasMedicoCitas>();
        }

        public int Codigo { get; set; }
        public int CodigoConsultorio { get; set; }
        public int CodigoMedicoEspecialidad { get; set; }
        public string Estado { get; set; }

        public CqasConsultorio CodigoConsultorioNavigation { get; set; }
        public CqasMedicoEspecialidad CodigoMedicoEspecialidadNavigation { get; set; }
        public ICollection<CqasConsultorioHorario> CqasConsultorioHorario { get; set; }
        public ICollection<CqasMedicoCitas> CqasMedicoCitas { get; set; }
    }
}
