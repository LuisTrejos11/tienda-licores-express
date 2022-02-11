//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Licorera.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            this.Carrito = new HashSet<Carrito>();
        }
    
        public int IIDPRODUCTO { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<int> IIDCATEGORIA { get; set; }
        public Nullable<int> BHABILITADO { get; set; }
        public Nullable<decimal> PRECIO { get; set; }
        public byte[] FOTO { get; set; }
        public string NOMBREFOTO { get; set; }
        public Nullable<int> IIDTIPOPRODUCTO { get; set; }
        public string VARIEDAD { get; set; }
        public string ENOTECNIA { get; set; }
        public string COLOR { get; set; }
        public string AROMA { get; set; }
        public string TEXTURA { get; set; }
        public string GRADOALCOHOL { get; set; }
        public string SABOR { get; set; }
        public string PAIS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carrito> Carrito { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual TIPOPRODUCTO TIPOPRODUCTO { get; set; }
    }
}