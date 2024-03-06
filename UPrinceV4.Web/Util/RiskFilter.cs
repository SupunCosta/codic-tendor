using System.Collections.Generic;
using System.Linq;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Util;

public class RiskFilter
{
    public IEnumerable<Risk> FilterTitle(string title, IEnumerable<Risk> riskList)
    {
        riskList = riskList.Where(p => p.Title != null && p.Title.ToLower().Contains(title.ToLower().Replace("'","''")));
        return riskList;
    }

    public IEnumerable<Risk> FilterByType(string typeId, IEnumerable<Risk> riskList)
    {
        riskList = riskList.Where(p => p.RiskTypeId != null && p.RiskTypeId.Equals(typeId));
        return riskList;
    }

    public IEnumerable<Risk> FilterByState(string stateId, IEnumerable<Risk> riskList)
    {
        riskList = riskList.Where(p => p.RiskStatusId != null && p.RiskStatusId.Equals(stateId));
        return riskList;
    }
}