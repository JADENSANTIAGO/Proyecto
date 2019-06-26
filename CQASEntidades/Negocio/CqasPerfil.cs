using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasPerfil
    {
        public CqasPerfil()
        {
            CqasMenuPerfil = new HashSet<CqasMenuPerfil>();
            CqasUsuario = new HashSet<CqasUsuario>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }

        public ICollection<CqasMenuPerfil> CqasMenuPerfil { get; set; }
        public ICollection<CqasUsuario> CqasUsuario { get; set; }
    }
}
