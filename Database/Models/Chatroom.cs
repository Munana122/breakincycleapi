using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class Chatroom
{
    public Guid Roomid { get; set; }

    public string Name { get; set; } = null!;

    public Guid Userid { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
