using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Text;
using System.Threading.Tasks;

namespace CARGAR_EXCEL
{
    public partial class Descargar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ExportNomina();
            okTralix();
        }
        //protected void ExportNomina(object sender, EventArgs e)
        //{
        //    string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
        //    DataTable dataTable = new DataTable();
        //    using (SqlConnection connection = new SqlConnection(cadena2))
        //    {
        //        connection.Open();
        //        using (SqlCommand selectCommand = new SqlCommand("sp_ObtRGNomina", connection))
        //        {

        //            selectCommand.CommandType = CommandType.StoredProcedure;
        //            selectCommand.CommandTimeout = 100000;

        //            selectCommand.ExecuteNonQuery();
        //            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
        //            {
        //                try
        //                {
        //                    //selectCommand.Connection.Open();
        //                    sqlDataAdapter.Fill(dataTable);
        //                    using (XLWorkbook wb = new XLWorkbook())
        //                    {
        //                        wb.Worksheets.Add(dataTable, "Nomina");
        //                        //wb.FirstRow().FirstCell().InsertData(dataTable.Rows);

        //                        Response.Clear();
        //                        Response.Buffer = true;
        //                        Response.Charset = "";
        //                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                        Response.AddHeader("content-disposition", "attachment;filename=Nomina.xlsx");
        //                        using (MemoryStream MyMemoryStream = new MemoryStream())
        //                        {
        //                            wb.SaveAs(MyMemoryStream);
        //                            MyMemoryStream.WriteTo(Response.OutputStream);
        //                            Response.Flush();
        //                            Response.End();
        //                        }
        //                    }
        //                }
        //                catch (SqlException ex)
        //                {
        //                    connection.Close();
        //                    string message = ex.Message;
        //                }
        //            }
        //        }
        //    }


        //}
        protected void Inicio(object sender, EventArgs e)
        {
            Response.Redirect("WebForm1.aspx", false);


        }

        //public void ExportNomina()
        //{
        //    //string fecha = Request.QueryString["fecha"];
        //    string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
        //    DataTable dataTable = new DataTable();
        //    string strFilePath = @"D:\Administracion\Respaldo de las app de TDR\CartaPorteCargaMasiva\TimbradoLiquidaciones\CARGAR_EXCEL\Archivos\" + string.Format("Nomina-{0}", string.Format("{0:yyyyMMddHHmmss}", DateTime.Now)) + ".csv";
        //    using (SqlConnection connection = new SqlConnection(cadena2))
        //    {
        //        connection.Open();
        //        using (SqlCommand selectCommand = new SqlCommand("SELECT * FROM TLiquidaciones", connection))
        //        {

        //            selectCommand.CommandType = CommandType.Text;
        //            selectCommand.CommandTimeout = 100000;

        //            selectCommand.ExecuteNonQuery();
        //            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
        //            {
        //                try
        //                {
        //                    //selectCommand.Connection.Open();
        //                    sqlDataAdapter.Fill(dataTable);
        //                    //Response.Clear();

        //                    //Response.ContentType = "application/CSV";
        //                    ////Response.AddHeader("content-disposition", "attachment;filename=Nomina.xlsx");

        //                    //Response.AddHeader("content-disposition", "attachment;filename=" + string.Format("Nomina-{0}.CSV", string.Format("{0:ddMMyyyy}", DateTime.Today)));
        //                    ////Response.BinaryWrite(bytes);


        //                    StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
        //                    long cantidadColumnas = dataTable.Columns.Count;
        //                    for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
        //                    {
        //                        sw.Write(dataTable.Columns[ncolumna]);
        //                        if (ncolumna < cantidadColumnas - 1)
        //                        {
        //                            sw.Write(",");
        //                        }
        //                    }
        //                    sw.Write(sw.NewLine); //saltamos linea
        //                    foreach (DataRow renglon in dataTable.Rows)
        //                    {
        //                        for (int ncolumna = 0; ncolumna < cantidadColumnas; ncolumna++)
        //                        {
        //                            if (!Convert.IsDBNull(renglon[ncolumna]))
        //                            {
        //                                sw.Write(renglon[ncolumna]);
        //                            }
        //                            if (ncolumna < cantidadColumnas)
        //                            {
        //                                sw.Write(",");
        //                            }
        //                        }
        //                        sw.Write(sw.NewLine); //saltamos linea
        //                    }
        //                    sw.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                    string message = ex.Message;
        //                }
        //                finally
        //                {
        //                    connection.Close();
        //                }
        //            }
        //        }
        //    }

        //}
        private async Task okTralix()
        {
            //DirectoryInfo files = new DirectoryInfo(@"\\10.223.208.41\inetpub\wwwroot\ComplementoPago\TxtGenerados\Generados\");
            DirectoryInfo files = new DirectoryInfo(@"D:\Administracion\Respaldo de las app de TDR\CartaPorteCargaMasiva\TimbradoLiquidaciones\CARGAR_EXCEL\Archivos\");
            FileInfo[] di = files.GetFiles("*.CSV").OrderByDescending(p => p.CreationTime).ToArray();

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Folio", typeof(string)), new DataColumn("Archivo", typeof(string)), new DataColumn("Descargar", typeof(string)) });
            //dt.Rows.Add("Movie 1", "http://www.aspforums.net");
            //dt.Rows.Add("Movie 2", "http://www.aspsnippets.com");
            //dt.Rows.Add("Movie 3", "http://www.jqueryfaqs.com");

            //Execute a loop over the rows.
            foreach (FileInfo row in di)
            {
                //var ultimo_archivo = (from f in di
                //                      orderby f.LastWriteTime descending
                //                      select f).First();
                string nombreA = row.Name;


                string folio = nombreA.Replace(".CSV", "");
                //string rutanueva = @"http://69.20.92.117:8083/TxtGenerados/Generados/";
                string rutanueva = @"http://localhost:56747/Archivos/";
                string rutaA = files.ToString();
                string completo = rutanueva + nombreA;
                string completo2 = rutaA + nombreA;

                //FileInfo sourceFile = new FileInfo(@completo2);
                //FileStream sourceStream = sourceFile.OpenRead();
                //// 2
                //FileStream stream = new FileStream(@"C:\CartaPorteCargaMasiva\TralixComplementoPago\CARGAR_EXCEL\TxtGenerados\ZipGenerados\"+folio+".zip", FileMode.Open);
                //// 3 
                //ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create);
                //// 4 
                //ZipArchiveEntry entry = archive.CreateEntry(sourceFile.Name);
                //// 5
                //Stream zipStream = entry.Open();
                //// 6
                //sourceStream.CopyTo(zipStream);
                //// 7
                //zipStream.Close();
                //sourceStream.Close();
                //archive.Dispose();
                //stream.Close();
                //Descargar(completo);
                //string completo2 = rutaA + nombreA;
                //compressDirectory(
                //     completo2,
                //     @"C:\CartaPorteCargaMasiva\TralixComplementoPago\CARGAR_EXCEL\TxtGenerados\ZipGenerados\MyOutputFile.zip",
                //     9
                // );
                dt.Rows.Add(folio, nombreA, completo);

            }
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

    }
}