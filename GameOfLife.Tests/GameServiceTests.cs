using GameOfLife.Infrastructure;
using GameOfLife.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NPOI.SS.Formula.Functions;


namespace GameOfLife.Tests
{
    public class GameServiceTests
    {
        private Mock<ApplicationDbContext> CreateMockDbContext()
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var mockDbContext = new Mock<ApplicationDbContext>(options);
           mockDbContext.

            return mockDbContext;
        }
        [Fact]
        public void CreateNewBoard_ShouldReturnNewBoardId()
        {
            // Arrange
            var dbContext = CreateMockDbContext();
            var service = new GameService(dbContext.Object);

            // Act
            var result = service.CreateNewBoard(new bool[,] { ... });

            // Assert
            Assert.IsType<int>(result);
        }

        // Additional test methods...
    }

}