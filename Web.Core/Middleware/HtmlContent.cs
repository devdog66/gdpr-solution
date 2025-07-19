namespace Web.Core.Middleware
{
    public class HtmlContent
    {
        private readonly IWebHostEnvironment _env;

        public HtmlContent(IWebHostEnvironment env)
        {
            _env = env;
        }

        public virtual string Get(string relativePath)
        {
            var filePath = _env.ContentRootPath + relativePath;
            var markup = File.ReadAllText(filePath);
            return markup;
        }
    }
}
