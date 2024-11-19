using System.Collections.Generic;
using System.Linq;

using AI;
using AI.Sense;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

using Util;

/**
 * <summary>
 * The singleton game manager
 * </summary>
 */
public sealed class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

	[field: SerializeField, ReadOnly]
	public List<GameObject> SpawnPoints { get; private set; } = new();
	private List<Pawn> Pawns { get; } = new();
	private List<Controller> Controllers { get; } = new();
	private List<PlayerController> PlayerControllers { get; } = new();
	private List<Sound> Sounds { get; } = new();

	[Header("Debug")]
	public bool spawnPlayerAtCamera = true;
	public int AICount = 4;
	public GameObject aiControllerPrefab1;
	public GameObject aiControllerPrefab2;
	public GameObject aiControllerPrefab3;
	public GameObject aiControllerPrefab4;
	public Camera cameraPrefab;
	private Controller playerControllerRemoveMe;

	[Header("World Objects")]
	public Camera defaultCamera;
	public MapGenerator mapGenerator;

	[Header("Default Prefabs")]
	public GameObject defaultPawnPrefab;
	public GameObject defaultControllerPrefab;

	[Header("Blackboard Keys")]
	public BlackboardKey<List<Sound>> SoundsKey = "Sounds";

	public GameManager() {
		// Unity objects should not use coalescing assignment. (UNT0023)
		// Instance ??= this;
		if (Instance != null) return;
		Instance = this;

		Blackboard.GLOBAL.Set(SoundsKey, Sounds);
	}

	// HACK
	public void HackyRespawnPawnPleaseRemoveMe(Controller controller) {
		GameObject spawnPoint = SpawnPoints.Random();
		GameObject pawnObject = Instantiate(
			defaultPawnPrefab,
			spawnPoint.transform.position,
			spawnPoint.transform.rotation
		);
		Pawn pawn = pawnObject.GetComponent<Pawn>();
		Camera camera = Instantiate(cameraPrefab);

		controller.Possess(pawn);
		pawn.AttachCamera(camera);
	}

	private void Awake() {
		// TODO: strip MonoBehavior from GameManager. Put this into the constructor.
		if (Instance != this) {
			Destroy(gameObject);
			return;
		} else {
			DontDestroyOnLoad(gameObject);
		}

		mapGenerator.Generate();
	}

	private void Start() {
		int spawnCount = (spawnPlayerAtCamera ? 0 : 1) + AICount;
		List<GameObject> selectedSpawnPoints = SpawnPoints.TakeRandom(spawnCount);

		SpawnPlayer(
			defaultPawnPrefab,
			defaultControllerPrefab,
			spawnPlayerAtCamera
				? Optional<GameObject>.None
				: selectedSpawnPoints.Last()
		);

		for (int i = 0; i < AICount; i++) {
			GameObject controller = (i % 4) switch {
				0 => aiControllerPrefab1,
				1 => aiControllerPrefab2,
				2 => aiControllerPrefab3,
				_ => aiControllerPrefab4
			};

			SpawnPlayer(defaultPawnPrefab, controller, selectedSpawnPoints[i]);
		}
	}

	private void Update() {
		for (int i = Sounds.Count - 1; i >= 0; i--) {
			Sounds[i].Update();
		}

		if (!playerControllerRemoveMe.PawnOptional) {
			HackyRespawnPawnPleaseRemoveMe(playerControllerRemoveMe);
		}
	}

	/**
	 * <summary>
	 * Register a spawn point with the game manager
	 * </summary>
	 */
	public void RegisterSpawnPoint(GameObject spawnPoint) {
		SpawnPoints.Add(spawnPoint);
	}

	/**
	 * <summary>
	 * Unregister a spawn point with the game manager
	 * </summary>
	 */
	public void UnregisterSpawnPoint(GameObject spawnPoint) {
		SpawnPoints.Remove(spawnPoint);
	}

	/**
	 * <summary>
	 * Register a controller with the game manager
	 * </summary>
	 */
	public void RegisterController(Controller controller) {
		if (!Controllers.Contains(controller)) Controllers.Add(controller);

		if (controller is not PlayerController playerController) return;

		PlayerControllers.Add(playerController);

		if (!controller.PawnOptional) return;
		if (PlayerControllers.Count > 1) return;

		controller.Pawn.AttachCamera(defaultCamera);
		playerControllerRemoveMe = controller;
	}

	/**
	 * <summary>
	 * Unregister a controller with the game manager
	 * </summary>
	 */
	public void UnregisterController(Controller controller) {
		Controllers.Remove(controller);

		if (controller is PlayerController playerController) {
			PlayerControllers.Remove(playerController);
		}
	}

	/**
	 * <summary>
	 * Register a pawn with the game manager
	 * </summary>
	 */
	public void RegisterPawn(Pawn pawn) {
		Pawns.Add(pawn);
	}

	/**
	 * <summary>
	 * Unregister a pawn with the game manager
	 * </summary>
	 */
	public void UnregisterPawn(Pawn pawn) {
		Pawns.Remove(pawn);
	}

	/**
	 * <summary>
	 * Spawn a new controller and pawn at the given spawn point.<br/>
	 * If no spawn point is provided, the editor camera transform will be used.
	 * </summary>
	 * <remarks>
	 * If no spawn point is provided and the game is running in production,<br/>
	 * it will spawn at <see cref="Vector3.zero"/> and <see cref="Quaternion.identity"/>.
	 * </remarks>
	 * <seealso cref="SpawnPlayer(GameObject, GameObject, Pose)"/>
	 */
	public void SpawnPlayer(
		GameObject pawnPrefab,
		GameObject controllerPrefab,
		Optional<GameObject> spawnPoint
	) {
		Pose spawn = spawnPoint
			? new Pose(spawnPoint.Value.transform.position, spawnPoint.Value.transform.rotation)
			: Pose.identity;

#if UNITY_EDITOR
		if (!spawnPoint) {
			SceneView sceneView = SceneView.lastActiveSceneView;
			spawn.rotation = Quaternion.Euler(0, sceneView.rotation.eulerAngles.y, 0);
			spawn.position = sceneView.pivot
				- sceneView.rotation * Vector3.forward * sceneView.cameraDistance;
		}
#endif

		SpawnPlayer(pawnPrefab, controllerPrefab, spawn);
	}

	/**
	 * <summary>
	 * Spawn a new controller and pawn at the given spawn pose.
	 * </summary>
	 * <seealso cref="SpawnPlayer(GameObject, GameObject, Optional&lt;GameObject&gt;)"/> 
	 */
	public void SpawnPlayer(
		GameObject pawnPrefab,
		GameObject controllerPrefab,
		Pose spawn
	) {
		GameObject pawnObject = Instantiate(
			pawnPrefab,
			spawn.position,
			spawn.rotation
		);
		Pawn pawn = pawnObject.GetComponent<Pawn>();

		GameObject controllerObject = Instantiate(
			controllerPrefab,
			spawn.position,
			spawn.rotation
		);
		Controller controller = controllerObject.GetComponent<Controller>();

		controller.Possess(pawn);
	}
}