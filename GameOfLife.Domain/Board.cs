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

        public void NextState()
        {
            bool[,] nextState = new bool[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int aliveNeighbors = CountAliveNeighbors(x, y);
                    bool isAlive = State[x, y];

                    nextState[x, y] = isAlive switch
                    {
                        true when aliveNeighbors < 2 => false, // Underpopulation
                        true when aliveNeighbors > 3 => false, // Overpopulation
                        false when aliveNeighbors == 3 => true, // Reproduction
                        _ => isAlive
                    };
                }
            }

            State = nextState;
        }

        private int CountAliveNeighbors(int x, int y)
        {
            int alive = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue; // Skip the cell itself

                    int neighborX = x + i;
                    int neighborY = y + j;

                    // Check boundaries
                    if (neighborX >= 0 && neighborX < Width && neighborY >= 0 && neighborY < Height)
                    {
                        alive += State[neighborX, neighborY] ? 1 : 0;
                    }
                }
            }

            return alive;
        }

        public List<bool[,]> GetFutureStates(int numberOfSteps)
        {
            List<bool[,]> futureStates = new List<bool[,]>();

            for (int step = 0; step < numberOfSteps; step++)
            {
                NextState();
                bool[,] stateCopy = new bool[Width, Height];
                Array.Copy(State, stateCopy, State.Length);
                futureStates.Add(stateCopy);
            }

            return futureStates;
        }

        public bool[,] FindFinalStateOrError(int maxIterations)
        {
            bool[,] previousState = new bool[Width, Height];
            Array.Copy(State, previousState, State.Length);

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                NextState();

                if (IsSameState(previousState, State))
                {
                    return State; // Final state found, no changes between iterations
                }

                Array.Copy(State, previousState, State.Length); // Update previous state for the next iteration
            }

            throw new InvalidOperationException("Final state not found within the specified iterations.");
        }

        private bool IsSameState(bool[,] state1, bool[,] state2)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (state1[x, y] != state2[x, y])
                        return false;
                }
            }
            return true;
        }

       
    }
}
