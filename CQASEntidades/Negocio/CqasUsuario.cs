using System;
using System.Collections.Generic;
namespace CQASEntidades.Negocio
{
    public partial class CqasUsuario
    {
        public int Codigo { get; set; }
        public int CodigoPersona { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public int CodigoPerfil { get; set; }

        public CqasPerfil CodigoPerfilNavigation { get; set; }
        public CqasPersona CodigoPersonaNavigation { get; set; }
    }
}
