using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasEstadoCivil
    {
        public CqasEstadoCivil()
        {
            CqasPersona = new HashSet<CqasPersona>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public ICollection<CqasPersona> CqasPersona { get; set; }
    }
}
