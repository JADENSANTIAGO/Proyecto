using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasConsultorio
    {
        public CqasConsultorio()
        {
            CqasAsignacionConsultorio = new HashSet<CqasAsignacionConsultorio>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public ICollection<CqasAsignacionConsultorio> CqasAsignacionConsultorio { get; set; }
    }
}
