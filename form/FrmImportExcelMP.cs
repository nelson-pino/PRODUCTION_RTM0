﻿using RitramaAPP.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace RitramaAPP.form
{
    public partial class FrmImportExcelMP : Form
    {
        public FrmImportExcelMP()
        {
            InitializeComponent();
        }
        string fileName;
        OpenFileDialog openFileDialog1;
        DataTable tbContainer;
        string strConn;
        DataView dv = new DataView();
        public List<ClassRecepcion> lista = new List<ClassRecepcion>();
        readonly RecepcionManager manager = new RecepcionManager();
        readonly ProduccionManager managerorden = new ProduccionManager();
        string filtro_codigo = "";
        string filtro_name = "";
        string filtro_id = "";
        Boolean ValidarData = false;
        private void FrmImportExcelMP_Load(object sender, EventArgs e)
        {

        }
        private void Bot_import_data_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();  //crea un Objeto openfileDialog. 
            openFileDialog1.Filter = "XML Files (*.xml; *.xls; *.xlsx; *.xlsm; *.xlsb) |*.xml; *.xls; *.xlsx; *.xlsm; *.xlsb";//open file format define Excel Files(.xls)|*.xls| Excel Files(.xlsx)|*.xlsx| 
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.Multiselect = false;        // no permite seleccionar multiples archivos
            openFileDialog1.Title = "Seleccione la hoja de excel a Exportar";   // define el nombre de la ventana
            openFileDialog1.InitialDirectory = @"Desktop"; // define el directorio inicial
            if (openFileDialog1.ShowDialog() == DialogResult.OK)        //executing when file open
            {
                string pathName = openFileDialog1.FileName;
                fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                tbContainer = new System.Data.DataTable();
                strConn = string.Empty;
                string sheetName = fileName;

                FileInfo file = new FileInfo(pathName);
                if (!file.Exists) { throw new Exception("Error, file doesn't exists!"); }
                string extension = file.Extension;
                switch (extension)
                {
                    case ".xls":
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'";
                        break;
                    case ".xlsx":
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;'";
                        break;
                    default:
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'";
                        break;
                }
                TXT_PATH_FILENAME.Text = file.FullName;
                TXT_NOMBRE_HOJA.Text = "Hoja1";
            }
        }
        private void LOAD_DATA_EXCEL()
        {
            try
            {
                OleDbConnection cnnxls = new OleDbConnection(strConn);
                OleDbCommand comando = new OleDbCommand("select * from [" + TXT_NOMBRE_HOJA.Text.Trim() + "$]", cnnxls);
                OleDbDataAdapter adapter = new OleDbDataAdapter(comando);
                adapter.Fill(tbContainer);
                DataRow dr = tbContainer.Rows[0];
                dr.Delete();
                tbContainer.AcceptChanges();
                if (!RA_PACKING.Checked)
                {
                    tbContainer.Columns.Add("Fila", typeof(System.Int32));
                    for (int i = 0; i <= tbContainer.Rows.Count - 1; i++)
                    {
                        tbContainer.Rows[i]["Fila"] = i + 2;
                    }
                }
                else
                {
                    DataRow dr1 = tbContainer.Rows[0];
                    dr1.Delete();
                    tbContainer.AcceptChanges();
                }
                dv = tbContainer.DefaultView;
                GRID_IMPORT.DataSource = dv;
                if (RA_MASTERS.Checked)
                {
                    LOADATA_MASTERS();
                    ValidarData = true;
                }
                if (RA_CORTADOS.Checked)
                {
                    LOADATA_CORTADOS();
                }
                if (RA_HOJAS.Checked)
                {
                    LOADATA_HOJAS();
                    //no tiene depuracion de data.
                    BOT_DEBUG_DATA.Enabled = false;
                }
                if (RA_GRAPHICS.Checked)
                {
                    LOADATA_GRAPHICS();
                }
                if (RA_PACKING.Checked)
                {
                    LOADATA_PACKING();
                }
                if (!RA_PACKING.Checked)
                {
                    GRID_IMPORT.Columns["Fila"].DisplayIndex = 0;
                }
                TXT_ROWS.Text = (tbContainer.Rows.Count).ToString();
                // si la busqueda es por rollo cortado
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!"+ex);
            }
        }
        private void BOT_LOAD_DATA_Click(object sender, EventArgs e)
        {
            LOAD_DATA_EXCEL();
        }
        private void LOADATA_MASTERS()
        {

            GRID_IMPORT.Columns[0].HeaderText = "Factura";
            GRID_IMPORT.Columns[1].HeaderText = "Fecha Llegada";
            GRID_IMPORT.Columns[2].HeaderText = "Codigo";
            GRID_IMPORT.Columns[3].HeaderText = "Descripcion";
            GRID_IMPORT.Columns[4].HeaderText = "ROLL ID";
            GRID_IMPORT.Columns[5].HeaderText = "Fecha Produccion";
            GRID_IMPORT.Columns[6].HeaderText = "# Rollo";
            GRID_IMPORT.Columns[7].HeaderText = "Ancho mm & Pulg.";
            GRID_IMPORT.Columns[8].HeaderText = "ML & Pies Lineales";
            GRID_IMPORT.Columns[9].HeaderText = "# Empalmes";
            GRID_IMPORT.Columns[10].HeaderText = "Inventario";
            GRID_IMPORT.Columns[11].HeaderText = "Fecha Salida";
            GRID_IMPORT.Columns[12].HeaderText = "Doc. Salida OC";
            GRID_IMPORT.Columns[13].HeaderText = "Notas";
            GRID_IMPORT.Columns[14].HeaderText = "Kilo Neto";
            GRID_IMPORT.Columns[15].HeaderText = "Kilo Bruto";
            GRID_IMPORT.Columns[16].HeaderText = "Almacen";
            GRID_IMPORT.Columns[17].HeaderText = "Fila";
            GRID_IMPORT.Columns[18].HeaderText = "Visto";
            GRID_IMPORT.Columns[19].HeaderText = "M2";
            GRID_IMPORT.Columns[20].HeaderText = "Count";
        }
        private void LOADATA_CORTADOS()
        {
            GRID_IMPORT.Columns["F1"].HeaderText = "Fecha OC.";
            GRID_IMPORT.Columns["F2"].HeaderText = "OC.";
            GRID_IMPORT.Columns["F3"].HeaderText = "# Rollo.";
            GRID_IMPORT.Columns["F4"].HeaderText = "Codigo";
            GRID_IMPORT.Columns["F5"].HeaderText = "Nombre del Producto";
            GRID_IMPORT.Columns["F6"].HeaderText = "Roll. ID";
            GRID_IMPORT.Columns["F7"].HeaderText = "Ancho [Pulg.]";
            GRID_IMPORT.Columns["F8"].HeaderText = "Largo [Pies]";
            GRID_IMPORT.Columns["F9"].HeaderText = "Msi.";
            GRID_IMPORT.Columns["F10"].HeaderText = "# Empalmes";
            GRID_IMPORT.Columns["F11"].HeaderText = "Code Perso.";
            GRID_IMPORT.Columns["F12"].HeaderText = "Codigo Unico";
            GRID_IMPORT.Columns["F13"].HeaderText = "Ratio Kilos";
            GRID_IMPORT.Columns["F14"].HeaderText = "Seccion";
            GRID_IMPORT.Columns["F15"].HeaderText = "Rack";
            GRID_IMPORT.Columns["F16"].HeaderText = "Inventario";
            GRID_IMPORT.Columns["F17"].HeaderText = "Fecha Salida";
            GRID_IMPORT.Columns["F18"].HeaderText = "Doc. Salida";
            GRID_IMPORT.Columns["F19"].HeaderText = "Cliente";
            GRID_IMPORT.Columns["F20"].HeaderText = "Comentario";
        }
        private void LOADATA_HOJAS()
        {
            GRID_IMPORT.Columns["F1"].HeaderText = "Num. Compra";
            GRID_IMPORT.Columns["F2"].HeaderText = "Fecha";
            GRID_IMPORT.Columns["F3"].HeaderText = "Id. Product.";
            GRID_IMPORT.Columns["F4"].HeaderText = "Nombre Producto";
            GRID_IMPORT.Columns["F5"].HeaderText = "Id Hojas";
            GRID_IMPORT.Columns["F6"].HeaderText = "Resmas x Paq.";
            GRID_IMPORT.Columns["F7"].HeaderText = "Ancho MM";
            GRID_IMPORT.Columns["F8"].HeaderText = "Largo MM";
            GRID_IMPORT.Columns["F9"].HeaderText = "M2";
            GRID_IMPORT.Columns["F10"].HeaderText = "Cantidad Hojas";
            GRID_IMPORT.Columns["F11"].HeaderText = "Kilos";
            GRID_IMPORT.Columns["F12"].HeaderText = "Kilos x Hojas";
            GRID_IMPORT.Columns["F13"].HeaderText = "Paleta";
            GRID_IMPORT.Columns["F14"].HeaderText = "Inventario";
            GRID_IMPORT.Columns["F15"].HeaderText = "Fecha de Salida";
            GRID_IMPORT.Columns["F16"].HeaderText = "Documento Salida";
            GRID_IMPORT.Columns["F17"].HeaderText = "Factura";
            GRID_IMPORT.Columns["F18"].HeaderText = "Cliente";
            GRID_IMPORT.Columns["F19"].HeaderText = "Territorio";
            GRID_IMPORT.Columns["F20"].HeaderText = "Almacen";
            GRID_IMPORT.Columns["F21"].HeaderText = "Fila";
        }
        private void LOADATA_GRAPHICS()
        {
            GRID_IMPORT.Columns["F1"].HeaderText = "Recepcion.";
            GRID_IMPORT.Columns["F2"].HeaderText = "Fecha Llegada.";
            GRID_IMPORT.Columns["F3"].HeaderText = "Codigo Producto";
            GRID_IMPORT.Columns["F4"].HeaderText = "Nombre del Producto";
            GRID_IMPORT.Columns["F5"].HeaderText = "Roll Id.";
            GRID_IMPORT.Columns["F6"].HeaderText = "Roll #";
            GRID_IMPORT.Columns["F7"].HeaderText = "Ancho MM";
            GRID_IMPORT.Columns["F8"].HeaderText = "Mts.";
            GRID_IMPORT.Columns["F9"].HeaderText = "M2";
            GRID_IMPORT.Columns["F10"].HeaderText = "Kilos";
            GRID_IMPORT.Columns["F11"].HeaderText = "Paleta";
            GRID_IMPORT.Columns["F12"].HeaderText = "Almacen";
            GRID_IMPORT.Columns["F13"].HeaderText = "Fila";
            GRID_IMPORT.Columns["F14"].HeaderText = "Fecha Salida";
            GRID_IMPORT.Columns["F15"].HeaderText = "Documento Salida";
            GRID_IMPORT.Columns["F16"].HeaderText = "Factura";
            GRID_IMPORT.Columns["F17"].HeaderText = "Inventario";
            GRID_IMPORT.Columns["F18"].HeaderText = "Cliente";
            GRID_IMPORT.Columns["F19"].HeaderText = "Territorio";
            GRID_IMPORT.Columns["F20"].HeaderText = "Nota";
            GRID_IMPORT.Columns["F21"].HeaderText = "Cuenta";
        }
        private void LOADATA_PACKING()
        {
            GRID_IMPORT.Columns["F1"].HeaderText = "Fila";
            GRID_IMPORT.Columns["F2"].HeaderText = "Master";
            GRID_IMPORT.Columns["F3"].HeaderText = "Hojas";
            GRID_IMPORT.Columns["F4"].HeaderText = "Graphics";
            GRID_IMPORT.Columns["F5"].HeaderText = "Numero Embarque";
            GRID_IMPORT.Columns["F6"].HeaderText = "Fecha Recepcion";
            GRID_IMPORT.Columns["F7"].HeaderText = "Fecha Producccion";
            GRID_IMPORT.Columns["F8"].HeaderText = "RollId";
            GRID_IMPORT.Columns["F9"].HeaderText = "Codigo Proveedor";
            GRID_IMPORT.Columns["F10"].HeaderText = "Nombre del Proveedor";
            GRID_IMPORT.Columns["F11"].HeaderText = "Codigo Producto";
            GRID_IMPORT.Columns["F12"].HeaderText = "Nombre del Producto";
            GRID_IMPORT.Columns["F13"].HeaderText = "Width [Pulg.]";
            GRID_IMPORT.Columns["F14"].HeaderText = "Lenght [Pies]";
            GRID_IMPORT.Columns["F15"].HeaderText = "Splice";
            GRID_IMPORT.Columns["F16"].HeaderText = "Core";
            GRID_IMPORT.Columns["F17"].HeaderText = "Ubicacion";
            GRID_IMPORT.Columns["F18"].HeaderText = "Hojas Id";
            GRID_IMPORT.Columns["F19"].HeaderText = "Resmas x Paq.";
            GRID_IMPORT.Columns["F20"].HeaderText = "Numero Hojas";
            GRID_IMPORT.Columns["F21"].HeaderText = "Palet Number";
            GRID_IMPORT.Columns["F22"].HeaderText = "Ancho MM";
            GRID_IMPORT.Columns["F23"].HeaderText = "Largo MM";
            GRID_IMPORT.Columns["F24"].HeaderText = "Roll-ID";
            GRID_IMPORT.Columns["F25"].HeaderText = "Ancho MM";
            GRID_IMPORT.Columns["F26"].HeaderText = "M2";
            GRID_IMPORT.Columns["F27"].HeaderText = "Palet Number";
        }
        private void DEBUG_MASTERS()
        {
            double numero = 0;
            string cadena = "";
            string numerostr = "";
            foreach (DataRow row in tbContainer.Rows)
            {
                //Columna de Ancho.
                cadena = Convert.ToString(row["F8"]);
                numerostr = "";
                foreach (char str in cadena)
                {
                    if (!char.IsLetter(str))
                        numerostr += str.ToString();
                }
                if (Convert.ToString(row["F8"]).Contains("mm"))
                {
                    numero = Math.Round(double.Parse(numerostr,
                         CultureInfo.InvariantCulture)
                        * R.CONSTANTES.FACTOR_MM_PULGADAS, 4,
                        MidpointRounding.AwayFromZero);
                }
                else
                {
                    numero = double.Parse(numerostr,
                         CultureInfo.InvariantCulture);
                }
                row["F8"] = Convert.ToString(numero);
                GRID_IMPORT.Columns[7].HeaderText = "Ancho [PULGADAS]";

                //Columna de Largo.
                cadena = Convert.ToString(row["F9"]);
                numerostr = "";
                foreach (char str in cadena)
                {
                    if (!char.IsLetter(str))
                        numerostr += str.ToString();
                }
                if (Convert.ToString(row["F9"]).Contains("Mts"))
                {

                    numero = Math.Round(double.Parse(numerostr,
                         CultureInfo.InvariantCulture)
                        * R.CONSTANTES.FACTOR_METROS_PIES, 4,
                        MidpointRounding.AwayFromZero);
                }
                else
                {
                    numero = double.Parse(numerostr,
                         CultureInfo.InvariantCulture);
                }
                row["F9"] = Convert.ToString(numero);
                GRID_IMPORT.Columns[8].HeaderText = "Largo [PIES]";

                //Columna splice.
                if (Convert.ToString(row["F10"]) == "")
                {
                    row["F10"] = 0;
                }
            }
            tbContainer.AcceptChanges();
            MessageBox.Show("Proceso de Validacion realizado con exito.");
            TXT_LOG_PROCESS.Text += "Conversiones de Ancho y Largo realizadas con exito...";
            ValidarData = false;
        }
        private void DEBUG_CORTADOS()
        {

        }
        private void SAVEDATA_MASTERS()
        {
            int ord = 1;
            foreach (DataGridViewRow row in GRID_IMPORT.Rows)
            {
                ClassRecepcion data = new ClassRecepcion
                {
                    Orden = ord.ToString(),
                    Embarque = "inimas",
                    Fecha_recepcion = DateTime.Today,
                    Fecha_produccion = DateTime.Today,
                    Fecha_reg = DateTime.Today,
                    Hora_reg = DateTime.Now.ToShortTimeString(),
                    Roll_ID = Convert.ToString(row.Cells["F5"].Value),
                    Supply_Id = "999",
                    Part_Number = Convert.ToString(row.Cells["F3"].Value),
                    Master = true,
                    Resma = false,
                    Graphics = false,
                    Palet_number = "0",
                    Palet_cant = 0,
                    Palet_paginas = 0,
                    Width = double.Parse(Convert.ToString(row.Cells["F8"].Value),
                    System.Globalization.NumberStyles.AllowDecimalPoint),
                    Lenght = Convert.ToDouble(row.Cells["F9"].Value),
                    Width_metros = Convert.ToDouble(row.Cells["F8"].Value) *
                    R.CONSTANTES.FACTOR_PULGADAS_METROS,
                    Lenght_metros = Convert.ToDouble(row.Cells["F9"].Value) *
                    R.CONSTANTES.FACTOR_PIES_METROS,
                    Splice = Convert.ToInt16(row.Cells["F10"].Value),
                    Core = 0,
                    Ubicacion = "",
                    Anulado = false,
                    Disponible = true
                };
                ord++;
                lista.Add(data);
            }
            foreach (ClassRecepcion item in lista)
            {
                manager.Add(item, false, 2);
            }
            MessageBox.Show("Se enviaron los datos correctamente...");
        }
        private void SAVEDATA_CORTADOS()
        {
            int OrdenCorte = 1;
            Orden ocorte = new Orden
            {
                Numero = OrdenCorte.ToString(),
                Fecha = DateTime.Today,
                Fecha_produccion = DateTime.Today,
                Product_id = Convert.ToString(GRID_IMPORT.Rows[0].Cells["F4"].Value),
                Rollid_1 = "1",
                Rollid_2 = "2",
                Width_1 = 0,
                Width_2 = 0,
                Lenght_1 = 0,
                Rest1_width = 0,
                Rest2_width = 0,
                Rest1_lenght = 0,
                Rest2_lenght = 0,
                Cantidad_Rollos = 0,
                Cantidad_Rollos2 = 0,
                Util1_real_Lenght = 0,
                Util2_real_Lenght = 0,
                Util1_Real_Width = 0,
                Util2_Real_Width = 0,
                Anulada = false,
                Procesado = false,
                Status = 0
            };
            ocorte.rollos = new List<Roll_Details>();
            ocorte.Cortes = new List<Corte>();
            Corte c = new Corte();
            c.Num = 0;
            c.Msi = 0;
            c.Width = 0;
            c.Lenght = 0;
            c.Orden = Convert.ToString(OrdenCorte);
            ocorte.Cortes.Add(c);
            int fil = 1;
            foreach (DataGridViewRow row in GRID_IMPORT.Rows)
            {
                Roll_Details data = new Roll_Details
                {
                    Fecha = DateTime.Today,
                    Numero_Orden = Convert.ToString(OrdenCorte),
                    Product_id = Convert.ToString(row.Cells["F4"].Value) + "0",
                    Product_name = Convert.ToString(GRID_IMPORT.Rows[fil - 1].Cells["F5"].Value),
                    Roll_number = Convert.ToString(fil),
                    Code_Person = Convert.ToString(row.Cells["F11"].Value),
                    Unique_code = Convert.ToString(row.Cells["F12"].Value),
                    Width = Convert.ToDecimal(row.Cells["F7"].Value),
                    Large = Convert.ToDecimal(row.Cells["F8"].Value),
                    Msi = Convert.ToDecimal(row.Cells["F9"].Value),
                    Roll_id = Convert.ToString(row.Cells["F6"].Value),
                    Splice = Convert.ToInt16(row.Cells["F10"].Value),
                    Status = "Ok",
                    Disponible = true
                };
                fil += 1;
                OrdenCorte++;
                ocorte.rollos.Add(data);

            }
            managerorden.AddRolls(ocorte, false);
            MessageBox.Show("se guardaron los datos correctamente en el servidor...");
        }
        private void SAVEDATA_HOJAS()
        {
            //if (!VALIDAR_NUMERO_CONSECUTIVO()) 
            //{
            //    return;
            //}
            int ord = 1;
            foreach (DataGridViewRow row in GRID_IMPORT.Rows)
            {
                ClassRecepcion data = new ClassRecepcion
                {
                    Orden = ord.ToString(),
                    Embarque = "inihoj",
                    Fecha_recepcion = DateTime.Today,
                    Fecha_produccion = DateTime.Today,
                    Fecha_reg = DateTime.Today,
                    Hora_reg = DateTime.Now.ToShortTimeString(),
                    Roll_ID = Convert.ToString(row.Cells["F5"].Value),
                    Supply_Id = "999",
                    SupplyName = "",
                    Part_Number = Convert.ToString(row.Cells["F3"].Value),
                    ProductName = Convert.ToString(row.Cells["F4"].Value),
                    Master = false,
                    Resma = true,
                    Graphics = false,
                    Palet_number = Convert.ToString(row.Cells["F13"].Value),
                    Palet_cant = Convert.ToDecimal(row.Cells["F6"].Value),
                    Palet_paginas = Int32.Parse(Convert.ToString(row.Cells["F10"].Value), System.Globalization.NumberStyles.Any),
                    Width = Convert.ToDouble(row.Cells["F8"].Value),
                    Lenght = Convert.ToDouble(row.Cells["F9"].Value),
                    Width_metros = 0,
                    Lenght_metros = 0,
                    Splice = 0,
                    Core = 0,
                    Ubicacion = "",
                    Tipo = "",
                    Unidad = "",
                    Anulado = false,
                    Disponible = true
                };
                ord++;
                lista.Add(data);
            }
            foreach (ClassRecepcion item in lista)
            {
                manager.Add(item, false, 3);
            }
            MessageBox.Show("Se enviaron los datos correctamente al servidor...");
        }
        private void SAVEDATA_GRAPHICS()
        {
            //if (!VALIDAR_NUMERO_CONSECUTIVO())
            //{
            //    return;
            //}
            int ord = 1;
            foreach (DataGridViewRow row in GRID_IMPORT.Rows)
            {
                ClassRecepcion data = new ClassRecepcion
                {
                    Orden = ord.ToString(),
                    Embarque = Convert.ToString(row.Cells["F1"].Value),
                    Fecha_recepcion = DateTime.Today,
                    Fecha_produccion = DateTime.Today,
                    Fecha_reg = DateTime.Today,
                    Hora_reg = DateTime.Now.ToShortTimeString(),
                    Roll_ID = Convert.ToString(row.Cells["F5"].Value),
                    Supply_Id = "999",
                    SupplyName = "",
                    Part_Number = Convert.ToString(row.Cells["F3"].Value),
                    ProductName = "",
                    Master = false,
                    Resma = false,
                    Graphics = true,
                    Palet_number = Convert.ToString(row.Cells["F11"].Value),
                    Palet_cant = 1,
                    Palet_paginas = 1,
                    Width = Convert.ToDouble(row.Cells["F7"].Value),
                    Lenght = Convert.ToDouble(row.Cells["F9"].Value),
                    Width_metros = 0,
                    Lenght_metros = 0,
                    Splice = 0,
                    Core = 0,
                    Ubicacion = "",
                    Tipo = "",
                    Unidad = "",
                    Anulado = false,
                    Disponible = true
                };
                ord++;
                lista.Add(data);
            }
            foreach (ClassRecepcion item in lista)
            {
                manager.Add(item, false, 4);
            }
            MessageBox.Show("Se enviaron los datos correctamente al servidor...");
        }
        private void SAVEDATA_PACKING()
        {
            foreach (DataGridViewRow row in GRID_IMPORT.Rows)
            {
                if (Convert.ToString(row.Cells["F2"].Value) == "1")
                {
                    //master.
                    ClassRecepcion data = new ClassRecepcion
                    {
                        Orden = Convert.ToString(manager.MaxConsecOrden()),
                        Embarque = Convert.ToString(row.Cells["F5"].Value),
                        Fecha_recepcion = Convert.ToDateTime(row.Cells["F6"].Value),
                        Fecha_produccion = Convert.ToDateTime(row.Cells["F7"].Value),
                        Fecha_reg = DateTime.Today,
                        Hora_reg = DateTime.Now.ToShortTimeString(),
                        Roll_ID = Convert.ToString(row.Cells["F8"].Value),
                        Supply_Id = Convert.ToString(row.Cells["F9"].Value),
                        Part_Number = Convert.ToString(row.Cells["F11"].Value),
                        Master = true,
                        Resma = false,
                        Graphics = false,
                        Palet_number = "0",
                        Palet_cant = 0,
                        Palet_paginas = 0,
                        Width = double.Parse(Convert.ToString(row.Cells["F13"].Value),
                        System.Globalization.NumberStyles.AllowDecimalPoint),
                        Lenght = Convert.ToDouble(row.Cells["F14"].Value),
                        Width_metros = Convert.ToDouble(row.Cells["F13"].Value) *
                        R.CONSTANTES.FACTOR_PULGADAS_METROS,
                        Lenght_metros = Convert.ToDouble(row.Cells["F14"].Value) *
                        R.CONSTANTES.FACTOR_PIES_METROS,
                        Splice = Convert.ToInt16(row.Cells["F15"].Value),
                        Core = Convert.ToDecimal(row.Cells["F16"].Value),
                        Ubicacion = Convert.ToString(row.Cells["F17"].Value),
                        Anulado = false,
                        Disponible = true
                    };
                    manager.Add(data, false, 1);
                }
                if (Convert.ToString(row.Cells["F3"].Value) == "1")
                {
                    //hojas. 
                    ClassRecepcion data = new ClassRecepcion
                    {
                        Orden = Convert.ToString(manager.MaxConsecOrden()),
                        Embarque = Convert.ToString(row.Cells["F5"].Value),
                        Fecha_recepcion = Convert.ToDateTime(row.Cells["F6"].Value),
                        Fecha_produccion = Convert.ToDateTime(row.Cells["F7"].Value),
                        Fecha_reg = DateTime.Today,
                        Hora_reg = DateTime.Now.ToShortTimeString(),
                        Roll_ID = Convert.ToString(row.Cells["F18"].Value),
                        Supply_Id = Convert.ToString(row.Cells["F9"].Value),
                        SupplyName = "",
                        Part_Number = Convert.ToString(row.Cells["F11"].Value),
                        Master = false,
                        Resma = true,
                        Graphics = false,
                        Palet_number = Convert.ToString(row.Cells["F21"].Value),
                        Palet_cant = Convert.ToDecimal(row.Cells["F19"].Value),
                        Palet_paginas = Int32.Parse(Convert.ToString(row.Cells["F20"].Value),
                        System.Globalization.NumberStyles.Any),
                        Width = Convert.ToDouble(row.Cells["F22"].Value),
                        Lenght = Convert.ToDouble(row.Cells["F23"].Value),
                        Width_metros = 0,
                        Lenght_metros = 0,
                        Splice = 0,
                        Core = 0,
                        Ubicacion = Convert.ToString(row.Cells["F17"].Value),
                        Tipo = "",
                        Unidad = "",
                        Anulado = false,
                        Disponible = true
                    };
                    manager.Add(data, false, 1);
                }
                if (Convert.ToString(row.Cells["F4"].Value) == "1")
                {
                    //graphics.  
                    ClassRecepcion data = new ClassRecepcion
                    {
                        Orden = Convert.ToString(manager.MaxConsecOrden()),
                        Embarque = Convert.ToString(row.Cells["F5"].Value),
                        Fecha_recepcion = Convert.ToDateTime(row.Cells["F6"].Value),
                        Fecha_produccion = Convert.ToDateTime(row.Cells["F7"].Value),
                        Fecha_reg = DateTime.Today,
                        Hora_reg = DateTime.Now.ToShortTimeString(),
                        Roll_ID = Convert.ToString(row.Cells["F24"].Value),
                        Supply_Id = Convert.ToString(row.Cells["F9"].Value),
                        SupplyName = "",
                        Part_Number = Convert.ToString(row.Cells["F11"].Value),
                        Master = false,
                        Resma = false,
                        Graphics = true,
                        Palet_number = Convert.ToString(row.Cells["F27"].Value),
                        Palet_cant = 0,
                        Palet_paginas = 0,
                        Width = Convert.ToDouble(row.Cells["F25"].Value),
                        Lenght = Convert.ToDouble(row.Cells["F26"].Value),
                        Width_metros = 0,
                        Lenght_metros = 0,
                        Splice = 0,
                        Core = 0,
                        Ubicacion = Convert.ToString(row.Cells["F17"].Value),
                        Tipo = "",
                        Unidad = "",
                        Anulado = false,
                        Disponible = true
                    };
                    manager.Add(data, false, 1);
                }
            }
            MessageBox.Show("Se enviaron los datos correctamentes al servidor...");
        }
        private void SearchData()
        {
            if (RA_MASTERS.Checked || RA_HOJAS.Checked)
            {
                filtro_codigo = "F3 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_name = "F4 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_id = "F5 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
            }
            if (RA_CORTADOS.Checked)
            {
                filtro_codigo = "F4 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_name = "F5 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_id = "F6 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
            }
            if (RA_GRAPHICS.Checked)
            {
                filtro_codigo = "F3 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_name = "F4 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_id = "F5 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
            }
            if (RA_PACKING.Checked)
            {
                filtro_codigo = "F12 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_name = "F13 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
                filtro_id = "F9 LIKE '%" + this.TXT_BUSCAR.Text + "%'";
            }
            if (RA_CODIGO.Checked)
            {
                dv.RowFilter = filtro_codigo;
            }
            if (RA_PRODUCTNAME.Checked)
            {
                dv.RowFilter = filtro_name;
            }
            if (RA_ROLLID.Checked)
            {
                dv.RowFilter = filtro_id;
            }
            ROWS_FOUND.Text = Convert.ToString(dv.Count) + " encontrados.";
        }
        private void GRID_IMPORT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void bot_borrar_data_Click(object sender, EventArgs e)
        {
            GRID_IMPORT.DataSource = "";
        }
        private void bot_convertir_Click(object sender, EventArgs e)
        {
            if (RA_MASTERS.Checked)
            {
                DEBUG_MASTERS();
            }
            if (RA_CORTADOS.Checked)
            {
                DEBUG_CORTADOS();
            }
        }
        private void BOT_SEND_DATA_Click(object sender, EventArgs e)
        {
            if (RA_MASTERS.Checked)
            {
                if (ValidarData == true)
                {
                    MessageBox.Show("debe validar la data de los master primero...");
                    return;
                }
                SAVEDATA_MASTERS();
            }
            if (RA_CORTADOS.Checked)
            {
                SAVEDATA_CORTADOS();
            }
            if (RA_HOJAS.Checked)
            {
                SAVEDATA_HOJAS();
            }
            if (RA_GRAPHICS.Checked)
            {
                SAVEDATA_GRAPHICS();
            }
            if (RA_PACKING.Checked)
            {
                SAVEDATA_PACKING();
            }
        }
        private void RA_HOJAS_CheckedChanged(object sender, EventArgs e)
        {
            if (RA_HOJAS.Checked)
            {
                RA_HOJAS.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            }
            else
            {
                RA_HOJAS.Font = new Font("Tahoma", 8.25F, FontStyle.Regular);
            }

        }
        private void RA_GRAPHICS_CheckedChanged(object sender, EventArgs e)
        {
            if (RA_GRAPHICS.Checked)
            {
                RA_GRAPHICS.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            }
            else
            {
                RA_GRAPHICS.Font = new Font("Tahoma", 8.25F, FontStyle.Regular);
            }
        }
        private void bot_buscar_Click(object sender, EventArgs e)
        {
            SearchData();
        }
        private void TXT_BUSCAR_TextChanged(object sender, EventArgs e)
        {
            if (TXT_BUSCAR.Text.Length == 0)
            {
                dv.RowFilter = "";
                ROWS_FOUND.Text = " 0 encontrados.";
            }
        }
        private Boolean VALIDAR_NUMERO_CONSECUTIVO()
        {
            if (TXT_NUMDOC.Text.Length == 0)
            {
                MessageBox.Show("Debe introducir el numero inicial de los documentos.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
