using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryManager : MonoBehaviour {

    // Set in editor
    public RectTransform gameEndPanel;
    public RectTransform gameLostPanel;
    public RectTransform messagePanel;
    public Text authorText;
    public Text speechText;
    public GameObject shieldModel;
    public GameObject spaceEffects;
    public int charSoundFrequency;

    private enum Character
    {
        CAPTAIN,
        ICE,
        BASE_COMMAND,
    }
    private Character currentSpeaker;

    private Color basePanelColor = new Color(1, 1, 1, (float)150 / 255);
    private Color bossPanelColor = new Color((float)182 / 255, 1, 1, (float)150 / 255);
    private Color captainPanelColor = new Color((float)51 / 255, (float)204 / 255, 0, (float)100 / 255);

    private enum WaitAction
    {
        TIMER,
        SPEECH,
        BOSS_DEFEATED,
        GAME_OVER
    }
    private WaitAction waitAction;

    private int storyCounter = 0;
    private float waitTime;
    private float timer = 0;
    private float charTimer = 0;
    private int charSoundTimer = 0;
    private const float TIME_BETWEEN_CHARS = 0.0f;
    private string speechString;
    private int maxProgress = 0;

    private const int DRONE_START_COUNTER = 9;
    private const int BOSS_START_COUNTER = 39;

    public DroneSpawner droneSpawner;
    public Boss boss;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private AudioSource myAs;
    

	// Use this for initialization
	void Start () {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        myAs = GetComponent<AudioSource>();
        HandleStoryProgress();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        charTimer += Time.deltaTime;

        // Skip dialogue
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)) &&
            messagePanel.gameObject.activeSelf && waitAction == WaitAction.TIMER) {
                timer += waitTime + 1;
        }

        switch (waitAction) {
            case WaitAction.TIMER:
                if (timer > waitTime) {
                    HandleStoryProgress();
                }
                break;
            case WaitAction.SPEECH:
                if (charTimer > TIME_BETWEEN_CHARS) {
                    AddToSpeechBubble();
                }
                break;
            case WaitAction.BOSS_DEFEATED:
                if (boss.healthRemaining <= 0) {
                    HandleStoryProgress();
                }
                break;
            case WaitAction.GAME_OVER:
                // do nothing
                break;
        }
	}

    // If anyone ever reads this code: Don't do this. It's terrible.
    // It doesn't scal well at all. If you want to add an event at the beginning
    // you need to renumber all of the events. Throw things like this into external
    // configuration files that can be loaded and sorted at runtime :)
    void HandleStoryProgress() {
        switch (storyCounter) {
            case 0: // Start of game
                playerAttack.canAttack = false;
                playerMovement.canMove = true;
                shieldModel.SetActive(false);
                SetTimer(2);
                break;
            case 1:
                ActivateSpeech(Character.BASE_COMMAND,
                    "Captain, we're getting reports of lots of drones in the incoming area.");
                break;
            case 2:
                ActivateSpeech(Character.CAPTAIN,
                    "Understood. What kind of heat am I packing on this ship?");
                break;
            case 3:
                ActivateSpeech(Character.BASE_COMMAND,
                    "You've got state of the art HEAT missiles, captain. Nothing to worry about.");
                break;
            case 4:
                ActivateSpeech(Character.CAPTAIN,
                    "HEAT missiles? I've never heard of them.");
                break;
            case 5:
                ActivateSpeech(Character.BASE_COMMAND,
                    "HEAT missiles: High Enthalpy Automated Tracking missiles. They'll search for anything in " +
                    "the area with high heat and boost towards it. Just hold down the left mouse button and your worries " +
                    "will be gone! It's the latest tech!");
                break;
            case 6:
                ActivateSpeech(Character.CAPTAIN,
                    ".......................");
                break;
            case 7:
                ActivateSpeech(Character.CAPTAIN,
                    "Doesn't my ship emit a lot of heat?");
                break;
            case 8:
                ActivateSpeech(Character.BASE_COMMAND,
                    "Don't worry, the drones emit more heat than you do. " +
                    "Just be careful not to fly into the HEAT missiles after they activate.");
                break;
            case 9:
                playerMovement.canMove = true;
                playerAttack.canAttack = true;
                shieldModel.gameObject.SetActive(true);
                ActivateSpeech(Character.CAPTAIN,
                    "Drones incoming! HEAT missiles enabled! Regenerating shields activated!");
                break;
            case 10:
                DeactivateSpeech();
                SetTimer(20);
                droneSpawner.gameObject.SetActive(true);
                droneSpawner.maxSpawnTime = 3;
                droneSpawner.minSpawnTime = 2;
                break;
            case 11:
                SetTimer(20);
                droneSpawner.maxSpawnTime = 2;
                droneSpawner.minSpawnTime = 1;
                break;
            case 12:
                SetTimer(20);
                droneSpawner.maxSpawnTime = 1;
                droneSpawner.minSpawnTime = 0.5f;
                break;
            case 13:
                ActivateSpeech(Character.CAPTAIN,
                    "Base command! There's more drones than I've ever seen before here! What's going on?");
                droneSpawner.maxSpawnTime = 0.5f;
                droneSpawner.minSpawnTime = 0.1f;
                break;
            case 14:
                ActivateSpeech(Character.BASE_COMMAND,
                    "Uncertain, captain. Our sensors are reading some kind of anomoly headed your way!");
                break;
            case 15:
                DeactivateSpeech();
                SetTimer(20);
                break;
            case 16:
                droneSpawner.gameObject.SetActive(false);
                SetTimer(10);
                break;
            case 17:
                playerAttack.canAttack = false;
                playerMovement.canMove = false;
                ActivateSpeech(Character.CAPTAIN,
                    "Looks like the drones are gone.");
                break;
            case 18:
                shieldModel.SetActive(true); // In case I'm debugging and starting from here
                SpawnBoss();
                ActivateSpeech(Character.ICE,
                    "Just when things were starting to heat up!");
                break;
            case 19:
                ActivateSpeech(Character.CAPTAIN,
                    "Who the hell is that?!");
                break;
            case 20:
                ActivateSpeech(Character.BASE_COMMAND,
                    GetCharacterLabel(Character.ICE) + ". He's one of the drone commanders. You are ordered to destroy " +
                    "his ship at all costs. Your HEAT missiles should do the job nicely.");
                break;
            case 21:
                ActivateSpeech(Character.CAPTAIN,
                    "Fire missiles!");
                break;
            case 22:
                FireMissile();
                break;
            case 23:
                FireMissile();
                SetTimer(3);
                break;
            case 24:
                ActivateSpeech(Character.CAPTAIN,
                    "Base Command! The HEAT missiles aren't tracking Ice!");
                break;
            case 25:
                ActivateSpeech(Character.BASE_COMMAND,
                    "Strange. It looks like his ship is too cold to be picked up by the HEAT missiles' sensors.");
                break;
            case 26:
                ActivateSpeech(Character.CAPTAIN,
                    "What am I supposed to do if my HEAT missiles won't track anything?");
                break;
            case 27:
                playerAttack.attackSelf = true;
                ActivateSpeech(Character.ICE,
                    "Let me fix that for you.");
                break;
            case 28:
                DeactivateSpeech();
                shieldModel.SetActive(false);
                playerHealth.targetColor = playerHealth.redColor2;
                SetTimer(3);
                break;
            case 29:
                ActivateSpeech(Character.CAPTAIN,
                    "Engine temperatures reaching critical levels! He's trying to burn us alive!");
                break;
            case 30:
                ActivateSpeech(Character.ICE,
                    "Need some ice for that burn?");
                break;
            case 31:
                shieldModel.SetActive(true);
                ActivateSpeech(Character.CAPTAIN,
                    "FIRE ZE MISSILES!");
                break;
            case 32:
                FireMissile();
                break;
            case 33:
                FireMissile();
                SetTimer(3);
                break;
            case 34:
                ActivateSpeech(Character.CAPTAIN,
                    "Base command! The HEAT missiles just tracked back to our ship!");
                break;
            case 35:
                ActivateSpeech(Character.BASE_COMMAND,
                    "It looks like the raised engine temperatures is tricking their sensors!");
                break;
            case 36:
                ActivateSpeech(Character.CAPTAIN,
                    "Alright. Tell me what auxiliary weapons systems I have at my disposal on this vessel.");
                break;
            case 37:
                ActivateSpeech(Character.BASE_COMMAND,
                    ".....................");
                break;
            case 38:
                ActivateSpeech(Character.BASE_COMMAND,
                    "Due to recent budget cuts we have enacted a One Weapon Per Ship policy. " +
                    "The HEAT missiles are all you have.");
                break;
            case 39:
                ActivateSpeech(Character.CAPTAIN,
                    "I guess I'll have to do this the hard way...");
                break;
            case 40:
                DeactivateSpeech();
                playerAttack.canAttack = true;
                playerMovement.canMove = true;
                boss.currentMode = Boss.Mode.ATTACK_DONE;
                waitAction = WaitAction.BOSS_DEFEATED;
                break;
            case 41:
                ActivateSpeech(Character.ICE,
                    "The HEAT missiles are causing intense damage! The structure of my ship is melting away!");
                playerAttack.canAttack = false;
                playerMovement.canMove = false;
                ClearAllMissiles(); // Don't let the player die after they win! :P
                boss.currentMode = Boss.Mode.STORY;
                boss.moveTarget = boss.transform.position;
                boss.GetComponent<Rigidbody>().AddTorque(new Vector3(1, 1, 0) * 1000);
                break;
            case 42:
                ActivateSpeech(Character.ICE,
                    "Ugggghgghghg...");
                break;
            case 43:
                boss.Explode();
                boss.gameObject.SetActive(false);
                ActivateSpeech(Character.ICE,
                    ".............................");
                break;
            case 44:
                ActivateSpeech(Character.CAPTAIN,
                    "I guess some people just can't take the HEAT.");
                break;
            case 45:
                DeactivateSpeech();
                gameEndPanel.gameObject.SetActive(true);
                waitAction = WaitAction.GAME_OVER;
                break;
        }
        maxProgress = storyCounter;
        storyCounter++;
    }

    private void FireMissile() {
        playerAttack.Attack();
        SetTimer(playerAttack.attackSpeed);
    }

    private void SpawnBoss() {
        boss.gameObject.SetActive(true);
        int spawnOnRight = (playerMovement.transform.position.x > 0 ? -1 : 1);
        boss.transform.position = Vector3.right * 15 * spawnOnRight;
        boss.lookTarget = Vector3.right * -spawnOnRight;
        boss.moveTarget = boss.transform.position + Vector3.right * -spawnOnRight * 10;
    }

    private void SetTimer(float waitTime) {
        this.waitTime = waitTime;
        timer = 0;
        waitAction = WaitAction.TIMER;
    }

    private void ActivateSpeech(Character author, string speech) {
        messagePanel.gameObject.SetActive(true);
        authorText.text = GetCharacterLabel(author);
        currentSpeaker = author;
        SetMessagePanelColor(author);
        speechText.text = "";
        speechString = speech;
        AddToSpeechBubble();
        waitAction = WaitAction.SPEECH;
    }

    private void AddToSpeechBubble() {
        charTimer = 0;
        if (speechText.text.Length < speechString.Length) {
            speechText.text = speechString.Substring(0, speechText.text.Length + 1);
            if (++charSoundTimer > charSoundFrequency) {
                charSoundTimer = 0;
                myAs.pitch = GetCharacterPitch(currentSpeaker);
                myAs.Play();
            }
        }
        else {
            SetTimer(speechString.Length * 0.02f + 2);
            //SetTimer(0); // quick speech :)
        }
    }

    private void DeactivateSpeech() {
        messagePanel.gameObject.SetActive(false);
    }

    private void SetMessagePanelColor(Character character) {
        if (character == Character.BASE_COMMAND) {
            messagePanel.GetComponent<Image>().color = basePanelColor;
        }
        else if (character == Character.CAPTAIN) {
            messagePanel.GetComponent<Image>().color = captainPanelColor;
        }
        else {
            messagePanel.GetComponent<Image>().color = bossPanelColor;
        }
    }

    private string GetCharacterLabel(Character character) {
        if (character == Character.BASE_COMMAND) {
            return "Base Command";
        }
        else if (character == Character.CAPTAIN) {
            return "Captain";
        }
        else {
            return "Ice";
        }
    }

    private float GetCharacterPitch(Character character) {
        if (character == Character.BASE_COMMAND) {
            return Random.Range(0.5f, 0.7f);
        }
        else if (character == Character.CAPTAIN) {
            return Random.Range(0.8f, 1.0f);
        }
        else {
            return Random.Range(1.1f, 1.3f);
        }
    }

    public void RegisterHeroDeath() {
        waitAction = WaitAction.GAME_OVER;
        boss.currentMode = Boss.Mode.STORY;
        boss.moveTarget = boss.transform.position;
        gameLostPanel.gameObject.SetActive(true);
    }

    public void OnClickRetry() {
        // Terrible hard-coded logic
        gameLostPanel.gameObject.SetActive(false);
        ResetPlayer();
        droneSpawner.gameObject.SetActive(false);
        if (boss.gameObject.activeSelf) {
            boss.Init();
        }
        ClearAllDrones();
        ClearAllMissiles();
        ClearAllParticleSystems();
        boss.gameObject.SetActive(false);
        if (maxProgress >= BOSS_START_COUNTER) {
            storyCounter = BOSS_START_COUNTER;
            SpawnBoss();
            playerAttack.canAttack = false;
            playerMovement.canMove = false;
        }
        else if (maxProgress >= DRONE_START_COUNTER) {
            storyCounter = DRONE_START_COUNTER;
        }
        else {
            Debug.LogError("Not possible");
        }
        HandleStoryProgress();
    }

    private void ResetPlayer() {
        playerAttack.gameObject.SetActive(true);
        playerAttack.GetComponent<PlayerHealth>().Init();
        playerAttack.transform.position = Vector3.zero;
    }

    private void ClearAllDrones() {
        foreach (Drone drone in FindObjectsOfType<Drone>()) {
            Destroy(drone.gameObject);
        }
    }

    private void ClearAllMissiles() {
        foreach (Rocket missile in FindObjectsOfType<Rocket>()) {
            Destroy(missile.gameObject);
        }
    }

    private void ClearAllParticleSystems() {
        spaceEffects.SetActive(false);
        foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>()) {
            Destroy(ps.gameObject);
        }
        spaceEffects.SetActive(true);
    }

    public void OnClickTwitter() {
        Application.OpenURL("https://www.twitter.com/intrepid_games");
    }

    public void GoToMenu() {
        Application.LoadLevel("menu");
    }
}
