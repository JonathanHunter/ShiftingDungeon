namespace ShiftingDungeon.Util.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class StateMapAttribute : Attribute
    {
        public string Layer { get; set; }

        public string[] StateMachines { get; set; }
    }
}
