using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licorera.Models;

using System.Security;
using System.Security.Cryptography;
using System.Text;
using Licorera.Filters;

namespace Licorera.Controllers
{
    [Acceder]
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            listaRol();
            List<UsuarioCLS> lista = new List<UsuarioCLS>();


            using (var bd = new DBLicoreraEntities()) {

                lista = (from usuario in bd.Usuario
                         join rol in bd.Rol
                         on usuario.IIDROL equals rol.IIDROL
                         where usuario.BHABILITADO == 1
                         select new UsuarioCLS
                         {

                             iidusuario = usuario.IIDUSUARIO,
                             nombre = usuario.NOMBRE,
                             email = usuario.EMAIL,
                             nombreRol = rol.NOMBRE

                         }).ToList();
            }

            return View(lista);
        }

        private void listaRol() {

            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new DBLicoreraEntities()) {

                lista = (from item in bd.Rol
                         where item.BHABILITADO == 1
                         select new SelectListItem
                         {

                             Text = item.NOMBRE,
                             Value = item.IIDROL.ToString()

                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = " " });
                ViewBag.listaRol = lista;

            }

        }

        public ActionResult Filtrar(string nombrePersona) {

            List<UsuarioCLS> lista = new List<UsuarioCLS>();

            using (var bd = new DBLicoreraEntities()) {

                if (nombrePersona == null)
                {
                    lista = (from usuario in bd.Usuario
                             join rol in bd.Rol
                             on usuario.IIDROL equals rol.IIDROL
                             where usuario.BHABILITADO == 1 && usuario.IIDROL == 1
                             select new UsuarioCLS
                             {

                                 iidusuario = usuario.IIDUSUARIO,
                                 nombre = usuario.NOMBRE,
                                 email = usuario.EMAIL,
                                 nombreRol = rol.NOMBRE

                             }).ToList();

                }
                else {


                    lista = (from usuario in bd.Usuario
                             join rol in bd.Rol
                             on usuario.IIDROL equals rol.IIDROL
                             where usuario.BHABILITADO == 1 && usuario.NOMBRE.Contains(nombrePersona)
                             select new UsuarioCLS
                             {

                                 iidusuario = usuario.IIDUSUARIO,
                                 nombre = usuario.NOMBRE,
                                 email = usuario.EMAIL,
                                 nombreRol = rol.NOMBRE

                             }).ToList();



                }

            }

            return PartialView("_TableUsuario", lista);

        }

        public string GuardarAdmin(UsuarioCLS oUsuarioCLS, int titulo) {

            string rpta = "";
            int cantidad = 0;
            try {

                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    rpta += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        rpta += "<li class='list-group-item'>" + item + "</li>";
                    }
                    rpta += "</ul>";

                }
                else {

                    using (var bd = new DBLicoreraEntities()) {


                        if (titulo == -1)
                        {

                            cantidad = bd.Usuario.Where(p => p.NOMBRE == oUsuarioCLS.nombre).Count();
                            if (cantidad >= 1)
                            {

                                rpta = "-1";
                            }
                            else
                            {

                                Usuario oUsuario = new Usuario();
                                oUsuario.NOMBRE = oUsuarioCLS.nombre;
                                oUsuario.EMAIL = oUsuarioCLS.email;
                                SHA256Managed sha = new SHA256Managed();
                                byte[] byteContra = Encoding.Default.GetBytes(oUsuarioCLS.passwword);
                                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                                oUsuario.PASSWORD = cadenaContraCifrada;
                                oUsuario.IIDROL = 1;
                                oUsuario.BHABILITADO = 1;
                                bd.Usuario.Add(oUsuario);
                                rpta = bd.SaveChanges().ToString();
                                if (rpta == "0") rpta = "";

                            }
                        }
                        else {
                            

                                Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();
                                oUsuario.NOMBRE = oUsuarioCLS.nombre;
                                oUsuario.EMAIL = oUsuarioCLS.email;
                                SHA256Managed sha = new SHA256Managed();
                                byte[] byteContra = Encoding.Default.GetBytes(oUsuarioCLS.passwword);
                                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                                oUsuario.PASSWORD = cadenaContraCifrada;
                                oUsuario.IIDROL = 1;
                                rpta = bd.SaveChanges().ToString();


                            

                        }



                    }

                }


            }
            catch (Exception ex) {

                rpta = "";

            }

            return rpta;
        }


        public int Eliminar(int titulo) {

            int rpta = 0;

            try
            {
                using (var bd = new DBLicoreraEntities()) {

                    Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();
                    oUsuario.BHABILITADO = 0;
                    rpta = bd.SaveChanges();

                }


            }
            catch (Exception ex) {

                rpta = 0;

            }
            return rpta;

        }

        public JsonResult recuperarInformacion(int titulo) {

            UsuarioCLS oUsuarioCLS = new UsuarioCLS();

            using (var bd = new DBLicoreraEntities()) {

                Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == titulo).First();

                oUsuarioCLS.nombre = oUsuario.NOMBRE;
                oUsuarioCLS.email = oUsuario.EMAIL;

            }

            return Json(oUsuarioCLS, JsonRequestBehavior.AllowGet);

        }


       
    }
}