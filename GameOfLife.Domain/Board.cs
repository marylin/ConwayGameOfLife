using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GameOfLife.Domain
{
    /// <summary>
    /// Board State: The board state is stored in a 2D boolean array, where true represents a live cell and false represents a dead cell.
    /// Constructor: Initializes the board with an initial state.
    /// NextState Method: Calculates the next state of the board based on Conway's Game of Life rules.
    /// CountAliveNeighbors Method: A helper method that counts alive neighboring cells for a given cell.
    /// Properties: Width and Height are for easy access to the dimensions of the board.
    /// </summary>
    public class Board
    {
        public int Id { get; set; }
        public bool[,] State { get; set; }
        public int Width => State.GetLength(0);
        public int Height => State.GetLength(1);

        public Board(bool[,] initialState)
        {
            State = initialState ?? throw new ArgumentNullException(nameof(initialState));
        }
                     
    }
}
