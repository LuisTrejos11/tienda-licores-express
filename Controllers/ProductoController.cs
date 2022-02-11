using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licorera.Models;
using System.IO;
using Licorera.Filters; 

namespace Licorera.Controllers
{
    //[Acceder]
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            listarTipoProducto(); 
            listarCategoria(); 
            List<ProductoCLS> lista = new List<ProductoCLS>();

            using (var bd = new DBLicoreraEntities()) {

                lista = (from item in bd.Producto
                         join categoria in bd.Categoria
                         on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                         join tipoProducto in bd.TIPOPRODUCTO
                         on item.IIDTIPOPRODUCTO equals tipoProducto.IIDTIPOPRODUCTO
                         where item.BHABILITADO == 1
                         select new ProductoCLS
                         {
                             iidproducto = item.IIDPRODUCTO,
                             nombre = item.NOMBRE,
                             descripcion = item.DESCRIPCION,
                             categoria = categoria.NOMBRE,
                             precio = (decimal)item.PRECIO,
                             nombreTipoProducto = tipoProducto.NOMBRE 
                         }).ToList(); 

            }
                return View(lista);
        }

      

        public ActionResult gestionVinos() {

            listarTipoProducto();
            listarCategoria();
            List<VinoCLS> lista = new List<VinoCLS>();

            using (var bd = new DBLicoreraEntities())
            {

                lista = (from item in bd.Producto
                         join categoria in bd.Categoria
                         on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                         join tipoProducto in bd.TIPOPRODUCTO
                         on item.IIDTIPOPRODUCTO equals tipoProducto.IIDTIPOPRODUCTO
                         where item.BHABILITADO == 1 && categoria.NOMBRE == "Vinos"
                         select new VinoCLS
                         {
                             iidproducto = item.IIDPRODUCTO,
                             nombre = item.NOMBRE,
                             descripcion = item.DESCRIPCION,
                             categoria = categoria.NOMBRE,
                             precio = (decimal)item.PRECIO,
                             nombreTipoProducto = tipoProducto.NOMBRE,
                             variedad = item.VARIEDAD,
                             enotecnia = item.ENOTECNIA,
                             color= item.COLOR,
                             aroma = item.AROMA,
                             textura = item.TEXTURA, 
                             gradoAlcohol = item.GRADOALCOHOL,
                             sabor = item.SABOR

                         }).ToList();

            }
            return View(lista);

        }

        private void listarCategoria()
        {

            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new DBLicoreraEntities())
            {

                lista = (from item in bd.Categoria
                         where item.BHABILTADO == 1
                         select new SelectListItem
                         {

                             Text = item.NOMBRE,
                             Value = item.IIDCATEGORIA.ToString()
                         }).ToList();

                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = " " });
                ViewBag.listaCategoria = lista;
            }
        }


        private void listarTipoProducto()
        {

            List<SelectListItem> lista = new List<SelectListItem>();

            using (var bd = new DBLicoreraEntities())
            {

                lista = (from item in bd.TIPOPRODUCTO
                         where item.BHABILTADO == 1
                         select new SelectListItem
                         {

                             Text = item.NOMBRE,
                             Value = item.IIDTIPOPRODUCTO.ToString()
                         }).ToList();

                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = " " });
                ViewBag.listaTipoProducto = lista;
            }
        }
        public string Guardar(ProductoCLS oProductoCLS, int titulo, HttpPostedFileBase fotico) {

            string mensaje = "";
            try
            {
                if (!ModelState.IsValid || (fotico == null && titulo == -1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    if (fotico == null && titulo == -1)
                    {
                        oProductoCLS.mensaje = "La foto es obligatoria";
                        mensaje += "<ul><li> Debe ingresar la foto</li></ul>";
                    }
                    mensaje += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        mensaje += "<li class='list-group-item'>" + item + "</li>";
                    }
                    mensaje += "</ul>";

                }
                else
                {
                    byte[] fotoBD = null;
                    if (fotico != null)
                    {
                        BinaryReader lector = new BinaryReader(fotico.InputStream);
                        fotoBD = lector.ReadBytes((int)fotico.ContentLength);
                    }
                    using (var bd = new DBLicoreraEntities())
                    {
                        if (titulo == -1)
                        {
                            Producto oProducto = new Producto();
                            oProducto.NOMBRE = oProductoCLS.nombre;
                            oProducto.DESCRIPCION = oProductoCLS.descripcion;
                            oProducto.IIDCATEGORIA = oProductoCLS.iidcategoria;
                            oProducto.IIDTIPOPRODUCTO = oProductoCLS.iidtipoproducto; 
                            oProducto.PRECIO = oProductoCLS.precio;
                            oProducto.NOMBREFOTO = oProductoCLS.nombreFoto; 
                            oProducto.FOTO = fotoBD;
                           
                            oProducto.BHABILITADO = 1;
                            bd.Producto.Add(oProducto);
                            mensaje = bd.SaveChanges().ToString();
                            if (mensaje == "0") mensaje = "";


                        }
                        else
                        {
                            Producto oProducto = bd.Producto.Where(p => p.IIDPRODUCTO == titulo).First();
                            oProducto.NOMBRE = oProductoCLS.nombre;
                            oProducto.DESCRIPCION = oProductoCLS.descripcion;
                            oProducto.IIDCATEGORIA = oProductoCLS.iidcategoria;
                            oProducto.IIDTIPOPRODUCTO = oProductoCLS.iidtipoproducto; 
                            oProducto.PRECIO = oProductoCLS.precio;
                           
                            if (fotico != null) oProducto.FOTO = fotoBD;
                            mensaje = bd.SaveChanges().ToString();

                        }

                    }

                }



            }
            catch (Exception ex)
            {
                mensaje = "";
            }



            return mensaje;

        }

        public string GuardarVino(VinoCLS oVinoCLS, int titulo, HttpPostedFileBase fotico)
        {

            string mensaje = "";
            try
            {
                if (!ModelState.IsValid || (fotico == null && titulo == -1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    if (fotico == null && titulo == -1)
                    {
                        oVinoCLS.mensaje = "La foto es obligatoria";
                        mensaje += "<ul><li> Debe ingresar la foto</li></ul>";
                    }
                    mensaje += "<ul class='list-group'>";
                    foreach (var item in query)
                    {
                        mensaje += "<li class='list-group-item'>" + item + "</li>";
                    }
                    mensaje += "</ul>";

                }
                else
                {
                    byte[] fotoBD = null;
                    if (fotico != null)
                    {
                        BinaryReader lector = new BinaryReader(fotico.InputStream);
                        fotoBD = lector.ReadBytes((int)fotico.ContentLength);
                    }
                    using (var bd = new DBLicoreraEntities())
                    {
                        if (titulo == -1)
                        {
                            Producto oProducto = new Producto();
                            oProducto.NOMBRE = oVinoCLS.nombre;
                            oProducto.DESCRIPCION = oVinoCLS.descripcion;
                            oProducto.IIDCATEGORIA = 5;
                            oProducto.IIDTIPOPRODUCTO = oVinoCLS.iidtipoproducto;
                            oProducto.PRECIO = oVinoCLS.precio;
                            oProducto.NOMBREFOTO = oVinoCLS.nombreFoto;
                            oProducto.FOTO = fotoBD;
                            oProducto.VARIEDAD = oVinoCLS.variedad;
                            oProducto.ENOTECNIA = oVinoCLS.enotecnia;
                            oProducto.COLOR = oVinoCLS.color;
                            oProducto.SABOR = oVinoCLS.sabor;
                            oProducto.AROMA = oVinoCLS.aroma;
                            oProducto.TEXTURA = oVinoCLS.textura;
                            oProducto.GRADOALCOHOL = oVinoCLS.gradoAlcohol;
                            oProducto.PAIS = oVinoCLS.pais; 

                            oProducto.BHABILITADO = 1;
                            bd.Producto.Add(oProducto);
                            mensaje = bd.SaveChanges().ToString();
                            if (mensaje == "0") mensaje = "";


                        }
                        else
                        {
                            Producto oProducto = bd.Producto.Where(p => p.IIDPRODUCTO == titulo).First();
                            oProducto.NOMBRE = oVinoCLS.nombre;
                            oProducto.DESCRIPCION = oVinoCLS.descripcion;
                            oProducto.IIDCATEGORIA = 5;
                            oProducto.IIDTIPOPRODUCTO = oVinoCLS.iidtipoproducto;
                            oProducto.PRECIO = oVinoCLS.precio;
                            oProducto.VARIEDAD = oVinoCLS.variedad;
                            oProducto.ENOTECNIA = oVinoCLS.enotecnia;
                            oProducto.COLOR = oVinoCLS.color;
                            oProducto.SABOR = oVinoCLS.sabor;
                            oProducto.AROMA = oVinoCLS.aroma;
                            oProducto.TEXTURA = oVinoCLS.textura;
                            oProducto.GRADOALCOHOL = oVinoCLS.gradoAlcohol;
                            oProducto.PAIS = oVinoCLS.pais;

                            if (fotico != null) oProducto.FOTO = fotoBD;
                            mensaje = bd.SaveChanges().ToString();

                        }

                    }

                }



            }
            catch (Exception ex)
            {
                mensaje = "";
            }



            return mensaje;

        }

        public ActionResult Filtro(string nombreFiltro) {

            List<ProductoCLS> lista = new List<ProductoCLS>();

            using (var bd = new DBLicoreraEntities()) {

                if (nombreFiltro == null)
                {
                    lista = (from item in bd.Producto
                             join categoria in bd.Categoria
                             on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                             where item.BHABILITADO == 1
                             select new ProductoCLS
                             {
                                 iidproducto = item.IIDPRODUCTO,
                                 nombre = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,
                                 categoria = categoria.NOMBRE,
                                 precio = (decimal)item.PRECIO
                             }).ToList();
                }
                else {
                    lista = (from item in bd.Producto
                             join categoria in bd.Categoria
                             on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                             where item.BHABILITADO == 1 && item.NOMBRE.Contains(nombreFiltro)
                             select new ProductoCLS
                             {
                                 iidproducto = item.IIDPRODUCTO,
                                 nombre = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,
                                 categoria = categoria.NOMBRE,
                                 precio = (decimal)item.PRECIO
                             }).ToList();


                }

            }
            return PartialView("_TableProducto", lista);

        }

        public ActionResult FiltroVino(string nombreFiltro)
        {

            List<VinoCLS> lista = new List<VinoCLS>();

            using (var bd = new DBLicoreraEntities())
            {

                if (nombreFiltro == null)
                {
                    lista = (from item in bd.Producto
                             join categoria in bd.Categoria
                             on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                             where item.BHABILITADO == 1 &&  item.IIDCATEGORIA == 5
                             select new VinoCLS
                             {
                                 iidproducto = item.IIDPRODUCTO,
                                 nombre = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,
                                 categoria = categoria.NOMBRE,
                                 precio = (decimal)item.PRECIO
                             }).ToList();
                }
                else
                {
                    lista = (from item in bd.Producto
                             join categoria in bd.Categoria
                             on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                             where item.BHABILITADO == 1 && item.NOMBRE.Contains(nombreFiltro) && item.IIDCATEGORIA == 5
                             select new VinoCLS
                             {
                                 iidproducto = item.IIDPRODUCTO,
                                 nombre = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,
                                 categoria = categoria.NOMBRE,
                                 precio = (decimal)item.PRECIO
                             }).ToList();


                }

            }
            return PartialView("_TableVinos", lista);

        }

        public JsonResult RecuperarInformacion(int titulo) {

            ProductoCLS oProductoCLS = new ProductoCLS(); 
            using (var bd =  new DBLicoreraEntities()){

                Producto oProducto = bd.Producto.Where(p => p.IIDPRODUCTO == titulo).First();
                oProductoCLS.nombre = oProducto.NOMBRE;
                oProductoCLS.descripcion = oProducto.DESCRIPCION;
                oProductoCLS.iidcategoria = (int)oProducto.IIDCATEGORIA;
                oProductoCLS.iidtipoproducto = (int)oProducto.IIDTIPOPRODUCTO; 
                oProductoCLS.precio = (int)oProducto.PRECIO;
                oProductoCLS.nombreFoto = oProducto.NOMBREFOTO;
                

                oProductoCLS.extension = Path.GetExtension(oProducto.NOMBREFOTO);
                oProductoCLS.fotoRecuperCadena = Convert.ToBase64String(oProducto.FOTO);


            }

            return Json(oProductoCLS, JsonRequestBehavior.AllowGet); 
        }

       



        public int Eliminar(int idProducto) {

            int rpta = 0;

            try
            {
                using (var bd = new DBLicoreraEntities())
                {

                    Producto oProducto = bd.Producto.Where(p => p.IIDPRODUCTO == idProducto).First();
                    oProducto.BHABILITADO = 0;
                    rpta = bd.SaveChanges();

                }
            }
            catch (Exception ex) {

                rpta = 0; 
            }

            return rpta; 

        }
    }
}