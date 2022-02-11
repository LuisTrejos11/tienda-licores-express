using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licorera.Models
{
    public class SliderCLS
    {
        [Display(Name ="Id Slider")]
        public int iidslider { get; set; }

        [Display(Name = "Titulo")]
        [Required]
        public string titulo { get; set; }

        [Display(Name = "Descripcion")]
        [Required]
        public string descripcion { get; set; }

        public byte[] foto { get; set;  }



        // proiedades adicionales 

        public string nombreArchivo { get; set;  }
        public string nombreFoto { get; set; }

        public string mensaje { get; set; }

        public string extension { get; set; }

        public string fotoRecuperCadena { get; set; }

       

       
    }
}