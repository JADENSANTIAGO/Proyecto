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
    public class ConsultaCitasMedicoController : Controller
    {
        private readonly CMContext _context;
        public ConsultaCitasMedicoController(CMContext context)
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

            CqasMedico datospersona = new CqasMedico();
            AscqViewModelMedico ascqViewModelMedico = new AscqViewModelMedico();
            List<AscqViewModelCitasPaciente> listacitas = new List<AscqViewModelCitasPaciente>();
            try
            {
                ascqViewModelMedico.Fecha = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                if (usuariologer != null)
                {
                    datospersona = _context.CqasMedico.Where(s => s.CodigoPersona == usuariologer.CodigoPersona).FirstOrDefault();

                    if (datospersona != null)
                    {
                        listacitas = _context.CqasCita
                                        .Select(x => new AscqViewModelCitasPaciente
                                        {
                                            Codigo = x.Codigo,
                                            Fecha = x.Fecha.ToString("dd/MM/yyyy"),
                                            Hora = x.HoraIncio,
                                            NombrePersona = string.Format("{0} {1}", x.CodigoPacienteNavigation.CodigoPersonaNavigation.Nombre, x.CodigoPacienteNavigation.CodigoPersonaNavigation.Apellido),
                                            CodigoMedico = x.CodigoConsultorioHorarioNavigation.CodigoConsultorioAsignadoNavigation.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.Codigo

                                        }).Where(s => s.CodigoMedico == datospersona.Codigo && s.Fecha == ascqViewModelMedico.Fecha.ToString()).ToList();
                        if (listacitas.Count > 0)
                        {
                            ascqViewModelMedico.ListaPaciente = new List<AscqViewModelCitasPaciente>();
                            ascqViewModelMedico.ListaPaciente.AddRange(listacitas);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return View(ascqViewModelMedico);
            }
            return View(ascqViewModelMedico);

        }
        [HttpPost]
        public IActionResult Index(AscqViewModelMedico ModelMedico)
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

            CqasMedico datospersona = new CqasMedico();
            AscqViewModelMedico ascqViewModelMedico = new AscqViewModelMedico();
            List<AscqViewModelCitasPaciente> listacitas = new List<AscqViewModelCitasPaciente>();
            try
            {
                ascqViewModelMedico.Fecha = ModelMedico.Fecha;
                if (usuariologer != null)
                {
                    datospersona = _context.CqasMedico.Where(s => s.CodigoPersona == usuariologer.CodigoPersona).FirstOrDefault();

                    if (datospersona != null)
                    {
                        listacitas = _context.CqasCita
                                        .Select(x => new AscqViewModelCitasPaciente
                                        {
                                            Codigo = x.Codigo,
                                            Fecha = x.Fecha.ToString("dd/MM/yyyy"),
                                            Hora = x.HoraIncio,
                                            NombrePersona = string.Format("{0} {1}", x.CodigoPacienteNavigation.CodigoPersonaNavigation.Nombre, x.CodigoPacienteNavigation.CodigoPersonaNavigation.Apellido),
                                            CodigoMedico = x.CodigoConsultorioHorarioNavigation.CodigoConsultorioAsignadoNavigation.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.Codigo

                                        }).Where(s => s.CodigoMedico == datospersona.Codigo && s.Fecha == ascqViewModelMedico.Fecha.ToString("dd/MM/yyyy")).ToList();
                        if (listacitas.Count > 0)
                        {
                            ascqViewModelMedico.ListaPaciente = new List<AscqViewModelCitasPaciente>();
                            ascqViewModelMedico.ListaPaciente.AddRange(listacitas);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return View(ascqViewModelMedico);
            }
            return View(ascqViewModelMedico);

        }
        public IActionResult Diagnostico(string id)
        {
            CqasHistoriaClinica clinica = new CqasHistoriaClinica();
            CqasHistoriaClinica clinicadatos = _context.CqasHistoriaClinica.Where(x => x.CodigoCita == Convert.ToInt32(id)).FirstOrDefault();
            if (clinicadatos == null)
            {
                clinica.CodigoCita = Convert.ToInt32(id);
            }
            else
            {
                clinica = clinicadatos;
            }

            return View(clinica);

        }
        [HttpPost]
        public async Task<IActionResult> Diagnostico(CqasHistoriaClinica cqasHistoriaClinica)
        {
            try
            {
                CqasHistoriaClinica clinica = new CqasHistoriaClinica();
                clinica = _context.CqasHistoriaClinica.Where(x => x.CodigoCita == Convert.ToInt32(cqasHistoriaClinica.CodigoCita)).FirstOrDefault();
                if (clinica != null)
                {
                    clinica.Descripcion = cqasHistoriaClinica.Descripcion;
                    clinica.Reseta = cqasHistoriaClinica.Reseta;
                    _context.CqasHistoriaClinica.Update(clinica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    _context.CqasHistoriaClinica.Add(cqasHistoriaClinica);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception )
            {
                return RedirectToAction("Index");
            }
            
        }

    }
}