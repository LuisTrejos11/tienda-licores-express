using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licorera.Models
{
    public class IndexViewModel: PaginadorCLS
    {

        public List<ProductoCLS> Productos { get; set; }

    }
}