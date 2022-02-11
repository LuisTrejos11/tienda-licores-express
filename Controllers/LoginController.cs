using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licorera.Models;
using System.Security.Cryptography;
using System.Text;
using Licorera.ClasesAuxiliares; 

namespace Licorera.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSession()
        {
            Session["Usuario"] = null;
            Session["Rol"] = null;
            return RedirectToAction("Index");
        }

        public string Login(UsuarioCLS oUsuarioCLS) {

            string rpta = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    rpta += "<ul class= 'list-group' >";
                    foreach (var item in query)
                    {
                        rpta += "<li class = 'list-group'>" + item + "</li>";
                    }

                    rpta += "</ul>";
                }
                else {
                    string nombreUsuario = oUsuarioCLS.nombre;
                    string password = oUsuarioCLS.passwword;
                    //Cifrar
                    SHA256Managed sha = new SHA256Managed();
                    byte[] byteContra = Encoding.Default.GetBytes(password);
                    byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                    //Eso es lo que cuenta
                    string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");

                    using (var bd = new DBLicoreraEntities()) {

                        int numeroVeces = bd.Usuario.Where(p => p.NOMBRE == nombreUsuario && p.PASSWORD == cadenaContraCifrada).Count();
                        rpta = numeroVeces.ToString();
                        if (rpta == "0") rpta = "Usuario o contraseña incorrecta";
                        else {

                            Usuario oUsuario = bd.Usuario.Where(p => p.NOMBRE == nombreUsuario && p.PASSWORD == cadenaContraCifrada).First();
                            Session["Usuario"] = oUsuario;
                            Session["Rol"] = oUsuario.IIDROL;
                            Session["NombreUsuario"] = oUsuario.NOMBRE; 
                        }
                    }



                }

            }
            catch (Exception ex) {

                rpta = "";
            }


            return rpta;

        }

        public ActionResult Registrarse() {

            return View(); 
        }

        public string Registro(UsuarioCLS oUsuarioCLS) {

            string rpta = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    rpta += "<ul class= 'list-group' >";
                    foreach (var item in query)
                    {
                        rpta += "<li class = 'list-group'>" + item + "</li>";
                    }

                    rpta += "</ul>";
                }
                else {

                    string nombreUsuario = oUsuarioCLS.nombre;
                    string password = oUsuarioCLS.passwword;
                    string email = oUsuarioCLS.email; 
                    //Cifrar
                    SHA256Managed sha = new SHA256Managed();
                    byte[] byteContra = Encoding.Default.GetBytes(password);
                    byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                    //Eso es lo que cuenta
                    string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                    int cantidad = 0;

                    using (var bd = new DBLicoreraEntities()) {

                        cantidad = bd.Usuario.Where(p => p.NOMBRE == nombreUsuario).Count();

                        if (cantidad >= 1)
                        {

                            rpta = "-1";

                        }
                        else {

                            Usuario oUsuario = new Usuario();
                            oUsuario.NOMBRE = nombreUsuario;
                            oUsuario.PASSWORD = cadenaContraCifrada;
                            oUsuario.EMAIL = email;
                            oUsuario.IIDROL = 2;
                            oUsuario.BHABILITADO = 1;
                            bd.Usuario.Add(oUsuario);
                            rpta = bd.SaveChanges().ToString();
                            if (rpta == "0") rpta = ""; 

                        }

                    }

                }

            }
            catch (Exception ex) {

                rpta = ""; 
            }

            return rpta; 

        }

        public string RecuperarContra(string correo ) {

            string mensaje = ""; 
            using (var bd = new DBLicoreraEntities()) {
                int cantidad;
                cantidad = bd.Usuario.Where(p => p.EMAIL == correo && p.BHABILITADO == 1).Count();
                int iidUsuario = bd.Usuario.Where(p => p.EMAIL == correo).First().IIDUSUARIO; 
                if (cantidad == 0)
                {

                    mensaje = "No existe un usuario con ese correo";
                }
                else {
                    // obtener usuario
                    Usuario oUsuario = bd.Usuario.Where(p => p.IIDUSUARIO == iidUsuario).First();
                    // modificar clave
                    Random ra = new Random();
                    int n1 = ra.Next(0, 9);
                    int n2 = ra.Next(0, 9);
                    int n3 = ra.Next(0, 9);
                    int n4 = ra.Next(0, 9);

                    string nuevaContra = n1.ToString() + n2 + n3 + n4;


                    // cifrar la clave 
                    SHA256Managed sha = new SHA256Managed();
                    byte[] byteContra = Encoding.Default.GetBytes(nuevaContra);
                    byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                    //Eso es lo que cuenta
                    string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");
                    oUsuario.PASSWORD = cadenaContraCifrada;
                     mensaje = bd.SaveChanges().ToString(); 

                    Correo.EnviarCorreo(correo, "Recuperar contraseña","Se ha cambiado su contraseña, ahora su contraseña es:" +nuevaContra , "Home/Login");
                }


            }

            return mensaje; 
        }
    }
}