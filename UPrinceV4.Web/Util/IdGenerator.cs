using System;
using System.Linq;
using UPrinceV4.Web.Data;

namespace UPrinceV4.Web.Util;

public class IdGenerator
{
    public string GenerateId(ApplicationDbContext context, string prefix, string key)
    {
        var property = context.Properties.FirstOrDefault(p => p.Key == key);
        string last;
        if (property == null)
        {
            property= new Properties { Key = key, Value = prefix + "0000" };
            context.Properties.Add(property);
            context.SaveChanges();
            last = prefix + "0000";
        }
        else
        {
            last = property.Value;
        }

        var len = last.Length;
        //string prefix = last.Substring(0, 1);
        //string numString = last.Substring(1, len - prefix.Length).TrimStart(new Char[] { '0' });
        var numString = last.Replace(prefix, "");
        var nextNum = int.Parse(numString) + 1;
        var numberLength = 0;
        if (nextNum == 0)
            numberLength = 1;
        else
            numberLength = (int)Math.Floor(Math.Log10(nextNum)) + 1;

        var numOfMissingZero = len - prefix.Length - numberLength;
        var zeroString = "";
        for (var i = 1; i <= numOfMissingZero; i++) zeroString += "0";

        var sqNumber = prefix + zeroString + nextNum;

        property.Value = sqNumber;
        context.Properties.Update(property);
        context.SaveChanges();


        return sqNumber;
    }
}