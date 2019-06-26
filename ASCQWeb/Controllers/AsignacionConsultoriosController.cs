using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.VIewModel;
using CQASEntidades.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class AsignacionConsultoriosController : Controller
    {
        private readonly CMContext _context;
        public AsignacionConsultoriosController(CMContext context)
        {
            _context = context;
        }
        public void Combox()
        {
            ViewData["Consultorio"] = new SelectList(_context.CqasConsultorio.ToList(), "Codigo", "Descripcion");

        }
        public IActionResult Index()
        {
            List<AscqViewModelMedico> datosmedico = new List<AscqViewModelMedico>();
            try
            {
                //var Especialidad = _context.CqasAsignacionConsultorio.Select(x => new AscqViewModelMedico
                //{
                //    Codigo = x.Codigo,
                //    CodigoMedico = x.CodigoMedicoEspecialidadNavigation.CodigoMedico,
                //    NombreMedico = string.Format("{0} {1}", x.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido),
                //    NombreEspecialidad = x.CodigoMedicoEspecialidadNavigation.CodigoEspecialidadNavigation.Descripcion
                //}).ToList();

                var Especialidad = _context.CqasMedicoEspecialidad.Select(x => new AscqViewModelMedico
                {
                    Codigo = x.Codigo,
                    CodigoMedico = x.CodigoMedico,
                    NombreMedico = string.Format("{0} {1}", x.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido),
                    NombreEspecialidad = x.CodigoEspecialidadNavigation.Descripcion
                }).ToList();
                datosmedico.AddRange(Especialidad);
                return View(datosmedico);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var medicoDelete = _context.CqasAsignacionConsultorio.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (medicoDelete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasAsignacionConsultorio.Remove(medicoDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public IActionResult Consultorios(string id)
        {
            try
            {
                AscqViewModelMedico Especialidad = new AscqViewModelMedico();
                Combox();
                Especialidad = _context.CqasMedicoEspecialidad.Where(s => s.Codigo == Convert.ToInt32(id))
                    .Select(x => new AscqViewModelMedico
                {
                    Codigo = x.Codigo,
                    CodigoMedico = x.CodigoMedico,
                    NombreMedico = string.Format("{0} {1}", x.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido),
                    NombreEspecialidad = x.CodigoEspecialidadNavigation.Descripcion
                }).FirstOrDefault();                                
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
                if (!ExisteAsignacionConsultorio(ascqViewModelMedico))
                {
                    CqasAsignacionConsultorio nuevomedicoespecialidad = new CqasAsignacionConsultorio();
                    nuevomedicoespecialidad.CodigoMedicoEspecialidad = ascqViewModelMedico.Codigo;
                    nuevomedicoespecialidad.CodigoConsultorio = ascqViewModelMedico.CodigoConsultorio;
                    nuevomedicoespecialidad.Estado = "A";
                    _context.CqasAsignacionConsultorio.Add(nuevomedicoespecialidad);
                    await _context.SaveChangesAsync();
                }
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
        public async Task<IActionResult> EliminarHorario(string id)
        {
            try
            {
                var consultorioDelete = _context.CqasConsultorioHorario.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (consultorioDelete == null)
                {
                    //return RedirectToAction("Especialidades", new { id = ascqViewModelMedico.CodigoMedico });
                }
                _context.CqasConsultorioHorario.Remove(consultorioDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Horario", new { id = consultorioDelete.CodigoConsultorioAsignado });


            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        
        public IActionResult Horario(string id)
        {
            AscqViewModelConsultorioHorario ConsultorioHorario = new AscqViewModelConsultorioHorario();
            try
            {
                ConsultorioHorario.CodigoConsultorioAsignado = Convert.ToInt32(id);
                ConsultorioHorario.ListaConsultorioHorario = _context.CqasConsultorioHorario.Where(x => x.CodigoConsultorioAsignado == Convert.ToInt32(id)).ToList();
                return View(ConsultorioHorario);


            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Horario(AscqViewModelConsultorioHorario ascqViewModelConsultorioHorario)
        {
            CqasConsultorioHorario ConsultorioHorario = new CqasConsultorioHorario();
            try
            {
                if (!ExisteConsultorioHorario(ascqViewModelConsultorioHorario))
                {
                    ConsultorioHorario.CodigoConsultorioAsignado = ascqViewModelConsultorioHorario.CodigoConsultorioAsignado;
                    ConsultorioHorario.HoraInicio = ascqViewModelConsultorioHorario.HoraInicio;
                    ConsultorioHorario.HoraFin = ascqViewModelConsultorioHorario.HoraFin;
                    ConsultorioHorario.Dia = ascqViewModelConsultorioHorario.Dia;
                    ConsultorioHorario.Estado = 1;
                    _context.CqasConsultorioHorario.Add(ConsultorioHorario);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Horario", new { id = ascqViewModelConsultorioHorario.CodigoConsultorioAsignado });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Horario", new { id = ascqViewModelConsultorioHorario.CodigoConsultorioAsignado });
            }
        }
        public bool ExisteAsignacionConsultorio(AscqViewModelMedico ascqViewModelMedico)
        {
            bool respuesta = false;
            if (_context.CqasAsignacionConsultorio.Any(x => x.CodigoConsultorio == ascqViewModelMedico.CodigoConsultorio))
            {
                respuesta = true;
            }
            return respuesta;
        }
        public bool ExisteConsultorioHorario(AscqViewModelConsultorioHorario ascqViewModelConsultorioHorario)
        {
            bool respuesta = false;
            if (_context.CqasConsultorioHorario.Any(x => x.Dia == ascqViewModelConsultorioHorario.Dia && x.CodigoConsultorioAsignado == ascqViewModelConsultorioHorario.CodigoConsultorioAsignado))
            {
                respuesta = true;
            }
            return respuesta;
        }

    }
}