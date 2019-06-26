using System;
using System.Collections.Generic;

namespace CQASEntidades.Negocio
{
    public partial class CqasPersona
    {
        public CqasPersona()
        {
            CqasMedico = new HashSet<CqasMedico>();
            CqasPaciente = new HashSet<CqasPaciente>();
            CqasUsuario = new HashSet<CqasUsuario>();
        }

        public int Codigo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int CodigoGenero { get; set; }
        public string Direccion { get; set; }
        public int CodigoEstadoCivil { get; set; }
        public string Nacionalidad { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }

        public CqasEstadoCivil CodigoEstadoCivilNavigation { get; set; }
        public CqasGenero CodigoGeneroNavigation { get; set; }
        public ICollection<CqasMedico> CqasMedico { get; set; }
        public ICollection<CqasPaciente> CqasPaciente { get; set; }
        public ICollection<CqasUsuario> CqasUsuario { get; set; }
    }
}
