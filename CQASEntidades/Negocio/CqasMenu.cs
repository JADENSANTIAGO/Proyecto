using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasMenu
    {
        public CqasMenu()
        {
            CqasMenuPerfil = new HashSet<CqasMenuPerfil>();
        }

        public int Codigo { get; set; }
        public string Cabezera { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public int Estado { get; set; }

        public ICollection<CqasMenuPerfil> CqasMenuPerfil { get; set; }
    }
}
