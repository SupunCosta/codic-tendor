namespace UPrinceV4.Web.Models;

public class CommonResponse<T>
{
    public CommonResponse(T t)
    {
        data = t;
    }

    public bool status { get; set; }

    public string message { get; set; }

    public virtual T data { get; set; }
}