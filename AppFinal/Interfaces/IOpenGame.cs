namespace AppFinal.Interfaces
{
    /// <summary>
    /// Contract for the interface to open the game
    /// </summary>
    public interface IOpenGame
    {
        /// <summary>
        /// opens the game with the param name
        /// </summary>
        /// <param name="game"></param>
        void OpenGame(string game);
    }
}

