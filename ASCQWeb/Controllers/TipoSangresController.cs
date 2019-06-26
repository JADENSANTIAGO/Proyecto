using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.Util;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class TipoSangresController : Controller
    {
        private readonly CMContext _context;
        public TipoSangresController(CMContext context)
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
            var lista = new List<CqasTipoSangre>();
            try
            {
                lista = _context.CqasTipoSangre.ToList();
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
        public async Task<IActionResult> Create(CqasTipoSangre ascqtiposangre)
        {
            try
            {
                if (!Existe(ascqtiposangre))
                {
                  
                    _context.CqasTipoSangre.Add(ascqtiposangre);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqtiposangre);
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
                        var consultorio = _context.CqasTipoSangre.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
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
        public async Task<IActionResult> Edit(string id, CqasTipoSangre ascqtiposangre)
        {
            try
            {
                if (!Existe(ascqtiposangre))
                {
                   
                    _context.CqasTipoSangre.Update(ascqtiposangre);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqtiposangre);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasTipoSangre.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasTipoSangre.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasTipoSangre ascqtiposangre)
        {
            bool respuesta = false;
            CqasTipoSangre ascqtiposangre1 = _context.CqasTipoSangre.Where(x => x.Descripcion == ascqtiposangre.Descripcion).FirstOrDefault();
            if (ascqtiposangre1 != null)
                respuesta = true;
            return respuesta;
        }
    }
}