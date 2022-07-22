using System.ComponentModel.DataAnnotations;

namespace akkasimulator.Data
{
    public class CreateInstanceModel
    {
        [Required]
        public string? Name { get; set; }
    }
}
