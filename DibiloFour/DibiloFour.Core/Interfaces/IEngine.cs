using System;
namespace DibiloFour.Core.Interfaces
{

    using DibiloFour.Models.Dibils;

    public interface IEngine
    {
        Player CurrentlyActivePlayer { get; set; }

        void Run();
    }
}