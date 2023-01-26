
using System.Data.Entity;

namespace Project_06.Models
{
    public class BetContext : DbContext
    {
        public BetContext()
        {
        }
        public virtual DbSet<Bet> Bets { get; set; }
    }
}
