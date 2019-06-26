using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace CQASWeb.Controllers
{
    public class EtniasController : Controller
    {
        private readonly CMContext _context;
        public EtniasController(CMContext context)
        {
            _context = context;
        }
        //metodo para elimar registro ligados
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
            var lista = new List<CqasEtnia>();
            try
            {
                lista = _context.CqasEtnia.ToList();
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
        public async Task<IActionResult> Create(CqasEtnia cqasEtnia)
        {
            try
            {
                if (!Existe(cqasEtnia))
                {
                    cqasEtnia.Estado = 1;
                    _context.CqasEtnia.Add(cqasEtnia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(cqasEtnia);
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
                        var etnia = _context.CqasEtnia.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        return View(etnia);

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
        public async Task<IActionResult> Edit(string id, CqasEtnia cqasEtnia)
        {
            try
            {
                if (!Existe(cqasEtnia))
                {
                    cqasEtnia.Estado = 1;
                    _context.CqasEtnia.Update(cqasEtnia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(cqasEtnia);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasEtnia.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasEtnia.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasEtnia cqasEtnia)
        {
            bool respuesta = false;
            CqasEtnia cqasEtnia1 = _context.CqasEtnia.Where(x => x.Descripcion == cqasEtnia.Descripcion).FirstOrDefault();
            if (cqasEtnia1 != null)
                respuesta = true;
            return respuesta;
        }



    }
}