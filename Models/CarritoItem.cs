using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licorera.Models
{
	public class CarritoItem
	{
        private Producto _producto;

        public Producto Producto
        {
            get { return _producto; }
            set { _producto = value; }
        }
        private int _cantidad;

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public CarritoItem()
        {

        }

        public CarritoItem(Producto producto, int cantidad)
        {
            this._producto = producto;
            this._cantidad = cantidad;
        }
    }
}