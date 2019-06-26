using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class GenerosController : Controller
    {
        private readonly CMContext _context;
        public GenerosController(CMContext context)
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
            var lista = new List<CqasGenero>();
            try
            {
                lista = _context.CqasGenero.ToList();
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
        public async Task<IActionResult> Create(CqasGenero ascqgenero)
        {
            try
            {
                if (!Existe(ascqgenero))
                {
                  
                    _context.CqasGenero.Add(ascqgenero);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqgenero);
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
                        var consultorio = _context.CqasGenero.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        return View(consultorio);

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
        public async Task<IActionResult> Edit(string id, CqasGenero ascqgenero)
        {
            try
            {
                if (!Existe(ascqgenero))
                {
                
                    _context.CqasGenero.Update(ascqgenero);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqgenero);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasGenero.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasGenero.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasGenero ascqgenero)
        {
            bool respuesta = false;
            CqasGenero ascqgenero1 = _context.CqasGenero.Where(x => x.Descripcion == ascqgenero.Descripcion).FirstOrDefault();
            if (ascqgenero1 != null)
                respuesta = true;
            return respuesta;
        }



    }
}