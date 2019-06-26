using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.VIewModel;
using CQASEntidades.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class CitasAgendadasController : Controller
    {
        private readonly CMContext _context;
        public CitasAgendadasController(CMContext context)
        {
            _context = context;
        }
        public void Combox()
        {
            ViewData["Especialidad"] = new SelectList(_context.CqasAsignacionConsultorio.Select(x => new CqasEspecialidad
            {
                Codigo = x.Codigo,
                Descripcion = x.CodigoMedicoEspecialidadNavigation.CodigoEspecialidadNavigation.Descripcion
            }).ToList(), "Codigo", "Descripcion");

        }
        public IActionResult Index()
        {
            AscqViewModelUsuario usuariologer = new AscqViewModelUsuario();
            Combox();
            var session = HttpContext.Session.GetString("DatosUsuario");
            if (session != null)
            {
                usuariologer = JsonConvert.DeserializeObject<AscqViewModelUsuario>(session);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            AscqViewModelMedico datospersona = new AscqViewModelMedico();
            try
            {
                if (usuariologer != null)
                {
                    datospersona = _context.CqasPaciente.Select(x => new AscqViewModelMedico
                    {
                        Codigo = x.Codigo,
                        CodigoPersona = x.CodigoPersona,
                        Cedula = x.CodigoPersonaNavigation.Cedula,
                        NombrePersona = string.Format("{0} {1}", x.CodigoPersonaNavigation.Nombre, x.CodigoPersonaNavigation.Apellido)
                    }).Where(s => s.CodigoPersona == usuariologer.CodigoPersona).FirstOrDefault();
                }
                return View(datospersona);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Index(AscqViewModelMedico ascqViewModelMedico)
        {
            AscqViewModelMedico datospersona = new AscqViewModelMedico();
            try
            {
                Combox();

                if (!ExisteCitasAgendad(ascqViewModelMedico))
                {
                    Datos(ascqViewModelMedico);
                    var aw = _context.CqasAsignacionConsultorio.Where(s => s.Codigo == ascqViewModelMedico.CodigoEspecialista).FirstOrDefault();
                    if (aw != null)
                    {
                        var aww = _context.CqasMedicoEspecialidad.Where(s => s.Codigo == aw.CodigoMedicoEspecialidad).FirstOrDefault();
                        if (aww != null)
                        {
                            var aa = _context.CqasMedicoEspecialidad.Where(s => s.CodigoEspecialidad == aww.CodigoEspecialidad).Select(x => new AscqViewModelMedicoEspecialidad
                            {
                                NombreMedico = string.Format("{0} {1}", x.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido),
                                NombreEspecialidad = x.CodigoEspecialidadNavigation.Descripcion,
                            }).ToList();
                            // Ascqmedicocitas citasdisponibles = _context.Ascqmedicocitas.Where(s => s.CodigoAsignacionConsultorio == aw.Codigo).FirstOrDefault();

                            datospersona.Cedula = ascqViewModelMedico.Cedula;
                            datospersona.NombrePersona = ascqViewModelMedico.NombrePersona;
                            datospersona.CodigoEspecialista = ascqViewModelMedico.CodigoEspecialista;
                            datospersona.ListaMedicos = aa;
                        }
                    }
                }
                // variable de session datos paciente
                HttpContext.Session.SetString("DatosPaciente", JsonConvert.SerializeObject(datospersona));
            }
            catch (Exception ex)
            {

                throw;
            }
            return View(datospersona);
        }

        //public IActionResult Horarios()
        //{
        //    try
        //    {
        //        AscqViewModelMedico paciente = JsonConvert.DeserializeObject<AscqViewModelMedico>(HttpContext.Session.GetString("DatosPaciente"));
        //        var aaaa = _context.AscqHorario.ToList();

        //        return View(aaaa);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}
        public void Datos(AscqViewModelMedico ascqViewModelMedico) {
            var citasdisponibles = _context.CqasMedicoCitas.Where(s => s.CodigoAsignacionConsultorio == ascqViewModelMedico.CodigoEspecialista).FirstOrDefault();
            if (citasdisponibles != null)
            {
                var mesesActual = DateTime.Now.Day;
                var diaActual = DateTime.Now.Day;

                var mesesFinal = citasdisponibles.FechaFin.Month;
                var diaFinal = citasdisponibles.FechaFin.Day;
                if (mesesActual >= mesesFinal && diaActual <= diaFinal)
                {
                    var dias = diaFinal - diaActual;
                }



            }
        }
        //public IActionResult Agendar(string id)
        //{
        //    try
        //    {
        //        AscqViewModelMedico paciente = JsonConvert.DeserializeObject<AscqViewModelMedico>(HttpContext.Session.GetString("DatosPaciente"));
        //        var aaaa = _context.AscqHorario.ToList();

        //        return View(aaaa);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}
        public IActionResult Consultorios(string id)
        {
            try
            {
                AscqViewModelMedico Especialidad = new AscqViewModelMedico();
                Combox();
                Especialidad = _context.CqasMedicoEspecialidad.Select(x => new AscqViewModelMedico
                {
                    Codigo = x.Codigo,
                    CodigoMedico = x.CodigoMedico,
                    NombreMedico = string.Format("{0} {1}", x.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido),
                    NombreEspecialidad = x.CodigoEspecialidadNavigation.Descripcion
                    //NombreConsultorio = x.CodigoConsultorioNavigation.Descripcion
                }).Where(s => s.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Especialidad != null)
                {
                    var espe = _context.CqasAsignacionConsultorio.Where(x => x.CodigoMedicoEspecialidad == Especialidad.Codigo).FirstOrDefault();
                    if (espe != null)
                    {
                        Especialidad.ListaConsultorios = _context.CqasConsultorio.Where(x => x.Codigo == espe.CodigoConsultorio).ToList();
                        Especialidad.CodigoConsultorio = Especialidad.ListaConsultorios.FirstOrDefault().Codigo;
                    }
                }
                return View(Especialidad);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Consultorios(AscqViewModelMedico ascqViewModelMedico)
        {
            try
            {
                //if (!ExisteAsignacionConsultorio(ascqViewModelMedico))
                //{
                CqasAsignacionConsultorio nuevomedicoespecialidad = new CqasAsignacionConsultorio();
                nuevomedicoespecialidad.CodigoMedicoEspecialidad = ascqViewModelMedico.Codigo;
                nuevomedicoespecialidad.CodigoConsultorio = ascqViewModelMedico.CodigoConsultorio;
                nuevomedicoespecialidad.Estado = "A";
                _context.CqasAsignacionConsultorio.Add(nuevomedicoespecialidad);
                await _context.SaveChangesAsync();
                //}
                return RedirectToAction("Consultorios", new { id = ascqViewModelMedico.Codigo });
            }
            catch (Exception ex)
            {
                Combox();
                return BadRequest();
            }
        }
        public async Task<IActionResult> EliminarConsultorio(string id)
        {
            try
            {
                var consultorioDelete = _context.CqasAsignacionConsultorio.Where(x => x.CodigoMedicoEspecialidad == Convert.ToInt32(id)).FirstOrDefault();
                if (consultorioDelete == null)
                {
                    //return RedirectToAction("Especialidades", new { id = ascqViewModelMedico.CodigoMedico });
                }
                _context.CqasAsignacionConsultorio.Remove(consultorioDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Consultorios", new { id = consultorioDelete.CodigoMedicoEspecialidad });


            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public bool ExisteCitasAgendad(AscqViewModelMedico ascqViewModelMedico)
        {
            bool respuesta = false;
            //if (_context.AscqCita.Any(x => x.CodigoPaciente == ascqViewModelMedico.Codigo && x.CodigoAsignacionConsultorio == ascqViewModelMedico.CodigoEspecialista))
            //{
            //    respuesta = true;
            //}
            return respuesta;
        }

    }
}