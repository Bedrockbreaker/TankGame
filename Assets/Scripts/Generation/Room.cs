using System;
// using System.Collections.Generic;

using UnityEngine;

/**
 * <summary>
 * A room
 * </summary>
 */
public class Room : MonoBehaviour {

	[field: SerializeField]
	public RoomConnections Connections { get; private set; }

	// TODO: add list of specific neighbors
	/* [SerializeField]
	protected List<Room> neighbors = new();
	[SerializeField]
	protected bool blacklist = true; */
}

[Serializable]
public struct RoomConnections {
	public bool north;
	public bool east;
	public bool south;
	public bool west;
}