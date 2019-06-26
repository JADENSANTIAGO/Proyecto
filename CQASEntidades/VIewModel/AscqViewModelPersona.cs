using System;
using System.Collections.Generic;

namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelPersona
    {
        public AscqViewModelPersona()
        {
            Ascqmedico = new HashSet<AscqViewModelMedico>();
            Ascqpaciente = new HashSet<AscqViewModelPaciente>();
        }

        public int Codigo { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int CodigoGenero { get; set; }
        public string Direccion { get; set; }

        public AscqViewModelGenero CodigoGeneroNavigation { get; set; }
        public ICollection<AscqViewModelMedico> Ascqmedico { get; set; }
        public ICollection<AscqViewModelPaciente> Ascqpaciente { get; set; }
    }
}
