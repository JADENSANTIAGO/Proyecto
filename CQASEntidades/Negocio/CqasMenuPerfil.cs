using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasMenuPerfil
    {
        public int Codigo { get; set; }
        public int CodigoPerfil { get; set; }
        public int CodigoMenu { get; set; }

        public CqasMenu CodigoMenuNavigation { get; set; }
        public CqasPerfil CodigoPerfilNavigation { get; set; }
    }
}
