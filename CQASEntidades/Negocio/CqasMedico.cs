using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasMedico
    {
        public CqasMedico()
        {
            CqasMedicoEspecialidad = new HashSet<CqasMedicoEspecialidad>();
        }

        public int Codigo { get; set; }
        public int CodigoPersona { get; set; }
        public int Estado { get; set; }

        public CqasPersona CodigoPersonaNavigation { get; set; }
        public ICollection<CqasMedicoEspecialidad> CqasMedicoEspecialidad { get; set; }
    }
}
