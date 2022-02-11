using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licorera.Models
{
    public class VinoCLS: ProductoCLS

    {
        [Display(Name = "Variedad")]
        public string variedad { get; set;  }

        [Display(Name = "Enotecnia")]
        public string enotecnia { get; set; }

        [Display(Name = "Color")]
        public string color { get; set; }

        [Display(Name = "Aroma")]
        public string aroma { get; set; }

        [Display(Name = "Sabor")]
        public string sabor { get; set; }

        [Display(Name = "Textura")]
        public string textura { get; set; }

        [Display(Name = "Grado de alcohol")]
        public string gradoAlcohol { get; set; }

        [Display(Name = "Pais")]
        public string pais { get; set; }


    }
}