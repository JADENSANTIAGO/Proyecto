using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasEspecialidad
    {
        public CqasEspecialidad()
        {
            CqasMedicoEspecialidad = new HashSet<CqasMedicoEspecialidad>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }

        public ICollection<CqasMedicoEspecialidad> CqasMedicoEspecialidad { get; set; }
    }
}
