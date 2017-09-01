namespace ShiftingDungeon.Character.Player
{
    using System.Collections.Generic;
    using UnityEngine;
    using Util;

    public class StateMap
    {
        /// <summary> Mapping between state hashes and state names. </summary>
        private Dictionary<int, Enums.PlayerState> stateMap;

        /// <summary> Creates a Mapping between Animator state hashes and the AnimatorState enum. </summary>
        public StateMap()
        {
            this.stateMap = new Dictionary<int, Enums.PlayerState>();
            this.InitializeStateMap(stateMap);
        }

        /// <summary> Initiallizes the given map with the meta data on the AnimatorState enum. </summary>
        /// <param name="map"> The map to initialize. </param>
        private void InitializeStateMap(Dictionary<int, Enums.PlayerState> map)
        {
            for (int i = 0; i < (int)Enums.PlayerState.length; i++)
            {
                Enums.PlayerState state = (Enums.PlayerState)i;
                int hash = GetPathHash(state);
                map.Add(hash, state);
            }
            if (map.Keys.Count < (int)Enums.PlayerState.length)
                Debug.LogError("States are Missing from Behavior stateMap!");
        }

        /// <summary> Gets the path hash for a single state </summary>
        /// <param name="state"> The state to hash. </param>
        /// <returns> The hash. </returns>
        public int GetPathHash(Enums.PlayerState state)
        {
            return Animator.StringToHash("Base Layer." + state.ToString());
        }

        /// <summary> Returns the AnimatorState enum corresponding to the current state of the provided animator. </summary>
        /// <param name="hash"> The state hash to retrieve. </param>
        /// <returns> The AnimatorState for the given hash. </returns>
        public Enums.PlayerState GetState(int hash)
        {
            if (this.stateMap.ContainsKey(hash))
                return this.stateMap[hash];
            else
            {
                Debug.LogError("Error: Unable to find given hash in StateMap");
                return Enums.PlayerState.Idle;
            }
        }
    }
}
