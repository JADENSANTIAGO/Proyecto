using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.Util;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class MedicosController : Controller
    {
        private readonly CMContext _context;
        public MedicosController(CMContext context)
        {
            _context = context;
        }
        public void Combox()
        {
            ViewData["Especialidad"] = new SelectList(_context.CqasEspecialidad.ToList(), "Codigo", "Descripcion");

        }
        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }
        public IActionResult Index(string mensaje)
        {
            InicializarMensaje(mensaje);
            List<AscqViewModelMedico> datosmedico = new List<AscqViewModelMedico>();
            try
            {
                var persona = _context.CqasPersona.ToList();
                foreach (var item in persona)
                {
                    var medico = _context.CqasMedico.Where(y => y.CodigoPersona == item.Codigo).FirstOrDefault();
                    if (medico != null)
                    {
                        var medico1 = _context.CqasPersona.Where(y => y.Codigo == item.Codigo).Select(x => new AscqViewModelMedico
                        {
                            Codigo = medico.Codigo,
                            CodigoPersona = x.Codigo,
                            NombreMedico = string.Format("{0} {1}", x.Nombre, x.Apellido)
                        }).FirstOrDefault();
                        datosmedico.Add(medico1);
                    }

                }
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
                var medicoDelete = _context.CqasMedico.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (medicoDelete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasMedico.Remove(medicoDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public IActionResult Asignar()
        {
            try
            {
                List<AscqViewModelMedico> datosmedico = new List<AscqViewModelMedico>();
                var persona = _context.CqasPersona.ToList();
                foreach (var item in persona)
                {
                    var medico = _context.CqasMedico.Where(y => y.CodigoPersona == item.Codigo).FirstOrDefault();
                    if (medico == null)
                    {
                        var medico1 = _context.CqasPersona.Where(y => y.Codigo == item.Codigo).Select(x => new AscqViewModelMedico
                        {
                            CodigoPersona = x.Codigo,
                            NombreMedico = string.Format("{0} {1}", x.Nombre, x.Apellido)
                        }).FirstOrDefault();
                        if (medico1 != null)
                        {
                            datosmedico.Add(medico1);
                        }

                    }

                }
                return View(datosmedico);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> AsignarMedico(string id)
        {
            try
            {
                CqasMedico nuevomedico = new CqasMedico();
                nuevomedico.CodigoPersona = Convert.ToInt32(id);
                nuevomedico.Estado = 1;
                _context.CqasMedico.Add(nuevomedico);

                // usuario 
                CqasUsuario cqasUsuario = new CqasUsuario();
                CqasPersona cqasPersona = new CqasPersona();
                cqasPersona = _context.CqasPersona.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();

                cqasUsuario.CodigoPersona = nuevomedico.CodigoPersona;
                cqasUsuario.Usuario = string.Format("M{0}", cqasPersona.Cedula);
                cqasUsuario.Contrasena = cqasPersona.Cedula;
                cqasUsuario.CodigoPerfil = 1;
                _context.CqasUsuario.Add(cqasUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public IActionResult Especialidades(string id)
        {
            Combox();
            AscqViewModelMedico datosmedico = new AscqViewModelMedico();
            try
            {
                var DatosMedico = _context.CqasMedico.Where(y => y.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (DatosMedico != null)
                {
                    var medico1 = _context.CqasPersona.Where(y => y.Codigo == DatosMedico.CodigoPersona).Select(x => new AscqViewModelMedico
                    {
                        Codigo = DatosMedico.Codigo,
                        CodigoPersona = x.Codigo,
                        NombreMedico = string.Format("{0} {1}", x.Nombre, x.Apellido)
                    }).FirstOrDefault();

                    var especialidades = _context.CqasMedicoEspecialidad.Where(y => y.CodigoMedico == DatosMedico.Codigo).Select(x => new AscqViewModelEpecialidad
                    {
                        Codigo = x.Codigo,
                        Descripcion = x.CodigoEspecialidadNavigation.Descripcion,
                        Estado = x.CodigoEspecialidadNavigation.Estado
                    }).ToList();
                    datosmedico.ListaEspecialidades = especialidades;
                    datosmedico.NombreMedico = medico1.NombreMedico;
                    datosmedico.CodigoMedico = medico1.Codigo;
                    datosmedico.CodigoPersona = medico1.CodigoPersona;
                }
                return View(datosmedico);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Especialidades(AscqViewModelMedico ascqViewModelMedico)
        {
            try
            {
                if (!ExisteEspecialidadesMedico(ascqViewModelMedico))
                {
                    CqasMedicoEspecialidad nuevomedicoespecialidad = new CqasMedicoEspecialidad();
                    nuevomedicoespecialidad.CodigoMedico = ascqViewModelMedico.CodigoMedico;
                    nuevomedicoespecialidad.CodigoEspecialidad = ascqViewModelMedico.CodigoEspecialista;
                    nuevomedicoespecialidad.Estado = 1;
                    _context.CqasMedicoEspecialidad.Add(nuevomedicoespecialidad);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Especialidades", new { id = ascqViewModelMedico.CodigoMedico });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> EliminarEspecialidad(string id)
        {
            try
            {
                var consultorioDelete = _context.CqasMedicoEspecialidad.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (consultorioDelete == null)
                {
                    //return RedirectToAction("Especialidades", new { id = ascqViewModelMedico.CodigoMedico });
                }
                _context.CqasMedicoEspecialidad.Remove(consultorioDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Especialidades", new { id = consultorioDelete.CodigoMedico });

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public bool ExisteEspecialidadesMedico(AscqViewModelMedico ascqViewModelMedico)
        {
            bool respuesta = false;
            if (_context.CqasMedicoEspecialidad.Any(x => x.CodigoEspecialidad == ascqViewModelMedico.CodigoEspecialista && x.CodigoMedico == ascqViewModelMedico.CodigoMedico))
            {
                respuesta = true;
            }
            return respuesta;
        }
    }
}