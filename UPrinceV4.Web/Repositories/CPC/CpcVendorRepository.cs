using System.Threading.Tasks;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Repositories.CPC;

public class CpcVendorRepository : ICpcVendorRepository
{
    public async Task<string> UpdateVendor(CpcVendorParameters cpcParameters)
    {
        var vendor = new CpcVendor
        {
            CoperateProductCatalogId = cpcParameters.CpcVendorDto.CoperateProductCatalogId,
            MaxOrderQuantity = cpcParameters.CpcVendorDto.MaxOrderQuantity,
            MinOrderQuantity = cpcParameters.CpcVendorDto.MinOrderQuantity,
            PurchasingUnit = cpcParameters.CpcVendorDto.PurchasingUnit,
            ResourceLeadTime = cpcParameters.CpcVendorDto.ResourceLeadTime,
            ResourceNumber = cpcParameters.CpcVendorDto.ResourceNumber,
            ResourcePrice = cpcParameters.CpcVendorDto.ResourcePrice,
            ResourceTitle = cpcParameters.CpcVendorDto.ResourceTitle,
            RoundingValue = cpcParameters.CpcVendorDto.RoundingValue,
            PreferredParty = cpcParameters.CpcVendorDto.PreferredParty,
            Id = cpcParameters.CpcVendorDto.Id
        };
        cpcParameters.Context.CpcVendor.Update(vendor);
        await cpcParameters.Context.SaveChangesAsync();

        return vendor.Id;
    }
}