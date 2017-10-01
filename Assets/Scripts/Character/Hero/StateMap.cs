namespace ShiftingDungeon.Character.Hero
{
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using Util;
    using Util.Attributes;

    public class StateMap
    {
        /// <summary> Mapping between state hashes and state names. </summary>
        private Dictionary<int, Enums.HeroState> stateMap;

        /// <summary> Creates a Mapping between Animator state hashes and the AnimatorState enum. </summary>
        public StateMap()
        {
            this.stateMap = new Dictionary<int, Enums.HeroState>();
            this.InitializeStateMap(stateMap);
        }

        /// <summary> Initiallizes the given map with the meta data on the AnimatorState enum. </summary>
        /// <param name="map"> The map to initialize. </param>
        private void InitializeStateMap(Dictionary<int, Enums.HeroState> map)
        {
            for (int i = 0; i < (int)Enums.HeroState.length; i++)
            {
                Enums.HeroState state = (Enums.HeroState)i;
                int hash = GetPathHash(state);
                map.Add(hash, state);
            }
            if (map.Keys.Count < (int)Enums.HeroState.length)
                Debug.LogError("States are Missing from Behavior stateMap!");
        }

        /// <summary> Gets the path hash for a single state </summary>
        /// <param name="state"> The state to hash. </param>
        /// <returns> The hash. </returns>
        public int GetPathHash(Enums.HeroState state)
        {
            StateMapAttribute metadata = (StateMapAttribute)typeof(Enums.HeroState).GetField(state.ToString()).GetCustomAttributes(false)[0];
            StringBuilder path = new StringBuilder(metadata.Layer + ".");
            foreach (string machine in metadata.StateMachines)
                path.Append(machine + ".");
            path.Append(state.ToString());
            return Animator.StringToHash(path.ToString());
        }

        /// <summary> Returns the AnimatorState enum corresponding to the current state of the provided animator. </summary>
        /// <param name="hash"> The state hash to retrieve. </param>
        /// <returns> The AnimatorState for the given hash. </returns>
        public Enums.HeroState GetState(int hash)
        {
            if (this.stateMap.ContainsKey(hash))
                return this.stateMap[hash];
            else
            {
                Debug.LogError("Error: Unable to find given hash in StateMap");
                return Enums.HeroState.Idle;
            }
        }
    }
}
