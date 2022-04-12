using CAS_API.Models.Interfaces;

namespace CAS_API.Models
{
#pragma warning disable 1591
    public class ApiSettings : IApiSettings
    {
        public string FileFolder { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
#pragma warning restore 1591
}
