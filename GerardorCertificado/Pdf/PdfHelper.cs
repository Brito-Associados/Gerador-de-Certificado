using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace GerardorCertificado.Pdf
{
    public class PdfHelper
    {
        #region Properties
        private readonly Document _doc;
        private readonly PdfContentByte _contentByte;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Estilo", "IDE0044:Adicionar modificador somente leitura", Justification = "Não ajuda")]
        private PdfWriter pdf;
        #endregion

        internal PdfHelper(Document doc, MemoryStream ms)
        {
            this._doc = doc;
            pdf = PdfWriter.GetInstance(doc, ms);
            pdf.CloseStream = false;
            this._doc.Open();
            this._contentByte = pdf.DirectContent;
        }

        #region Text
        private void Text(string text, string nameFont, int sizeFont, float positionX, float positionY, BaseColor color, int align)
        {
            var font = GetFont(nameFont);
            this._contentByte.SetColorFill(color);
            this._contentByte.SetFontAndSize(font, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(align, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        internal void TextCenter(string text, string nameFont, int sizeFont, float positionX, float positionY)
            => Text(text, nameFont, sizeFont, positionX, positionY, BaseColor.Black, Element.ALIGN_CENTER);

        private BaseFont GetFont(string font)
            => BaseFont.CreateFont(font, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        #endregion

        #region Image and formats

        internal void AddImage(Image image, float absoluteX, float absoluteY, float width, float height)
        {
            image.ScaleToFit(width, height);
            image.SetAbsolutePosition(absoluteX, absoluteY);

            this._doc.Add(image);
        }

        #endregion

        #region Position
        internal float CenterX()
            => _doc.PageSize.Width / 2;
      
        #endregion
    }
}
