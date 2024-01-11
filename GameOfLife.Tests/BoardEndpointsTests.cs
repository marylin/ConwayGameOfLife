using GameOfLife.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameOfLife.Tests
{
    public class BoardEndpointsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public BoardEndpointsTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetBoard_ShouldReturnBoardDetails()
        {
            // Arrange
            // Create a new board and get its ID...

            // Act
            var response = await _client.GetAsync($"/api/boards/{boardId}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(responseString);
            // Additional assertions...
        }
    }

}
