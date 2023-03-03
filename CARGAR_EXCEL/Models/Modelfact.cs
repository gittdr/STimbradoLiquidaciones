using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CARGAR_EXCEL.Models
{
    public class ModelFact


    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string uuid { get; set; }
        public string motivo { get; set; }
        public string status { get; set; }

        public string folio { get; set; }
        public string fecha { get; set; }
        public string serie { get; set; }
        public string rfc { get; set; }
        public string pdfAndXmlDownload { get; set; }
        public string pdfDownload { get; set; }
        public string xmlDownload { get; set; }
        public string monto { get; set; }
        public string tipoMoneda { get; set; }

        public string ord_hdrnumber { get; set; }
        public string tcfix { get; set; }
        private const string facturas = "select folio as Folio,fhemision as Fecha, nombrecliente as Cliente, idreceptor from VISTA_fe_Header";
        private const string facturasClientes = "select distinct  idreceptor from vista_fe_header";
        private const string facturasPorProcesar = "select * from VISTA_fe_Header where  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','LIVERDED')";
        private const string facturasPorProcesarLivepool = "select * from VISTA_fe_Header where idreceptor in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','LIVERDED')";
        private const string facturaAdendaReferencia = "select ref_number, ref_type from referencenumber where ord_hdrnumber = @orden and (ref_type = 'ADEHOJ' or ref_type = 'ADEPED' or ref_type = 'LPROV')";
        private const string datosFactura = "select * from VISTA_fe_Header where folio = @factura";
        private const string detalle = "select * from vista_Fe_detail where folio = @factura";
        private const string detalle33 = "select * from vista_Fe_detail where folio = @factura";
        private const string invoice = "select ivh_invoicestatus,ivh_mbnumber,ivh_ref_number from invoiceheader where ivh_invoicenumber = @factura";
        private const string updateTrans = "update invoiceheader set ivh_ref_number = @idComprobante where ivh_invoicenumber = @fact";
        private const string updateTransMaster = "update invoiceheader set ivh_ref_number = @idComprobante where ivh_mbnumber  = @master";
        private const string insertaGeneradas = "insert into VISTA_Fe_generadas (nmaster,invoice,serie,idreceptor,fhemision,total,moneda,rutapdf,rutaxml,imaging,bandera,\r\n            provfact,status,ultinvoice,hechapor,orden,rfc) values (@master,@factura,@serie,@idreceptor,@fhemision,@total,@moneda,\r\n            @rutapdf,@rutaxml,@imaging,@bandera,@provfactura,@status,@ultinvoice,@hechapor,@orden,@rfc)";
        private const string parmFactura = "( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)";
        private const string masterFactura = "select * from vista_fe_header where ultinvoice = @parmFact";
        private const string minInvoice = "select invoice from vista_fe_header where ultinvoice = @parmFact";
        private const string P_fact = "@factura";
        private const string P_idComprobante = "@idComprobante";
        private const string P_master = "@master";
        private const string P_fact2 = "@fact";
        private const string P_pfact = "@parmFact";
        private const string P_invoice = "@lastInvoice";
        private const string P_serie = "@serie";
        private const string P_idReceptor = "@idreceptor";
        private const string P_fhemision = "@fhemision";
        private const string P_total = "@total";
        private const string P_moneda = "@moneda";
        private const string P_rutaPdf = "@rutapdf";
        private const string P_rutaXML = "@rutaxml";
        private const string P_imaging = "@imaging";
        private const string P_bandera = "@bandera";
        private const string P_provFact = "@provfactura";
        private const string P_status = "@status";
        private const string P_ultinvoice = "@ultinvoice";
        private const string P_hechapor = "@hechapor";
        private const string P_orden = "@orden";
        private const string P_rfc = "@rfc";
        private string _ConnectionString;

        public ModelFact()
        {
            this._ConnectionString = new Connection().connectionString;
        }
        public void RegEjecucion()
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("usp_registarejecucion", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    //selectCommand.Parameters.AddWithValue("@leg", (object)leg);
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
        public DataTable GetCalendar()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT psh_id,psh_name,psh_description,timestamp,psh_status FROM payschedulesheader WHERE psh_id = 101", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable Getrgnomina()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT psd_date FROM rgnomina", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetOperadorDetalle(string asgn_id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT asgn_id,codigo,monto FROM stl_liq_detalle_concepto_jr WHERE asgn_id = @asgn_id", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@asgn_id", (object)asgn_id);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable Stl_liq_detalle_total()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT sum(total_depositado) as total_liq FROM stl_liquidaciones_encabezado_jr", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetOperador()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT asgn_id FROM stl_liq_detalle_concepto_jr GROUP BY asgn_id order by asgn_id", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetCodigo()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT DISTINCT codigo FROM stl_liq_detalle_concepto_jr ORDER BY codigo", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetOCodigo(string codigo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT asgn_id,monto FROM stl_liq_detalle_concepto_jr WHERE codigo = @codigo", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@codigo", (object)codigo);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable Stl_liq_detalle_all_total()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT no_liq,asgn_id,nombre,deducciones,reembolsos,total_liq FROM stl_liquidaciones_encabezado_jr", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable Stl_liq_detalle2_total()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT suma_total FROM stl_liq_detalle_total_jr", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable GetEstatus(string orden)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_get_estatus", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@orden", (object)orden);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable ObtSegmento(string orden)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_segmento_legheader", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@orden", (object)orden);
                    //selectCommand.Parameters.AddWithValue("@tipom", (object)tipom);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public void enviarNotificacion(string leg, string titulo, string mensaje)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_NotificacionesLiverded", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.Parameters.AddWithValue("@titulo", (object)titulo);
                    selectCommand.Parameters.AddWithValue("@mensaje", (object)mensaje);
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
        public void spLiquidacionesNomina(string OFecha)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("spLiquidacionesNomina", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@OFecha", (object)OFecha);
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
        public DataTable getFacturas()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select folio as Folio,fhemision as Fecha, nombrecliente as Cliente, idreceptor from VISTA_fe_Header", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public void GetMerc(string Ai_orden, string Av_cmd_code, string Av_cmd_description, string Af_weight, string Av_weightunit, string Af_count, string Av_countunit)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Obtiene_Stops_Orden_JR", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@Ai_orden", Ai_orden);
                    selectCommand.Parameters.AddWithValue("@Av_cmd_code", Av_cmd_code);
                    selectCommand.Parameters.AddWithValue("@Av_cmd_description", Av_cmd_description);
                    selectCommand.Parameters.AddWithValue("@Af_weight", Af_weight);
                    selectCommand.Parameters.AddWithValue("@Av_weightunit", Av_weightunit);
                    selectCommand.Parameters.AddWithValue("@Af_count", Af_count);
                    selectCommand.Parameters.AddWithValue("@Av_countunit", Av_countunit);

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
        public void GNomina(string asgn_id, string r1, string m1, string r2, string m2, string r3, string m3, string r4, string m4, string r5, string m5, string r6, string m6, string r7, string m7, string r8, string m8, string r9, string m9, string r10, string m10, string r11, string m11, string r12, string m12, string r13, string m13, string r14, string m14, string r15, string m15, string r16, string m16, string r17, string m17, string r18, string m18, string r19, string m19, string r20, string m20, string r21, string m21, string r22, string m22, string r23, string m23, string r24, string m24, string r25, string m25)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_GNomina", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@asgn_id", asgn_id);
                    selectCommand.Parameters.AddWithValue("@r1", (object)r1 == null ? "" : (object)r1);
                    selectCommand.Parameters.AddWithValue("@m1", (object)m1 == null ? "" : (object)m1);
                    selectCommand.Parameters.AddWithValue("@r2", (object)r2 == null ? "" : (object)r2);
                    selectCommand.Parameters.AddWithValue("@m2", (object)m2 == null ? "" : (object)m2);
                    selectCommand.Parameters.AddWithValue("@r3", (object)r3 == null ? "" : (object)r3);
                    selectCommand.Parameters.AddWithValue("@m3", (object)m3 == null ? "" : (object)m3);
                    selectCommand.Parameters.AddWithValue("@r4", (object)r4 == null ? "" : (object)r4);
                    selectCommand.Parameters.AddWithValue("@m4", (object)m4 == null ? "" : (object)m4);
                    selectCommand.Parameters.AddWithValue("@r5", (object)r5 == null ? "" : (object)r5);
                    selectCommand.Parameters.AddWithValue("@m5", (object)m5 == null ? "" : (object)m5);
                    selectCommand.Parameters.AddWithValue("@r6", (object)r6 == null ? "" : (object)r6);
                    selectCommand.Parameters.AddWithValue("@m6", (object)m6 == null ? "" : (object)m6);
                    selectCommand.Parameters.AddWithValue("@r7", (object)r7 == null ? "" : (object)r7);
                    selectCommand.Parameters.AddWithValue("@m7", (object)m7 == null ? "" : (object)m7);
                    selectCommand.Parameters.AddWithValue("@r8", (object)r8 == null ? "" : (object)r8);
                    selectCommand.Parameters.AddWithValue("@m8", (object)m8 == null ? "" : (object)m8);
                    selectCommand.Parameters.AddWithValue("@r9", (object)r9 == null ? "" : (object)r9);
                    selectCommand.Parameters.AddWithValue("@m9", (object)m9 == null ? "" : (object)m9);
                    selectCommand.Parameters.AddWithValue("@r10", (object)r10 == null ? "" : (object)r10);
                    selectCommand.Parameters.AddWithValue("@m10", (object)m10 == null ? "" : (object)m10);
                    selectCommand.Parameters.AddWithValue("@r11", (object)r11 == null ? "" : (object)r11);
                    selectCommand.Parameters.AddWithValue("@m11", (object)m11 == null ? "" : (object)m11);
                    selectCommand.Parameters.AddWithValue("@r12", (object)r12 == null ? "" : (object)r12);
                    selectCommand.Parameters.AddWithValue("@m12", (object)m12 == null ? "" : (object)m12);
                    selectCommand.Parameters.AddWithValue("@r13", (object)r13 == null ? "" : (object)r13);
                    selectCommand.Parameters.AddWithValue("@m13", (object)m13 == null ? "" : (object)m13);
                    selectCommand.Parameters.AddWithValue("@r14", (object)r14 == null ? "" : (object)r14);
                    selectCommand.Parameters.AddWithValue("@m14", (object)m14 == null ? "" : (object)m14);
                    selectCommand.Parameters.AddWithValue("@r15", (object)r15 == null ? "" : (object)r15);
                    selectCommand.Parameters.AddWithValue("@m15", (object)m15 == null ? "" : (object)m15);

                    selectCommand.Parameters.AddWithValue("@r16", (object)r16 == null ? "" : (object)r16);
                    selectCommand.Parameters.AddWithValue("@m16", (object)m16 == null ? "" : (object)m16);
                    selectCommand.Parameters.AddWithValue("@r17", (object)r17 == null ? "" : (object)r17);
                    selectCommand.Parameters.AddWithValue("@m17", (object)m17 == null ? "" : (object)m17);
                    selectCommand.Parameters.AddWithValue("@r18", (object)r18 == null ? "" : (object)r18);
                    selectCommand.Parameters.AddWithValue("@m18", (object)m18 == null ? "" : (object)m18);
                    selectCommand.Parameters.AddWithValue("@r19", (object)r19 == null ? "" : (object)r19);
                    selectCommand.Parameters.AddWithValue("@m19", (object)m19 == null ? "" : (object)m19);
                    selectCommand.Parameters.AddWithValue("@r20", (object)r20 == null ? "" : (object)r20);
                    selectCommand.Parameters.AddWithValue("@m20", (object)m20 == null ? "" : (object)m20);
                    selectCommand.Parameters.AddWithValue("@r21", (object)r21 == null ? "" : (object)r21);
                    selectCommand.Parameters.AddWithValue("@m21", (object)m21 == null ? "" : (object)m21);
                    selectCommand.Parameters.AddWithValue("@r22", (object)r22 == null ? "" : (object)r22);
                    selectCommand.Parameters.AddWithValue("@m22", (object)m22 == null ? "" : (object)m22);
                    selectCommand.Parameters.AddWithValue("@r23", (object)r23 == null ? "" : (object)r23);
                    selectCommand.Parameters.AddWithValue("@m23", (object)m23 == null ? "" : (object)m23);
                    selectCommand.Parameters.AddWithValue("@r24", (object)r24 == null ? "" : (object)r24);
                    selectCommand.Parameters.AddWithValue("@m24", (object)m24 == null ? "" : (object)m24);
                    selectCommand.Parameters.AddWithValue("@r25", (object)r25 == null ? "" : (object)r25);
                    selectCommand.Parameters.AddWithValue("@m25", (object)m25 == null ? "" : (object)m25);
                    //selectCommand.Parameters.AddWithValue("@r26", (object)r26 == null ? "" : (object)r26);
                    //selectCommand.Parameters.AddWithValue("@m26", (object)m26 == null ? "" : (object)m26);
                    //selectCommand.Parameters.AddWithValue("@r27", (object)r27 == null ? "" : (object)r27);
                    //selectCommand.Parameters.AddWithValue("@m27", (object)m27 == null ? "" : (object)m27);
                    //selectCommand.Parameters.AddWithValue("@r28", (object)r28 == null ? "" : (object)r28);
                    //selectCommand.Parameters.AddWithValue("@m28", (object)m28 == null ? "" : (object)m28);
                    //selectCommand.Parameters.AddWithValue("@r29", (object)r29 == null ? "" : (object)r29);
                    //selectCommand.Parameters.AddWithValue("@m29", (object)m29 == null ? "" : (object)m29);
                    //selectCommand.Parameters.AddWithValue("@r30", (object)r30 == null ? "" : (object)r30);
                    //selectCommand.Parameters.AddWithValue("@m30", (object)m30 == null ? "" : (object)m30);

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
        public void DeleteMerc(string Ai_orden)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_ordenborrar_mercancias", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@Ai_orden", Ai_orden);
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
        public void Rgnomina(string fecha)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_RGNomina", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@fecha", fecha);
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

        public void Ioperador(string asgn_id)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("INSERT INTO TLiquidaciones(asgn_id)VALUES(@asgn_id)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@asgn_id", (object)asgn_id);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
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

        }
        public DataTable Uoperador(string codigo, string asgn_id, decimal monto)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_Uoperador", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@codigo", (object)codigo);
                    selectCommand.Parameters.AddWithValue("@asgn_id", (object)asgn_id);
                    selectCommand.Parameters.AddWithValue("@monto", (object)monto);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        
        public DataTable getLeg()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("SELECT DISTINCT segmento FROM segmentosportimbrar_JR WHERE billto = 'LIVERDED' and estatus = '1'", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable UpdateLeg(string leg, string tipom)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_segmento_actualizado", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.Parameters.AddWithValue("@tipom", (object)tipom);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public void GetMerca(string Ai_orden, string segmentod, string Av_cmd_code, string Av_cmd_description, string Af_weight, string Av_weightunit, string Af_count, string Av_countunit)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Obtiene_Stops_Orden_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@Ai_orden", Ai_orden);
                    selectCommand.Parameters.AddWithValue("@Ai_Segmento", segmentod);
                    selectCommand.Parameters.AddWithValue("@Av_cmd_code", Av_cmd_code);
                    selectCommand.Parameters.AddWithValue("@Av_cmd_description", Av_cmd_description);
                    selectCommand.Parameters.AddWithValue("@Af_weight", Af_weight);
                    selectCommand.Parameters.AddWithValue("@Av_weightunit", Av_weightunit);
                    selectCommand.Parameters.AddWithValue("@Af_count", Af_count);
                    selectCommand.Parameters.AddWithValue("@Av_countunit", Av_countunit);

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
        public void DeleteMerca(string segmentod)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_ordenborrar_mercancias_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@lgh_number", segmentod);
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
        public void InvoiceHeader(string leg, string rfecha)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_Invoice_Header_JC", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100121220;
                    selectCommand.Parameters.AddWithValue("@leg", leg);
                    selectCommand.Parameters.AddWithValue("@fecha", rfecha);
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
        public void CreateTable(string str)
        {
            string cadena2 = @"Data source=172.24.16.112; Initial Catalog=TMWSuite; User ID=sa; Password=tdr9312;Trusted_Connection=false;MultipleActiveResultSets=true";
            //DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(cadena2))
            {

                using (SqlCommand selectCommand = new SqlCommand("sp_trnom", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100121220;
                    selectCommand.Parameters.AddWithValue("@str", str);
                    //selectCommand.Parameters.AddWithValue("@fecha", rfecha);
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
        public DataTable VerErrores(string leg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_ver_errores", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    //selectCommand.Parameters.AddWithValue("@tipom", (object)tipom);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetSegmentoRepetido(string leg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_segmento_repetido", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable TieneMercancias(string leg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_tiene_mercancias", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable ExisteSegmentos(string seg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_existe_segmentos", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@seg", (object)seg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable ExisteSegmento(string leg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_existe_segmento", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)leg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getFacturasClientes()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select distinct  idreceptor from vista_fe_header", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasGeneradas()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_fe_generadas where fhemision >'2019-01-01' and rutapdf not like '%:9050%' order by 5 desc", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturasPorProcesar(string billto)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor = @idreceptor and  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV','LIVERDED')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@idreceptor", (object)billto);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getOrder(string segmento)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor = @idreceptor and  idreceptor not in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV','LIVERDED')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@segmento", (object)segmento);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable UpdateOrderHeader(string orheader, string fecha)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_update_orderheader", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@orheader", (object)orheader);
                    selectCommand.Parameters.AddWithValue("@fecha", (object)fecha);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable SelectLegHeader(string orseg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_order_header", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)orseg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable SelectLegHeaderZp(string orseg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_order_headerZP", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)orseg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable SelectInvoiceHeader(string orseg)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("sp_obtener_Invoice_header", connection))
                {

                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@leg", (object)orseg);
                    selectCommand.ExecuteNonQuery();
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            //selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            connection.Close();
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getFacturasPorProcesarLivepool()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from VISTA_fe_Header where idreceptor in ('liverpol','GLOBALIV','LIVERTIJ','SFERALIV','FACTUMLV','MERCANLV','LIVERDED')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getFacturaAdendaReferencia(string Ord)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select ref_number, ref_type from referencenumber where ord_hdrnumber = @orden and (ref_type = 'ADEHOJ' or ref_type = 'ADEPED' or ref_type = 'LPROV')", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.Parameters.AddWithValue("@orden", (object)Ord);
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getDatosFacturas(string fact)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string str = "";
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
                if (dataTable1.Rows.Count != 0 && dataTable1 != null)
                    str = dataTable1.Rows[0].ItemArray[0].ToString();
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_fe_header where ultinvoice = @parmFact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@parmFact", (object)str);
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable2);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
            }
            return dataTable2;
        }

        public DataTable getDetalle(string p)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_Fe_detail where folio = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)p);
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getDetalle33(string p)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select * from vista_Fe_detail where folio = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)p);
                    selectCommand.CommandTimeout = 100000;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable getInvoice(string fact)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select ivh_invoicestatus,ivh_mbnumber,ivh_ref_number from invoiceheader where ivh_invoicenumber = @factura", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }

        public void updateFactura(string fact, string comprobante, int mbnumber)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand(mbnumber != 0 ? "update invoiceheader set ivh_ref_number = @idComprobante where ivh_mbnumber  = @master" : "update invoiceheader set ivh_ref_number = @idComprobante where ivh_invoicenumber = @fact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@idComprobante", (object)comprobante);
                    if (mbnumber == 0)
                        selectCommand.Parameters.AddWithValue("@fact", (object)fact);
                    else
                        selectCommand.Parameters.AddWithValue("@master", (object)mbnumber);
                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }
        public DataTable getUser(string user)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("select top 1 usr_password from  tlbUserAccess tu inner join ttsusers ttu on usr_userid= ttu.usr_userid where usr_mail =@user and usr_access = 'Y'", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandTimeout = 100000;
                    selectCommand.Parameters.AddWithValue("@user", (object)user);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable);
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataTable getLastInvoice(string ivh)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            string str = "";
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("( select case when(select ivh_mbnumber from invoiceheader with (nolock) where ivh_invoicenumber = @factura) = 0 then @factura else (select max(ivh_invoicenumber) from invoiceheader with (nolock) where ivh_mbnumber = (select ivh_mbnumber from invoiceheader with (nolock) where ivh_mbnumber != 0 and ivh_invoicenumber = @factura)) end)", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@factura", (object)ivh);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable1);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
                if (dataTable1.Rows.Count != 0 && dataTable1 != null)
                    str = dataTable1.Rows[0].ItemArray[0].ToString();
                using (SqlCommand selectCommand = new SqlCommand("select invoice from vista_fe_header where ultinvoice = @parmFact", connection))
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("@parmFact", (object)str);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            sqlDataAdapter.Fill(dataTable2);
                            selectCommand.Connection.Close();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                            selectCommand.Connection.Close();
                        }
                    }
                }
            }
            return dataTable2;
        }

        public void correcionGeneradas(
      string fact,
      string serie,
      string rutaPdf,
      string rutaXML,
      string UID)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("update vista_fe_generadas set rutapdf=@rutapdf, rutaxml=@rutaxml, UID=@UID where invoice = @factura and serie = @serie", connection))
                {
                    selectCommand.CommandType = CommandType.Text;

                    selectCommand.Parameters.AddWithValue("@factura", (object)fact);
                    selectCommand.Parameters.AddWithValue("@serie", (object)serie);

                    selectCommand.Parameters.AddWithValue("@rutapdf", (object)rutaPdf);
                    selectCommand.Parameters.AddWithValue("@rutaxml", (object)rutaXML);

                    selectCommand.Parameters.AddWithValue("@UID", (object)UID);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }



        public void actualizaGeneradas(
          string folioFactura,
          string serieFactura,
          string uuidFactura,
          string pdf_xml_descargaFactura,
          string pdf_descargaFactura,
          string xlm_descargaFactura,
          string cancelFactura,
          string LegNum,
          string Fecha,
          string Total,
          string Moneda,
          string RFC,
          string Origen,
          string Destino)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("INSERT INTO [dbo].[VISTA_Carta_Porte]([Folio],[Serie],[UUID],[Pdf_xml_descarga],[Pdf_descargaFactura],[xlm_descargaFactura],[cancelFactura],[LegNum],[Fecha],[Total],[Moneda],[RFC],[Origen],[Destino])VALUES(@Folio, @Serie, @UUID, @Pdf_xml_descarga, @Pdf_descargaFactura, @xlm_descargaFactura, @cancelFactura, @LegNum, @Fecha, @Total, @Moneda, @RFC, @Origen, @Destino)", connection))
                {
                    selectCommand.Parameters.AddWithValue("@Folio", (object)folioFactura);
                    selectCommand.Parameters.AddWithValue("@Serie", (object)serieFactura);
                    selectCommand.Parameters.AddWithValue("@UUID", (object)uuidFactura);
                    selectCommand.Parameters.AddWithValue("@Pdf_xml_descarga", (object)pdf_xml_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@Pdf_descargaFactura", (object)pdf_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@xlm_descargaFactura", (object)xlm_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@cancelFactura", (object)cancelFactura);
                    selectCommand.Parameters.AddWithValue("@LegNum", (object)LegNum);
                    selectCommand.Parameters.AddWithValue("@Fecha", (object)Fecha);
                    selectCommand.Parameters.AddWithValue("@Total", (object)Total);
                    selectCommand.Parameters.AddWithValue("@Moneda", (object)Moneda);
                    selectCommand.Parameters.AddWithValue("@RFC", (object)RFC);
                    selectCommand.Parameters.AddWithValue("@Origen", (object)Origen);
                    selectCommand.Parameters.AddWithValue("@Destino", (object)Destino);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }

        public void insertfaltantes(
          string folioFactura,
          string serieFactura,
          string uuidFactura,
          string pdf_xml_descargaFactura,
          string pdf_descargaFactura,
          string xlm_descargaFactura,
          string cancelFactura,
          string LegNum,
          string Fecha,
          string Total,
          string Moneda,
          string RFC,
          string Origen,
          string Destino)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("INSERT INTO [dbo].[VISTA_Carta_Porte]([Folio],[Serie],[UUID],[Pdf_xml_descarga],[Pdf_descargaFactura],[xlm_descargaFactura],[cancelFactura],[LegNum],[Fecha],[Total],[Moneda],[RFC],[Origen],[Destino])VALUES(@Folio, @Serie, @UUID, @Pdf_xml_descarga, @Pdf_descargaFactura, @xlm_descargaFactura, @cancelFactura, @LegNum, @Fecha, @Total, @Moneda, @RFC, @Origen, @Destino)", connection))
                {
                    selectCommand.Parameters.AddWithValue("@Folio", (object)folioFactura);
                    selectCommand.Parameters.AddWithValue("@Serie", (object)serieFactura);
                    selectCommand.Parameters.AddWithValue("@UUID", (object)uuidFactura);
                    selectCommand.Parameters.AddWithValue("@Pdf_xml_descarga", (object)pdf_xml_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@Pdf_descargaFactura", (object)pdf_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@xlm_descargaFactura", (object)xlm_descargaFactura);
                    selectCommand.Parameters.AddWithValue("@cancelFactura", (object)cancelFactura);
                    selectCommand.Parameters.AddWithValue("@LegNum", (object)LegNum);
                    selectCommand.Parameters.AddWithValue("@Fecha", (object)Fecha);
                    selectCommand.Parameters.AddWithValue("@Total", (object)Total);
                    selectCommand.Parameters.AddWithValue("@Moneda", (object)Moneda);
                    selectCommand.Parameters.AddWithValue("@RFC", (object)RFC);
                    selectCommand.Parameters.AddWithValue("@Origen", (object)Origen);
                    selectCommand.Parameters.AddWithValue("@Destino", (object)Destino);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }


        public void ErrorGeneradasCP(
            string Fecha,
            string Folio,
            string Erro1,
            string Erro2,
            string Erro3,
            string Erro4,
            string Erro5,
            string Erro6,
            string Erro7)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(this._ConnectionString))
            {
                using (SqlCommand selectCommand = new SqlCommand("INSERT INTO [dbo].[VISTA_Carta_Porte_Errores]([Fecha],[Folio],[Erro1],[Erro2],[Erro3],[Erro4],[Erro5],[Erro6],[Erro7])VALUES(@Fecha, @Folio, @Erro1, @Erro2, @Erro3, @Erro4, @Erro5, @Erro6, @Erro7)", connection))
                {
                    selectCommand.Parameters.AddWithValue("@Fecha", (object)Fecha);
                    selectCommand.Parameters.AddWithValue("@Folio", (object)Folio);
                    selectCommand.Parameters.AddWithValue("@Erro1", (object)Erro1);
                    selectCommand.Parameters.AddWithValue("@Erro2", (object)Erro2);
                    selectCommand.Parameters.AddWithValue("@Erro3", (object)Erro3);
                    selectCommand.Parameters.AddWithValue("@Erro4", (object)Erro4);
                    selectCommand.Parameters.AddWithValue("@Erro5", (object)Erro5);
                    selectCommand.Parameters.AddWithValue("@Erro6", (object)Erro6);
                    selectCommand.Parameters.AddWithValue("@Erro7", (object)Erro7);
                    using (new SqlDataAdapter(selectCommand))
                    {
                        try
                        {
                            selectCommand.Connection.Open();
                            selectCommand.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
            }
        }
    }
}