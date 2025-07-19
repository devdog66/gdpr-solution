using WebUtils.Web;

namespace UnitTests.Fakes
{
    internal class TestCookie : ICookie
    {
        public string? Domain { get; set; }
        public string? Path { get; set; }
        public DateTimeOffset Expires { get; set; }
        public bool Secure { get; set; }
        public SameSiteEnum SameSite { get; set; }
        public bool HttpOnly { get; set; }
        public bool IsEssential { get; set; }
        public string? Value { get; set; }
        public string? Name { get; set; }
    }
}