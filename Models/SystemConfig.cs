using System.ComponentModel.DataAnnotations;

namespace tinderr.Models
{
    public class SystemConfig
    {
        public string webName { get; set; }
        public string LoGoWeb { get; set; }
        public string OrtherLinkSupport { get; set; }
        public bool CSKH { get; set; }
        public bool ObligatoryBalance { get; set; }
    }
}
