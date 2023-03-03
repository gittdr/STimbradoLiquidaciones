using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using System.Data;
using System.Data.SqlClient;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Globalization;
using System.Web.UI.HtmlControls;
using CARGAR_EXCEL.Models;
using System.Collections;
using System.Web.Services;
using RestSharp;
using System.Net;
using System.Text.RegularExpressions;
using iTextSharp.text.html;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;
using System.Web.DynamicData;
using static NPOI.HSSF.Util.HSSFColor;
using System.Security.Cryptography;
using Tamir.SharpSsh.jsch.jce;

namespace CARGAR_EXCEL
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        static storedProcedure sql = new storedProcedure("miConexion");
        public static FacLabControler facLabControler = new FacLabControler();
        public static string jsonFactura = "", idSucursal = "", idTipoFactura = "", IdApiEmpresa = "";
        public string leg;
        public static List<string> result = new List<string>();
        static string Fecha;
        static string Subtotal;
        static string Totalimptrasl;
        static string Totalimpreten;
        static string Descuentos;
        static string Total;
        static string FormaPago;
        static string Condipago;
        static string MetodoPago;
        static string Moneda;
        static string RFC;
        static string CodSAT;
        static string IdProducto;
        static string Producto;
        static string Origen;
        static string Destino;
        private string _ConnectionString;
        public decimal total_liq;
        public decimal suma_total;
        public int contador;
        public string r1;
        public string m1;
        public string r2;
        public string m2;
        public string r3;
        public string m3;
        public string r4;
        public string m4;
        public string r5;
        public string m5;
        public string r6;
        public string m6;
        public string r7;
        public string m7;
        public string r8;
        public string m8;
        public string r9;
        public string m9;
        public string r10;
        public string m10;
        public string r11;
        public string m11;
        public string r12;
        public string m12;
        public string r13;
        public string m13;
        public string r14;
        public string m14;
        public string r15;
        public string m15;
        public string asgn_id;
        public string dr;

        public static List<string> results = new List<string>();
        static HtmlTable table = new HtmlTable();

        static char[] caracter = { '|' };
        static string[] words;
        protected void Page_Load(object sender, EventArgs e)
        {
            //string numero = "1424566";
            if (!IsPostBack)
            {
               
                f1.Visible= true;
                f2.Visible = true;
                bn1.Visible = true;
                Button1.Visible = true;
                Button2.Visible = false;
                tb1.Visible = false;
                tb2.Visible = false;
                nt.Visible = false;
                bn.Visible = false;
                //GetOperador2();
                LlenarDropDownList();
                GN();
                //GetOperador();
            }
            
            
            
        }

        protected void GN()
        {
            DataTable resa = facLabControler.GetCalendar();
            //                    //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
            if (resa.Rows.Count > 0)
            {
                foreach (DataRow gsegta in resa.Rows)
                {
                    string psh_name = gsegta["psh_name"].ToString();
                    RCalendario.Text = gsegta["psh_id"].ToString() + " | "+ psh_name;
                    
                }
            }
        }
        protected WebForm1()
        {
            this._ConnectionString = new Connection().connectionString;
        }
        protected void LlenarDropDownList()
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 10 psd_id as psd_id,CONVERT(varchar,psd_date,23) as psd_date FROM payschedulesdetail WHERE psd_date not in (SELECT psd_date FROM rgnomina) AND psh_id = 101 ORDER BY psd_id DESC", Conexion.Open());
            //SqlCommand cmd = new SqlCommand("SELECT TOP 10 p.psd_id as psd_id,CONVERT(varchar,p.psd_date,23) as psd_date FROM payschedulesdetail as p LEFT JOIN rgnomina as r ON p.psd_date != r.psd_date WHERE p.psh_id = 101 ORDER BY p.psd_id DESC", Conexion.Open());
            //SqlCommand cmd = new SqlCommand("SELECT TOP 10 p.psd_id as psd_id,CONVERT(varchar,p.psd_date,23) as psd_date FROM payschedulesdetail as p INNER JOIN rgnomina as r ON p.psd_date != r.psd_date WHERE p.psh_id = 101 ORDER BY p.psd_id DESC", Conexion.Open());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            Fechas.DataSource = ds.Tables[0];
            Fechas.DataTextField = ds.Tables[0].Columns["psd_date"].ColumnName.ToString();
            Fechas.DataValueField = ds.Tables[0].Columns["psd_id"].ColumnName.ToString();
            Fechas.DataBind();
            //Fechas.Items.Insert(0, new ListItem("<Selecciona Fecha>", "0"));


        }
        //protected void GetOperador2()
        //{
        //    //Paso 1 creo la tabla
        //    DataTable resax = facLabControler.GetCodigo();
        //    if (resax.Rows.Count > 0)
        //    {
        //        int conta = resax.Rows.Count;
        //        int i = 1;
        //        foreach (DataRow items in resax.Rows)
        //        {
        //            string codigo = items["codigo"].ToString().Trim();

        //                if (i == conta)
        //                {
        //                    dr += "[" + codigo + "] " + "varchar(100) NULL DEFAULT '0.00'";
        //                }
        //                else
        //                {
        //                    dr += "[" + codigo + "]"  + "varchar(100) NULL DEFAULT '0.00',";
        //                }

        //            i++;
        //        }
        //        string str = "CREATE TABLE nTestTable" +
        //                       "(id_num int NOT NULL IDENTITY(1,1) PRIMARY KEY,asgn_id varchar(100)," +
        //                       dr +
        //                       ")";
                

        //    }
        //    //Paso 2 Ingreso los operadores
        //    DataTable resa = facLabControler.GetOperador();
        //    if (resa.Rows.Count > 0)
        //    {
        //        foreach (DataRow gsegta in resa.Rows)
        //        {

        //            string rasg = gsegta["asgn_id"].ToString();
        //            asgn_id = rasg.Replace("0", "");
        //            facLabControler.Ioperador(asgn_id);


        //        }
        //    }
        //    //Paso3 
        //    DataTable resaxz = facLabControler.GetCodigo();
        //    if (resaxz.Rows.Count > 0)
        //    {
        //        foreach (DataRow gsegtax in resaxz.Rows)
        //        {
        //            string codigo = gsegtax["codigo"].ToString().Trim();
        //            DataTable resaxza = facLabControler.GetOCodigo(codigo);
        //            if (resaxza.Rows.Count > 0)
        //            {
        //                foreach (DataRow gsegtaxz in resaxza.Rows)
        //                {
        //                    asgn_id = gsegtaxz["asgn_id"].ToString();
        //                    decimal mm1 = Convert.ToDecimal(gsegtaxz["monto"]);
        //                    string monto = mm1.ToString("F");
        //                    facLabControler.Uoperador(codigo,asgn_id,monto);
        //                }
        //            }
        //        }
        //        //AQUI REGISTRO LA FECHA PARA QUE DESAPARESCA DEL LISTADO
        //        facLabControler.Rgnomina(fecha);
        //        card3.Visible = false;
        //        string msg = "Nomina Generada";
        //        //Rcartaporte.Value = msg;
        //        ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Nomina generada', 'success');setTimeout(function(){window.location.href ='Descargar.aspx'}, 7000)", true);

        //    }





        //}
        
        protected void TMF(string numero)
        {
            DirectoryInfo di24a = new DirectoryInfo(@"D:\Administracion\Respaldo de las app de TDR\Administración\Proyecto TimbradoFacturasMasiva");

            FileInfo[] files24a = di24a.GetFiles("*.tsv");


            int cantidad24a = files24a.Length;
            if (cantidad24a > 0)
            {
                foreach (var itema in files24a)
                {
                    string sourceFile = @"D:\Administracion\Respaldo de las app de TDR\Administración\Proyecto TimbradoFacturasMasiva\" + itema.Name;
                    string[] strAllLines = File.ReadAllLines(sourceFile, Encoding.UTF8);
                    File.WriteAllLines(sourceFile, strAllLines.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
                    string[] lineas1 = File.ReadAllLines(sourceFile, Encoding.UTF8);
                    lineas1 = lineas1.Skip(1).ToArray();
                    foreach (string line in lineas1)
                    {
                        string renglones = line;
                        char delimitador = '\t';
                        string[] valores = renglones.Split(delimitador);
                        string col1 = valores[0].ToString();
                        string col2 = valores[1].ToString();

                        if (col1 != "" || col2 != "")
                        {
                            int segm = Int32.Parse(col1);
                            var request28196 = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/bf2e1036-ba47-49a0-8cd9-e04b36d5afd4/cfdis?folioEspecifico=" + segm);
                            var response28196 = (HttpWebResponse)request28196.GetResponse();
                            var responseString28196 = new StreamReader(response28196.GetResponseStream()).ReadToEnd();
                            List<ModelFact> separados819 = JsonConvert.DeserializeObject<List<ModelFact>>(responseString28196);
                            //PASO 2 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
                            if (separados819 != null)
                            {
                                foreach (var rlist in separados819)
                                {
                                    string serie = rlist.serie;
                                    if (serie != "TDRZP") {
                                        valida(numero, col1, col2);
                                    }
                                }
                            }

                                
                        }


                    }
                    string destinationFile = @"D:\Administracion\Respaldo de las app de TDR\Administración\Proyecto TimbradoFacturasMasiva\Procesadas\" + itema.Name;
                    System.IO.File.Move(sourceFile, destinationFile);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            string OFecha = Fechas.SelectedItem.ToString();
            facLabControler.spLiquidacionesNomina(OFecha);

            //Obtenemos el total_liq
            DataTable resa = facLabControler.Stl_liq_detalle_total();
            if (resa.Rows.Count > 0)
            {
                foreach (DataRow gsegta in resa.Rows)
                {
                    decimal rest = Decimal.Parse(gsegta["total_liq"].ToString());
                    total_liq = decimal.Round(rest, 2, MidpointRounding.AwayFromZero);
                }
            }
            //Obtenemos el stl_liq_detalle_total
            DataTable resas = facLabControler.Stl_liq_detalle2_total();
            if (resas.Rows.Count > 0)
            {
                foreach (DataRow gsegtas in resas.Rows)
                {
                    decimal rest = Decimal.Parse(gsegtas["suma_total"].ToString());
                    suma_total = decimal.Round(rest, 2, MidpointRounding.AwayFromZero);
                }
            }
            decimal r = total_liq;
            decimal rs = suma_total;
            
            if (r == rs)
            {
                Td.Text = "$" + r.ToString();
                Rtd.Text = "$" + rs.ToString();
                GetCat();
                Button2.Visible = true;
            }
            else
            {
                f1.Visible = false;
                f2.Visible = false;
                nt.Visible = true;
                bn.Visible = true;
                bn1.Visible = false;
                Button2.Visible = false;
            }
            




            //var client = new RestClient("https://jsonplaceholder.typicode.com/posts");
            //var request = new RestRequest(Method.GET);

            ////request.AddHeader("cache-control", "no-cache");

            ////request.AddHeader("content-length", "834");
            ////request.AddHeader("accept-encoding", "gzip, deflate");
            ////request.AddHeader("Host", "canal1.xsa.com.mx:9050");
            ////request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
            ////request.AddHeader("Cache-Control", "no-cache");
            ////request.AddHeader("Accept", "*/*");
            ////request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");


            ////request.AddParameter("application/json", jsonFactura, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

            //string respuesta = response.StatusCode.ToString();
            //if (respuesta != "BadRequest")
            //{

            //}
            //string numero = "1424566";
            //string consecutivo = "1250001";
            //string refencia = "";
            //DataTable rorder = facLabControler.SelectLegHeaderZp(numero);

            //if (rorder.Rows.Count > 0)
            //{
            //    foreach (DataRow reslo in rorder.Rows)
            //    {

            //        DateTime dt = DateTime.Parse(reslo["Fecha"].ToString());
            //        string rfecha = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");

            //        DataTable rordera = facLabControler.SelectInvoiceHeader(numero);
            //        if (rordera.Rows.Count > 0)
            //        {
            //            foreach (DataRow reslos in rordera.Rows)
            //            {
            //                string ivnumber = reslos["ivh_invoicenumber"].ToString();
            //                facLabControler.InvoiceHeader(ivnumber, rfecha);
            //            }
            //        }


            //    }
            //}

            //string merror = "<br> <br>";
            //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert("+merror+");", true);
            //string msg = "pariatur?";
            //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Factura timbrada ', 'success');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
            //pop(numero);



            //valida(numero, consecutivo);
        }
        private void GetCat()
        {
            tb1.Visible = true;
            tb2.Visible = true;
            DataTable cargaStops = facLabControler.Stl_liq_detalle_all_total();
            //cargaStops.AsDataView().RowFilter("");
            int numCells = 6;
            int rownum = 0;
            //cargaStops = cargaStops.Orde
            foreach (DataRow row in cargaStops.Rows)
            {
                TableRow r = new TableRow();
                for (int i = 0; i < numCells; i++)
                {
                    if (i == 0)
                    {


                        HyperLink hp1 = new HyperLink();

                        hp1.ID = "hpIndex" + rownum.ToString();
                        //hp1.Text = "<i class='fa fa-minus-square btn btn-danger' aria-hidden='true'></i>";
                        //hp1.NavigateUrl = "DeleteR.aspx?idnum=" + row[i].ToString();
                        hp1.Text = "<button type='button' class='btn btn-primary'>" + row[i].ToString() + "</button>";
                        //hp1.NavigateUrl = "DetallesComplemento.aspx?factura=" + row[i].ToString();
                        TableCell c = new TableCell();
                        c.Controls.Add(hp1);
                        r.Cells.Add(c);

                    }
                    else
                    {
                        TableCell c = new TableCell();
                        c.Controls.Add(new LiteralControl("row "
                            + rownum.ToString() + ", cell " + i.ToString()));
                        c.Text = row[i].ToString();
                        r.Cells.Add(c);
                    }
                }


                tablaStops.Rows.Add(r);
                rownum++;

            }

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string OFecha = Fechas.SelectedItem.ToString();
            Response.Redirect("Procesar.aspx?fecha=" + OFecha, false);
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("WebForm1.aspx", false);
        }

        //protected void GetOperador()
        //{
            
        //    DataTable resa = facLabControler.GetOperador();
        //    if (resa.Rows.Count > 0)
        //    {
        //        foreach (DataRow gsegta in resa.Rows)
        //        {
        //            asgn_id = gsegta["asgn_id"].ToString();
        //            DataTable resar = facLabControler.GetOperadorDetalle(asgn_id);
        //            if (resar.Rows.Count > 0)
        //            {
        //                int ct = 1;
        //                foreach (DataRow gsegtar in resar.Rows)
        //                {
        //                    contador = resar.Rows.Count;
                            
        //                    if (ct <= contador)
        //                    {
        //                        switch (ct)
        //                        {
        //                            case 1:
        //                                 r1 = gsegtar["codigo"].ToString();
        //                                decimal mm1 = Convert.ToDecimal(gsegtar["monto"]);
        //                                 m1 = mm1.ToString("F");
        //                                break;
        //                            case 2:
        //                                 r2 = gsegtar["codigo"].ToString();
        //                                decimal mm2 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m2 = mm2.ToString("F");
        //                                break;
        //                            case 3:
        //                                 r3 = gsegtar["codigo"].ToString();
        //                                decimal mm3 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m3 = mm3.ToString("F");
        //                                break;
        //                            case 4:
        //                                 r4 = gsegtar["codigo"].ToString();
        //                                decimal mm4 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m4 = mm4.ToString("F");
        //                                break;
        //                            case 5:
        //                                 r5 = gsegtar["codigo"].ToString();
        //                                decimal mm5 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m5 = mm5.ToString("F");
        //                                break;
        //                            case 6:
        //                                 r6 = gsegtar["codigo"].ToString();
        //                                decimal mm6 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m6 = mm6.ToString("F");
        //                                break;
        //                            case 7:
        //                                 r7 = gsegtar["codigo"].ToString();
        //                                decimal mm7 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m7 = mm7.ToString("F");
        //                                break;
        //                            case 8:
        //                                 r8 = gsegtar["codigo"].ToString();
        //                                decimal mm8 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m8 = mm8.ToString("F");
        //                                break;
        //                            case 9:
        //                                 r9 = gsegtar["codigo"].ToString();
        //                                decimal mm9 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m9 = mm9.ToString("F");
        //                                break;
        //                            case 10:
        //                                r10 = gsegtar["codigo"].ToString();
        //                                decimal mm10 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m10 = mm10.ToString("F");
        //                                break;
        //                            case 11:
        //                                r11 = gsegtar["codigo"].ToString();
        //                                decimal mm11 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m11 = mm11.ToString("F");
        //                                break;
        //                            case 12:
        //                                r12 = gsegtar["codigo"].ToString();
        //                                decimal mm12 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m12 = mm12.ToString("F");
        //                                break;
        //                            case 13:
        //                                r13 = gsegtar["codigo"].ToString();
        //                                decimal mm13 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m13 = mm13.ToString("F");
        //                                break;
        //                            case 14:
        //                                r14 = gsegtar["codigo"].ToString();
        //                                decimal mm14 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m14 = mm14.ToString("F");
        //                                break;
        //                            case 15:
        //                                r15 = gsegtar["codigo"].ToString();
        //                                decimal mm15 = Convert.ToDecimal(gsegtar["monto"]);
        //                                m15 = mm15.ToString("F");
        //                                break;
        //                        }
        //                    }

        //                    ct++;
        //                }
        //                facLabControler.GNomina(asgn_id,r1, m1,r2, m2,r3, m3,r4, m4,r5, m5,r6, m6,r7, m7,r8, m8,r9, m9, r10, m10, r11, m11, r12, m12, r13, m13, r14, m14, r15, m15);
        //                r1 = ""; m1 = ""; r2 = ""; m2 = ""; r3 = ""; m3 = ""; r4 = ""; m4 = ""; r5 = ""; m5 = ""; r6 = ""; m6 = ""; r7 = "";m7 = ""; r8 = ""; m8 = ""; r9 = ""; m9 = "";r10 = ""; m10 = "";r11 = ""; m11 = "";r12 = ""; m12 = "";r13 = ""; m13 = "";r14 = ""; m14 = "";r15 = ""; m15 = "";


        //            }


        //        }
                


        //    }
        //}



        //public void Reporte()
        //{
        //    //DirectoryInfo di24a = new DirectoryInfo(@"\\10.223.208.41\Users\Administrator\Documents\LIVERDEDUPLOADS");
        //    DirectoryInfo di24a = new DirectoryInfo(@"D:\Administracion\Respaldo de las app de TDR\Administración\Proyecto TimbradoFacturasMasiva");

        //    FileInfo[] files24a = di24a.GetFiles("*.dat");


        //    int cantidad24a = files24a.Length;
        //    if (cantidad24a > 0)
        //    {
        //        foreach (var itema in files24a)
        //        {
        //            //string sourceFilea = @"\\10.223.208.41\Users\Administrator\Documents\LIVERDEDUPLOADS\" + itema.Name;
        //            //string sourceFilea = @"\\10.223.208.41\Users\Administrator\Documents\LIVERDED\" + itema.Name;
        //            string sourceFile = @"C:\Administración\Proyecto LIVERDED\Procesadas\" + itema.Name;

        //            string lna = itema.Name.ToLower();
        //            string Ai_orden = lna.Replace(".dat", "");
        //            //facLabControler.PullOrderReport(Ai_orden);
        //            //string destinationFile = @"C:\Administración\Proyecto LIVERDED\Rpro\" + itema.Name;
        //            //System.IO.File.Move(sourceFile, destinationFile);

        //            DataTable rtds = facLabControler.ObtSegmento(Ai_orden);
        //            if (rtds.Rows.Count > 0)
        //            {
        //                foreach (DataRow iseg in rtds.Rows)
        //                {
        //                    string nseg = iseg["segmento"].ToString();
        //                    DataTable resa = facLabControler.GetSegmentoRepetidoReporte(nseg);
        //                    //PASO 4 - SI EXISTE LE ACTUALIZA EL ESTATUS A 9
        //                    if (resa.Rows.Count > 0)
        //                    {
        //                        foreach (DataRow gsegta in resa.Rows)
        //                        {
        //                            //OBTENGO EL BILLTO Y EL ESTATUS DE SEGMENTOSPORTIMBRARJR Y LO INSERTO
        //                            string nfolio = gsegta["Folio"].ToString();
        //                            DateTime dt = DateTime.Parse(gsegta["Fecha"].ToString());
        //                            string rfecha = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");
        //                            DataTable resae = facLabControler.GetSegmentoJr(nfolio);
        //                            if (resae.Rows.Count > 0)
        //                            {
        //                                foreach (DataRow gsegtas in resae.Rows)
        //                                {
        //                                    string rrseg = gsegtas["segmento"].ToString();
        //                                    string rrbillto = gsegtas["billto"].ToString();
        //                                    string rrestatus = gsegtas["estatus"].ToString();
        //                                    string fechatim = rfecha;
        //                                    facLabControler.PullReportUpdate(Ai_orden, rrseg, rrbillto, rrestatus, fechatim);
        //                                    //string destinationFile = @"\\10.223.208.41\Users\Administrator\Documents\LIVERDEDUPLOADS\" + item.Name;


        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //OBTENER ESTATUS DEL segmentosportimbrar_JR E INSERTAR EN TABLA
        //                        DataTable resae = facLabControler.GetSegmentoJr(nseg);
        //                        if (resae.Rows.Count > 0)
        //                        {
        //                            foreach (DataRow gsegtas in resae.Rows)
        //                            {
        //                                string rrseg = gsegtas["segmento"].ToString();
        //                                string rrbillto = gsegtas["billto"].ToString();
        //                                string rrestatus = gsegtas["estatus"].ToString();
        //                                string fechatim = "null";
        //                                facLabControler.PullReportUpdate(Ai_orden, rrseg, rrbillto, rrestatus, fechatim);

        //                            }
        //                        }
        //                    }
        //                    string destinationFiles = @"C:\Administración\Proyecto LIVERDED\Rpro\" + itema.Name;
        //                    System.IO.File.Move(sourceFile, destinationFiles);

        //                }
        //            }
        //            else
        //            {
        //                string rrseg = "Cancelada";
        //                facLabControler.PullReportUpdate2(Ai_orden, rrseg);
        //                string destinationFile = @"C:\Administración\Proyecto LIVERDED\Rpro\" + itema.Name;
        //                System.IO.File.Move(sourceFile, destinationFile);
        //            }

        //        }

        //    }
        //}
        public List<string> valida(string leg, string consecutivo, string col2)
        {
            string compCarta = "";
            results.Clear();
            //PASO 6 - VALIDA EL TAMAÑO DEL SEGMENTO
            if (leg.Length > 0 && leg != "null" && leg != "")
            {
                try
                {
                    //VALIDO QUE TENGA MERCANCIA

                    List<string> validaCFDI = new List<string>();
                    //PASO 7 - VALIDA QUE ESTE OK LA CARTAPORTE
                    validaCFDI = sql.recuperaRegistros("exec sp_validaCFDICartaporteFactura_especialliver " + leg + "," + consecutivo);
                    if (validaCFDI.Count > 0)
                    {
                        //PASO 8 - VALIDA QUE ESTE OK EL RESULTADO
                        if (validaCFDI[1].Contains("OK"))
                        {
                            //PASO 9 - CREA EL CUERPO DEL TXT
                            compCarta = sql.recuperaValor("exec sp_compCartaPortev2_factura_especialLiver " + leg + "," + consecutivo + "," + col2);
                            if (compCarta.Length > 0)
                            {
                                tiposCfds();
                                words = Regex.Replace(compCarta, @"\r\n?|\n", "").Split('|');
                                iniciaDatos();
                                //PASO 10 - INGRESA PARA TIMBRAR LA CARTAPORTE
                                if (Cartaporte(leg, compCarta))
                                {
                                    string msg = "Existoso: Se timbro correctamente la FACTURA:" + leg;
                                    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Factura timbrada ', 'success');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                                    //PASO 14 - ACTUALIZA EL ESTATUS A 2 - OK 
                                    results.Add("ok");//mostrar  }


                                    //CON ESTO ACTUALIZAMOS EL ORDERHEADER 
                                    //DataTable rorder = facLabControler.SelectLegHeaderZp(leg);

                                    //if (rorder.Rows.Count > 0)
                                    //{
                                    //    foreach (DataRow reslo in rorder.Rows)
                                    //    {
                                            
                                    //        DateTime dt = DateTime.Parse(reslo["fecha"].ToString());
                                    //        string rfecha = dt.ToString("yyyy'/'MM'/'dd HH:mm:ss");

                                    //        DataTable rordera = facLabControler.SelectInvoiceHeader(leg);
                                    //        if (rordera.Rows.Count > 0)
                                    //        {
                                    //            foreach (DataRow reslos in rordera.Rows)
                                    //            {
                                    //                string ivnumber = reslos["ivh_invoicenumber"].ToString();
                                    //                facLabControler.InvoiceHeader(ivnumber, rfecha);
                                    //            }
                                    //        }


                                    //    }
                                    //}

                                    //facLabControler.enviarNotificacion(leg, mensaje);

                                    //Aqui actualizamos en estatus 

                                }
                                else
                                {
                                    results.Clear();
                                    results.Add("Error1");
                                    results.Add("Ver el historial de errores para mas información, copiar el error y reportar a TI.");
                                    //string tipom = "3";
                                    //string titulo = "Error en el segmento: ";
                                    //string mensaje = "Ver el historial de errores para mas información, copiar el error y reportar a TI.";
                                    string msg = "Error: No se pudo timbrar la FACTURA:" + leg;
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Ver el historial de errores para mas información, copiar el error y reportar a TI ', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                                    //DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);



                                }
                            }
                            else
                            {
                                results.Clear();
                                results.Add("Error1");
                                results.Add("Error al generar carta porte.");//mostrar 
                                //string tipom = "3";
                                //string titulo = "Error en el segmento: ";
                                //string mensaje = "Error al generar carta porte.";
                                string msg = "Error: No se pudo timbrar la FACTURA:" + leg;
                                HiddenField1.Value = msg;
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert2()", true);
                            }
                        }
                        else
                        {
                            // ERROR: YA EXISTE O YA ESTA TIMBRADO
                            results.Clear();
                            results.Add("Error");
                            results.Add("Error en la obtención de datos: \r\n" + validaCFDI[0]);//mostrar 
                                                                                                //string merror = validaCFDI[0].ToString();

                            TextBox1.Value = validaCFDI[0];

                            string msg = "Error: en la obtención de datos:" + leg;
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert()", true);
                            //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error en la obtención de datos', 'error')", true);
                            //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error en la obtención de datos', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 10000)", true);
                        }
                    }
                    else
                    {
                        results.Clear();
                        results.Add("Error");
                        results.Add("Error al validar el segmento.");//mostrar 

                        
                        string msg = "Error: al validar el segmento" + leg;
                        HiddenField1.Value = msg;
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert2()", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error al validar el segmento.', 'error')", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Error al validar el segmento.', 'error');setTimeout(function(){window.location.href ='WebForm1.aspx'}, 100000)", true);
                    }
                }
                catch (Exception)
                {
                    results.Clear();
                    results.Add("Error");
                    results.Add("Segmento invalido");
                    string msg = "Error: Segmento invalido:" + leg;
                    HiddenField1.Value = msg;
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert2()", true);
                }
            }
            else { results.Add("Error3"); }
            return results;
        }

        public static void tiposCfds()
        {
            var request_ = (HttpWebRequest)WebRequest.Create("https://canal1.xsa.com.mx:9050/" + "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4" + "/tiposCfds");
            var response_ = (HttpWebResponse)request_.GetResponse();
            var responseString_ = new StreamReader(response_.GetResponseStream()).ReadToEnd();

            string[] separadas_ = responseString_.Split('}');

            foreach (string dato in separadas_)
            {
                if (dato.Contains("TDRXP"))
                {
                    string[] separadasSucursal_ = dato.Split(',');
                    foreach (string datoSuc in separadasSucursal_)
                    {
                        if (datoSuc.Contains("idSucursal"))
                        {
                            idSucursal = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];
                        }

                        if (datoSuc.Contains("id") && !datoSuc.Contains("idSucursal"))
                        {
                            idTipoFactura = datoSuc.Replace(dato.Substring(0, 8), "").Replace("\"", "").Split(':')[1];
                        }
                    }
                }
            }
        }

        //PASO 11 - RECIBE EL SEGMENTO Y EL CUERPO DEL TXT
        //public void api()
        //{
        //    string url = "https://myurl.com";
        //    string client_id = "client_id";
        //    string client_secret = "client_secret";
        //    //request token
        //    var restclient = new RestClient(url);
        //    RestRequest request = new RestRequest("request/oauth") { Method = Method.POST };
        //    request.AddHeader("Accept", "application/json");
        //    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //    request.AddParameter("client_id", client_id);
        //    request.AddParameter("client_secret", client_secret);
        //    request.AddParameter("grant_type", "client_credentials");
        //    var tResponse = restclient.Execute(request);
        //    var responseJson = tResponse.Content;
        //    var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();
            

        //    //string token = "";
        //    //try
        //    //{
        //    //    callApi(token);
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    var client = new RestClient("www.example.com/api/token");
        //    //    var request = new RestRequest(Method.POST);
        //    //    request.AddHeader("content-type", "application/x-www-form-urlencoded");
        //    //    request.AddHeader("cache-control", "no-cache");
        //    //    request.AddParameter("application/x-www-form-urlencoded", "grant_type=password&username=us&password=pas", ParameterType.RequestBody);
        //    //    IRestResponse response = client.Execute(request);

        //    //    dynamic resp = JObject.Parse(response.Content);
        //    //    token = resp.access_token;

        //    //    callApi(token);
        //    //}
        //}
        //public void callApi(string token)
        //{
        //    var client = new RestClient("www.example.com/api/data");
        //    var request = new RestRequest(Method.GET);
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("authorization", "Bearer " + token);
        //    request.AddHeader("accept", "application/json; charset=utf-8");
        //    IRestResponse response = client.Execute(request);
        //}
        public  bool Cartaporte(string consecutivo, string strtext)
        {
            jsonFactura = "{\r\n\r\n  \"idTipoCfd\":" + "\"" + idTipoFactura + "\"";
            jsonFactura += ",\r\n\r\n  \"nombre\":" + "\"" + consecutivo + ".txt" + "\"";
            jsonFactura += ",\r\n\r\n  \"idSucursal\":" + "\"" + idSucursal + "\"";
            //jsonFactura += ", \r\n\r\n  \"archivoFuente\":" + "\"" + Regex.Replace(strtext, @"\r\n?|\n", "") + "\"" + "\r\n\r\n}";
            jsonFactura += ", \r\n\r\n  \"archivoFuente\":" + "\"" + strtext + "\"" + "\r\n\r\n}";

            string folioFactura = "", serieFactura = "", uuidFactura = "", pdf_xml_descargaFactura = "", pdf_descargaFactura = "", xlm_descargaFactura = "", cancelFactura = "", error = "";
            string salida = "";

            try
            {
                //IdApiEmpresa = "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4";
                //PASO 12 - HACE UNA PETICION PUT A TRALIX PARA TIMBRAR LA CARTAPORTE
                var client = new RestClient("https://canal1.xsa.com.mx:9050/" + "bf2e1036-ba47-49a0-8cd9-e04b36d5afd4" + "/cfdis");
                var request = new RestRequest(Method.PUT);

                request.AddHeader("cache-control", "no-cache");

                request.AddHeader("content-length", "834");
                request.AddHeader("accept-encoding", "gzip, deflate");
                request.AddHeader("Host", "canal1.xsa.com.mx:9050");
                request.AddHeader("Postman-Token", "b6b7d8eb-29f2-420f-8d70-7775701ec765,a4b60b83-429b-4188-98d4-7983acc6742e");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");


                request.AddParameter("application/json", jsonFactura, ParameterType.RequestBody);
                request.Timeout = 1919919289;
                request.ReadWriteTimeout = 1919919289;
                IRestResponse response = client.Execute(request);

                string respuesta = response.StatusCode.ToString();
                //PASO 13 - AQUI VALIDA LA RESPUESTA DE TRALIX Y SI ES OK AVANZA Y SUBE AL FTP E INSERTA EL REGISTRO A VISTA_CARTA_PORTE
                if (respuesta == "BadRequest")
                {
                    string titulo = "Error en el segmento: ";
                    //string mensaje = "Error al validar el segmento.";
                    string merror = response.Content.ToString();
                    TextBox1.Value = response.Content.ToString();
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert()", true);
                    //DataTable updateLeg = facLabControler.UpdateLeg(leg, tipom);
                    //facLabControler.enviarNotificacion(leg, titulo, merror);
                    return false;
                }
                string[] separadaFactura = response.Content.ToString().Split(',');

                List<string> erroes = new List<string>();

                for (int i = 0; i < 7; i++)
                {
                    try
                    {

                        error = separadaFactura[i].Replace("\\n", "").Replace("]}", "").Replace(@"\", "").Replace("\\t", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                        erroes.Add(error);
                    }
                    catch (Exception)
                    {
                        erroes.Add("N/A");
                    }
                }



                foreach (string factura in separadaFactura)
                {
                    if (factura.Contains("errors") || factura.Contains("error"))
                    {

                        salida = "FALLA AL SUBIR";

                        DateTime fecha1 = DateTime.Now;
                        string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;

                        facLabControler.ErroresgeneradasCP(fechaFinal, leg, erroes[0], erroes[1], erroes[2], erroes[3], erroes[4], erroes[5], erroes[6]);
                        return false;
                    }
                    else
                    {
                        if (factura.Contains("folio"))
                        {
                            folioFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("serie"))
                        {
                            serieFactura = factura.Replace(factura.Substring(0, 5), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("uuid"))
                        {
                            uuidFactura = factura.Replace(factura.Substring(0, 4), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("pdfAndXmlDownload"))
                        {
                            pdf_xml_descargaFactura = factura.Replace(factura.Substring(0, 17), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("pdfDownload"))
                        {
                            pdf_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("xmlDownload") && !factura.Contains("pdfAndXmlDownload"))
                        {
                            xlm_descargaFactura = "https://canal1.xsa.com.mx:9050" + factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                        }

                        if (factura.Contains("cancellCfdi"))
                        {
                            cancelFactura = factura.Replace(factura.Substring(0, 11), "").Replace("\"", "").Split(':')[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error1 = ex.Message;
            }

            string ftp = System.Web.Configuration.WebConfigurationManager.AppSettings["ftp"];
            if (ftp.Equals("Si"))
            {
                string path = System.Web.Configuration.WebConfigurationManager.AppSettings["dir"] + leg + ".txt";
                UploadFile file = new UploadFile();
            }
            if (salida != "FALLA AL SUBIR")
            {
                if (System.Web.Configuration.WebConfigurationManager.AppSettings["activa"].Equals("Si"))
                {
                    //Modifica referencia
                    string imaging = "http://172.16.136.34/cgi-bin/img-docfind.pl?reftype=ORD&refnum=" + consecutivo.Trim();

                    DateTime fecha1 = Convert.ToDateTime(Fecha);
                    string fechaFinal = fecha1.Year + "-" + fecha1.Month + "-" + fecha1.Day + " " + fecha1.Hour + ":" + fecha1.Minute + ":" + fecha1.Second + "." + fecha1.Millisecond;

                    facLabControler.generadas(folioFactura, serieFactura, uuidFactura, pdf_xml_descargaFactura, pdf_descargaFactura, xlm_descargaFactura, cancelFactura, consecutivo, fechaFinal, Total, Moneda, RFC, Origen, Destino);
                    result.Add(folioFactura);
                    result.Add(serieFactura);
                    result.Add(uuidFactura);
                    result.Add(pdf_xml_descargaFactura);
                    result.Add(pdf_descargaFactura);
                    result.Add(xlm_descargaFactura);
                    result.Add(cancelFactura);
                    result.Add(consecutivo);
                    result.Add(fechaFinal);
                    return true;
                }
                return true;
            }
            else
            {
                return false;//"Error al conectar al servicio XSA";
            }
        }
        public static void iniciaDatos()
        {
            Fecha = words[4].ToString();
            Subtotal = words[5].ToString();
            Totalimptrasl = words[6].ToString();
            Totalimpreten = words[7].ToString();
            Descuentos = words[8].ToString();
            Total = words[9].ToString();
            FormaPago = words[11].ToString();
            Condipago = words[12].ToString();
            MetodoPago = words[13].ToString();
            Moneda = words[14].ToString();
            RFC = words[22].ToString();
            CodSAT = words[39].ToString();
            IdProducto = words[43].ToString();
            Producto = "Viaje";
            Origen = "";// words[321].ToString();
            Destino = "";// words[322].ToString();

            result.Add(Fecha);
            result.Add(Subtotal);
            result.Add(Totalimptrasl);
            result.Add(Totalimpreten);
            result.Add(Descuentos);
            result.Add(Total);
            result.Add(FormaPago);
            result.Add(Condipago);
            result.Add(MetodoPago);
            result.Add(Moneda);
            result.Add(RFC);
            result.Add(CodSAT);
            result.Add(IdProducto);
            result.Add(Producto);
            result.Add(Origen);
            result.Add(Destino);
        }
        public static Hashtable generaActualizacion()
        {
            Hashtable datosTabla = conceptosFinales();
            Hashtable actualiza = new Hashtable();

            foreach (int item in datosTabla.Keys)
            {
                ArrayList list = (ArrayList)datosTabla[item];
                string tipoConcepto = list[3].ToString();
                double total = double.Parse(list[5].ToString());
                if (actualiza.ContainsKey(tipoConcepto))
                {
                    double val = double.Parse(actualiza[tipoConcepto].ToString());
                    actualiza[tipoConcepto] = val + total;
                }
                else
                {
                    actualiza.Add(tipoConcepto, total);
                }
            }
            return actualiza;
        }


        [WebMethod]
        public static object gettable()
        {
            List<CartaPorterest> lista = new List<CartaPorterest>();

            DataTable data = new DataTable();
            data = sql.ObtieneTabla("SELECT TOP 25 Folio, Serie, UUID, Pdf_xml_descarga, Pdf_descargaFactura, replace(xlm_descargaFactura,'}','') as xml_descargaFactura, replace(cancelFactura,'}','') as cancelFactura, LegNum, Fecha, Total, Moneda, RFC,Origen, Destino FROM VISTA_Carta_Porte ORDER BY FECHA DESC");
            if (data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    lista.Add(new CartaPorterest(data.Rows[i][0].ToString(), data.Rows[i][1].ToString(), data.Rows[i][2].ToString(), "<a href=" + '\u0022' + "https://canal1.xsa.com.mx:9050" + data.Rows[i][3].ToString() + '\u0022' + ">" + "<input type=" + '\u0022' + "submit" + '\u0022' + "value=" + '\u0022' + "ZIP" + '\u0022' + "/>" + "</a>", "<a href=" + '\u0022' + data.Rows[i][4].ToString() + '\u0022' + ">" + "<input type=" + '\u0022' + "submit" + '\u0022' + "value=" + '\u0022' + "PDF" + '\u0022' + "/>" + "</a>", "<a href=" + '\u0022' + data.Rows[i][5].ToString() + '\u0022' + ">" + "<input type=" + '\u0022' + "submit" + '\u0022' + "value=" + '\u0022' + "XML" + '\u0022' + "/>" + "</a>", "<button type=" + '\u0022' + "button" + '\u0022' + " OnClick=" + '\u0022' + "cancelCP('" + data.Rows[i][2].ToString() + "'" + ", '" + data.Rows[i][0].ToString() + "' )" + '\u0022' + ">" + "Cancelar" + "</button>", data.Rows[i][7].ToString(), data.Rows[i][8].ToString(), data.Rows[i][9].ToString(), data.Rows[i][10].ToString(), data.Rows[i][11].ToString(), data.Rows[i][12].ToString(), data.Rows[i][13].ToString()));
                }
            }
            object json = new { data = lista };
            return json;
        }

        public static Hashtable conceptosFinales()
        {
            table = new HtmlTable();
            Hashtable datos = new Hashtable();
            for (int i = 0; i < table.Rows.Count - 1; i++)
            {
                TextBox cant = (TextBox)table.FindControl("" + i + "1");
                TextBox unidad = (TextBox)table.FindControl("" + i + "1");
                TextBox concepto = (TextBox)table.FindControl("" + i + "2");
                DropDownList tmp = (DropDownList)table.FindControl("" + i + "3");
                TextBox valor = (TextBox)table.FindControl("" + i + "4");
                TextBox importe = (TextBox)table.FindControl("" + i + "5");

                double cantidad = Math.Abs(double.Parse(cant.Text));

                //double cantidad = Double.Parse(cant.Text);

                ArrayList list = new ArrayList();
                list.Add(cantidad.ToString());
                list.Add(unidad.Text);
                list.Add(concepto.Text);
                list.Add(tmp.SelectedValue);
                list.Add(valor.Text);
                list.Add(importe.Text);

                if (datos.ContainsKey(tmp.Text))
                {
                    datos[i] = list;
                }
                else
                {
                    datos.Add(i, list);
                }
            }
            return datos;
        }







    }
}