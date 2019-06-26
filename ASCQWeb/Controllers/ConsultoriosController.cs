using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.VIewModel;
using CQASEntidades.Util;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class ConsultoriosController : Controller
    {
        private readonly CMContext _context;
        public ConsultoriosController(CMContext context)
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
            List<CqasConsultorio> datosconsultorio = new List<CqasConsultorio>();           
            try
            {
                datosconsultorio = _context.CqasConsultorio.ToList();               
                return View(datosconsultorio);
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
        public async Task<IActionResult> Create(CqasConsultorio ascqconsultorio)
        {
            try
            {
                if (!Existe(ascqconsultorio))
                {

                    _context.CqasConsultorio.Add(ascqconsultorio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqconsultorio);
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
                        var consultorio = _context.CqasConsultorio.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
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
        public async Task<IActionResult> Edit(string id, CqasConsultorio ascqconsultorio)
        {
            try
            {
                if (!Existe(ascqconsultorio))
                {

                    _context.CqasConsultorio.Update(ascqconsultorio);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index", new { mensaje = "El Registro ya Existe" });
            }
            catch (Exception ex)
            {
                return View(ascqconsultorio);
            }

        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var consultorioDelete = _context.CqasConsultorio.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (consultorioDelete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasConsultorio.Remove(consultorioDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }
        public bool Existe(CqasConsultorio ascqconsultorio)
        {
            bool respuesta = false;
            CqasConsultorio ascqconsultorio1 = _context.CqasConsultorio.Where(x => x.Descripcion == ascqconsultorio.Descripcion).FirstOrDefault();
            if (ascqconsultorio1 != null)
                respuesta = true;
            return respuesta;
        }


    }
}