using System.ComponentModel.DataAnnotations;

namespace UPrinceV4.Web.Data;

public class Location
{
    [Required] public string Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double HorizontalAccuracy { get; set; }
    public double VerticleAccuracy { get; set; }
    public double Speed { get; set; }
    public double Heading { get; set; }
}

public class createLocationDto
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Altitude { get; set; }

    public double HorizontalAccuracy { get; set; }

    public double VerticleAccuracy { get; set; }

    public double Speed { get; set; }

    public double Heading { get; set; }
}