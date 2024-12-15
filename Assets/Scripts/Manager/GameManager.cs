using System.Collections.Generic;

using AI;
using AI.Sense;

using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Audio;

using Util;

/**
 * <summary>
 * The singleton game manager
 * </summary>
 */
public sealed class GameManager : MonoBehaviour {

	public static GameManager Instance { get; private set; }

	[field: SerializeField, ReadOnly]
	public List<PawnSpawnPoint> SpawnPoints { get; private set; } = new();
	private List<Pawn> Pawns { get; } = new();
	private List<Controller> Controllers { get; } = new();
	private List<PlayerController> PlayerControllers { get; } = new();
	private List<Sound> Sounds { get; } = new();

	[Header("Debug")]
	public bool spawnPlayerAtCamera = true;
	public int AICount = 4;
	public AIController aiControllerPrefab1;
	public AIController aiControllerPrefab2;
	public AIController aiControllerPrefab3;
	public AIController aiControllerPrefab4;
	public Camera cameraPrefab;

	[Header("World Objects")]
	public Camera defaultCamera;
	public MapGenerator mapGenerator;

	[field: Header("UI")]
	[field: SerializeField, ReadOnly]
	public Optional<Menu> ActiveMenu { get; private set; } = Optional<Menu>.None;

	[Header("Audio")]
	public AudioClip menuMusic;
	public AudioClip gameMusic;
	[field: SerializeField]
	public AudioMixer AudioMixer { get; private set; }
	[field: SerializeField]
	public AudioSource MusicSource { get; private set; }
	[field: SerializeField]
	public AudioSource EffectSource { get; private set; }
	[field: SerializeField]
	public List<string> VolumeParameters { get; private set; } = new();

	[Header("Default Prefabs")]
	public Pawn defaultPawnPrefab;
	public Controller defaultControllerPrefab;
	public Camera defaultCameraPrefab;
	public HUD defaultHUDPrefab;

	[Header("Blackboard Keys")]
	public BlackboardKey<List<Sound>> SoundsKey = "Sounds";

	public GameManager() {
		// Unity objects should not use coalescing assignment. (UNT0023)
		// Instance ??= this;
		if (Instance != null) return;
		Instance = this;

		Blackboard.GLOBAL.Set(SoundsKey, Sounds);
	}

	private void Awake() {
		// TODO: strip MonoBehavior from GameManager. Put this into the constructor.
		if (Instance != this) {
			Instance.PlayMusic(menuMusic);
			Destroy(gameObject);
			return;
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start() {
		foreach (string volumeParameter in VolumeParameters) {
			SetMixerAttenuation(
				volumeParameter,
				PlayerPrefs.GetFloat(volumeParameter, 1f)
			);
		}
	}

	private void Update() {
		for (int i = Sounds.Count - 1; i >= 0; i--) {
			Sounds[i].Update();
		}
	}

	/**
	 * <summary>
	 * Generate the map, spawn the pawns/controllers, and start the game
	 * </summary>
	 */
	public void StartGame(long seed = 0, bool multiplayer = false) {
		PlayMusic(gameMusic);

		mapGenerator.Generate(seed);

		int numPlayers = multiplayer ? 2 : 1;

		int spawnCount = (spawnPlayerAtCamera ? 0 : numPlayers) + AICount;
		List<PawnSpawnPoint> selectedSpawnPoints = SpawnPoints.TakeRandom(spawnCount);

		defaultCamera.gameObject.SetActive(false);

		for (int i = 0; i < numPlayers; i++) {
			SpawnPlayer(
				defaultPawnPrefab,
				defaultControllerPrefab,
				spawnPlayerAtCamera
					? Optional<PawnSpawnPoint>.None
					: selectedSpawnPoints[^(i + 1)]
			);
		}

		for (int i = 0; i < AICount; i++) {
			Controller controller = (i % 4) switch {
				0 => aiControllerPrefab1,
				1 => aiControllerPrefab2,
				2 => aiControllerPrefab3,
				_ => aiControllerPrefab4
			};

			SpawnPlayer(defaultPawnPrefab, controller, selectedSpawnPoints[i]);
		}
	}

	/**
	 * <summary>
	 * Register a spawn point with the game manager
	 * </summary>
	 */
	public void RegisterSpawnPoint(PawnSpawnPoint spawnPoint) {
		SpawnPoints.Add(spawnPoint);
	}

	/**
	 * <summary>
	 * Unregister a spawn point with the game manager
	 * </summary>
	 */
	public void UnregisterSpawnPoint(PawnSpawnPoint spawnPoint) {
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
		HUD hud = Instantiate(defaultHUDPrefab);
		playerController.SetHUD(hud);

		if (!controller.PawnOptional) return;

		Camera camera = Instantiate(defaultCameraPrefab);
		controller.BindCamera(camera);
		hud.SetCamera(camera);

		if (PlayerControllers.Count == 2) {
			// HACK: we're assuming RegisterController is only called when the game starts
			// which means Pawn and AttachedCamera are always valid
			Camera camera1 = PlayerControllers[0].Pawn.AttachedCamera.Value;
			camera1.rect = new Rect(0, 0.5f, 1, 0.5f);
			camera.rect = new Rect(0, 0, 1, 0.5f);
		}
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
	 * Spawns a new pawn at a random spawn point and binds it to the given (existing) controller.
	 * </summary>
	 */
	public void RespawnPlayer(
		Pawn pawnPrefab,
		Controller controller,
		Optional<PawnSpawnPoint> spawnPoint
	) {
		spawnPoint |= SpawnPoints.Random();
		SpawnPlayer(pawnPrefab, controller, spawnPoint, false);
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
	 * <seealso cref="SpawnPlayer(Pawn, Controller, Pose, bool)"/>
	 */
	public void SpawnPlayer(
		Pawn pawnPrefab,
		Controller controllerPrefab,
		Optional<PawnSpawnPoint> spawnPoint,
		bool controllerIsPrefab = true
	) {
		Pose spawn = spawnPoint
			.Then(x => new Pose(x.transform.position, x.transform.rotation))
			.ValueOrDefault(Pose.identity);

#if UNITY_EDITOR
		if (!spawnPoint) {
			SceneView sceneView = SceneView.lastActiveSceneView;
			spawn.rotation = Quaternion.Euler(0, sceneView.rotation.eulerAngles.y, 0);
			spawn.position = sceneView.pivot
				- sceneView.rotation * Vector3.forward * sceneView.cameraDistance;
		}
#endif

		SpawnPlayer(pawnPrefab, controllerPrefab, spawn, controllerIsPrefab);
	}

	/**
	 * <summary>
	 * Spawn a new pawn at the given spawn pose.<br/>
	 * Spawns a new controller if it's a prefab.
	 * </summary>
	 * <seealso cref="SpawnPlayer(Pawn, Controller, Optional&lt;PawnSpawnPoint&gt;)"/> 
	 */
	public void SpawnPlayer(
		Pawn pawnPrefab,
		Controller controller,
		Pose spawn,
		bool controllerIsPrefab = true
	) {
		Pawn pawn = Instantiate(
			pawnPrefab,
			spawn.position,
			spawn.rotation
		);

		controller.IsPrefabDefinition();

		if (controllerIsPrefab) {
			controller = Instantiate(
				controller,
				spawn.position,
				spawn.rotation
			);
		}

		controller.Possess(pawn);
	}

	/**
	 * <summary>
	 * Enable the given menu
	 * </summary>
	 */
	public void SetMenuActive(Optional<Menu> menu) {
		ActiveMenu.Then(x => x.gameObject.SetActive(false));
		ActiveMenu = menu;
		ActiveMenu.Then(x => x.gameObject.SetActive(true));
	}

	/**
	 * <summary>
	 * Play a music clip
	 * </summary>
	 */
	public void PlayMusic(AudioClip clip) {
		MusicSource.clip = clip;
		MusicSource.Play();
	}

	/**
	 * <summary>
	 * Play a one-shot sound
	 * </summary>
	 */
	public void PlayOneShot(AudioClip clip, float volume = 1f) {
		EffectSource.PlayOneShot(clip, volume);
	}

	/**
	 * <summary>
	 * Set the volume of an audio mixer group
	 * </summary>
	 */
	public void SetMixerAttenuation(string volumeParameter, float value) {
		AudioMixer.SetFloat(
			volumeParameter,
			value <= 0f ? -80f : Mathf.Log10(value) * 20f
		);
	}
}