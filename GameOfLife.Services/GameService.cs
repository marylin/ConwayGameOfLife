using GameOfLife.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameOfLife.Infrastructure;

namespace GameOfLife.Services
{
    public class GameService
    {
        private readonly BoardRepository _boardRepository;

        public GameService(BoardRepository boardRepository)
        {
            
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        }

        public async Task<int> CreateNewBoardAsync(bool[,] initialState)
        {
            var board = new Board(initialState);
            await _boardRepository.AddBoardAsync(board);

            return board.Id;
        }

        public async Task<bool[,]> GetNextStateAsync(int boardId)
        {
            var board = await _boardRepository.GetBoardAsync(boardId);
            if (board == null)
                throw new KeyNotFoundException($"Board with ID {boardId} does not exist.");

            var currentState = board.State;
            var nextState = CalculateNextState(currentState);
            board.State = nextState;

            await _boardRepository.UpdateBoardAsync(board);

            return nextState;
        }

        public async Task<List<bool[,]>> GetFutureStatesAsync(int boardId, int numberOfSteps)
        {
            var board = await _boardRepository.GetBoardAsync(boardId);
            if (board == null)
                throw new KeyNotFoundException($"Board with ID {boardId} does not exist.");

            var currentState = board.State;
            var futureStates = new List<bool[,]>();
            for (int i = 0; i < numberOfSteps; i++)
            {
                currentState = CalculateNextState(currentState);
                futureStates.Add(currentState);
            }

            board.State = currentState;
            await _boardRepository.UpdateBoardAsync(board);

            return futureStates;
        }


        public async Task<bool[,]> FindFinalStateOrErrorAsync(int boardId, int maxIterations)
        {
            var board = await _boardRepository.GetBoardAsync(boardId);
            if (board == null)
                throw new KeyNotFoundException($"Board with ID {boardId} does not exist.");

            var currentState = board.State;
            var iterations = 0;
            while (iterations < maxIterations)
            {
                var nextState = CalculateNextState(currentState);
                if (AreStatesEqual(currentState, nextState))
                {
                    board.State = nextState;
                    await _boardRepository.UpdateBoardAsync(board);
                    return nextState;
                }
                currentState = nextState;
                iterations++;
            }

            throw new InvalidOperationException("Final state not found within the specified iterations.");
        }


        //Applies Conway's Game of Life rules to compute the next state.
        private bool[,] CalculateNextState(bool[,] currentState)
        {
            int width = currentState.GetLength(0);
            int height = currentState.GetLength(1);
            bool[,] nextState = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int aliveNeighbors = CountAliveNeighbors(currentState, x, y);

                    if (currentState[x, y])
                    {
                        nextState[x, y] = aliveNeighbors == 2 || aliveNeighbors == 3;
                    }
                    else
                    {
                        nextState[x, y] = aliveNeighbors == 3;
                    }
                }
            }

            return nextState;
        }

        //A helper method that counts alive neighboring cells for a given cell.
        private int CountAliveNeighbors(bool[,] state, int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int newX = x + i, newY = y + j;
                    if (newX >= 0 && newX < state.GetLength(0) && newY >= 0 && newY < state.GetLength(1))
                    {
                        count += state[newX, newY] ? 1 : 0;
                    }
                }
            }
            return count;
        }

        //Checks if two states are identical.
        private bool AreStatesEqual(bool[,] state1, bool[,] state2)
        {
            if (state1.GetLength(0) != state2.GetLength(0) || state1.GetLength(1) != state2.GetLength(1))
                return false;

            for (int x = 0; x < state1.GetLength(0); x++)
            {
                for (int y = 0; y < state1.GetLength(1); y++)
                {
                    if (state1[x, y] != state2[x, y])
                        return false;
                }
            }

            return true;
        }

        // Serializes the state into a string. This basic implementation uses '1' for true (alive) and '0' for false (dead), separated by commas for rows. Adjust this based on your needs.
        private static string ConvertStateToString(bool[,] state)
        {
            var stringBuilder = new System.Text.StringBuilder();
            for (int x = 0; x < state.GetLength(0); x++)
            {
                for (int y = 0; y < state.GetLength(1); y++)
                {
                    stringBuilder.Append(state[x, y] ? "1" : "0");
                }
                if (x < state.GetLength(0) - 1)
                    stringBuilder.Append(",");
            }
            return stringBuilder.ToString();
        }

        //Deserializes the string back into a state. It must match the format used in ConvertStateToString.
        private static bool[,] ConvertStringToState(string stateString)
        {
            var rows = stateString.Split(',');
            int width = rows.Length;
            int height = rows[0].Length;
            bool[,] state = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    state[x, y] = rows[x][y] == '1';
                }
            }

            return state;
        }

    }
}
