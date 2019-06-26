using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasPaciente
    {
        public CqasPaciente()
        {
            CqasCita = new HashSet<CqasCita>();
        }
        public int Codigo { get; set; }
        public int CodigoPersona { get; set; }
        public int Estado { get; set; }
        public int? CodigoTipoSangre { get; set; }
        public int? CodigoEtnia { get; set; }
        public int? CodigoSexo { get; set; }
        public string LugarNacimiento { get; set; }
        public string LugarRecidencia { get; set; }

        public CqasEtnia CodigoEtniaNavigation { get; set; }
        public CqasPersona CodigoPersonaNavigation { get; set; }
        public CqasSexo CodigoSexoNavigation { get; set; }
        public CqasTipoSangre CodigoTipoSangreNavigation { get; set; }
        public ICollection<CqasCita> CqasCita { get; set; }
    }
}
