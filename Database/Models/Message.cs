using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class Message
{
    public long MessageId { get; set; }

    public Guid Roomid { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Message1 { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public virtual Chatroom Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
