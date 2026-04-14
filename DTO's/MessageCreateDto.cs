namespace breakincycleapi.DTO_s
{
    public class MessageCreateDto
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
