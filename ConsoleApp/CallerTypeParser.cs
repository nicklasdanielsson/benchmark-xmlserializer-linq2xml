using System.Text.RegularExpressions;

namespace ConsoleApp;

public class CallerTypeParser : ICallerTypeParser
{
    public CallerType Parse(string userId)
    {
        if (userId.StartsWith("18") || userId.StartsWith("19") || userId.StartsWith("20"))
            return CallerType.PERSNR;

        var isValidBAccount = Regex.IsMatch(userId.ToUpper(), @"^B[0-9]{2,3}[a-zA-Z]{2,3}$") || Regex.IsMatch(userId.ToUpper(), @"^\d*[a-zA-Z0-9 ]*$");
        return isValidBAccount ? CallerType.HNDKOD : CallerType.VIPIDE;
    }
}