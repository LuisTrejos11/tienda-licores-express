using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licorera.Models
{
    public class ProductoCLS
    {
        [Display(Name = "Id del Producto")]
        public int iidproducto { get; set; }

        [Display(Name = "Nombre")] 
        [Required]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string descripcion { get; set; }

        [Display(Name = "Id Categoria")]
        [Required]
        public int iidcategoria { get; set; }

        [Display(Name = "Precio")]
        [Range(0, 200000, ErrorMessage = "Precio fuera del rango permitido")]
        [Required]
        public decimal precio { get; set; }

        public byte[] foto { get; set;  }

        public string nombreArchivo { get; set;  }

        public int iidtipoproducto { get; set; }


        public int bhabilitado { get; set; }


        // proiedades adicionales 
        public string nombreFoto { get; set;  }

        public string mensaje { get; set; }

        public string extension { get; set; }

        public string fotoRecuperCadena { get; set; }

        public string categoria { get; set; }

        [Display(Name ="Tipo de Producto")]
        public string nombreTipoProducto { get; set; }

    }
}