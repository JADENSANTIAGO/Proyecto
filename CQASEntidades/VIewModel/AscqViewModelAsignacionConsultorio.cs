using System;
using System.Collections.Generic;

namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelAsignacionConsultorio
    {
        public int Codigo { get; set; }
        public int CodigoPaciente { get; set; }
        public int CodigoMedico { get; set; }
        public int CodigoConsultorio { get; set; }
        public string Descripcion { get; set; }

        public AscqViewModelMedico CodigoMedicoNavigation { get; set; }
        public AscqViewModelPaciente CodigoPacienteNavigation { get; set; }
    }
}
