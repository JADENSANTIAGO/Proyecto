using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using Microsoft.AspNetCore.Mvc;

namespace CQASWeb.Controllers
{
    public class SexosController : Controller
    {
        private readonly CMContext _context;
        public SexosController(CMContext context)
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
            var lista = new List<CqasSexo>();
            try
            {
                lista = _context.CqasSexo.ToList();
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
        public async Task<IActionResult> Create(CqasSexo cqasSexo)
        {
            try
            {
                if (!Existe(cqasSexo))
                {
                    cqasSexo.Estado = 1;
                    _context.CqasSexo.Add(cqasSexo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(cqasSexo);
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
                        var sexo = _context.CqasSexo.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        return View(sexo);

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
        public async Task<IActionResult> Edit(string id, CqasSexo cqasSexo)
        {
            try
            {
                if (!Existe(cqasSexo))
                {
                    cqasSexo.Estado = 1;
                    _context.CqasSexo.Update(cqasSexo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(cqasSexo);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var Delete = _context.CqasSexo.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (Delete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasSexo.Remove(Delete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasSexo cqasSexo)
        {
            bool respuesta = false;
            CqasSexo cqasSexo1 = _context.CqasSexo.Where(x => x.Descripcion == cqasSexo.Descripcion).FirstOrDefault();
            if (cqasSexo1 != null)
                respuesta = true;
            return respuesta;
        }

    }
}