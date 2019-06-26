using CQASEntidades.Negocio;
using System;
using System.Collections.Generic;

namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelUsuario
    {
        public int Codigo { get; set; }
        public int CodigoPerfil { get; set; }
        public int CodigoPersona { get; set; }
        public string NombreUsuario { get; set; }
        public string Perfil { get; set; }
        public List<CqasMenu> ListaMenu { get; set; }
        public List<CqasMenu> ListaMenuMedico { get; set; }
        public List<CqasMenu> ListaMenuPaciente { get; set; }
        public List<CqasMenu> ListaMenuReportes { get; set; }
        public List<CqasMenu> ListaMenuSeguridad { get; set; }
    }
}
