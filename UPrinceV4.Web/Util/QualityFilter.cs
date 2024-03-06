using System.Collections.Generic;
using System.Linq;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Util;

public class QualityFilter
{
    public IEnumerable<Quality> FilterTitle(string title, IEnumerable<Quality> qualityList)
    {
        qualityList = qualityList.Where(p => p.Title != null && p.Title.ToLower().Contains(title.ToLower().Replace("'","''")));
        return qualityList;
    }
}