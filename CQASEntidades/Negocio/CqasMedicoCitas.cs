using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasMedicoCitas
    {
        public int Codigo { get; set; }
        public int CodigoAsignacionConsultorio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public CqasAsignacionConsultorio CodigoAsignacionConsultorioNavigation { get; set; }
    }
}
