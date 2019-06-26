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
    public class UsuariosController : Controller
    {
        private readonly CMContext _context;
        public UsuariosController(CMContext context)
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
        public IActionResult Index()
        {

            var lista = new List<CqasUsuario>();
            try
            {
                lista = _context.CqasUsuario.ToList();
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CqasUsuario cqasUsuario)
        {
            try
            {
                _context.CqasUsuario.Add(cqasUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(cqasUsuario);
            }

        }
        public async Task<IActionResult> ResetearClave(string id)
        {
            try
            {
                var Delete = _context.CqasUsuario.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                Delete.Contrasena = "citasmedicas";
                _context.CqasUsuario.Update(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public IActionResult CambiarContrasena()
        {
            try
            {
                InicializarMensaje("");
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

                var Delete = _context.CqasUsuario.Where(x => x.Codigo == Convert.ToInt32(usuariologer.Codigo)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                return View(Delete);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(CqasUsuario cqasUsuario)
        {
            try
            {
                _context.CqasUsuario.Update(cqasUsuario);
                await _context.SaveChangesAsync();
                InicializarMensaje("Contraseña Actualizada");
                return View(cqasUsuario);

            }
            catch (Exception)
            {
                InicializarMensaje("No se pudo actualizar la contraseña");
                return View(cqasUsuario);
            }
        }


    }
}