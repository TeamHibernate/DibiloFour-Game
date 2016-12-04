namespace DibiloFour.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Data;

    internal sealed class Configuration : DbMigrationsConfiguration<DibiloFourContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DibiloFourContext context)
        {
        }
    }
}
