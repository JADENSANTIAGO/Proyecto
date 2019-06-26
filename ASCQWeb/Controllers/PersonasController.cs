using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.Util;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class PersonasController : Controller
    {
        private readonly CMContext _context;
        public PersonasController(CMContext context)
        {
            _context = context;
        }

        //metodo para validar eliminacion de campos anclados
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
            ViewData["Genero"] = new SelectList(_context.CqasGenero.ToList(), "Codigo", "Descripcion");
            ViewData["EstadoCivil"] = new SelectList(_context.CqasEstadoCivil.ToList(), "Codigo", "Descripcion");
        }
        public IActionResult Index(string mensaje)
        {
            InicializarMensaje(mensaje);
            var lista = new List<CqasPersona>();
            try
            {
                lista = _context.CqasPersona.ToList();
                return View(lista);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public IActionResult Create(string mensaje)
        {
            InicializarMensaje(mensaje);
            Combox();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CqasPersona ascqpersona)
        {
            Response response = new Response();
            try
            {
                if(ascqpersona.FechaNacimiento > DateTime.Now)
                {
                    Combox();
                    InicializarMensaje("Fecha de nacimiento incorrecta");
                    return View(ascqpersona);
                }

                    
                if (!ExistePersona(ascqpersona)) {
                    _context.CqasPersona.Add(ascqpersona);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                Combox();
                InicializarMensaje("Numero de cedula registrada");
                return View(ascqpersona);
            }
            catch (Exception ex)
            {
                Combox();
                return View(ascqpersona);
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
                        var consultorio = _context.CqasPersona.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                        Combox();
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
        public async Task<IActionResult> Edit(string id, CqasPersona ascqpersona)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    try
                    {
                        _context.CqasPersona.Update(ascqpersona);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");

                    }
                    catch (Exception ex)
                    {
                        Combox();

                    }
                }

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
            return View(ascqpersona);
        }
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var consultorioDelete = _context.CqasPersona.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
                if (consultorioDelete == null)
                {
                    return RedirectToAction("Index");
                }
                _context.CqasPersona.Remove(consultorioDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { mensaje = "No se puede eliminar el registro" });
            }
        }

        public bool ExistePersona(CqasPersona cqasPersona)
        {
            bool respuesta = false;
            if (_context.CqasPersona.Any(x => x.Cedula == cqasPersona.Cedula))
            {
                respuesta = true;
            }
            return respuesta;
        }
    }
}