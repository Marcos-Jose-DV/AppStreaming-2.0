using System.IO;

namespace FileUpload.PickFile;

public class PickerFileHelper
{

    /// <summary>
    /// Get file, custom FIle Type options FilePickerFileType
    /// </summary>
    /// <returns></returns>
    public async static Task<FileResult> PickerImage(FilePickerFileType customFileType = null)
    {
        try
        {
            PickOptions options = new()
            {
                PickerTitle = "Selecione o arquivo",
                FileTypes = customFileType,
            };

            var fileResult = await FilePicker.PickAsync(options);

            if (fileResult == null) return null;

            return fileResult;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
