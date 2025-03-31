using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RunForHope.Models;

namespace RunForHope_️.Data
{
    public class ParticipantesContext : DbContext
    {
        public ParticipantesContext (DbContextOptions<ParticipantesContext> options)
            : base(options)
        {
        }

        public DbSet<RunForHope.Models.Participantes> Participantes { get; set; } = default!;
    }
}
