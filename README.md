# Conway's Game of Life API

## Overview

This ASP.NET Core Web API provides a backend for Conway's Game of Life. It allows users to create game boards, retrieve and calculate future states of the board, and find final states.

## Features

- Upload new board states and receive a unique board ID
- Retrieve the next state of a board
- Get a specified number of future states for a board
- Find the final state of a board within a certain number of iterations
- Persist board states in a database to retain state across service restarts

## Getting Started

### Prerequisites

- .NET 7.0 SDK
- Visual Studio 2022 (or later) / Visual Studio Code
- SQL Server (for database-related operations)

### Installation

1. Clone the repository:
   ```bash
   git clone https://your-repository-url

### Installation

2. Open the solution in Visual Studio or Visual Studio Code.
3. Restore NuGet packages:
   ```bash
   dotnet restore
4. Update the database (if using Entity Framework Core):

   ```bash
   dotnet ef database update
  

### Running the Application
Run the application in Visual Studio or use the following command:

     dotnet run --project path/to/your/solutionfolder/GameOfLife

The API will be available at http://localhost:5000/.

## Usage

### Endpoints

- `POST /api/boards`: Create a new board.
- `GET /api/boards/{id}/next`: Get the next state of a board.
- `GET /api/boards/{id}/future/{steps}`: Get future states of a board.
- `GET /api/boards/{id}/final/{maxIterations}`: Find the final state of a board.


## Examples

### Creating a board:

```bash
curl -X POST http://localhost:5000/api/boards -H "Content-Type: application/json" -d "{ \"initialState\": [[true, false], [false, true]] }"




  
