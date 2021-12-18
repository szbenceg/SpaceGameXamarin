using System;

namespace beadXamarin.Persistence
{
    public class GameDataException : Exception
    {
        public GameDataException()
        {
        }
        public GameDataException(string message) : base(message)
        {
        }
    }
}