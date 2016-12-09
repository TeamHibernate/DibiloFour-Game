namespace DibiloFour.Core.Commands
{
    using Interfaces;

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

        public string Explanation { get; set; }

        public abstract void Execute();
    }
}
