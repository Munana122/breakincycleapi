namespace breakincycleapi.DTO_s
{
    public class ProgressCreateDto
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string ProgressStatus { get; set; } = string.Empty;
    }
}
