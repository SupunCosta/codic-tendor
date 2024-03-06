namespace UPrinceV4.Web.Models;

public class CabPersonFilterModel
{
    public string FullName { get; set; }
    public string JobTitle { get; set; }
    public string Organisation { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }

    public string IsSaved { get; set; }

    //public string LandPhoneNumber { get; set; }
    //public string WhatsApp { get; set; }
    //public string Skype { get; set; }
    public CabPersonSortingModel CabPersonSortingModel { get; set; }
}

public class OrganizationCabPersonFilterModel
{
    public string FullName { get; set; }
    public string JobTitle { get; set; }
    public string Organisation { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }

    public string IsSaved { get; set; }

    //public string LandPhoneNumber { get; set; }
    //public string WhatsApp { get; set; }
    //public string Skype { get; set; }
    public CabPersonSortingModel CabPersonSortingModel { get; set; }

    public string ParentId { get; set; }
    public string ComId { get; set; }
}

public class CabPersonSortingModel
{
    public string Attribute { get; set; }
    public string Order { get; set; }
}

public class OrganizationCabPersonFilterDto
{
    public string CuId { get; set; }
    public string BuID { get; set; }
    public string FullName { get; set; }
    public CabPersonSortingModel CabPersonSortingModel { get; set; }
    public string ComId { get; set; }
    public string TeamId { get; set; }
}