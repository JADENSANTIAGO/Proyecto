using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQASDatos;
using CQASEntidades.Negocio;
using CQASEntidades.Util;
using CQASEntidades.VIewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASCQWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly CMContext _context;
        public LoginController(CMContext context)
        {
            _context = context;
        }

        private void InicializarMensaje(string mensaje)
        {
            if (mensaje == null)
                mensaje = "";

            ViewData["Error"] = mensaje;
        }

        public IActionResult Index(string mensaje, string returnUrl = null)
        {
            InicializarMensaje(mensaje);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Index(CqasUsuario ascqusuario)
        {

            try
            {
                if (_context.CqasUsuario.Any(x => x.Usuario == ascqusuario.Usuario && x.Contrasena == ascqusuario.Contrasena))
                {
                    var user = _context.CqasUsuario.Where(x => x.Usuario == ascqusuario.Usuario && x.Contrasena == ascqusuario.Contrasena).Select(x => new AscqViewModelUsuario
                    {
                        Codigo = x.Codigo,
                        CodigoPersona = x.CodigoPersona,
                        CodigoPerfil = x.CodigoPerfil,
                        NombreUsuario = x.Usuario,
                        Perfil = x.CodigoPerfilNavigation.Descripcion
                    }).FirstOrDefault();
                    var menu = _context.CqasMenuPerfil.Where(x => x.CodigoPerfil == user.CodigoPerfil).Select(x => new CqasMenu 
                    {
                        Cabezera = x.CodigoMenuNavigation.Cabezera,
                        Descripcion =x.CodigoMenuNavigation.Descripcion,
                        Url = x.CodigoMenuNavigation.Url
                    }).ToList();
                    user.ListaMenu = menu.Where(x=>x.Cabezera == "Administracion").ToList();
                    user.ListaMenuMedico = menu.Where(x => x.Cabezera == "Medico").ToList(); 
                    user.ListaMenuPaciente = menu.Where(x => x.Cabezera == "Paciente").ToList();
                    user.ListaMenuReportes = menu.Where(x => x.Cabezera == "Reportes").ToList();
                    user.ListaMenuSeguridad = menu.Where(x => x.Cabezera == "Seguridad").ToList();
                    // variable de session datos paciente
                    HttpContext.Session.SetString("DatosUsuario", JsonConvert.SerializeObject(user));
                    if (user.Perfil == "Administrador")
                    {
                        return RedirectToAction("Index", "Personas");
                    }
                    else if (user.Perfil == "Medico")
                    {
                        return RedirectToAction("Index", "ConsultaCitasMedico");
                    }
                    else if (user.Perfil == "Paciente")
                    {
                        return RedirectToAction("Index", "AgendamientoCitas");
                    }
                    
                }
                else
                {
                    InicializarMensaje("Usuario o Contraseña Incorrectos");
                }
                return View(ascqusuario);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public IActionResult Salir()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");           
        }

        //private async Task<Adscpassw> GetAdscPassws(Adscpassw adscpassw)
        //{
        //    try
        //    {
        //        if (!adscpassw.Equals(null))
        //        {
        //            var respuesta = await apiServicio.ObtenerElementoAsync<entidades.Utils.Response>(adscpassw, new Uri(WebApp.BaseAddressSeguridad), "api/Adscpassws/SeleccionarMiembroLogueado");
        //            if (respuesta.IsSuccess)
        //            {
        //                var obje = JsonConvert.DeserializeObject<Adscpassw>(respuesta.Resultado.ToString());
        //                return obje;
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //private async Task<entidades.Utils.Response> EliminarToken(Adscpassw adscpassw)
        //{
        //    entidades.Utils.Response response = new entidades.Utils.Response();
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(adscpassw.AdpsLogin))
        //        {
        //            response = await apiServicio.EditarAsync<entidades.Utils.Response>(adscpassw, new Uri(WebApp.BaseAddressSeguridad), "api/Adscpassws/EliminarToken");
        //            if (response.IsSuccess)
        //            {
        //                await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.WebAppRM), EntityID = string.Format("{0} : {1}", "Sistema", adscpassw.AdpsLogin), LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit), LogLevelShortName = Convert.ToString(LogLevelParameter.ADV), Message = "Se ha actualizado un estado civil", UserName = "Usuario 1" });
        //                return response;
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        await GuardarLogService.SaveLogEntry(new LogEntryTranfer { ApplicationName = Convert.ToString(Aplicacion.WebAppRM), Message = "Editando un estado civil", LogCategoryParametre = Convert.ToString(LogCategoryParameter.Edit), LogLevelShortName = Convert.ToString(LogLevelParameter.ERR), UserName = "Usuario APP webapprm" });
        //        return null;
        //    }
        //}

        //private async Task<entidades.Utils.Response> EliminarTokenTemp(Adscpassw adscpassw)
        //{
        //    entidades.Utils.Response response = new entidades.Utils.Response();
        //    if (!string.IsNullOrEmpty(adscpassw.AdpsLogin))
        //    {
        //        response = await apiServicio.EditarAsync<entidades.Utils.Response>(adscpassw, new Uri(WebApp.BaseAddressSeguridad), "api/Adscpassws/EliminarTokenTemp");
        //        if (response.IsSuccess)
        //            return response;
        //    }
        //    return null;
        //}
    }
}