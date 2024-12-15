using System;
using System.Collections.Generic;
using System.Linq;

using Unity.AI.Navigation;

using UnityEngine;

/**
 * <summary>
 * Generates a map from a list of rooms using Wave Function Collapse
 * </summary>
 */
public class MapGenerator : MonoBehaviour {

	public static MapGenerator Instance { get; protected set; }

	protected Room[,] cells;
	protected List<Room>[,] quantumCells;

	[Header("Navigation")]
	[SerializeField]
	protected NavMeshSurface navSurface;

	[Header("Rooms")]
	public List<Room> rooms = new();
	public List<Room> startRooms = new();

	[Header("Grid")]
	public int cellsHorizontal = 10;
	public int cellsVertical = 10;
	public int cellWidth = 20;
	public int cellHeight = 20;

	[Header("Random")]
	public long seed = 0;

	public MapGenerator() {
		// Unity objects should not use coalescing assignment. (UNT0023)
		// Instance ??= this;
		if (Instance != null) return;
		Instance = this;
	}

	protected void Awake() {
		if (Instance != this) {
			foreach (Transform child in Instance.transform) Destroy(child.gameObject);
			Instance.seed = 0;
			Destroy(gameObject);
			return;
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}

	/**
	 * <summary>
	 * Initialize the map and its starting conditions
	 * </summary>
	 */
	protected List<Tuple<int, int>> Initialize(long seed = 0) {
		if (seed == 0) {
			seed = this.seed == 0
				? DateTime.Now.Ticks
				: this.seed;
		}
		this.seed = seed;
		UnityEngine.Random.InitState((int)seed);

		List<Tuple<int, int>> initialCells = new();

		cells = new Room[cellsHorizontal, cellsVertical];
		quantumCells = new List<Room>[cellsHorizontal, cellsVertical];
		for (int x = 0; x < cellsHorizontal; x++) {
			for (int y = 0; y < cellsVertical; y++) {
				quantumCells[x, y] = new List<Room>(rooms);

				if (x == 0) {
					quantumCells[x, y].RemoveAll(room => room.Connections.west);
				} else if (x == cellsHorizontal - 1) {
					quantumCells[x, y].RemoveAll(room => room.Connections.east);
				}

				if (y == 0) {
					quantumCells[x, y].RemoveAll(room => room.Connections.north);
				} else if (y == cellsVertical - 1) {
					quantumCells[x, y].RemoveAll(room => room.Connections.south);
				}

				// Propagate(x, y);

				if (quantumCells[x, y].Any(room => startRooms.Contains(room))) {
					initialCells.Add(new Tuple<int, int>(x, y));
				}
			}
		}

		return initialCells;
	}

	/**
	 * <summary>
	 * Collapse quantum cells until each cell has only one possible room
	 * </summary>
	 */
	protected void Collapse() {
		while (true) {
			int minEntropy = int.MaxValue;
			int targetX = -1;
			int targetY = -1;

			for (int x = 0; x < cellsHorizontal; x++) {
				for (int y = 0; y < cellsVertical; y++) {
					int entropy = quantumCells[x, y].Count;

					if (entropy > 1 && entropy < minEntropy) {
						minEntropy = entropy;
						targetX = x;
						targetY = y;
					}
				}
			}

			if (targetX == -1) break;

			Room room = quantumCells[targetX, targetY][UnityEngine.Random.Range(0, minEntropy)];
			quantumCells[targetX, targetY] = new List<Room>() { room };

			Propagate(targetX, targetY);
		}
	}

	/**
	 * <summary>
	 * Propagate the collapse of a cell to its neighbors
	 * </summary>
	 */
	protected void Propagate(int x, int y) {
		Queue<Tuple<int, int>> queue = new();
		queue.Enqueue(new Tuple<int, int>(x, y));

		while (queue.Count > 0) {
			Tuple<int, int> cell = queue.Dequeue();
			int cx = cell.Item1;
			int cy = cell.Item2;

			for (int i = 0; i < 4; i++) {
				int nx = cx + (i == 1 ? 1 : (i == 3 ? -1 : 0));
				int ny = cy + (i == 0 ? -1 : (i == 2 ? 1 : 0));

				if (
					nx < 0 || nx >= cellsHorizontal
					|| ny < 0 || ny >= cellsVertical
				) {
					continue;
				}

				HashSet<Room> compatibleRooms = new();
				foreach (Room room in quantumCells[cx, cy]) {
					foreach (Room otherRoom in rooms) {
						if (
							(i == 0 && room.Connections.north == otherRoom.Connections.south)
							|| (i == 1 && room.Connections.east == otherRoom.Connections.west)
							|| (i == 2 && room.Connections.south == otherRoom.Connections.north)
							|| (i == 3 && room.Connections.west == otherRoom.Connections.east)
						) {
							compatibleRooms.Add(otherRoom);
						}
					}
				}

				List<Room> nqCell = quantumCells[nx, ny];
				int oldCount = nqCell.Count;
				for (int j = nqCell.Count - 1; j >= 0; j--) {
					Room room = nqCell[j];
					if (!compatibleRooms.Contains(room)) {
						nqCell.Remove(room);
					}
				}

				if (nqCell.Count < oldCount) {
					queue.Enqueue(new Tuple<int, int>(nx, ny));
				}
			}
		}
	}

	/**
	 * <summary>
	 * Remove all rooms that are not reachable from the start room
	 * </summary>
	 */
	public void PruneUnreachableRooms(int x, int y) {
		HashSet<Tuple<int, int>> visited = new();
		Queue<Tuple<int, int>> queue = new();
		queue.Enqueue(new Tuple<int, int>(x, y));

		while (queue.Count > 0) {
			Tuple<int, int> cell = queue.Dequeue();
			if (visited.Contains(cell)) continue;

			visited.Add(cell);
			int cx = cell.Item1;
			int cy = cell.Item2;

			if (quantumCells[cx, cy][0].Connections.north) {
				queue.Enqueue(new Tuple<int, int>(cx, cy - 1));
			}

			if (quantumCells[cx, cy][0].Connections.east) {
				queue.Enqueue(new Tuple<int, int>(cx + 1, cy));
			}

			if (quantumCells[cx, cy][0].Connections.south) {
				queue.Enqueue(new Tuple<int, int>(cx, cy + 1));
			}

			if (quantumCells[cx, cy][0].Connections.west) {
				queue.Enqueue(new Tuple<int, int>(cx - 1, cy));
			}
		}

		for (int i = 0; i < cellsHorizontal; i++) {
			for (int j = 0; j < cellsVertical; j++) {
				quantumCells[i, j] = visited.Contains(new Tuple<int, int>(i, j))
					? quantumCells[i, j]
					: new List<Room>();
			}
		}
	}

	/**
	 * <summary>
	 * Generate the map
	 * </summary>
	 */
	public void Generate(long seed = 0) {
		List<Tuple<int, int>> initialCells = Initialize(seed);

		if (initialCells.Count == 0) {
			throw new Exception("No valid position for the start room found.");
		}

		Tuple<int, int> startCellPosition
			= initialCells[UnityEngine.Random.Range(0, initialCells.Count)];
		quantumCells[startCellPosition.Item1, startCellPosition.Item2]
			= new List<Room>(startRooms);

		Propagate(startCellPosition.Item1, startCellPosition.Item2);

		Collapse();

		PruneUnreachableRooms(startCellPosition.Item1, startCellPosition.Item2);

		for (int x = 0; x < cellsHorizontal; x++) {
			for (int y = 0; y < cellsVertical; y++) {
				if (quantumCells[x, y].Count == 0) continue;

				cells[x, y] = quantumCells[x, y][0];

				Vector3 position = new(
					(cellsHorizontal / 2f - y) * cellWidth,
					0,
					(cellsVertical / 2f - x) * cellHeight
				);

				Room room = Instantiate(
					cells[x, y],
					position + gameObject.transform.position,
					Quaternion.identity
				);

				room.transform.parent = gameObject.transform;
			}
		}

		navSurface.BuildNavMesh();
	}
}