using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EveryoneToTheHackathon.Domain.Contexts.Factories;

// public class HackathonContextFactory : IDesignTimeDbContextFactory<HackathonContext>
// {
//     
//     public HackathonContext CreateDbContext(string[] args)
//     {
//         var options = new DbContextOptionsBuilder<HackathonContext>()
//             .UseSqlServer("Server=localhost,1433;Database=EveryoneToTheHackathon;User=SA;Password=foolMe_aaaa123;TrustServerCertificate=True");
//         return new HackathonContext(options.Options);
//     }
//     
// }