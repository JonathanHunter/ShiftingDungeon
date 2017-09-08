namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MiniMap : MonoBehaviour
    {
        [SerializeField]
        private Image[] gridPoints;
        [SerializeField]
        private int numCols;
        [SerializeField]
        private Sprite off;
        [SerializeField]
        private Sprite on;
        [SerializeField]
        private Sprite visited;

        private int[,] grid;
        private Vector2 current;

        public void Init(int[,] grid, Vector2 start)
        {
            this.grid = new int[grid.GetLength(0), grid.GetLength(1)];
            for (int r = 0; r < this.grid.GetLength(0); r++)
            {
                for (int c = 0; c < this.grid.GetLength(1); c++)
                {
                    if (grid[r, c] > 1)
                        this.grid[r, c] = 1;
                    else
                        this.grid[r, c] = 0;
                }
            }

            this.current = start;
            this.grid[(int)this.current.x, (int)this.current.y] = 3;
            UpdateMapState();
        }

        public void UpdateMiniMap(Util.Enums.Direction direction)
        {
            if (this.grid == null)
                return;
            this.grid[(int)this.current.x, (int)this.current.y] = 2;
            if (direction == Util.Enums.Direction.Up)
                this.current = new Vector2(this.current.x, this.current.y - 1);
            if (direction == Util.Enums.Direction.Down)
                this.current = new Vector2(this.current.x, this.current.y + 1);
            if (direction == Util.Enums.Direction.Left)
                this.current = new Vector2(this.current.x - 1, this.current.y);
            if (direction == Util.Enums.Direction.Right)
                this.current = new Vector2(this.current.x + 1, this.current.y);
            this.grid[(int)this.current.x, (int)this.current.y] = 3;
            UpdateMapState();
        }

        private void UpdateMapState()
        {
            for(int r = 0; r < this.numCols; r++)
            {
                for(int c = 0; c < this.numCols; c++)
                {
                    int x = c + (int)this.current.x - (this.numCols / 2);
                    int y = r + (int)this.current.y - (this.numCols / 2);

                    if (x < 0 ||
                        x >= this.grid.GetLength(0) ||
                        y < 0 ||
                        y >= this.grid.GetLength(0) ||
                        this.grid[x, y] <= 1)
                        GetImageAt(r, c).sprite = this.off;
                    else if (this.grid[x, y] == 2)
                        GetImageAt(r, c).sprite = this.visited;
                    else if (this.grid[x, y] == 3)
                        GetImageAt(r, c).sprite = this.on;
                }
            }
        }

        private Image GetImageAt(int r, int c)
        {
            return this.gridPoints[r * this.numCols + c];
        }
    }
}
