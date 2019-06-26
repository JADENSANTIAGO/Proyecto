using System;
using System.Collections.Generic;


namespace CQASEntidades.Negocio
{
    public partial class CqasGenero
    {
        public CqasGenero()
        {
            CqasPersona = new HashSet<CqasPersona>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public ICollection<CqasPersona> CqasPersona { get; set; }
    }
}
