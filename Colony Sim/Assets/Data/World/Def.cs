using System.Collections.Generic;
using UnityEngine;

namespace ColonySim
{
	/// Definition of all our entities.
	[System.Serializable]
	public class Def
	{
		/* Unique string identifier */
		public string uid { get; set; }

		public override int GetHashCode()
		{
			return this.uid.GetHashCode();
		}
	}
}