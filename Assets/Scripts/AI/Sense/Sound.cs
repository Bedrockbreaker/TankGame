using System.Collections.Generic;

using UnityEngine;

using Util;

namespace AI.Sense {
	/**
	 * <summary>
	 * Represents an instance of a sound
	 * </summary>
	 */
	public class Sound {

		protected List<Sound> parentList;

		public Optional<Controller> owner;
		public Vector3 position;
		public float volume;

		public Sound(
			List<Sound> parentList,
			Optional<Controller> owner,
			Vector3 position,
			float volume
		) {
			this.parentList = parentList;
			this.owner = owner;
			this.position = position;
			this.volume = volume;
		}

		public virtual void Update() {
			parentList.Remove(this);
		}
	}
}