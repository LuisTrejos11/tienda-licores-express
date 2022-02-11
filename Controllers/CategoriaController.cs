using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licorera.Models;
using Licorera.Filters; 

namespace Licorera.Controllers
{
    [Acceder]
    public class CategoriaController : Controller
    {
        // GET: Categoria
        public ActionResult Index()
        {
            List<CategoriaCLS> listaCategoria = new List<CategoriaCLS>();

            using (var bd = new DBLicoreraEntities()) {

                listaCategoria = (from categoria in bd.Categoria
                                  where categoria.BHABILTADO == 1
                                  select new CategoriaCLS
                                  {

                                      iidcategoria = categoria.IIDCATEGORIA,
                                      nombre = categoria.NOMBRE

                                  }).ToList(); 
            }

                return View(listaCategoria);
        }

        public ActionResult Filtro(string nombreFiltro) {

            List<CategoriaCLS> lista = new List<CategoriaCLS>();
            using (var bd = new DBLicoreraEntities()) {

                if (nombreFiltro == null)
                {

                    lista = (from item in bd.Categoria
                             where item.BHABILTADO == 1
                             select new CategoriaCLS
                             {

                                 nombre = item.NOMBRE,
                                 iidcategoria = item.IIDCATEGORIA

                             }).ToList();

                }
                else {

                    lista = (from item in bd.Categoria
                             where item.BHABILTADO == 1 && item.NOMBRE.Contains(nombreFiltro)
                             select new CategoriaCLS
                             {

                                 nombre = item.NOMBRE,
                                 iidcategoria = item.IIDCATEGORIA

                             }).ToList();
                }

            }

                return PartialView("_TableCategoria", lista); 
        }

        public string Guardar(CategoriaCLS oCategoriaCLS, int titulo) {

            string rpta = "";
           
           

          try {

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
                    using (var bd = new DBLicoreraEntities()) {
                        int cantidad = 0; 
                        if (titulo == -1)
                        {
                            cantidad = bd.Categoria.Where(p => p.NOMBRE == oCategoriaCLS.nombre).Count();
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else { 

                            Categoria oCategoria = new Categoria();
                            oCategoria.NOMBRE = oCategoriaCLS.nombre;
                            oCategoria.BHABILTADO = 1;
                                bd.Categoria.Add(oCategoria); 
                            rpta = bd.SaveChanges().ToString();
                            if (rpta == "0") rpta = "";
                            }
                        }
                        else {
                            cantidad = bd.Categoria.Where(p => p.NOMBRE == oCategoriaCLS.nombre && p.IIDCATEGORIA != titulo).Count();
                            if (cantidad >= 1)
                            {
                                rpta = "-1";
                            }
                            else {

                                Categoria oCategoria = bd.Categoria.Where(p => p.IIDCATEGORIA == titulo).First();
                                oCategoria.NOMBRE = oCategoriaCLS.nombre;
                                rpta = bd.SaveChanges().ToString();
                                if (rpta == "0") rpta = ""; 
                            }
                        }
                    }

                }
            }
            catch (Exception ex) {
                rpta = "";

            }

            return rpta; 
        }

        public string Eliminar(int titulo) {

            string rpta = "";

            try
            {
                using (var bd = new DBLicoreraEntities()) {

                    Categoria oCategoria = bd.Categoria.Where(p => p.IIDCATEGORIA == titulo).First();
                    oCategoria.BHABILTADO = 0;
                    rpta = bd.SaveChanges().ToString(); 

                }

            }
            catch (Exception ex) {

                rpta = ""; 
            }

            return rpta;

        }
    }
}