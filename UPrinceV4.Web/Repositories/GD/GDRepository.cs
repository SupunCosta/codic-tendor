using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UPrinceV4.Web.Data.GD.Vehicle;
using UPrinceV4.Web.Repositories.Interfaces.GD;
using Urls;

//using RestClient.Net;

namespace UPrinceV4.Web.Repositories.GD;

public class GDRepository : IGDRepository
{
    private const string RestCountriesAllUriString = "https://restcountries.eu/rest/v2";

    private static readonly HttpClient client = new();

    private readonly AbsoluteUrl RestCountriesAllUri = new(RestCountriesAllUriString);

    public async Task<string> CreateToken(GDParameter GDParameter)
    {
        return Base64Encode("Administrator Bieke|Careye Andy:Betonwerken16");
    }

    public async Task<List<Vehicle>> GetVehicles(GDParameter GDParameter)
    {
        // var response = await "https://api.intellitracer.be/api/v1"
        //     .ToAbsoluteUrl()
        //     .WithCredentials()
        //     .GetAsync<List<Vehicle>>();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Content", "application/json");
        //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("Administrator Bieke|Careye Andy:Betonwerken16");
        var plainTextBytes = Encoding.UTF8.GetBytes("Webservices|Careye Andy:BetonwerkenCareye");


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(plainTextBytes));


        //client.DefaultRequestHeaders.Add("Authorization", "Basic " + CreateToken(new GDParameter()));

        var vehicle = client.GetStringAsync("https://api.intellitracer.be/api/v1/vehicle");

        //var vehicle2 =  await client.GetStringAsync("https://api.intellitracer.be/api/v1/vehicle");

        var ff = vehicle.Result;

        var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(vehicle.Result);

        return vehicles;
    }

    public async Task<List<VehiclePosition>> GetVehiclePosition(GDParameter GDParameter)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Content", "application/json");
        //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("Administrator Bieke|Careye Andy:Betonwerken16");
        var plainTextBytes = Encoding.UTF8.GetBytes("Webservices|Careye Andy:BetonwerkenCareye");


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(plainTextBytes));

        var vehiclePosition = client.GetStringAsync(
            "https://api.intellitracer.be/api/v1/location/position?resourceId=" +
            GDParameter.VehiclePositionDto.ResourceId + "&from=" +
            GDParameter.VehiclePositionDto.Date.ToString("yyyy-MM-dd") + "&to=" +
            GDParameter.VehiclePositionDto.Date.AddHours(24).ToString("yyyy-MM-dd"));

        var ff = vehiclePosition.Result;

        var vehicles = JsonConvert.DeserializeObject<List<VehiclePosition>>(vehiclePosition.Result);

        return vehicles;
    }

    public async Task<Bar[]> GetVehicleStatus(GDParameter GDParameter)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Content", "application/json");
        //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes("Administrator Bieke|Careye Andy:Betonwerken16");
        var plainTextBytes = Encoding.UTF8.GetBytes("Webservices|Careye Andy:BetonwerkenCareye");


        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(plainTextBytes));

        var vehicleStatus = await client.GetStringAsync(
            "https://api.intellitracer.be/api/v1/location/status?resourceId=" +
            GDParameter.VehiclePositionDto.ResourceId + "&from=" +
            GDParameter.VehiclePositionDto.Date.ToString("yyyy-MM-dd") + "&to=" +
            GDParameter.VehiclePositionDto.Date.AddHours(24).ToString("yyyy-MM-dd"));

        var ff = vehicleStatus;

        var vehicles = JsonConvert.DeserializeObject<VehicleStatus>(vehicleStatus);

        return vehicles?.StatusResults[0].Bars;
    }


    public static string Base64Encode(string login)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(login);
        return Convert.ToBase64String(plainTextBytes);
    }
}