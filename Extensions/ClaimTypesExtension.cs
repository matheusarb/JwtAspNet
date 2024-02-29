
using System.Security.Claims;

namespace JwtAspNet.Extensions;

public static class ClaimTypesExtension
{
    public static int Id(this ClaimsPrincipal user)
    {
        try
        {
            var id = user.Claims.FirstOrDefault(x => x.Type == "Id").Value ?? "0";
            return int.Parse(id);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static string Name(this ClaimsPrincipal user)
    {
        try
        {
            var name = user.Claims.FirstOrDefault(x => x.Type == "Name").Value ?? "";
            return name;
        }
        catch (Exception)
        {
            return "";
        }
    }
}