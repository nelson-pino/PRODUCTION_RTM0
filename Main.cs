using RitramaAPP.form;
using System.Windows.Forms;

namespace RitramaAPP
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "Recepciones":
                    panel1.Controls.Clear();
                    FrmMateriaPrima f = new FrmMateriaPrima();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;
                case "Pedidos Clientes":
                    panel1.Controls.Clear();
                    FrmPedidos fpedido = new FrmPedidos();
                    fpedido.TopLevel = false;
                    this.panel1.Controls.Add(fpedido);
                    fpedido.Dock = DockStyle.Fill;
                    fpedido.Show();
                    break;
                case "Ordenes Corte":
                    panel1.Controls.Clear();
                    FrmOrdenCorte fcorte = new FrmOrdenCorte();
                    fcorte.TopLevel = false;
                    this.panel1.Controls.Add(fcorte);
                    fcorte.Dock = DockStyle.Fill;
                    fcorte.Show();
                    break;
                case "Productos":
                    panel1.Controls.Clear();
                    FrmProducts fproducts = new FrmProducts();
                    fproducts.TopLevel = false;
                    this.panel1.Controls.Add(fproducts);
                    fproducts.Dock = DockStyle.Fill;
                    fproducts.Show();
                    break;
                case "Customers":
                    panel1.Controls.Clear();
                    FrmCustomers fcust = new FrmCustomers();
                    fcust.TopLevel = false;
                    this.panel1.Controls.Add(fcust);
                    fcust.Dock = DockStyle.Fill;
                    fcust.Show();
                    break;
                case "Proveedores":
                    panel1.Controls.Clear();
                    FrmSupply fsuply = new FrmSupply();
                    fsuply.TopLevel = false;
                    this.panel1.Controls.Add(fsuply);
                    fsuply.Dock = DockStyle.Fill;
                    fsuply.Show();
                    break;
                case "Modulo de Inventarios":
                    panel1.Controls.Clear();
                    FrmInventario finvent = new FrmInventario();
                    finvent.TopLevel = false;
                    this.panel1.Controls.Add(finvent);
                    finvent.Dock = DockStyle.Fill;
                    finvent.Show();
                    break;
                case "Parametros":
                    panel1.Controls.Clear();
                    FrmParameters formParam = new FrmParameters();
                    formParam.TopLevel = false;
                    this.panel1.Controls.Add(formParam);
                    formParam.Dock = DockStyle.Fill;
                    formParam.Show();
                    break;
                case "Despacho":
                    panel1.Controls.Clear();
                    FrmDespacho fDespachos = new FrmDespacho();
                    fDespachos.TopLevel = false;
                    this.panel1.Controls.Add(fDespachos);
                    fDespachos.Dock = DockStyle.Fill;
                    fDespachos.Show();
                    break;
            }
        }
    }
}
