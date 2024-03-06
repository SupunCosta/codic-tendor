using System.Threading.Tasks;
using UPrinceV4.Web.Data.CPC;
using UPrinceV4.Web.Repositories.Interfaces.CPC;

namespace UPrinceV4.Web.Repositories.CPC;

public class CpcNicknameRepository : ICpcNicknameRepository
{
    public async Task<string> UpdateNickname(CpcNicknameParameters nicknameParameters)
    {
        var nickNameObj = new CpcResourceNickname
        {
            CoperateProductCatalogId = nicknameParameters.CpcNicknameDto.CoperateProductCatalogId,
            NickName = nicknameParameters.CpcNicknameDto.NickName,
            Language = nicknameParameters.CpcNicknameDto.Language,
            LocaleCode = nicknameParameters.CpcNicknameDto.LocaleCode,
            Id = nicknameParameters.CpcNicknameDto.Id
        };
        nicknameParameters.Context.CpcResourceNickname.Update(nickNameObj);
        await nicknameParameters.Context.SaveChangesAsync();
        return nickNameObj.Id;
    }
}