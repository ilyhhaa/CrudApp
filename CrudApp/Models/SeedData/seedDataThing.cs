using CrudApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CrudApp.Models.SeedData
{
    public class seedDataThing
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CrudAppContext(serviceProvider.GetRequiredService<DbContextOptions<CrudAppContext>>()))
            {
                if (context.Thing.Any())
                {
                    return;
                }

                context.Thing.AddRange(

                    new Thing
                    {
                        Title = "Brush",
                        Description = "Brush for Brushing"
                    },
                    new Thing
                    {
                        Title = "Car",
                        Description = "Car For Driving"
                    },
                    new Thing
                    {
                        Title = "Boat",
                        Description = "Boat for boating"
                    },
                    new Thing
                    {
                        Title = "Messi",
                        Description = "For GOATing"
                    },
                    new Thing
                    {
                        Title = "Bed",
                        Description = "For sleeping"
                    },
                    new Thing
                    {
                        Title = "Table",
                        Description = "for Tabling(Working)"
                    }

                );

                context.SaveChanges();
            }
        }

    }
}
