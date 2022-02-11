using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licorera.Models
{
    public class UsuarioCLS
    {

        public int iidusuario { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string passwword { get; set; }
       
        public string email { get; set; }

      

        public int iidrol { get; set; }

        public int bhabilitado { get; set; }

        //Propiedad Adicional

       
        public string nombreRol { get; set; }
     
    }
}