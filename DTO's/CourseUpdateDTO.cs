namespace breakincycleapi.DTO_s
{
    public class CourseUpdateDTO
    {
        public Guid CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
