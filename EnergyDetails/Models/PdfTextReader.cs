//using System;
//using System.Web;
//using System.IO;
//using System.Text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;

//namespace EnergyDetails.Models
//{
//    public class PdfTextReader
//    {
//        private string GetTextFromPDF()
//        {
//            //Server.MapPath("~/Content/") + Request.Files["file"].FileName;
//            //PdfReader reader = new PdfReader((HttpContext.Current.Server.MapPath("~/Content/") + Request.Files["file"].FileName));
//            PdfReader pdfReader = new PdfReader((HttpContext.Current.Server.MapPath("~/Pdf/billconvert.pdf")));
//            // PdfReader pdfReader = new PdfReader(files[fileIndex]);
//            //for (int pageNumber = 1; pageNumber <= pdfReader.NumberOfPages; pageNumber++)
//            //{
//            //    PdfReader pdf = new PdfReader(reader);
//            //    PdfDictionary pg = pdf.GetPageN(pageNumber);
//            //    PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
//            //    PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
//            //    foreach (PdfName name in xobj.Keys)
//            //    {
//            //        PdfObject obj = xobj.Get(name);
//            //        if (obj.IsIndirect())
//            //        {
//            //            PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
//            //            string width = tg.Get(PdfName.WIDTH).ToString();
//            //            string height = tg.Get(PdfName.HEIGHT).ToString();
//            //            ImageRenderInfo imgRI = ImageRenderInfo.CreateForXObject(new Matrix(float.Parse(width), float.Parse(height)), (PRIndirectReference)obj, tg);
//            //           // RenderImage(imgRI);
//            //        }
//            //    }
//            //}

//            //for (int page = 1; page <= pdfReader.NumberOfPages; page++)
//            //{
//            //    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
//            //    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

//            //    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
//            //    //text.Append(currentText);
//            //}
//            //pdfReader.Close();

//            int intPageNum = reader.NumberOfPages;
//            string[] words;
//            string line;
//            string text;

//            for (int i = 1; i <= intPageNum; i++)
//            {
//                text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

//                words = text.Split('\n');
//                int index = Array.IndexOf(words, 11);
//                // int keyIndex = Array.FindIndex(words, w => w.IsKey);
//                string MeterId, PreviousReading, CurrentValue, PreviousTimestamp, CurrentTimestamp;
//                for (int j = 0, len = words.Length; j < len; j++)
//                {
//                    if (j == 36 && i == 2)
//                    {
//                        //string MeterId = words[j];
//                        string[] meterids = words[j].Split(' ');
//                        for (int k = 0; k < meterids.Length; k++)
//                        {
//                            if (k == 2)
//                            {
//                                MeterId = meterids[k];
//                            }
//                        }
//                    }
//                    if (j == 35 && i == 2)
//                    {
//                        string[] meterids = words[j].Split(' ');
//                        for (int l = 0; l < meterids.Length; l++)
//                        {
//                            if (l == 2)
//                            {
//                                PreviousReading = meterids[l];
//                            }
//                            if (l == 0)
//                            {
//                                CurrentValue = meterids[l];
//                            }
//                        }
//                    }
//                    if (j == 31 && i == 2)
//                    {
//                        //string MeterId = words[j];
//                        string[] meterids = words[j].Split(' ');
//                        for (int m = 0; m < meterids.Length; m++)
//                        {
//                            if (m == 0)
//                            {
//                                PreviousTimestamp = meterids[m];
//                            }
//                            if (m == 1)
//                            {
//                                CurrentTimestamp = meterids[m];
//                            }
//                        }
//                    }

//                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));
//                    char[] delimiters = new[] { ',', ';', ' ' };  // List of your delimiters
//                    var splittedArray = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
//                }


//            }
//            //StringBuilder text = new StringBuilder();
//            //using (PdfReader reader = new PdfReader((Server.MapPath("~/Pdf/PO_23078707_1ph_Meter_Genus.pdf"))))
//            //{ 
//            //    for (int i = 1; i <= reader.NumberOfPages; i++)
//            //    {
//            //        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
//            //    }
//            //}

//            return "";
//        }
//    }
//}