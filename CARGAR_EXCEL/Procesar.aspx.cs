using CARGAR_EXCEL.Models;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CARGAR_EXCEL
{
    public partial class Procesar : System.Web.UI.Page
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
        public string r16;
        public string m16;
        public string r17;
        public string m17;
        public string r18;
        public string m18;
        public string r19;
        public string m19;
        public string r20;
        public string m20;
        public string r21;
        public string m21;
        public string r22;
        public string m22;
        public string r23;
        public string m23;
        public string r24;
        public string m24;
        public string r25;
        public string m25;
        public string r26;
        public string m26;
        public string r27;
        public string m27;
        public string r28;
        public string m28;
        public string r29;
        public string m29;
        public string r30;
        public string m30;
        public string asgn_id;
        public string dr;
        public string csp;
        public static List<string> results = new List<string>();
        static HtmlTable table = new HtmlTable();

        static char[] caracter = { '|' };
        static string[] words;
        protected void Page_Load(object sender, EventArgs e)
        {
            string fecha = Request.QueryString["fecha"];
            GetOperador(fecha);
        }
        protected void DropTable()
        {

            string str = "DROP TABLE TLiquidaciones";


            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand(str, connection))
                {

                    //selectCommand.CommandType = CommandType.StoredProcedure;
                    //selectCommand.CommandTimeout = 100000;
                    //selectCommand.Parameters.AddWithValue("@OFecha", (object)OFecha);
                    //selectCommand.Parameters.AddWithValue("@titulo", (object)titulo);
                    //selectCommand.Parameters.AddWithValue("@mensaje", (object)mensaje);
                    try
                    {
                        connection.Open();
                        selectCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }





        }
        protected void GetOperador(string fecha)
        {
            DropTable();
            //Paso 1 creo la tabla
            DataTable resax = facLabControler.GetCodigo();
            if (resax.Rows.Count > 0)
            {
                int conta = resax.Rows.Count;
                int i = 1;
                foreach (DataRow items in resax.Rows)
                {
                    string codigo = items["codigo"].ToString().Trim();

                    if (i == conta)
                    {
                        string defal = "0.00";
                        dr += "[" + codigo + "] " + "decimal(8,2) NULL DEFAULT " + defal;
                    }
                    else
                    {
                        string defal = "0.00";
                        dr += "[" + codigo + "]" + "decimal(8,2) NULL DEFAULT " + defal + ",";
                    }

                    i++;
                }
                string str = "CREATE TABLE TLiquidaciones" +
                               "(id_num int NOT NULL IDENTITY(1,1) PRIMARY KEY,asgn_id varchar(100)," +
                               dr +
                               ")";
                //facLabControler.CreateTable(str);

                string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";

                using (SqlConnection connection = new SqlConnection(cadena2))
                {

                    using (SqlCommand selectCommand = new SqlCommand(str, connection))
                    {

                        //selectCommand.CommandType = CommandType.StoredProcedure;
                        //selectCommand.CommandTimeout = 100000;
                        //selectCommand.Parameters.AddWithValue("@OFecha", (object)OFecha);
                        //selectCommand.Parameters.AddWithValue("@titulo", (object)titulo);
                        //selectCommand.Parameters.AddWithValue("@mensaje", (object)mensaje);
                        try
                        {
                            connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }






            }
            //Paso 2 Ingreso los operadores
            DataTable resa = facLabControler.GetOperador();
            if (resa.Rows.Count > 0)
            {
                foreach (DataRow gsegta in resa.Rows)
                {

                    string rasg = gsegta["asgn_id"].ToString();
                    asgn_id = rasg.Replace("0", "");
                    facLabControler.Ioperador(asgn_id);


                }
            }

            //CREAR SP DINAMICO

            //Paso3 
            DataTable resaxz = facLabControler.GetCodigo();
            if (resaxz.Rows.Count > 0)
            {
                foreach (DataRow gsegtax in resaxz.Rows)
                {
                    string codigo = gsegtax["codigo"].ToString().Trim();

                    string cadena3 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
                   
                        //string codigo = itemsz["codigo"].ToString().Trim();
                        string codigocc = "[" + codigo + "]";

                        csp = "UPDATE TLiquidaciones SET " + codigocc + " =" + "@monto WHERE asgn_id = @asgn_id";
                        //"[" + codigo + "] " + "decimal(8,2) NULL DEFAULT " + defal;
                        string sp = "ALTER PROCEDURE [dbo].[sp_Uoperador](@codigo varchar(100),@asgn_id varchar(100),@monto decimal(8,2))AS BEGIN " + " " +
                        csp + " " +
                        "END";
                        using (SqlConnection connection = new SqlConnection(cadena3))
                        {

                            using (SqlCommand selectCommand = new SqlCommand(sp, connection))
                            {

                                //selectCommand.CommandType = CommandType.StoredProcedure;
                                //selectCommand.CommandTimeout = 100000;
                                //selectCommand.Parameters.AddWithValue("@OFecha", (object)OFecha);
                                //selectCommand.Parameters.AddWithValue("@titulo", (object)titulo);
                                //selectCommand.Parameters.AddWithValue("@mensaje", (object)mensaje);
                                try
                                {
                                    connection.Open();
                                    selectCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    string message = ex.Message;
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }

                    
                    DataTable resaxza = facLabControler.GetOCodigo(codigo);
                    if (resaxza.Rows.Count > 0)
                    {
                        foreach (DataRow gsegtaxz in resaxza.Rows)
                        {
                            string rasgt = gsegtaxz["asgn_id"].ToString();
                            asgn_id = rasgt.Replace("0", "");
                            
                            decimal mm1 = Convert.ToDecimal(gsegtaxz["monto"]);
                            string mm2 = mm1.ToString("F");
                            decimal monto = Convert.ToDecimal(mm2);
                            facLabControler.Uoperador(codigo, asgn_id, monto);
                        }
                    }
                }
                //AQUI REGISTRO LA FECHA PARA QUE DESAPARESCA DEL LISTADO
                ExportNomina(fecha);
                facLabControler.Rgnomina(fecha);
                card3.Visible = false;
                string msg = "Nomina Generada";
                //Rcartaporte.Value = msg;
                ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Nomina generada', 'success');setTimeout(function(){window.location.href ='Descargar.aspx'}, 7000)", true);

            }
            //DataTable resa = facLabControler.GetOperador();
            //if (resa.Rows.Count > 0)
            //{
            //    foreach (DataRow gsegta in resa.Rows)
            //    {

            //        string rasg = gsegta["asgn_id"].ToString();
            //        asgn_id = rasg.Replace("0", "");
            //        DataTable resar = facLabControler.GetOperadorDetalle(asgn_id);
            //        if (resar.Rows.Count > 0)
            //        {
            //            int ct = 1;
            //            foreach (DataRow gsegtar in resar.Rows)
            //            {
            //                contador = resar.Rows.Count;

            //                if (ct <= contador)
            //                {
            //                    switch (ct)
            //                    {
            //                        case 1:
            //                            r1 = gsegtar["codigo"].ToString();
            //                            decimal mm1 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m1 = mm1.ToString("F");
            //                            break;
            //                        case 2:
            //                            r2 = gsegtar["codigo"].ToString();
            //                            decimal mm2 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m2 = mm2.ToString("F");
            //                            break;
            //                        case 3:
            //                            r3 = gsegtar["codigo"].ToString();
            //                            decimal mm3 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m3 = mm3.ToString("F");
            //                            break;
            //                        case 4:
            //                            r4 = gsegtar["codigo"].ToString();
            //                            decimal mm4 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m4 = mm4.ToString("F");
            //                            break;
            //                        case 5:
            //                            r5 = gsegtar["codigo"].ToString();
            //                            decimal mm5 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m5 = mm5.ToString("F");
            //                            break;
            //                        case 6:
            //                            r6 = gsegtar["codigo"].ToString();
            //                            decimal mm6 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m6 = mm6.ToString("F");
            //                            break;
            //                        case 7:
            //                            r7 = gsegtar["codigo"].ToString();
            //                            decimal mm7 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m7 = mm7.ToString("F");
            //                            break;
            //                        case 8:
            //                            r8 = gsegtar["codigo"].ToString();
            //                            decimal mm8 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m8 = mm8.ToString("F");
            //                            break;
            //                        case 9:
            //                            r9 = gsegtar["codigo"].ToString();
            //                            decimal mm9 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m9 = mm9.ToString("F");
            //                            break;
            //                        case 10:
            //                            r10 = gsegtar["codigo"].ToString();
            //                            decimal mm10 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m10 = mm10.ToString("F");
            //                            break;
            //                        case 11:
            //                            r11 = gsegtar["codigo"].ToString();
            //                            decimal mm11 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m11 = mm11.ToString("F");
            //                            break;
            //                        case 12:
            //                            r12 = gsegtar["codigo"].ToString();
            //                            decimal mm12 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m12 = mm12.ToString("F");
            //                            break;
            //                        case 13:
            //                            r13 = gsegtar["codigo"].ToString();
            //                            decimal mm13 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m13 = mm13.ToString("F");
            //                            break;
            //                        case 14:
            //                            r14 = gsegtar["codigo"].ToString();
            //                            decimal mm14 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m14 = mm14.ToString("F");
            //                            break;
            //                        case 15:
            //                            r15 = gsegtar["codigo"].ToString();
            //                            decimal mm15 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m15 = mm15.ToString("F");
            //                            break;
            //                        case 16:
            //                            r16 = gsegtar["codigo"].ToString();
            //                            decimal mm16 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m16 = mm16.ToString("F");
            //                            break;
            //                        case 17:
            //                            r17 = gsegtar["codigo"].ToString();
            //                            decimal mm17 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m17 = mm17.ToString("F");
            //                            break;
            //                        case 18:
            //                            r18 = gsegtar["codigo"].ToString();
            //                            decimal mm18 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m18 = mm18.ToString("F");
            //                            break;
            //                        case 19:
            //                            r19 = gsegtar["codigo"].ToString();
            //                            decimal mm19 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m19 = mm19.ToString("F");
            //                            break;
            //                        case 20:
            //                            r20 = gsegtar["codigo"].ToString();
            //                            decimal mm20 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m20 = mm20.ToString("F");
            //                            break;
            //                        case 21:
            //                            r21 = gsegtar["codigo"].ToString();
            //                            decimal mm21 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m21 = mm21.ToString("F");
            //                            break;
            //                        case 22:
            //                            r22 = gsegtar["codigo"].ToString();
            //                            decimal mm22 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m22 = mm22.ToString("F");
            //                            break;
            //                        case 23:
            //                            r23 = gsegtar["codigo"].ToString();
            //                            decimal mm23 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m23 = mm23.ToString("F");
            //                            break;
            //                        case 24:
            //                            r24 = gsegtar["codigo"].ToString();
            //                            decimal mm24 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m24 = mm24.ToString("F");
            //                            break;
            //                        case 25:
            //                            r25 = gsegtar["codigo"].ToString();
            //                            decimal mm25 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m25 = mm25.ToString("F");
            //                            break;
            //                        case 26:
            //                            r26 = gsegtar["codigo"].ToString();
            //                            decimal mm26 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m26 = mm26.ToString("F");
            //                            break;
            //                        case 27:
            //                            r27 = gsegtar["codigo"].ToString();
            //                            decimal mm27 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m27 = mm27.ToString("F");
            //                            break;
            //                        case 28:
            //                            r28 = gsegtar["codigo"].ToString();
            //                            decimal mm28 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m28 = mm28.ToString("F");
            //                            break;
            //                        case 29:
            //                            r29 = gsegtar["codigo"].ToString();
            //                            decimal mm29 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m29 = mm29.ToString("F");
            //                            break;
            //                        case 30:
            //                            r30 = gsegtar["codigo"].ToString();
            //                            decimal mm30 = Convert.ToDecimal(gsegtar["monto"]);
            //                            m30 = mm30.ToString("F");
            //                            break;
            //                    }
            //                }

            //                ct++;
            //            }
            //            facLabControler.GNomina(asgn_id, r1, m1, r2, m2, r3, m3, r4, m4, r5, m5, r6, m6, r7, m7, r8, m8, r9, m9, r10, m10, r11, m11, r12, m12, r13, m13, r14, m14, r15, m15, r16, m16, r17, m17, r18, m18, r19, m19, r20, m20, r21, m21, r22, m22, r23, m23, r24, m24, r25, m25);
            //            r1 = ""; m1 = ""; r2 = ""; m2 = ""; r3 = ""; m3 = ""; r4 = ""; m4 = ""; r5 = ""; m5 = ""; r6 = ""; m6 = ""; r7 = ""; m7 = ""; r8 = ""; m8 = ""; r9 = ""; m9 = ""; r10 = ""; m10 = ""; r11 = ""; m11 = ""; r12 = ""; m12 = ""; r13 = ""; m13 = ""; r14 = ""; m14 = ""; r15 = ""; m15 = ""; r16 = ""; m16 = ""; r17 = ""; m17 = ""; r18 = ""; m18 = ""; r19 = ""; m19 = ""; r20 = ""; m20 = ""; r21 = ""; m21 = ""; r22 = ""; m22 = ""; r23 = ""; m23 = ""; r24 = ""; m24 = ""; r25 = ""; m25 = ""; r26 = ""; m26 = ""; r27 = ""; m27 = ""; r28 = ""; m28 = ""; r29 = ""; m29 = ""; r30 = ""; m30 = "";


            //        }


            //    }
            //    //AQUI REGISTRO LA FECHA PARA QUE DESAPARESCA DEL LISTADO
            //    facLabControler.Rgnomina(fecha);
            //    card3.Visible = false;
            //    string msg = "Nomina Generada";
            //    //Rcartaporte.Value = msg;
            //    ScriptManager.RegisterStartupScript(this, GetType(), "swal", "swal('" + msg + "', 'Nomina generada', 'success');setTimeout(function(){window.location.href ='Descargar.aspx'}, 7000)", true);

            //}
        }
        public void ExportNomina(string fecha)
        {
            //string fecha = Request.QueryString["fecha"];
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            DataTable dataTable = new DataTable();
            string strFilePath = @"D:\Administracion\Respaldo de las app de TDR\CartaPorteCargaMasiva\TimbradoLiquidaciones\CARGAR_EXCEL\Archivos\Nomina-"+fecha+"-" + string.Format("Procesada-{0}", string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) + ".csv";
            using (SqlConnection connection = new SqlConnection(cadena2))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("SELECT * FROM TLiquidaciones", connection))
                {

                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;

                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            //Response.Clear();

                            //Response.ContentType = "application/CSV";
                            ////Response.AddHeader("content-disposition", "attachment;filename=Nomina.xlsx");

                            //Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("Nomina-{0}.CSV", string.Format("{0:ddMMyyyy}", DateTime.Today)));
                            ////Response.BinaryWrite(bytes);


                            StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                            long cantidadColumnas = dataTable.Columns.Count;
                            for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
                            {
                                sw.Write(dataTable.Columns[ncolumna]);
                                if (ncolumna < cantidadColumnas - 1)
                                {
                                    sw.Write(",");
                                }
                            }
                            sw.Write(sw.NewLine); //saltamos linea
                            foreach (DataRow renglon in dataTable.Rows)
                            {
                                for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
                                {
                                    if (!Convert.IsDBNull(renglon[ncolumna]))
                                    {
                                        sw.Write(renglon[ncolumna]);
                                    }
                                    if (ncolumna < cantidadColumnas)
                                    {
                                        sw.Write(",");
                                    }
                                }
                                sw.Write(sw.NewLine); //saltamos linea
                            }
                            sw.Close();
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }

        }
    }
}