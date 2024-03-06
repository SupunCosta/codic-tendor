namespace UPrinceV4.Web.Util;

public class UploadData
{
    public UploadData()
    {
        Uploaded = 1;
    }

    public int Uploaded { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
}