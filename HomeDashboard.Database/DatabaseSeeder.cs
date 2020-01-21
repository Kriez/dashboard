using HomeDashboard.Library.Models;
using kriez.HomeDashboard.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kriez.HomeDashboard.Data
{

    public class DatabaseSeeder
    {
        private DatabaseContext Context;
        public DatabaseSeeder(DatabaseContext context)
        {
            Context = context;
        }

        public void Seed()
        {
            SeedUpdateTable();

            if (Context.ChangeTracker.HasChanges())
            {
                Context.SaveChanges();
            }
        }

        public void SeedUpdateTable()
        {
            foreach (UpdateType updateTypeEnum in (UpdateType[])Enum.GetValues(typeof(UpdateType)))
            {
                var updateType = Context.UpdateTables.FirstOrDefault(u => u.Key.Equals(updateTypeEnum));
                if (updateType != null)
                {
                    continue;
                }

                UpdateTable table = new UpdateTable()
                {
                    Key = updateTypeEnum,
                    LastUpdated = DateTime.MinValue
                };

                Context.UpdateTables.Add(table);
            }
        }
    }
}
