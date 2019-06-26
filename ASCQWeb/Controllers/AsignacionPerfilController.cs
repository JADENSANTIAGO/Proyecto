using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CQASWeb.Controllers
{
    public class AsignacionPerfilController : Controller
    {
        private readonly CMContext _context;
        public AsignacionPerfilController(CMContext context)
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
            ViewData["CodigoMenu"] = new SelectList(_context.CqasMenu.ToList(), "Codigo", "Descripcion");

        }
        public IActionResult Index()
        {
            var lista = new List<CqasPerfil>();
            try
            {
                lista = _context.CqasPerfil.ToList();
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public IActionResult Acceso(string id, string mensaje)
        {
            InicializarMensaje(mensaje);
            AscqViewModelMenu lista = new AscqViewModelMenu();
            Combox();
            lista.Codigo = Convert.ToInt32(id);
            try
            {
                lista.ListaMenuPerfil = _context.CqasMenuPerfil.Where(x => x.CodigoPerfil == Convert.ToInt32(id)).Select(x => new CqasMenu
                {
                    Codigo = x.Codigo,
                    Descripcion = x.CodigoMenuNavigation.Descripcion,
                    Url = x.CodigoMenuNavigation.Url
                }).ToList();
                return View(lista);
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
                var Delete = _context.CqasMenuPerfil.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasMenuPerfil.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        public async Task<IActionResult> Asignar(AscqViewModelMenu ascqViewModelMenu)
        {
            try
            {

                if (!ValidaMenu(ascqViewModelMenu))
                {
                    CqasMenuPerfil menuPerfil = new CqasMenuPerfil();
                    menuPerfil.CodigoPerfil = ascqViewModelMenu.Codigo;
                    menuPerfil.CodigoMenu = ascqViewModelMenu.CodigoMenu;
                    _context.CqasMenuPerfil.Add(menuPerfil);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Acceso", new { id = ascqViewModelMenu.Codigo,  mensaje = "Menú ya asignado" });

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        public bool ValidaMenu(AscqViewModelMenu ascqViewModelMenu)
        {
            bool respuesta = false;
            CqasMenuPerfil cqasMenuPerfil = _context.CqasMenuPerfil.Where(x => x.CodigoPerfil == ascqViewModelMenu.Codigo && x.CodigoMenu == ascqViewModelMenu.CodigoMenu).FirstOrDefault();
            if (cqasMenuPerfil != null)
                respuesta = true;
            return respuesta;
        }

    }
}