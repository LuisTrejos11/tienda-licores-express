using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Licorera.Models;
using Licorera.Filters;
using System.IO;

namespace Licorera.Controllers
{
    [Acceder]
    public class SliderController : Controller
    {
        // GET: Slider
        public ActionResult Index()
        {
            List<SliderCLS> lista = new List<SliderCLS>();

            using (var bd = new DBLicoreraEntities()) {

                lista = (from item in bd.Slider
                         where item.BHABILTADO == 1
                         select new SliderCLS
                         {

                             iidslider = item.IIDSLIDER,
                             titulo = item.NOMBRE,
                             descripcion = item.DESCRIPCION
                         }).ToList(); 

            }

                return View(lista);
        }


        public ActionResult Filtro(string nombreFiltro) {

            List<SliderCLS> lista = new List<SliderCLS>();


            using (var bd = new DBLicoreraEntities()) {

                if (nombreFiltro == null)
                {

                    lista = (from item in bd.Slider
                             where item.BHABILTADO == 1
                             select new SliderCLS
                             {

                                 iidslider = item.IIDSLIDER,
                                 titulo = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,

                             }).ToList();

                }
                else {

                    lista = (from item in bd.Slider
                             where item.BHABILTADO == 1 && item.NOMBRE.Contains(nombreFiltro)
                             select new SliderCLS
                             {

                                 iidslider = item.IIDSLIDER,
                                 titulo = item.NOMBRE,
                                 descripcion = item.DESCRIPCION,

                             }).ToList();


                }

                return PartialView("_TableSlider", lista); 

            }


        }

        public string Guardar(SliderCLS oSliderCLS, int valor, HttpPostedFileBase fotico) {

            string mensaje = "";

            try
            {
                if (!ModelState.IsValid || (fotico == null && valor == -1))
                {
                    var query = (from state in ModelState.Values
                                 from error in state.Errors
                                 select error.ErrorMessage).ToList();
                    if (fotico == null && valor == -1)
                    {
                        oSliderCLS.mensaje = "La foto es obligatoria";
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
                        if (valor == -1)
                        {
                            Slider oSlider = new Slider();
                            oSlider.NOMBRE = oSliderCLS.titulo;
                            oSlider.DESCRIPCION = oSliderCLS.descripcion;
                            oSlider.NOMBREFOTO = oSliderCLS.nombreFoto;
                            oSlider.FOTO = fotoBD;

                            oSlider.BHABILTADO = 1;
                            bd.Slider.Add(oSlider);
                            mensaje = bd.SaveChanges().ToString();
                            if (mensaje == "0") mensaje = "";


                        }
                        else
                        {
                            Slider oSlider = bd.Slider.Where(p => p.IIDSLIDER == valor).First();
                            oSlider.NOMBRE = oSliderCLS.titulo;
                            oSlider.DESCRIPCION = oSliderCLS.descripcion;
                           

                            if (fotico != null) oSlider.FOTO = fotoBD;
                            mensaje = bd.SaveChanges().ToString();

                        }

                    }
                }
            }
            catch (Exception ex) {

                mensaje = "0"; 

            }

            return mensaje; 

        }

        public string Eliminar(int iidSlider) {

            string rpta = "";

            try
            {
                using (var bd = new DBLicoreraEntities()) {

                    Slider oSlider = bd.Slider.Where(p => p.IIDSLIDER == iidSlider).First();
                    oSlider.BHABILTADO = 0;
                    rpta = bd.SaveChanges().ToString() ; 
                }


            }
            catch (Exception ex) {

                rpta = "0"; 

            }
            return rpta; 
        }

        public JsonResult RecuperarInformacion(int valor) {

            SliderCLS oSliderCLS = new SliderCLS();

            using (var bd = new DBLicoreraEntities()) {

                Slider oSlider = bd.Slider.Where(p => p.IIDSLIDER == valor).First();

                oSliderCLS.iidslider = oSlider.IIDSLIDER;
                oSliderCLS.titulo = oSlider.NOMBRE;
                oSliderCLS.descripcion = oSlider.DESCRIPCION;

                oSliderCLS.extension = Path.GetExtension(oSlider.NOMBREFOTO);
                oSliderCLS.fotoRecuperCadena = Convert.ToBase64String(oSlider.FOTO);

            }

            return Json(oSliderCLS, JsonRequestBehavior.AllowGet); 

        }


       
    }
}