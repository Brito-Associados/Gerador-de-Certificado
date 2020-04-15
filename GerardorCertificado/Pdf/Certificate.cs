using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GerardorCertificado.Pdf
{
    public static class Certificate
    {
        public static MemoryStream Generate(string nameCertify, Stream imageBack,float positionName,int moreWidthToImage=0,int moreHeightToImage=0)
        {
            PdfHelper pdfElemment;
            var ms = new MemoryStream();
            var doc = new Document();

            doc.SetPageSize(PageSize.A4.Rotate());
            doc.SetMargins(0, 0, 0, 0);
            pdfElemment = new PdfHelper(doc, ms);
            var image = Image.GetInstance(imageBack);
            pdfElemment.AddImage(image, 0, 0, doc.PageSize.Width + moreWidthToImage, doc.PageSize.Height + moreHeightToImage);

            pdfElemment.TextCenter(nameCertify, BaseFont.HELVETICA_BOLD, 25, pdfElemment.CenterX(), positionName);

            doc.Close();

            var msInfo = ms.ToArray();
            ms.Write(msInfo, 0, msInfo.Length);

            ms.Position = 0;
            return ms;

        }


    }

    
}
