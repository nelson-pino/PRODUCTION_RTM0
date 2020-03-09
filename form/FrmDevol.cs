using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RitramaAPP.form
{
    public partial class FrmDevol : Form
    {
        public FrmDevol()
        {
            InitializeComponent();
        }

        private void FrmDevol_Load(object sender, EventArgs e)
        {
            GridDevol.AutoGenerateColumns = false;
            AGREGAR_COLUMN_GRID("idproduct",68, "Id Product","idproduct", GridDevol);
            AGREGAR_COLUMN_GRID("productname", 240, "Nombre del Producto", "productname", GridDevol);
            AGREGAR_COLUMN_GRID("cant", 68, "Cantidad", "cant", GridDevol);
            AGREGAR_COLUMN_GRID("ID / RC", 68, "Numero ID", "id", GridDevol);
        }
        private void AGREGAR_COLUMN_GRID(string name, int size, string title, string field_bd, DataGridView grid)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                Name = name,
                Width = size,
                HeaderText = title,
                DataPropertyName = field_bd
            };
            grid.Columns.Add(col);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bot_nuevo_Click(object sender, EventArgs e)
        {
            //abro textbox
            TXT_NUMERO.ReadOnly = false;
            TXT_FECHA.Enabled = true;
            TXT_RAZON_DEVOL.ReadOnly = false;
            

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            using (SeleccionCustomers customers = new SeleccionCustomers()) 
            {
                customers.ShowDialog();
            };
        }
    }
}
