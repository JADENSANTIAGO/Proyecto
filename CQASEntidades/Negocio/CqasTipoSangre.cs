using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasTipoSangre
    {
        public CqasTipoSangre()
        {
            CqasPaciente = new HashSet<CqasPaciente>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public ICollection<CqasPaciente> CqasPaciente { get; set; }
    }
}
