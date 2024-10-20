using System;
using System.Collections.Generic;

namespace waterfall_app.Classes;

public partial class Ticket
{
    public int IdTicket { get; set; }

    public string? NumberTicket { get; set; }

    public int? IdVisiter { get; set; }

    public int? IdShedule { get; set; }

    public virtual Shedule? IdSheduleNavigation { get; set; }

    public virtual Visiter? IdVisiterNavigation { get; set; }
}
