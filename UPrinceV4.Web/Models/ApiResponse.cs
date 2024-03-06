using Newtonsoft.Json;

namespace UPrinceV4.Web.Models;

public class ApiResponse
{
    public ApiResponse(int statusCode, bool status, string message = null)
    {
        StatusCode = statusCode;
        Status = status;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        Result = null;
        Errors = null;
    }

    public int StatusCode { set; get; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { set; get; }

    public bool Status { set; get; }

    public object Result { set; get; }

    public object Errors { set; get; }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        switch (statusCode)
        {
            case 200:
                return "Ok";
            case 204:
                return "No Content";
            case 404:
                return "Resource not found";
            case 500:
                return "An unhandled error occurred";
            case 401:
                return "Unauthorized";
            case 400:
                return "Bad Request";
            default:
                return null;
        }
    }
}