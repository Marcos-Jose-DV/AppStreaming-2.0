using AppAvaliacao.Helpers.DownloadDocx;
using AppAvaliacao.Helpers.ExcelHerper;
using AppAvaliacao.Helpers.PDFHerlper;
using AppAvaliacao.Helpers.TextHelper;
using Microsoft.Maui.Storage;
using Models.Movies;

namespace AppAvaliacao.Helpers.DownloadHelper;

public static class DownloadFileHerper
{
    public static Stream DownloadFile(string fileName, List<Assessments> assessments)
    {
        Stream stream = null;

        if (fileName.Equals("assessments.txt"))
        {
            var dowloandTextHelper = new DownloadTextHelper();
            stream = dowloandTextHelper.DownloadText(assessments);

            return stream;
        }
        if (fileName.Equals("assessments.pdf"))
        {
            var dowloandPDFHelper = new DownloadPDFHelper();
            stream = dowloandPDFHelper.DownloadPDF(assessments);

            return stream;
        }
        if (fileName.Equals("assessments.xlsx"))
        {
            var downloadFileHerper = new DownloadExcelHelper();
            stream = downloadFileHerper.DownloadExcel(assessments);

            return stream;
        }

        var downloadDocxHelper = new DownloadDocxHelper();
        stream = downloadDocxHelper.DownloadDocx(assessments);

        return stream;
    }
}
