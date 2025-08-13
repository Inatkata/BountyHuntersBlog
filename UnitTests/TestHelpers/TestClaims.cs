using System.Security.Claims;

namespace BountyHuntersBlog.UnitTests.TestHelpers
{
    public static class TestClaims
    {
        public static ClaimsPrincipal MakeUser(string userId = "user-1", bool isAdmin = false)
        {
            var id = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, "tester"),
                new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
            }, "TestAuth");
            return new ClaimsPrincipal(id);
        }
    }
}