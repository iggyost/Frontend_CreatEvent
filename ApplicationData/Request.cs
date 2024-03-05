﻿using System;
using System.Collections.Generic;

namespace Frontend_CreatEvent.ApplicationData;

public partial class Request
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? Time { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<RequestsCartDish> RequestsCartDishes { get; } = new List<RequestsCartDish>();

    public virtual Status? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
