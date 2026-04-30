using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace breakincycleapi.Database.Models;

public partial class Chatroom
{
    public Guid Roomid { get; set; }

    public string Name { get; set; } = null!;

    public Guid Userid { get; set; }

    public DateTime JoinedAt { get; set; }

    public string? MessageSenderName { get; set; } 
    public string? MessageContent { get; set; }
    public string? Description { get; internal set; }
    public long? MessageId { get; set; }

    [ForeignKey("MessageId")]
    public virtual Message? LastMessage { get; set; } // The link to the Message object
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
