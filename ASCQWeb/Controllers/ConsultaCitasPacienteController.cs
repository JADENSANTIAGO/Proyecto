using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CQASWeb.Controllers
{
    public class ConsultaCitasPacienteController : Controller
    {
        private readonly CMContext _context;
        public ConsultaCitasPacienteController(CMContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            AscqViewModelUsuario usuariologer = new AscqViewModelUsuario();
            var session = HttpContext.Session.GetString("DatosUsuario");
            if (session != null)
            {
                usuariologer = JsonConvert.DeserializeObject<AscqViewModelUsuario>(session);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            CqasPaciente datospersona = new CqasPaciente();
            List<AscqViewModelCitasPaciente> listacitas = new List<AscqViewModelCitasPaciente>();
            try
            {
                if (usuariologer != null)
                {
                    datospersona = _context.CqasPaciente.Where(s => s.CodigoPersona == usuariologer.CodigoPersona).FirstOrDefault();

                    if (datospersona != null)
                    {
                        listacitas = _context.CqasCita.Where(x => x.CodigoPaciente == datospersona.Codigo && x.Estado == "A" && x.Fecha >= DateTime.Now).
                                        Select(x => new AscqViewModelCitasPaciente
                                        {
                                            Codigo = x.Codigo,
                                            Fecha = x.Fecha.ToString("dd/MM/yyyy"),
                                            Hora = x.HoraIncio,
                                            NombreConsultorio = x.CodigoConsultorioHorarioNavigation.CodigoConsultorioAsignadoNavigation.CodigoConsultorioNavigation.Descripcion,
                                            NombreEspecialidad = x.CodigoConsultorioHorarioNavigation.CodigoConsultorioAsignadoNavigation.CodigoMedicoEspecialidadNavigation.CodigoEspecialidadNavigation.Descripcion,
                                            NombreMedico = string.Format("{0} {1}", x.CodigoConsultorioHorarioNavigation.CodigoConsultorioAsignadoNavigation.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoConsultorioHorarioNavigation.CodigoConsultorioAsignadoNavigation.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido)
                                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return View(listacitas);
        }

        public async Task<IActionResult> CancelarCita(string id)
        {
            try
            {
                CqasCita cita = new CqasCita();
                cita = _context.CqasCita.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (cita != null)
                {
                    cita.Estado = "C";
                    _context.CqasCita.Update(cita);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

            }
            return RedirectToAction("Index");
        }
    }
}