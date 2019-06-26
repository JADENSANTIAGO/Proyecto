using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace CQASWeb.Controllers
{
    public class MenusController : Controller
    {
        private readonly CMContext _context;
        public MenusController(CMContext context)
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
            var lista = new List<CqasMenu>();
            try
            {
                lista = _context.CqasMenu.ToList();
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
        public async Task<IActionResult> Create(CqasMenu  cqasMenu)
        {
            try
            {
                cqasMenu.Estado = 1;
                _context.CqasMenu.Add(cqasMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(cqasMenu);
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
                        var menu = _context.CqasMenu.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        return View(menu);

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
        public async Task<IActionResult> Edit(string id, CqasMenu  cqasMenu)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        _context.CqasMenu.Update(cqasMenu);
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
            return View(cqasMenu);
        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasMenu.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasMenu.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }


    }
}