using System;
using System.Collections.Generic;

namespace waterfall_app.Classes;

public partial class Visiter
{
    public int IdVisiter { get; set; }

    public string Firstname { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Fathersname { get; set; }

    public string Email { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int? Age { get; set; }

    public int? IdType { get; set; }

    public virtual TypeTicket? IdTypeNavigation { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
