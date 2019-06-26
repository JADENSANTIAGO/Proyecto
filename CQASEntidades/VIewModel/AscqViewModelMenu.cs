using CQASEntidades.Negocio;
using System;
using System.Collections.Generic;

namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelMenu
    {

        public int Codigo { get; set; }
        public int CodigoMenu { get; set; }
        public string Cabezera { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public int Estado { get; set; }
        public List<CqasMenu> ListaMenuPerfil { get; set; }
    }
}
