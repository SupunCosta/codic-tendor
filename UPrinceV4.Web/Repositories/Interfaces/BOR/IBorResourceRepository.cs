using System.Collections.Generic;
using System.Threading.Tasks;

namespace UPrinceV4.Web.Repositories.Interfaces.BOR;

public interface IBorResourceRepository
{
    Task<string> CreateBorLabour(BorParameter borParameter, bool isUpdate);
    Task<string> CreateBorMaterial(BorParameter borParameter, bool isUpdate);
    Task<string> CreateBorConsumable(BorParameter borParameter, bool isUpdate);
    Task<string> CreateBorTools(BorParameter borParameter, bool isUpdate);

    Task<string> UpdateBorMaterial(BorParameterResoruce borParameterResoruce);

    Task<string> UpdateBorTools(BorParameterResoruce borParameterResoruce);

    Task<string> UpdateBorConsumable(BorParameterResoruce borParameterResoruce);

    Task<string> UpdateBorLabour(BorParameterResoruce borParameterResoruce);

    Task<List<string>> DeleteBorMaterial(BorParameterResoruceDelete borParameterResoruce);

    Task<List<string>> DeleteBorTools(BorParameterResoruceDelete borParameterResoruce);

    Task<List<string>> DeleteBorConsumable(BorParameterResoruceDelete borParameterResoruce);

    Task<List<string>> DeleteBorLabour(BorParameterResoruceDelete borParameterResoruce);
}