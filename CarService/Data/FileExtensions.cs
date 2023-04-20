namespace CarService.Data;

public static class FileExtensions
{
    public static byte[] GetImageData(string fileName)
    {
        var imagesDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Images"));
        var imagePath = Path.Combine(imagesDirectory, fileName);
        return File.ReadAllBytes(imagePath);
    }

}