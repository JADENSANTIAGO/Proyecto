using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasHistoriaClinica
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int CodigoCita { get; set; }
        public string Reseta { get; set; }

        public CqasCita CodigoCitaNavigation { get; set; }
    }
}
