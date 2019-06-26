using System;
using System.Collections.Generic;

namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelGenero
    {
        public AscqViewModelGenero()
        {
            Ascqpersona = new HashSet<AscqViewModelPersona>();
        }

        public int Codigo { get; set; }
        public string  Descripcion { get; set; }

        public ICollection<AscqViewModelPersona> Ascqpersona { get; set; }
    }
}
