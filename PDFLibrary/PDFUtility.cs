using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Drawing = System.Drawing;
using System.Xml.Serialization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PDFLibraryCabcon
{
   
    public class PDFUtility
    {
        //Sets image resources for green line and lng Logo
        Drawing.Bitmap bitmapLogo = Resources.LogowithoutTagline_Transparent_;
        Drawing.Bitmap bitmapLine = Resources.L_Ggreenbar;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="destinationPath"></param>
        /// <param name="bLandscape"></param>
        /// <param name="headerDict"></param>
        /// <returns></returns>
        public string createAndSavePDF(DataSet ds, string destinationPath, bool bLandscape, Dictionary<string,string> headerDict)
        {
            Document doc = createPDF(destinationPath, bLandscape);

            doc.Open();

            PdfPTable table = new PdfPTable((int)Math.Ceiling((decimal)headerDict.Count/2));
            table.WidthPercentage = 100;

            foreach (var key in headerDict.Keys)
            {
                AddHeaderCell(table, key + " : " + headerDict[key], 1, 1, 0, System.Drawing.Color.FromArgb(235, 238, 244));
            }

            doc.Add(table);

            ExportDSToPdf(ds, doc);

            doc.Close();
            return "";
        }



        /// <summary>
        /// Creates a new pdf with logo using provided filepath and returns the document 
        /// </summary>
        /// <param name="destinationPath"></param>
        public Document createPDF(string destinationPath, bool bLandscape)
        {

            try
            {
                Document document = new Document(PageSize.A4, 35f, 35f, 50f, 50f);
                if (bLandscape) document.SetPageSize(PageSize.A4.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));

                //if(File.Exists(linepath) && File.Exists(logopath))
                     _streamPdf(writer, document);   //add logo and green bar at top
                

                return document;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Exception", MessageBoxButtons.OK);
                throw ex;
            }
  
        }

        /// <summary>
        /// Exports a DataTable to a PDF via provided document object
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="destinationPath"></param>
        public void ExportDSToPdf(DataSet ds, Document document)
        {
            //Document document = new Document(PageSize.A4, 35f, 35f, 50f, 50f);
            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationPath, FileMode.Create));
            
            //_streamPdf(writer, document);

            //HeaderFooter header = new HeaderFooter(new Phrase("PDF HEADER"), false);
            //// Remove the border that is set by default  
            //header.Border = Rectangle.TITLE;
            //// Align the text: 0 is left, 1 center and 2 right.  
            //header.Alignment = Element.ALIGN_CENTER;
            //header.BackgroundColor = new Color(Drawing.ColorTranslator.FromHtml("#EEEEEE")); ;
            //document.Header = header;
            //// Header. 

            //document.Open();

            //float curY = writer.GetVerticalPosition(false);
            //if (curY < 500)
            //    document.NewPage();

            try
            {
                foreach (DataTable dataTable in ds.Tables)
                {
                    PdfPTable table = new PdfPTable(dataTable.Columns.Count);
                    table.WidthPercentage = 100;

                    if (ds.Tables[0].TableName.Equals("tabErrorTypeMaster"))
                    {
                        float[] widths = new float[] { 1f, 1f, 3f, 5f };
                        table.SetWidths(widths);
                    }

                    //leave a gap before and after the table
                    table.SpacingBefore = 20f;
                    table.SpacingAfter = 10f;
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;
                    /////////////////////////////////////////////////

                    //Set columns names in the pdf file
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        AddHeaderCell(table, column.ColumnName, 1, 1, 0, System.Drawing.Color.FromArgb(235, 238, 244));
                    }

                    //Add values of DataTable in pdf file
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    Font times = new Font(bfTimes, 10, Font.NORMAL, Color.BLACK);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            PdfPCell cell;

                            if (column.DataType == typeof(DateTime))
                            {
                                cell = new PdfPCell(new Phrase(((DateTime)row[column]).ToString("dd/MM/yyyy hh:mm:ss tt"), times));
                            }
                            else
                            {
                                cell = new PdfPCell(new Phrase(row[column].ToString(), times));
                            }
                            
                            //Align the cell in the center
                            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            cell.Padding = 4;
                            table.AddCell(cell);
                        }
                    }

                    document.Add(table);

                }

                //draw a line after each xml//

                PdfPTable tableline = new PdfPTable(1);
                tableline.WidthPercentage = 100;
                tableline.SpacingBefore = 20f;
                tableline.SpacingAfter = 10f;
                tableline.HorizontalAlignment = Element.ALIGN_CENTER;
                //float curY = writer.GetVerticalPosition(false);
                PdfPCell cellline = new PdfPCell(new Phrase(""));
                cellline.Border = 0;
                cellline.BorderColorTop = new Color(System.Drawing.Color.Black);
                cellline.BorderWidthTop = 2f;
                tableline.AddCell(cellline);

                document.Add(tableline);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Exception", MessageBoxButtons.OK);
                throw ex;
            } 
        }

        /// <summary>
        /// Add header cell to PdfPtable with corresponding parameters
        /// </summary>
        /// <param name="table"></param>
        /// <param name="text"></param>
        /// <param name="rowspan"></param>
        /// <param name="colspan"></param>
        /// <param name="rotation"></param>
        /// <param name="backColor"></param>
        private void AddHeaderCell(PdfPTable table, string text, int rowspan, int colspan, int rotation, System.Drawing.Color backColor)
        {
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.BLACK);

            PdfPCell cell = new PdfPCell(new Phrase(text, times));
            cell.Rowspan = rowspan;
            cell.Colspan = colspan;
            cell.Rotation = rotation;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.BackgroundColor = new Color(backColor);
            table.AddCell(cell);
        }

        /// <summary>
        /// Add Cabcon Technologies Logo for each page for given writer,document object
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public void _streamPdf(PdfWriter writer, Document document)
        {
                // the image we're using for the page header      
            
                Image imageHeader = Image.GetInstance(
                   bitmapLogo, Drawing.Imaging.ImageFormat.Png
                );

                Image imageHeader2 = Image.GetInstance(
                      bitmapLine, Drawing.Imaging.ImageFormat.Png
                    );

                // instantiate the custom PdfPageEventHelper
                MyPageEventHandler e = new MyPageEventHandler()
                {
                    ImageHeader = imageHeader,
                    ImageHeader2 = imageHeader2
                };
                // and add it to the PdfWriter
                writer.PageEvent = e;
                document.Open();
         }

    }
    
    class MyPageEventHandler : PdfPageEventHelper
    {
        /*
         * We use a __single__ Image instance that's assigned __once__;
         * the image bytes added **ONCE** to the PDF file. If you create 
         * separate Image instances in OnEndPage()/OnEndPage(), for example,
         * you'll end up with a much bigger file size.
         */
         public Image ImageHeader { get; set; }
         public Image ImageHeader2 { get; set; }
        /// <summary>
        /// Overrides what happens when a page ends 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // cell height 
            float cellHeight = document.TopMargin -30;
            // PDF document size      
            Rectangle page = document.PageSize;

            // create two column table
            PdfPTable head = new PdfPTable(7);
            head.TotalWidth = page.Width;

            // add the left header text
            PdfPCell c = new PdfPCell(ImageHeader2, true);
            //PdfPCell c = new PdfPCell(new Phrase("PDF Header", new Font(Font.COURIER, 12, Font.BOLD)));
            c.Colspan = 6;
            c.Border = PdfPCell.NO_BORDER;
            c.VerticalAlignment = Element.ALIGN_CENTER;
            c.FixedHeight = cellHeight;
            head.AddCell(c);

            // add the middle header Text
            //c = new PdfPCell(new Phrase(
            //  "PDF Header",
            //  new Font(Font.COURIER, 12, Font.BOLD)
            //));
            //c.Border = PdfPCell.NO_BORDER;
            //c.HorizontalAlignment = Element.ALIGN_CENTER;
            //c.VerticalAlignment = Element.ALIGN_MIDDLE;
            //c.FixedHeight = cellHeight;
            //head.AddCell(c);

            // add image; PdfPCell() overload sizes image to fit cell
            //c = new PdfPCell(ImageHeader, true);
            c = new PdfPCell();
            c.FixedHeight = cellHeight;
            c.Border = PdfPCell.NO_BORDER;
            ImageHeader.ScaleAbsolute(50f, 20f);
            c.AddElement(ImageHeader);
            c.HorizontalAlignment = Element.ALIGN_RIGHT;
            c.VerticalAlignment = Element.ALIGN_BOTTOM;
            head.AddCell(c);

            // since the table header is implemented using a PdfPTable, we call
            // WriteSelectedRows(), which requires absolute positions!
            head.WriteSelectedRows(
              0, -1,  // first/last row; -1 flags all write all rows
              0,      // left offset
                // ** bottom** yPos of the table
              page.Height - cellHeight + head.TotalHeight,
              writer.DirectContent
            );
        }

        
    }
}
