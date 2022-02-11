using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Licorera.Models
{
    public class CategoriaCLS
    {
        [Display(Name = "Id Categoria")]
        public int iidcategoria { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }

        public int bhabilitado { get; set; }


        // propiedad adicional 
        public string mensajeError { get; set;  }
    }
}