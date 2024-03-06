using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPrinceV4.Web.Data.Mecops;

[Table("CustomerOrderToPo")]
public class Mecop
{
    public int Id { get; set; }
    public string AankoopOrder { get; set; }
    public string Lijn { get; set; }
    public string BrandWeerstand { get; set; }
    public string Type { get; set; }
    public string Materiaal { get; set; }
    public string Grondlaag { get; set; }
    public string Ral { get; set; }
    public string Ncs { get; set; }
    public string MuurDikte { get; set; }
    public string BreedteDeurlijsten { get; set; }
    public string BreedteDeurlijstenVerticaal { get; set; }
    public string BreedteDeurlijstenHorizontaal { get; set; }
    public bool OmlijstingenNummeren { get; set; }
    public string HoogteDeur { get; set; }
    public string BreedteDeur { get; set; }
    public string BreedteVleugel { get; set; }
    public string DikteDeur { get; set; }
    public string DraaiRichting { get; set; }
    public string Paumelles { get; set; }
    public string AantalPaumelles { get; set; }
    public string TegenPlaat { get; set; }
    public string SlotUitsparing { get; set; }
    public string KrukHoogte { get; set; }
    public string KabelDoorvoer { get; set; }
    public string HoogteKabelDoorvoer { get; set; }
    public string Extra1 { get; set; }
    public string Extra2 { get; set; }
    public string Extra3 { get; set; }
    public string Extra4 { get; set; }
    public string DeurNummer { get; set; }
    public string Aantal { get; set; }
    public string Eenheidsprijs { get; set; }
    public string TotaalPrijs { get; set; }
    public string Taal { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public string Status { get; set; }
    public string Klant { get; set; }
    public string Norm { get; set; }
}

public class MecopResponceData
{
    public string OdataContext { get; set; }
    public List<Mecop> Value { get; set; }
}

public class MecopDto
{
    public string Id { get; set; }
    public string AankoopOrder { get; set; }
    public string Lijn { get; set; }
    public string BrandWeerstand { get; set; }
    public string Type { get; set; }
    public string Materiaal { get; set; }
    public string Grondlaag { get; set; }
    public string Ral { get; set; }
    public string Ncs { get; set; }
    public string MuurDikte { get; set; }
    public string BreedteDeurlijsten { get; set; }
    public string BreedteDeurlijstenVerticaal { get; set; }
    public string BreedteDeurlijstenHorizontaal { get; set; }
    public bool OmlijstingenNummeren { get; set; }
    public string HoogteDeur { get; set; }
    public string BreedteDeur { get; set; }
    public string BreedteVleugel { get; set; }
    public string DikteDeur { get; set; }
    public string DraaiRichting { get; set; }
    public string Paumelles { get; set; }
    public string AantalPaumelles { get; set; }
    public string TegenPlaat { get; set; }
    public string SlotUitsparing { get; set; }
    public string KrukHoogte { get; set; }
    public string KabelDoorvoer { get; set; }
    public string HoogteKabelDoorvoer { get; set; }
    public string Extra1 { get; set; }
    public string Extra2 { get; set; }
    public string Extra3 { get; set; }
    public string Extra4 { get; set; }
    public string DeurNummer { get; set; }
    public string Aantal { get; set; }
    public string Eenheidsprijs { get; set; }
    public string TotaalPrijs { get; set; }
    public string Taal { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public string Status { get; set; }
    public string Klant { get; set; }
    public string Norm { get; set; }
    public string ParentId { get; set; }
    public string Title { get; set; }
    public string NodeLevel { get; set; }
}

public class MecopStatusUpdateDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ParentTitle { get; set; }
    public string NodeLevel { get; set; }
    public bool IsFinished { get; set; }
}