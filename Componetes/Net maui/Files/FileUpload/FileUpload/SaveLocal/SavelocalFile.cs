
using FileUpload.PickFile;
using Microsoft.Maui.Storage;

namespace FileUpload.SaveLocal;

public class SavelocalFile
{
    /// <summary>
    /// Save file e local path and return local path
    /// </summary>
    /// <param name="fileName">File name</param>s
    /// <param name="stream"></param>
    /// <returns></returns>
    public async Task UploadLocalAsync()
    {
        var fileResult = await PickerFileHelper.PickerImage();
        var stream = await fileResult.OpenReadAsync();


        var path = Path.Combine(FileSystem.AppDataDirectory, fileResult.FileName);

        using var fg = new FileStream(path, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fg);
    }
}
