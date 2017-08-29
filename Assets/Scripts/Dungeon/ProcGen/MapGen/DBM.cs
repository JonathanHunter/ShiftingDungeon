namespace ShiftingDungeon.Dungeon.ProcGen.MapGen
{
    using System.Collections.Generic;
    using UnityEngine;

    // Dielectric Breakdown Model
    public class DBM
    {
        /// <summary> Enum to track the state of a particular grid cell. </summary>
        public enum GridState { Empty, Filled, Candidate }

        /// <summary> The lenght/width of the grid.  Grids are always square. </summary>
        public int GridSize { get; private set; }

        private List<Cell> pattern;
        private List<Cell> candidates;
        private GridState[,] grid;

        /// <summary> Initallizes a DBM pattern with one cell in the center of the grid. </summary>
        /// <param name="gridSize"> The lenght/width of the grid.  Grids are always square. </param>
        public DBM(int gridSize)
        {
            this.GridSize = gridSize;
            this.pattern = new List<Cell>();
            this.candidates = new List<Cell>();
            this.grid = new GridState[gridSize, gridSize];
            Cell cellCenter = new Cell();
            cellCenter.Row = this.GridSize / 2;
            cellCenter.Col = this.GridSize / 2;
            this.pattern.Add(cellCenter);
            this.grid[cellCenter.Row, cellCenter.Col] = GridState.Filled;
        }

        /// <summary> Calculate a new cell to add to this pattern. </summary>
        public void AddCell()
        {
            foreach (Cell c in pattern)
                AddNeighbors(c.Row, c.Col);
            Cell newCell = SelectCandidate();
            AddPattern(newCell);
            UpdatePhi(newCell);
        }

        /// <summary> Converts the DBM cell pattern into a list of rooms. </summary>
        /// <param name="roomTemplet"> The templet Room GameObject used for instantiation. </param>
        /// <returns> The DBM pattern as an array of rooms. </returns>
        public Room[] GetPatternAsRooms(Room roomTemplet)
        {
            Room[] rooms = new Room[pattern.Count];
            for(int i = 0; i < rooms.Length; i++)
            {
                Room room = MonoBehaviour.Instantiate<Room>(roomTemplet);
                // Ensure room is in default position, orientation, and scale
                room.transform.position = Vector3.zero;
                room.transform.rotation = Quaternion.identity;
                room.transform.localScale = Vector3.one;
                room.Row = pattern[i].Row;
                room.Col = pattern[i].Col;
                room.IsGenerated = true;
                rooms[i] = room;
            }

            return rooms;
        }

        private void AddNeighbors(int r, int c)
        {
            if ((r < 1 || r > this.GridSize - 2) || (c < 1 || c > this.GridSize - 2))
                return;
            if (this.grid[r - 1, c] == GridState.Empty)
                AddCandidate(r - 1, c);
            if (this.grid[r + 1, c] == GridState.Empty)
                AddCandidate(r + 1, c);
            if (this.grid[r, c - 1] == GridState.Empty)
                AddCandidate(r, c - 1);
            if (this.grid[r, c + 1] == GridState.Empty)
                AddCandidate(r, c + 1);
        }

        private void AddCandidate(int r, int c)
        {
            Cell cell = new Cell();
            cell.Row = r;
            cell.Col = c;
            cell.Potential = CalcPhi(cell);
            candidates.Add(cell);
            this.grid[r, c] = GridState.Candidate;
        }

        private float CalcPhi(Cell c)
        {
            float sum = 0;
            foreach (Cell p in pattern)
                sum += (1f - (1f / Distance(c, p)));
            return sum;
        }

        private float Distance(Cell x, Cell y)
        {
            return Vector2.Distance(new Vector2(x.Row, x.Col), new Vector2(y.Row, y.Col));
        }

        private Cell SelectCandidate()
        {
            float max, min;
            CalcMinMax(out min, out max);
            float phiSum = CalcPhi(min, max);
            CalcProbability(phiSum);
            float probSum = 0;
            foreach (Cell c in candidates)
            {
                probSum += c.Probablility;
                c.ProbablilitySum = probSum;
            }

            float r = Random.Range(0, probSum);
            Cell select = null;
            foreach (Cell c in candidates)
            {
                if (r <= c.ProbablilitySum)
                {
                    if (select == null || select.ProbablilitySum > c.ProbablilitySum)
                        select = c;
                }
            }

            return select;
        }
        
        private void CalcMinMax(out float min, out float max)
        {
            max = System.Int32.MinValue;
            min = System.Int32.MaxValue;
            foreach (Cell c in candidates)
            {
                if (c.Potential > max)
                    max = c.Potential;
                if (c.Potential < min)
                    min = c.Potential;
            }
        }

        private float CalcPhi(float min, float max)
        {
            float eta = 0;
            float phiSum = 0;
            foreach (Cell c in candidates)
            {
                if (min == 0 && max == 0)
                    c.PhiEta = 0;
                else
                    c.PhiEta = (c.Potential - min) / (max - min);
                c.PhiEta = Mathf.Pow(c.PhiEta, eta);
                phiSum += c.PhiEta;
            }

            return phiSum;
        }

        private void CalcProbability(float phiSum)
        {
            foreach (Cell c in candidates)
                c.Probablility = c.PhiEta / phiSum;
            candidates.Sort((x, y) => x.Probablility.CompareTo(y.Probablility));
        }

        private void AddPattern(Cell newCell)
        {
            pattern.Add(newCell);
            candidates.Remove(newCell);
            this.grid[newCell.Row, newCell.Col] = GridState.Filled;
        }

        private void UpdatePhi(Cell Cnew)
        {
            foreach (Cell c in candidates)
                c.Potential += (1f - 1f / Distance(Cnew, c));
        }
    }
}
