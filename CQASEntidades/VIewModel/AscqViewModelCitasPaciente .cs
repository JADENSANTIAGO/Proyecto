using CQASEntidades.Negocio;
using System;
using System.Collections.Generic;
namespace CQASEntidades.VIewModel
{
    public partial class AscqViewModelCitasPaciente
    {
        public int Codigo { get; set; }
        public int CodigoPersona { get; set; }
        public int Estado { get; set; }
        public string NombreMedico { get; set; }
        public string Fecha { get; set; }
        public string NombreEspecialidad { get; set; }
        public string NombrePersona { get; set; }
        public string Cedula { get; set; }        
        public string NombreConsultorio { get; set; }
        public int CodigoEspecialista { get; set; }
        public int CodigoMedico { get; set; }
        public int CodigoConsultorio { get; set; }
        public int CodigoConsultorioHorario { get; set; }
        public int CodigoMedicoEspecialidad { get; set; }
        public TimeSpan Hora { get; set; }
        public List<AscqViewModelEpecialidad> ListaEspecialidades { get; set; }
        public List<CqasConsultorio> ListaConsultorios { get; set; }
        public List<AscqViewModelMedicoEspecialidad> ListaMedicos { get; set; }
    }
}
