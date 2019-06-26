using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasSexo
    {
        public CqasSexo()
        {
            CqasPaciente = new HashSet<CqasPaciente>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }

        public ICollection<CqasPaciente> CqasPaciente { get; set; }
    }
}
