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
    public class EspecialidadesController : Controller
    {
        private readonly CMContext _context;
        public EspecialidadesController(CMContext context)
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
            var lista = new List<CqasEspecialidad>();
            try
            {
                lista = _context.CqasEspecialidad.ToList();
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
        public async Task<IActionResult> Create(CqasEspecialidad ascqespecialidad)
        {
            try
            {
                if (!Existe(ascqespecialidad))
                {
                    ascqespecialidad.Estado = 1;
                    _context.CqasEspecialidad.Add(ascqespecialidad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqespecialidad);
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
                        var consultorio = _context.CqasEspecialidad.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
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
        public async Task<IActionResult> Edit(string id, CqasEspecialidad ascqespecialidad)
        {
            try
            {
                if (!Existe(ascqespecialidad))
                {
                    ascqespecialidad.Estado = 1;
                    _context.CqasEspecialidad.Update(ascqespecialidad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqespecialidad);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var consultorioDelete = _context.CqasEspecialidad.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (consultorioDelete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasEspecialidad.Remove(consultorioDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasEspecialidad ascqespecialidad)
        {
            bool respuesta = false;
            CqasEspecialidad ascqespecialidad1 = _context.CqasEspecialidad.Where(x => x.Descripcion == ascqespecialidad.Descripcion).FirstOrDefault();
            if (ascqespecialidad1 != null)
                respuesta = true;
            return respuesta;
        }



    }
}