using System;

namespace Kryz.CharacterStats
{
	public enum StatModType
	{
		Flat,
		PercentAdd,
	}

    [Serializable]
	public class StatModifier
	{
		public float Value;
		public StatModType Type;
		//public readonly object Source;

		public StatModifier(float value, StatModType type)
		{
			Value = value;
			Type = type;
		}
	}
}
