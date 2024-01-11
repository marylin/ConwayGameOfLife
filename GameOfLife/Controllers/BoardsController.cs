using Microsoft.AspNetCore.Mvc;
using GameOfLife.Services;
using System;
using System.Collections.Generic;

namespace GameOfLife.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly GameService _gameService;

        public BoardsController(GameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        // POST: api/boards
        [HttpPost]
        public ActionResult<int> CreateBoard([FromBody] bool[,] initialState)
        {
            if (initialState == null)
            {
                return BadRequest("Initial state is required.");
            }

            var boardId = _gameService.CreateNewBoardAsync(initialState);
            return Ok(boardId); // Returns the ID of the created board
        }

        // GET: api/boards/{id}/next
        [HttpGet("{id}/next")]
        public ActionResult<bool[,]> GetNextState(int id)
        {
            try
            {
                var nextState = _gameService.GetNextStateAsync(id);
                return Ok(nextState);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/boards/{id}/future/{steps}
        [HttpGet("{id}/future/{steps}")]
        public ActionResult<List<bool[,]>> GetFutureStates(int id, int steps)
        {
            try
            {
                var futureStates = _gameService.GetFutureStatesAsync(id, steps);
                return Ok(futureStates);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/boards/{id}/final/{maxIterations}
        [HttpGet("{id}/final/{maxIterations}")]
        public ActionResult<bool[,]> FindFinalStateOrError(int id, int maxIterations)
        {
            try
            {
                var finalState = _gameService.FindFinalStateOrErrorAsync(id, maxIterations);
                return Ok(finalState);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
