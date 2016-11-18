namespace DibiloFour.Core
{
    using System.Data.Entity;

    public class DibiloFourDBInitializer : CreateDatabaseIfNotExists<DibiloFourContext>
    {
        protected override void Seed(DibiloFourContext context)
        {
            base.Seed(context);
        }
    }
}