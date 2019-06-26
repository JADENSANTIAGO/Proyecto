using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class AgendamientoCitasController : Controller
    {
        private readonly CMContext _context;
        public AgendamientoCitasController(CMContext context)
        {
            _context = context;
        }
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
            ViewData["Especialidad"] = new SelectList(_context.CqasAsignacionConsultorio.Select(x => new CqasEspecialidad
            {
                Codigo = x.Codigo,
                Descripcion = x.CodigoMedicoEspecialidadNavigation.CodigoEspecialidadNavigation.Descripcion
            }).ToList(), "Codigo", "Descripcion");

        }
        public ActionResult ReporteCitas()
        {
            return View();
        }

        public IActionResult Index(string mensaje)
        {
            InicializarMensaje(mensaje);
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
                        Fecha = DateTime.Now,
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

                if (Convert.ToDateTime(ascqViewModelMedico.Fecha.ToShortDateString()) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                {
                    return RedirectToAction("Index", new { mensaje = "La fecha es incorecta" });
                }
                if (!ExisteCitasAgendad(ascqViewModelMedico))
                {
                    var Medicos = _context.CqasAsignacionConsultorio.Where(s => s.Codigo == ascqViewModelMedico.CodigoEspecialista).Select(x => new AscqViewModelMedicoEspecialidad
                    {
                        Codigo = x.Codigo,
                        NombreMedico = string.Format("{0} {1}", x.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.CodigoPersonaNavigation.Nombre, x.CodigoMedicoEspecialidadNavigation.CodigoMedicoNavigation.CodigoPersonaNavigation.Apellido),
                        NombreEspecialidad = x.CodigoMedicoEspecialidadNavigation.CodigoEspecialidadNavigation.Descripcion
                    }).ToList();

                    datospersona.Codigo = ascqViewModelMedico.Codigo;
                    datospersona.Cedula = ascqViewModelMedico.Cedula;
                    datospersona.NombrePersona = ascqViewModelMedico.NombrePersona;
                    datospersona.CodigoEspecialista = ascqViewModelMedico.CodigoEspecialista;
                    datospersona.CodigoConsultorio = ascqViewModelMedico.CodigoEspecialista;
                    datospersona.Fecha = ascqViewModelMedico.Fecha;
                    datospersona.ListaMedicos = Medicos;
                }
                // variable de session datos paciente
                HttpContext.Session.SetString("DatosPaciente", JsonConvert.SerializeObject(ascqViewModelMedico));
            }
            catch (Exception ex)
            {

                throw;
            }
            InicializarMensaje("");
            return View(datospersona);
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
        //public IActionResult Horarios()


        public IActionResult TurnosDisponibles(string id)
        {
            AscqViewModelMedico AscqViewModelMedico = new AscqViewModelMedico();
            var session = HttpContext.Session.GetString("DatosPaciente");
            if (session != null)
            {
                AscqViewModelMedico = JsonConvert.DeserializeObject<AscqViewModelMedico>(session);
            }
            AscqViewModelMedico = JsonConvert.DeserializeObject<AscqViewModelMedico>(session);
            List<CqasConsultorioHorario> LstTurnos = new List<CqasConsultorioHorario>();
            AscqViewModelMedico.CodigoMedico = Convert.ToInt32(id);
            // variable de session datos paciente
            HttpContext.Session.SetString("DatosPaciente", JsonConvert.SerializeObject(AscqViewModelMedico));

            var fecha = Convert.ToDateTime(AscqViewModelMedico.Fecha);
            var dia = fecha.ToString("dddd").ToUpper();
            var consul = _context.CqasConsultorioHorario.Where(x => x.CodigoConsultorioAsignado == Convert.ToInt32(id) && x.Dia == dia).ToList();
            foreach (var item in consul)
            {
                var Totalhoras = item.HoraFin - item.HoraInicio;
                var TotalMinutos = Totalhoras.TotalMinutes;
                var turnos = TotalMinutos / 20;
                var horasalir = item.HoraInicio;
                for (int i = 0; i < turnos; i++)
                {
                    CqasConsultorioHorario diass = new CqasConsultorioHorario();
                    diass.Codigo = item.Codigo;
                    diass.HoraInicio = horasalir;
                    horasalir = horasalir + TimeSpan.FromMinutes(20);
                    diass.HoraFin = horasalir;
                    LstTurnos.Add(diass);
                }
            }
            return View(CitasAgendadas(LstTurnos, fecha));
        }
        public async Task<IActionResult> AgendarCitas(string id)
        {
            try
            {
                var datos = id.Split(",");
                AscqViewModelMedico AscqViewModelMedico = new AscqViewModelMedico();
                var session = HttpContext.Session.GetString("DatosPaciente");
                if (session != null)
                {
                    AscqViewModelMedico = JsonConvert.DeserializeObject<AscqViewModelMedico>(session);
                }
                AscqViewModelMedico = JsonConvert.DeserializeObject<AscqViewModelMedico>(session);
                CqasCita citas = new CqasCita();
                citas.CodigoPaciente = AscqViewModelMedico.Codigo;
                citas.CodigoConsultorioHorario = Convert.ToInt32(datos[0]);
                citas.Fecha = AscqViewModelMedico.Fecha;
                citas.HoraIncio = TimeSpan.Parse(datos[1]);
                citas.HoraFin = TimeSpan.Parse(datos[2]);
                citas.Estado = "A";
                if (!CitasPaciente(citas))
                {
                    _context.CqasCita.Add(citas);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<CqasConsultorioHorario> CitasAgendadas(List<CqasConsultorioHorario> lista, DateTime fecha)
        {
            List<CqasConsultorioHorario> listaretorna = new List<CqasConsultorioHorario>();
            listaretorna.AddRange(lista);
            List<CqasCita> listacitas = _context.CqasCita.Where(x => x.Fecha == fecha && x.Estado == "A").ToList();
            foreach (var item in listacitas)
            {
                listaretorna.RemoveAll(x => x.HoraInicio == item.HoraIncio);
            }
            return listaretorna;
        }

        public bool CitasPaciente(CqasCita citas)
        {
            bool respuesta = false;
            List<CqasCita> listacitas = _context.CqasCita.Where(x => x.CodigoPaciente == citas.CodigoPaciente && x.Estado == "A" && x.Fecha == citas.Fecha && x.CodigoConsultorioHorario == citas.CodigoConsultorioHorario).ToList();
            if (listacitas.Count > 0)
                respuesta = true;
            return respuesta;
        }
        //Data para consultar los reportes y mostrar los graficos
        [HttpPost]
        public JsonResult reporteCita()
        {
            /**
             * select  per.* from CQAS_Cita c 
	         inner join CQAS_Paciente p on c.CodigoPaciente=p.Codigo
	         inner join CQAS_Persona per on p.CodigoPersona=per.Codigo
             **/
            var datospersona = from citas in _context.CqasCita
                               join paciente in _context.CqasPaciente on citas.CodigoPaciente equals paciente.Codigo
                               join persona in _context.CqasPersona on paciente.CodigoPersona equals persona.Codigo
                               join genero in _context.CqasGenero on persona.CodigoGenero equals genero.Codigo
                               join estado in _context.CqasEtnia on paciente.CodigoEtnia equals estado.Codigo
                               orderby persona.Codigo
                               select new { genero = genero.Descripcion, etnia = estado.Descripcion, persona.Codigo };
            return Json(datospersona);
        }
    }
}