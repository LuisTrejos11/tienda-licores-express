using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licorera.Models;
using System.Web.Routing;
using System.IO;
using Licorera.ClasesAuxiliares;
using PagedList; 

namespace Licorera.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index(int pagina = 1)
        //{

        //    listarCategoria();
        //    listarSlider();
        //    List<ProductoCLS> lista = new List<ProductoCLS>();
        //    var modelo = new IndexViewModel();
        //    var cantidadRegistrosPorPagina = 1;

        //    using (var bd = new DBLicoreraEntities())
        //    {

        //        lista = (from item in bd.Producto
        //                 join categoria in bd.Categoria
        //                 on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
        //                 join tipoProducto in bd.TIPOPRODUCTO
        //                 on item.IIDTIPOPRODUCTO equals tipoProducto.IIDTIPOPRODUCTO
        //                 where item.BHABILITADO == 1
        //                 select new ProductoCLS
        //                 {
        //                     iidproducto = item.IIDPRODUCTO,
        //                     nombre = item.NOMBRE,
        //                     descripcion = item.DESCRIPCION,
        //                     categoria = categoria.NOMBRE,
        //                     precio = (decimal)item.PRECIO,
        //                     nombreTipoProducto = tipoProducto.NOMBRE,
        //                     nombreArchivo = item.NOMBREFOTO,
        //                     foto = item.FOTO
        //                 }).ToList();

        //        var productos = lista.Skip((pagina - 1) * cantidadRegistrosPorPagina).Take(cantidadRegistrosPorPagina).ToList();

        //        var totalRegistros = bd.Producto.Count();


        //        // var modelo = new IndexViewModel();
        //        modelo.Productos = productos;

        //        modelo.PaginaActual = pagina;
        //        modelo.TotalDeRegistros = totalRegistros;
        //        modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;

        //        ViewBag.modelo = modelo;
        //    }


        //    return View(lista);
        //    //return View(lista);
        //}


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

        public void listarSlider()
        {
            List<SliderCLS> lista = new List<SliderCLS>();

            using (var bd = new DBLicoreraEntities())
            {

                lista = (from item in bd.Slider
                         where item.BHABILTADO == 1
                         select new SliderCLS
                         {

                             iidslider = item.IIDSLIDER,
                             titulo = item.NOMBRE,
                             descripcion = item.DESCRIPCION,
                             foto = item.FOTO,
                             nombreArchivo = item.NOMBREFOTO
                         }).ToList();

            }
            ViewBag.listaSlider = lista;

        }


        public ActionResult Index( string nombreProd, int? valorObtenidoRango, int? pageSize, int? page) {


            listarCategoria();
             listarSlider();
            List<VinoCLS> lista = new List<VinoCLS>();
            List<VinoCLS> listaRpta = new List<VinoCLS>();


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
                                 iidcategoria = (int)item.IIDCATEGORIA,
                                 iidproducto = item.IIDPRODUCTO,
                                 nombre = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,
                                 categoria = categoria.NOMBRE,
                                 precio = (decimal)item.PRECIO,
                                 nombreTipoProducto = tipoProducto.NOMBRE,
                                 foto = item.FOTO,
                                 nombreArchivo = item.NOMBREFOTO,
                                 variedad = item.VARIEDAD,
                                 enotecnia = item.ENOTECNIA,
                                 color = item.COLOR,
                                 sabor = item.SABOR,
                                 textura = item.TEXTURA, 
                                 gradoAlcohol = item.GRADOALCOHOL,
                                 pais = item.PAIS
                           

                             }).ToList();

                if ( nombreProd == null && (valorObtenidoRango == 0 || valorObtenidoRango == null))
                {

                    listaRpta = lista;
                }
                else {
                 
                 
                    if (nombreProd != null)
                    {

                        lista = lista.Where(p => p.nombre.ToString().Contains(nombreProd)).ToList();
                    }
                    if (valorObtenidoRango != 0  && valorObtenidoRango != null)
                    {
                        
                        lista = lista.Where(p => p.precio == valorObtenidoRango || p.precio <= valorObtenidoRango ).ToList();
                    }

                    listaRpta = lista; 
                }

                pageSize = (pageSize ?? 2);
                page = (page ?? 1);

                ViewBag.pageSize = pageSize; 
                
                
            }
         
            //return PartialView("_ProdCat", lista);
            return View(listaRpta.ToPagedList(page.Value, pageSize.Value));
        }

        public ActionResult MostrarProductos(string iidcategoria, string nombreProd, int? valorObtenidoRango, int? pageSize, int? page)
        {


            listarCategoria();
            listarSlider();
            List<ProductoCLS> lista = new List<ProductoCLS>();
            List<ProductoCLS> listaRpta = new List<ProductoCLS>();


            using (var bd = new DBLicoreraEntities())
            {


                lista = (from item in bd.Producto
                         join categoria in bd.Categoria
                         on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                         join tipoProducto in bd.TIPOPRODUCTO
                         on item.IIDTIPOPRODUCTO equals tipoProducto.IIDTIPOPRODUCTO
                         where item.BHABILITADO == 1


                         select new ProductoCLS
                         {
                             iidcategoria = (int)item.IIDCATEGORIA,
                             iidproducto = item.IIDPRODUCTO,
                             nombre = item.NOMBRE,
                             descripcion = item.DESCRIPCION,
                             categoria = categoria.NOMBRE,
                             precio = (decimal)item.PRECIO,
                             nombreTipoProducto = tipoProducto.NOMBRE,
                             foto = item.FOTO,
                             nombreArchivo = item.NOMBREFOTO
                         }).ToList();

                if ((iidcategoria == null || iidcategoria == " ") && nombreProd == null && (valorObtenidoRango == 0 || valorObtenidoRango == null))
                {

                    listaRpta = lista;
                }
                else
                {

                    if (iidcategoria != null && iidcategoria != " ")
                    {

                        lista = lista.Where(p => p.iidcategoria.ToString().Contains(iidcategoria)).ToList();


                    }
                    if (nombreProd != null)
                    {

                        lista = lista.Where(p => p.nombre.ToString().Contains(nombreProd)).ToList();
                    }
                    if (valorObtenidoRango != 0 && valorObtenidoRango != null)
                    {

                        lista = lista.Where(p => p.precio == valorObtenidoRango || p.precio <= valorObtenidoRango).ToList();
                    }

                    listaRpta = lista;
                }


                pageSize = (pageSize ?? 2);
                page = (page ?? 1);

                ViewBag.pageSize = pageSize;


            }

            //return PartialView("_ProdCat", lista);
            return View(listaRpta.ToPagedList(page.Value, pageSize.Value));
        }

        public ActionResult QuesosYembutidos(string nombreProd, int? valorObtenidoRango, int? pageSize, int? page)
        {


            listarCategoria();
            listarSlider();
            List<ProductoCLS> lista = new List<ProductoCLS>();
            List<ProductoCLS> listaRpta = new List<ProductoCLS>();


            using (var bd = new DBLicoreraEntities())
            {


                lista = (from item in bd.Producto
                         join categoria in bd.Categoria
                         on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                         join tipoProducto in bd.TIPOPRODUCTO
                         on item.IIDTIPOPRODUCTO equals tipoProducto.IIDTIPOPRODUCTO
                         where item.BHABILITADO == 1 && categoria.NOMBRE == "Quesos y Embutidos"


                         select new ProductoCLS
                         {
                             iidcategoria = (int)item.IIDCATEGORIA,
                             iidproducto = item.IIDPRODUCTO,
                             nombre = item.NOMBRE,
                             descripcion = item.DESCRIPCION,
                             categoria = categoria.NOMBRE,
                             precio = (decimal)item.PRECIO,
                             nombreTipoProducto = tipoProducto.NOMBRE,
                             foto = item.FOTO,
                             nombreArchivo = item.NOMBREFOTO
                             


                         }).ToList();

                if (nombreProd == null && (valorObtenidoRango == 0 || valorObtenidoRango == null))
                {

                    listaRpta = lista;
                }
                else
                {


                    if (nombreProd != null)
                    {

                        lista = lista.Where(p => p.nombre.ToString().Contains(nombreProd)).ToList();
                    }
                    if (valorObtenidoRango != 0 && valorObtenidoRango != null)
                    {

                        lista = lista.Where(p => p.precio == valorObtenidoRango || p.precio <= valorObtenidoRango).ToList();
                    }

                    listaRpta = lista;
                }

                pageSize = (pageSize ?? 2);
                page = (page ?? 1);

                ViewBag.pageSize = pageSize;



            }

            //return PartialView("_ProdCat", lista);
            return View(listaRpta.ToPagedList(page.Value, pageSize.Value));
        }


        public ActionResult productosGourmet(string nombreProd, int? valorObtenidoRango, int? pageSize, int? page)
        {


            listarCategoria();
            listarSlider();
            List<ProductoCLS> lista = new List<ProductoCLS>();
            List<ProductoCLS> listaRpta = new List<ProductoCLS>();


            using (var bd = new DBLicoreraEntities())
            {


                lista = (from item in bd.Producto
                         join categoria in bd.Categoria
                         on item.IIDCATEGORIA equals categoria.IIDCATEGORIA
                         join tipoProducto in bd.TIPOPRODUCTO
                         on item.IIDTIPOPRODUCTO equals tipoProducto.IIDTIPOPRODUCTO
                         where item.BHABILITADO == 1 && categoria.NOMBRE == "Gourmet"


                         select new ProductoCLS
                         {
                             iidcategoria = (int)item.IIDCATEGORIA,
                             iidproducto = item.IIDPRODUCTO,
                             nombre = item.NOMBRE,
                             descripcion = item.DESCRIPCION,
                             categoria = categoria.NOMBRE,
                             precio = (decimal)item.PRECIO,
                             nombreTipoProducto = tipoProducto.NOMBRE,
                             foto = item.FOTO,
                             nombreArchivo = item.NOMBREFOTO



                         }).ToList();

                if (nombreProd == null && (valorObtenidoRango == 0 || valorObtenidoRango == null))
                {

                    listaRpta = lista;
                }
                else
                {


                    if (nombreProd != null)
                    {

                        lista = lista.Where(p => p.nombre.ToString().Contains(nombreProd)).ToList();
                    }
                    if (valorObtenidoRango != 0 && valorObtenidoRango != null)
                    {

                        lista = lista.Where(p => p.precio == valorObtenidoRango || p.precio <= valorObtenidoRango).ToList();
                    }

                    listaRpta = lista;
                }



                pageSize = (pageSize ?? 2);
                page = (page ?? 1);

                ViewBag.pageSize = pageSize;

            }

            //return PartialView("_ProdCat", lista);
            return View(listaRpta.ToPagedList(page.Value, pageSize.Value));
        }

        public JsonResult RecuperarInformacion(int titulo)
        {

            ProductoCLS oProductoCLS = new ProductoCLS();
            using (var bd = new DBLicoreraEntities())
            {

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

        public JsonResult RecuperarInformacionVino(int titulo)
        {

            VinoCLS oProductoCLS = new VinoCLS();
            using (var bd = new DBLicoreraEntities())
            {

                Producto oProducto = bd.Producto.Where(p => p.IIDPRODUCTO == titulo).First();
                oProductoCLS.nombre = oProducto.NOMBRE;
                oProductoCLS.descripcion = oProducto.DESCRIPCION;
                oProductoCLS.iidcategoria = (int)oProducto.IIDCATEGORIA;
                oProductoCLS.iidtipoproducto = (int)oProducto.IIDTIPOPRODUCTO;
                oProductoCLS.precio = (int)oProducto.PRECIO;
                oProductoCLS.nombreFoto = oProducto.NOMBREFOTO;
                oProductoCLS.variedad = oProducto.VARIEDAD;
                oProductoCLS.color = oProducto.COLOR;
                oProductoCLS.sabor = oProducto.SABOR;
                oProductoCLS.textura = oProducto.TEXTURA;
                oProductoCLS.pais = oProducto.PAIS;
                oProductoCLS.enotecnia = oProducto.ENOTECNIA;
                oProductoCLS.gradoAlcohol = oProducto.GRADOALCOHOL;




                oProductoCLS.extension = Path.GetExtension(oProducto.NOMBREFOTO);
                oProductoCLS.fotoRecuperCadena = Convert.ToBase64String(oProducto.FOTO);


            }

            return Json(oProductoCLS, JsonRequestBehavior.AllowGet);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult banners() {

            return View(); 
        }

        public ActionResult slider() {
            listarSlider(); 
            return View(); 
        }

        public ActionResult Modal() {

            return View(); 
        }


        public string  correoContacto(string nombre, string correo, string telefono, string mensaje) {

            string rpta = "";


            if (nombre != null && correo != null && telefono != null && mensaje != null)
            {

                Correo.EnviarCorreo(correo, "Mensaje de formulario de contacto: "+nombre+" Telefono: "+telefono+" correo: "+correo , mensaje, "");
                rpta = "1";
            }
            else {

                rpta = "0"; 
            }

            return rpta; 

        }
    }

   
}