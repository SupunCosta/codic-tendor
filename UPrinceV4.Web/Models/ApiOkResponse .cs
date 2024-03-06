namespace UPrinceV4.Web.Models;

public class ApiOkResponse : ApiResponse
{
    public ApiOkResponse(object result, string message)
        : base(200, true)
    {
        Result = result;
        Message = message;
    }

    public ApiOkResponse(object result)
        : base(200, true)
    {
        Result = result;
    }

    public object Result { get; set; }
}