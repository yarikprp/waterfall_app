using System;
using System.Collections.Generic;

namespace waterfall_app.Classes;

public partial class Shedule
{
    public int IdShedule { get; set; }

    public DateTime EntranceTime { get; set; }

    public int? NumberOfPeople { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
