using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace CQASWeb.Controllers
{
    public class EstadoCivilesController : Controller
    {
        private readonly CMContext _context;
        public EstadoCivilesController(CMContext context)
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
            var lista = new List<CqasEstadoCivil>();
            try
            {
                lista = _context.CqasEstadoCivil.ToList();
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
        public async Task<IActionResult> Create(CqasEstadoCivil cqasEstadoCivil)
        {
            try
            {
                if (!Existe(cqasEstadoCivil))
                {
                    _context.CqasEstadoCivil.Add(cqasEstadoCivil);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(cqasEstadoCivil);
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
                        var civil = _context.CqasEstadoCivil.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        return View(civil);

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
        public async Task<IActionResult> Edit(string id, CqasEstadoCivil cqasEstadoCivil)
        {
            try
            {
                if (!Existe(cqasEstadoCivil))
                {

                    _context.CqasEstadoCivil.Update(cqasEstadoCivil);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(cqasEstadoCivil);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasEstadoCivil.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasEstadoCivil.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasEstadoCivil cqasEstadoCivil)
        {
            bool respuesta = false;
            CqasEstadoCivil cqasEstadoCivil1 = _context.CqasEstadoCivil.Where(x => x.Descripcion == cqasEstadoCivil.Descripcion).FirstOrDefault();
            if (cqasEstadoCivil1 != null)
                respuesta = true;
            return respuesta;
        }


    }
}