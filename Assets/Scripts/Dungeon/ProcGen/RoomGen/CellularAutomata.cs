namespace ShiftingDungeon.Dungeon.ProcGen.RoomGen
{
    using UnityEngine;

    public class CellularAutomata
    {
        private readonly int[,] rules = { { 0, 0, 0, 1, 0, 0, 0, 1, 1 }, { 0, 0, 0, 0, 1, 1, 1, 1, 1 } };

        private int[,] grid;
        private int gridSize;


        public CellularAutomata(int gridSize)
        {
            this.grid = new int[gridSize, gridSize];
            this.gridSize = gridSize;
        }

        public void GeneratePattern(int iterationCount)
        {
            RandomizeGrid();
            for(int i = 0; i < iterationCount; i++)
                UpdateGrid();
            ClearDoorPath();
        }

        public int[,] GetPattern()
        {
            return this.grid;
        }

        private void RandomizeGrid()
        {
            for(int r = 0; r < this.gridSize; r++)
            {
                for(int c = 0; c < this.gridSize; c++)
                {
                    this.grid[r, c] = Random.Range(0, 2);
                }
            }
        }

        private void UpdateGrid()
        {
            int neighbors = 0;
            int[,] newGrid = new int[this.gridSize, this.gridSize];
            for (int r = 0; r < this.gridSize; r++)
            {
                for (int c = 0; c < this.gridSize; c++)
                {
                    neighbors = getNeighbors(r, c);
                    newGrid[r, c] = this.rules[this.grid[r, c], neighbors];
                }
            }

            this.grid = newGrid;
        }
        
        private int getNeighbors(int r, int c)
        {
            int left = r - 1 < 0 ? r - 1 + this.gridSize : r - 1;
            int right = r + 1 >= this.gridSize ? r + 1 - this.gridSize : r + 1;
            int up = c - 1 < 0 ? c - 1 + this.gridSize : c - 1;
            int down = c + 1 >= this.gridSize ? c + 1 - this.gridSize : c + 1;
            int count = 0;
            count += this.grid[left, c];
            count += this.grid[right, c];
            count += this.grid[left, up];
            count += this.grid[left, down];
            count += this.grid[right, up];
            count += this.grid[right, down];
            count += this.grid[r, up];
            count += this.grid[r, down];
            return count;
        }

        private void ClearDoorPath()
        {
            for (int r = 0; r < this.gridSize; r++)
            {
                for (int c = 0; c < this.gridSize; c++)
                {
                    this.grid[r, this.gridSize / 2] = 1;
                    this.grid[this.gridSize / 2, c] = 1;
                }
            }
        }
    }
}
