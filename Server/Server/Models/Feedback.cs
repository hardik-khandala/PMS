using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Employee Receiver { get; set; } = null!;

    public virtual Employee Sender { get; set; } = null!;
}
