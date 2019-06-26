using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace CQASWeb.Controllers
{
    public class PerfilesController : Controller
    {
        private readonly CMContext _context;
        public PerfilesController(CMContext context)
        {
            _context = context;
        }
        // metodo para eliminar data ligada
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CqasPerfil cqasPerfil)
        {
            try
            {
                cqasPerfil.Estado = 1;
                _context.CqasPerfil.Add(cqasPerfil);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(cqasPerfil);
            }

        }
        public IActionResult Edit(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        var perfil = _context.CqasPerfil.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        return View(perfil);

                    }
                    catch (Exception ex)
                    {
                        return BadRequest();
                    }

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CqasPerfil cqasPerfil)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        _context.CqasPerfil.Update(cqasPerfil);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {

                        return BadRequest();

                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return View(cqasPerfil);
        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasPerfil.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasPerfil.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }


    }
}