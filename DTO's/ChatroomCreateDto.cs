namespace breakincycleapi.DTO_s
{
    public class ChatroomCreateDto
    {
        // This is the only data the user needs to provide
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
