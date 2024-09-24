namespace DotDispatch
{
    public class DotDispatchOptions
    {
        public string Root { get; set; }
        public string WebPath { get; set; }
        public string ContentFile { get; set; }
        public void Validate()
        {
            if (string.IsNullOrEmpty(Root))
            {
                Root = Environment.CurrentDirectory;
            }
            if (string.IsNullOrEmpty(WebPath))
            {
                WebPath = "wwwroot";
            }
            if (string.IsNullOrEmpty(ContentFile))
            {
                ContentFile = "index.html";
            }
        }
    }
}
