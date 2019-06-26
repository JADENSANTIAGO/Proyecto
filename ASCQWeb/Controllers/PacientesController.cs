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
    public class PacientesController : Controller
    {
        private readonly CMContext _context;
        public PacientesController(CMContext context)
        {
            _context = context;
        }

        //metodo para eliminar datos ligados
        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
            {
                mensaje = "";
            }
            ViewData["Error"] = mensaje;
        }
        public void Combox()
        {
            ViewData["TipoSangrePaciente"] = new SelectList(_context.CqasTipoSangre.ToList(), "Codigo", "Descripcion");
            ViewData["EtniaPaciente"] = new SelectList(_context.CqasEtnia.ToList(), "Codigo", "Descripcion");
            ViewData["SexoPaciente"] = new SelectList(_context.CqasSexo.ToList(), "Codigo", "Descripcion");

        }
        public IActionResult Index(string mensaje)
        {
            try
            {
                InicializarMensaje(mensaje);
                List<AscqViewModelPaciente> datospaciente = new List<AscqViewModelPaciente>();

                var persona = _context.CqasPersona.ToList();
                foreach (var item in persona)
                {
                    var paciente = _context.CqasPaciente.Where(y => y.CodigoPersona == item.Codigo).FirstOrDefault();
                    if (paciente != null)
                    {
                        var medico1 = _context.CqasPersona.Where(y => y.Codigo == item.Codigo).Select(x => new AscqViewModelPaciente
                        {
                            Codigo = paciente.Codigo,
                            CodigoPersona = x.Codigo,
                            NombrePaciente = string.Format("{0} {1}", x.Nombre, x.Apellido)
                        }).FirstOrDefault();
                        datospaciente.Add(medico1);
                    }

                }
                return View(datospaciente);
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
                var medicoDelete = _context.CqasPaciente.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (medicoDelete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasPaciente.Remove(medicoDelete);
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
                List<AscqViewModelPaciente> datosmedico = new List<AscqViewModelPaciente>();
                var persona = _context.CqasPersona.ToList();
                foreach (var item in persona)
                {
                    var medico = _context.CqasPaciente.Where(y => y.CodigoPersona == item.Codigo).FirstOrDefault();
                    if (medico == null)
                    {
                        var medico1 = _context.CqasPersona.Where(y => y.Codigo == item.Codigo).Select(x => new AscqViewModelPaciente
                        {

                            CodigoPersona = x.Codigo,
                            NombrePaciente = string.Format("{0} {1}", x.Nombre, x.Apellido)
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
                return BadRequest();
            }
        }
        public async Task<IActionResult> AsignarPaciente(string id)
        {
            try
            {
                CqasPaciente nuevopaciente = new CqasPaciente();
                nuevopaciente.CodigoPersona = Convert.ToInt32(id);
                nuevopaciente.Estado = 1;
                _context.CqasPaciente.Add(nuevopaciente);
                await _context.SaveChangesAsync();

                // usuario 
                CqasUsuario cqasUsuario = new CqasUsuario();
                CqasPersona cqasPersona = new CqasPersona();
                cqasPersona = _context.CqasPersona.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();

                cqasUsuario.CodigoPersona = nuevopaciente.CodigoPersona;
                cqasUsuario.Usuario = string.Format("P{0}",cqasPersona.Cedula);
                cqasUsuario.Contrasena = cqasPersona.Cedula;
                cqasUsuario.CodigoPerfil = 2;
                _context.CqasUsuario.Add(cqasUsuario);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("AsignarMedico"); ;
            }


        }
        public IActionResult FichaPaciente(string id)
        {
            try
            {
                CqasPaciente cqasPaciente = new CqasPaciente();
                cqasPaciente = _context.CqasPaciente.Where(x => x.Codigo == Convert.ToUInt32(id)).FirstOrDefault();
                AscqViewModelPaciente paciente = new AscqViewModelPaciente();
                paciente = _context.CqasPaciente.Where(y => y.Estado == 1 && y.CodigoPersona == cqasPaciente.CodigoPersona).Select(x => new AscqViewModelPaciente
                {
                    Codigo = x.Codigo,
                    Estado = x.Estado,
                    CodigoPersona = x.CodigoPersona,
                    CedulaPaciente = x.CodigoPersonaNavigation.Cedula,
                    NombrePaciente = x.CodigoPersonaNavigation.Nombre,
                    ApellidoPaciente = x.CodigoPersonaNavigation.Apellido,
                    FechaNacimientoPaciente = Convert.ToDateTime(x.CodigoPersonaNavigation.FechaNacimiento),
                    GeneroPaciente = x.CodigoPersonaNavigation.CodigoGeneroNavigation.Descripcion,
                    DirecionPaciente = x.CodigoPersonaNavigation.Direccion,
                    EstadoCivilPaciente = x.CodigoPersonaNavigation.CodigoEstadoCivilNavigation.Descripcion,
                    NacionalidadPaciente = x.CodigoPersonaNavigation.Nacionalidad,
                    TipoSangrePaciente = Convert.ToInt32(x.CodigoTipoSangre),
                    EtniaPaciente = Convert.ToInt32(x.CodigoEtnia),
                    SexoPaciente = Convert.ToInt32(x.CodigoSexo),
                    LugarNacimiento = x.LugarNacimiento,
                    LugarResidencia = x.LugarRecidencia,                    
                    Edad = CalcularEdad(Convert.ToDateTime(x.CodigoPersonaNavigation.FechaNacimiento), DateTime.Now)
                }).FirstOrDefault();
                Combox();
                return View(paciente);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarFichaPaciente(AscqViewModelPaciente ascqViewModelPaciente)
        {
            try
            {
                CqasPaciente paciente = new CqasPaciente();
                paciente.Codigo = ascqViewModelPaciente.Codigo;
                paciente.Estado = ascqViewModelPaciente.Estado;
                paciente.CodigoPersona = ascqViewModelPaciente.CodigoPersona;
                paciente.CodigoTipoSangre = ascqViewModelPaciente.TipoSangrePaciente;
                paciente.CodigoEtnia = ascqViewModelPaciente.EtniaPaciente;
                paciente.CodigoSexo = ascqViewModelPaciente.SexoPaciente;
                paciente.LugarNacimiento = ascqViewModelPaciente.LugarNacimiento;
                paciente.LugarRecidencia = ascqViewModelPaciente.LugarResidencia;
                _context.CqasPaciente.Update(paciente);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public int CalcularEdad(DateTime Naciento, DateTime Ahora)
        {
            int age = Ahora.Year - Naciento.Year;
            if (Ahora.Month < Naciento.Month || (Ahora.Month == Naciento.Month && Ahora.Day < Naciento.Day))
                age--;
            return age;
        }

    }
}