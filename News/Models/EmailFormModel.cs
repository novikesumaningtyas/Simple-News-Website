using System.ComponentModel.DataAnnotations;
using System.Web;

namespace News.Models
{
    public class EmailFormModel
    {

        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        public HttpPostedFileBase Upload { get; set; }
    }
}