using GameOfLife.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameOfLife.Infrastructure
{
    public class BoardRepository
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Board> GetBoardAsync(int id)
        {
            return await _context.Boards.FindAsync(id);
        }

        public async Task AddBoardAsync(Board board)
        {
            await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();
        }

        // Add other methods for updating, deleting, etc.
    }
}
