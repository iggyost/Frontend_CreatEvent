using System;
using System.Collections.Generic;

namespace Frontend_CreatEvent.ApplicationData;

public partial class RequestsCartDish
{
    public int Id { get; set; }

    public int RequestId { get; set; }

    public int CartDishesId { get; set; }

    public virtual CartsDish CartDishes { get; set; } = null!;

    public virtual Request Request { get; set; } = null!;
}
