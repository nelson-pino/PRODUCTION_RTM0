using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitramaAPP.Clases
{
    public class ClassDevolucion
    {
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Id_Cust { get; set; }
        public string Razon { get; set; }
        public bool DocAnulado { get; set; }
        List<Item_Devol> items;
        public ClassDevolucion()
        {
            this.items = new List<Item_Devol>();
        }


    }
}
