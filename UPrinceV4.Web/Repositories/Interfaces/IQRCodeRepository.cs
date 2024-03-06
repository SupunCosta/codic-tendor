using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Shared;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IQrCodeRepository
{
    Task<IEnumerable<QRCode>> GetQrCode(ApplicationDbContext context, string lang,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<QRCodeDapperDto> GetTqrCodeById(ApplicationDbContext context, string id, string lang,
        ITenantProvider iTenantProvider, string ContractingUnitSequenceId, string ProjectSequenceId);

    Task<IEnumerable<QRCode>> GetQrCodeByType(ApplicationDbContext context, int type,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<string> CreateQrCode(ApplicationDbContext context, CreateQRCodeDto qrDto, string createdUserId,
        string ContractingUnitSequenceId, string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<string> UpdateQrCode(ApplicationDbContext context, UpdateQRCodeDto qrDto, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    void DeleteQrCode(ApplicationDbContext context, string id, string ContractingUnitSequenceId,
        string ProjectSequenceId, ITenantProvider iTenantProvider);

    Task<IEnumerable<QRCodeDto>> Filter(ApplicationDbContext context, QRCodeFilter filter, string lang,
        ITimeClockActivityTypeRepository iTimeClockActivityTypeRepository, ITenantProvider iTenantProvider,
        string ContractingUnitSequenceId, string ProjectSequenceId);
}