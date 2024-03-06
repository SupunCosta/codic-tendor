using System.Collections.Generic;
using System.Threading.Tasks;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Repositories.Interfaces;

public interface IUPrinceCustomerContactPreferenceRepository
{
    Task<IEnumerable<UPrinceCustomerContactPreference>> GetUPrinceCustomerContactPreferences(
        UPrinceCustomerContex uprinceCustomerContext);

    Task<UPrinceCustomerContactPreference> GetUPrinceCustomerContactPreferenceById(
        UPrinceCustomerContex uprinceCustomerContext, int id);

    Task<int> AddUPrinceCustomerContactPreference(UPrinceCustomerContex uprinceCustomerContext,
        UPrinceCustomerContactPreference uprinceCustomerContactPreference);

    Task<UPrinceCustomerContactPreference> UpdateUPrinceCustomerContactPreference(
        UPrinceCustomerContex uprinceCustomerContexContext,
        UPrinceCustomerContactPreference uprinceCustomerContactPreference);

    bool DeleteUPrinceCustomerContactPreference(UPrinceCustomerContex uprinceCustomerContext, int id);

    Task<IEnumerable<UprinceCustomerContactPreferenceHistory>> GetUprinceCustomerContactPreferenceHistory(
        UPrinceCustomerContex context);
}