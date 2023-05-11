namespace CarService.Models;

public class Image
{
    public int Id { get; set; }
    public byte[] Data { get; set; }
    public override bool Equals(object obj)
    {
        // If the object is null, return false
        if (obj == null)
        {
            return false;
        }

        // If the object cannot be cast to an Image, return false
        if (!(obj is Image))
        {
            return false;
        }

        // Cast the object to an Image
        Image other = (Image)obj;

        // Compare the Id property
        if (Id != other.Id)
        {
            return false;
        }

        // Compare the Data property
        if (Data == null && other.Data != null)
        {
            return false;
        }
        else if (Data != null && other.Data == null)
        {
            return false;
        }
        else if (Data != null && other.Data != null && !Data.SequenceEqual(other.Data))
        {
            return false;
        }

        // If we've made it this far, the objects are equal
        return true;
    }

}