using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasMedicoEspecialidad
    {
        public CqasMedicoEspecialidad()
        {
            CqasAsignacionConsultorio = new HashSet<CqasAsignacionConsultorio>();
        }

        public int Codigo { get; set; }
        public int CodigoMedico { get; set; }
        public int CodigoEspecialidad { get; set; }
        public int Estado { get; set; }

        public CqasEspecialidad CodigoEspecialidadNavigation { get; set; }
        public CqasMedico CodigoMedicoNavigation { get; set; }
        public ICollection<CqasAsignacionConsultorio> CqasAsignacionConsultorio { get; set; }
    }
}
