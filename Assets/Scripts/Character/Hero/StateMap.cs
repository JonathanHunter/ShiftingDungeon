namespace ShiftingDungeon.Character.Hero
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateMap<T> where T : struct, IConvertible
    {
        /// <summary> Mapping between state hashes and state names. </summary>
        private Dictionary<int, T> stateMap;

        /// <summary> Creates a Mapping between Animator state hashes and the AnimatorState enum. </summary>
        public StateMap()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            this.stateMap = new Dictionary<int, T>();
            this.InitializeStateMap(stateMap);
        }

        /// <summary> Initiallizes the given map with the meta data on the AnimatorState enum. </summary>
        /// <param name="map"> The map to initialize. </param>
        private void InitializeStateMap(Dictionary<int, T> map)
        {
            Array states = Enum.GetValues(typeof(T));
            for (int i = 0; i < states.Length; i++)
            {
                int hash = GetPathHash((T)states.GetValue(i));
                map.Add(hash, (T)states.GetValue(i));
            }
            if (map.Keys.Count < states.Length)
                Debug.LogError("States are Missing from Behavior stateMap!");
        }

        /// <summary> Gets the path hash for a single state </summary>
        /// <param name="state"> The state to hash. </param>
        /// <returns> The hash. </returns>
        public int GetPathHash(T state)
        {
            return Animator.StringToHash("Base Layer." + state.ToString());
        }

        /// <summary> Returns the AnimatorState enum corresponding to the current state of the provided animator. </summary>
        /// <param name="hash"> The state hash to retrieve. </param>
        /// <returns> The AnimatorState for the given hash. </returns>
        public T GetState(int hash)
        {
            if (this.stateMap.ContainsKey(hash))
                return this.stateMap[hash];
            else
            {
                Debug.LogError("Error: Unable to find given hash in StateMap");
                return new T();
            }
        }
    }
}
