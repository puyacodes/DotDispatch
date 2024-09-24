namespace DotDispatch
{
    public class ProjectVersionProvider
    {
        string _version;
        string _projectname;
        public ProjectVersionProvider(string projectname)
        {
            _projectname = projectname;
        }
        public string GetVersion()
        {
            if (string.IsNullOrEmpty(_version))
            {
                _version = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => string.Equals(a.GetName().Name, _projectname, StringComparison.OrdinalIgnoreCase))?.GetName().Version.ToString();
            }

            return _version;
        }
    }
}
