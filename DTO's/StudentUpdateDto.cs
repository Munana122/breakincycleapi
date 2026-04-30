using Microsoft.AspNetCore.Components.Routing;

namespace breakincycleapi.DTO_s
{
    public class StudentUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Location { get; set; }
    }
}
