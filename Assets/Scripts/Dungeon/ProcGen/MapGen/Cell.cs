namespace ShiftingDungeon.Dungeon.ProcGen.MapGen
{
    public class Cell
    {
        /// <summary> Row on the grid. </summary>
        public int Row { get; set; }
        /// <summary> Column on the grid. </summary>
        public int Col { get; set; }
        /// <summary> The DBM potential for this cell. </summary>
        public float Potential { get; set; }
        /// <summary> The probability this cell will be picked. </summary>
        public float Probablility { get; set; }
        /// <summary> The DMB PhiEta for this cell. </summary>
        public float PhiEta { get; set; }
        /// <summary> The cell probability rescaled to be in sequence with other canidates. </summary>
        public float ProbablilitySum { get; set; }
    }
}
