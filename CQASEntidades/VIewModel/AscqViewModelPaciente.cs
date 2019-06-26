using System;
using System.Collections.Generic;

namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelPaciente
    {
        

        public int Codigo { get; set; }
        public int CodigoPersona { get; set; }
        public string CedulaPaciente { get; set; }
        public string NombrePaciente { get; set; }
        public string ApellidoPaciente { get; set; }
        public DateTime FechaNacimientoPaciente { get; set; }
        public string GeneroPaciente { get; set; }
        public string DirecionPaciente { get; set; }
        public string EstadoCivilPaciente { get; set; }
        public string NacionalidadPaciente { get; set; }
        public int Edad { get; set; }
        public int TipoSangrePaciente { get; set; }
        public int EtniaPaciente { get; set; }
        public int SexoPaciente { get; set; }
        public int Estado { get; set; }
        public string LugarNacimiento { get; set; }
        public string LugarResidencia { get; set; }

    }
}
