using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Tamir.SharpSsh.jsch.jce;
using static NPOI.HSSF.Util.HSSFColor;

namespace CARGAR_EXCEL.Models
{
    public class FacLabControler
    {
        public ModelFact modelFact = new ModelFact();

        public DataTable facturas()
        {
            return this.modelFact.getFacturas();
        }
        public DataTable Getrgnomina()
        {
            return this.modelFact.Getrgnomina();
        }
        public DataTable GetOperador()
        {
            return this.modelFact.GetOperador();
        }
        public DataTable GetCodigo()
        {
            return this.modelFact.GetCodigo();
        }
        public DataTable GetOperadorDetalle(string asgn_id)
        {
            return this.modelFact.GetOperadorDetalle(asgn_id);
        }
        public DataTable GetOCodigo(string codigo)
        {
            return this.modelFact.GetOCodigo(codigo);
        }
        public DataTable GetCalendar()
        {
            return this.modelFact.GetCalendar();
        }
        public DataTable Stl_liq_detalle_total()
        {
            return this.modelFact.Stl_liq_detalle_total();
        }
        public DataTable Stl_liq_detalle_all_total()
        {
            return this.modelFact.Stl_liq_detalle_all_total();
        }
        public DataTable Stl_liq_detalle2_total()
        {
            return this.modelFact.Stl_liq_detalle2_total();
        }
        public void GetMerc(string Ai_orden, string Av_cmd_code, string Av_cmd_description, string Af_weight, string Av_weightunit, string Af_count, string Av_countunit)
        {
            this.modelFact.GetMerc(Ai_orden, Av_cmd_code, Av_cmd_description, Af_weight, Av_weightunit, Af_count, Av_countunit);
        }
        public void DeleteMerc(string Ai_orden)
        {
            this.modelFact.DeleteMerc(Ai_orden);
        }
        public void CreateTable(string str)
        {
            this.modelFact.CreateTable(str);
        }
        public void Uoperador(string codigo, string asgn_id, decimal monto)
        {
            this.modelFact.Uoperador(codigo, asgn_id, monto);
        }
        public void Rgnomina(string fecha)
        {
            this.modelFact.Rgnomina(fecha);;
        }
        public void Ioperador(string asgn_id)
        {
            this.modelFact.Ioperador(asgn_id);
        }
        public void spLiquidacionesNomina(string OFecha)
        {
            this.modelFact.spLiquidacionesNomina(OFecha);
        }
        public void InvoiceHeader(string leg, string rfecha)
        {
            this.modelFact.InvoiceHeader(leg, rfecha);
        }
        public DataTable SelectLegHeader(string orseg)
        {
            return this.modelFact.SelectLegHeader(orseg);
        }
        public DataTable SelectLegHeaderZp(string orseg)
        {
            return this.modelFact.SelectLegHeaderZp(orseg);
        }
        public DataTable SelectInvoiceHeader(string orseg)
        {
            return this.modelFact.SelectInvoiceHeader(orseg);
        }
        public DataTable UpdateOrderHeader(string orheader, string fecha)
        {
            return this.modelFact.UpdateOrderHeader(orheader, fecha);
        }
        public DataTable GetLeg()
        {
            return this.modelFact.getLeg();
        }
        public DataTable ObtSegmento(string orden)
        {
            return this.modelFact.ObtSegmento(orden);
        }
        public DataTable ExisteSegmentos(string seg)
        {
            return this.modelFact.ExisteSegmentos(seg);
        }
        public void GNomina(string asgn_id, string r1, string m1, string r2, string m2, string r3, string m3, string r4, string m4, string r5, string m5, string r6, string m6, string r7, string m7, string r8, string m8, string r9, string m9, string r10, string m10, string r11, string m11, string r12, string m12, string r13, string m13, string r14, string m14, string r15, string m15, string r16, string m16, string r17, string m17, string r18, string m18, string r19, string m19, string r20, string m20, string r21, string m21, string r22, string m22, string r23, string m23, string r24, string m24, string r25, string m25)
        {
            this.modelFact.GNomina(asgn_id, r1, m1, r2, m2, r3, m3, r4, m4, r5, m5, r6, m6, r7, m7, r8, m8, r9, m9, r10, m10, r11, m11, r12, m12, r13, m13, r14, m14, r15, m15, r16, m16, r17, m17, r18, m18, r19, m19, r20, m20, r21, m21, r22, m22, r23, m23, r24, m24, r25, m25);
        }
    public DataTable GetEstatus(string orden)
    {
        return this.modelFact.GetEstatus(orden);
    }
    public DataTable GetSegmentoRepetido(string leg)
    {
        return this.modelFact.GetSegmentoRepetido(leg);
    }
    public DataTable TieneMercancias(string leg)
    {
        return this.modelFact.TieneMercancias(leg);
    }
    public void GetMerca(string Ai_orden, string segmentod, string Av_cmd_code, string Av_cmd_description, string Af_weight, string Av_weightunit, string Af_count, string Av_countunit)
    {
        this.modelFact.GetMerca(Ai_orden, segmentod, Av_cmd_code, Av_cmd_description, Af_weight, Av_weightunit, Af_count, Av_countunit);
    }
    public void DeleteMerca(string segmentod)
    {
        this.modelFact.DeleteMerca(segmentod);
    }
    public DataTable ExisteSegmento(string leg)
    {
            return this.modelFact.ExisteSegmento(leg);
        }
        public DataTable UpdateLeg(string leg, string tipom)
        {
            return this.modelFact.UpdateLeg(leg, tipom);
        }
        public DataTable VerErrores(string leg)
        {
            return this.modelFact.VerErrores(leg);
        }

        public DataTable facturasClientes()
        {
            return this.modelFact.getFacturasClientes();
        }

        public DataTable facturasGeneradas()
        {
            return this.modelFact.getFacturasGeneradas();
        }


        public DataTable FacturasPorProcesar(string billto)
        {
            return this.modelFact.getFacturasPorProcesar(billto);
        }

        public DataTable FacturasPorProcesarLiverpool()
        {
            return this.modelFact.getFacturasPorProcesarLivepool();
        }

        public DataTable detalleFacturas(string fact)
        {
            return this.modelFact.getDatosFacturas(fact);
        }

        public DataTable FacturaFacturaAdendaReferencia(string orden)
        {
            return this.modelFact.getFacturaAdendaReferencia(orden);
        }

        public DataTable detalle(string p)
        {
            return this.modelFact.getDetalle(p);
        }

        public DataTable detalle33p(string p)
        {
            return this.modelFact.getDetalle33(p);
        }

        public DataTable estatus(string fact)
        {
            return this.modelFact.getInvoice(fact);
        }

        public void actualizaFactura(string fact, string comprobante, int mbnumber)
        {
            this.modelFact.updateFactura(fact, comprobante, mbnumber);
        }
        public void enviarNotificacion(string leg, string titulo, string mensaje)
        {
            this.modelFact.enviarNotificacion(leg, titulo, mensaje);
        }
        public void RegEjecucion()
        {
            this.modelFact.RegEjecucion();
        }

        public string minInvoice(string ivh)
        {
            DataTable lastInvoice = this.modelFact.getLastInvoice(ivh);
            if (lastInvoice.Rows.Count != 0 && lastInvoice != null)
                return lastInvoice.Rows[0].ItemArray[0].ToString();
            return "";
        }

        public string facturaValida(string ivh)
        {
            string str = this.minInvoice(ivh);
            if (str.Equals(""))
                return ivh;
            return str;
        }
        public void correcionGeneradas(

      string fact,
      string serie,

      string rutaPdf,
      string rutaXML,

      string UID
      )
        {
            this.modelFact.correcionGeneradas(fact, serie, rutaPdf, rutaXML, UID);
        }


        public void generadas(
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
          string Destino
      )
        {
            this.modelFact.actualizaGeneradas(folioFactura, serieFactura, uuidFactura, pdf_xml_descargaFactura, pdf_descargaFactura, xlm_descargaFactura, cancelFactura, LegNum, Fecha, Total, Moneda, RFC, Origen, Destino);
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
         string Destino
     )
        {
            this.modelFact.insertfaltantes(folioFactura, serieFactura, uuidFactura, pdf_xml_descargaFactura, pdf_descargaFactura, xlm_descargaFactura, cancelFactura, LegNum, Fecha, Total, Moneda, RFC, Origen, Destino);
        }

        public void ErroresgeneradasCP(
            string Fecha,
            string Folio,
            string Erro1,
            string Erro2,
            string Erro3,
            string Erro4,
            string Erro5,
            string Erro6,
            string Erro7
    )
        {
            this.modelFact.ErrorGeneradasCP(Fecha, Folio, Erro1, Erro2, Erro3, Erro4, Erro5, Erro6, Erro7);
        }
    }
}