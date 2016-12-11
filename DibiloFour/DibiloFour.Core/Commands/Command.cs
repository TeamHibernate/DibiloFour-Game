namespace DibiloFour.Core.Commands
{
    using Interfaces;
    using Models.Dibils;

    public abstract class Command : ICommand
    {
        private string[] data;

        protected Command(string[] data)
        {
            this.Data = data;
        }

        protected string[] Data
        {
            get { return this.data; }
            private set { this.data = value; }
        }

        public abstract Player Execute();
    }
}
