using System.Threading.Tasks;
using UPrinceV4.Web.Data;
using UPrinceV4.Web.Data.CPC;

namespace UPrinceV4.Web.Repositories.Interfaces.CPC;

public interface ICpcNicknameRepository
{
    Task<string> UpdateNickname(CpcNicknameParameters nicknameParameters);
}

public class CpcNicknameParameters
{
    public ApplicationDbContext Context { get; set; }
    public CpcResourceNicknameCreateDto CpcNicknameDto { get; set; }
    public string ContractingUnitSequenceId { get; set; }
    public string ProjectSequenceId { get; set; }
}