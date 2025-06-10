## Backend Setup (ASP.NET Core)
1. Open a terminal and navigate to the `TreasureHuntApi` directory:
   ```sh
   cd TreasureHuntApi
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Run database migrations (creates the SQLite database):
   ```sh
   dotnet ef database update
   ```
4. Start the backend server:
   ```sh
   dotnet run
   ```
   The backend will run on `http://localhost:5209` (or as shown in the terminal).

## Frontend Setup (React)
1. Open a new terminal and navigate to the `treasure-hunt-frontend` directory:
   ```sh
   cd treasure-hunt-frontend
   ```
2. Install dependencies:
   ```sh
   npm install
   ```
3. Start the frontend development server:
   ```sh
   npm start
   ```
   The frontend will run on `http://localhost:3000`.

## Usage
1. Open your browser and go to [http://localhost:3000](http://localhost:3000).
2. Use the **Solver** tab to input the matrix and parameters, then solve the treasure hunt problem.
3. Use the **History** tab to view previous submissions and re-apply any problem to the solver.

## Notes
- The backend uses SQLite for storage (file: `treasurehunt.db`).
- CORS is enabled for local development.
- Make sure both backend and frontend are running for full functionality.
