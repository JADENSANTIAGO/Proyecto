using System;
using System.Collections.Generic;
namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelConsultorio
    {
        public int Codigo { get; set; }
        public int CodigoPaciente { get; set; }
        public int CodigoMedico { get; set; }
        public string Descripcion { get; set; }

        public AscqViewModelMedico CodigoMedicoNavigation { get; set; }
        public AscqViewModelPaciente CodigoPacienteNavigation { get; set; }
    }
}
