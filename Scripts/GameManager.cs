using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static GameManager gm;
	public GameState gameState;

	// public variables
	public int score=0;

	public bool canBeatLevel = false;
	public int beatLevelScore=0;

	public float startTime=5.0f;
	
	public Text mainScoreDisplay;
	public Text mainTimerDisplay;

	public GameObject gameOverScoreOutline;

	public AudioSource musicAudioSource;

	public bool gameIsOver = false;
	public static bool isPaused = false;

	public GameObject playAgainButtons;
	public string playAgainLevelToLoad;

	public GameObject nextLevelButtons;
	public string nextLevelToLoad;

	public GameObject backtoMenuButtons;

	public GameObject pauseMenuUI;

	public string CurrentLevel;

	private float currentTime;

	 void Awake() {
		gm = this;
	}

	// setup the game
	void Start () {

		// set the current time to the startTime specified
		currentTime = startTime;

		// get a reference to the GameManager component for use by other scripts
		if (gm == null) 
			gm = this.gameObject.GetComponent<GameManager>();

		// init scoreboard to 0
		if(mainScoreDisplay)mainScoreDisplay.text = "0";

		// inactivate the gameOverScoreOutline gameObject, if it is set
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (false);

		// inactivate the playAgainButtons gameObject, if it is set
		if (playAgainButtons)
			playAgainButtons.SetActive (false);

		// inactivate the nextLevelButtons gameObject, if it is set
		if (nextLevelButtons)
			nextLevelButtons.SetActive (false);

	   // inactivate the backtoMenuButtons gameObject, if it is set
		if (backtoMenuButtons)
			backtoMenuButtons.SetActive (false);

		//inactivate the pauseMenuUI gameObject, if is not set
		if(pauseMenuUI)
			pauseMenuUI.SetActive (false);
	}

	// this is the main game event loop
	void Update () {
		if (!gameIsOver) {
			if (canBeatLevel && (score >= beatLevelScore)) {  // check to see if beat game
			//This switch statement will distinguish the final level from the rest of the game in terms of level compeletion
				switch(CurrentLevel)
				{
					case "Level3":
						WinGame();
						break;
					default:
						BeatLevel ();
						break;
				}
			} else if (currentTime < 0) { // check to see if timer has run out
				EndGame ();
			} else { // game playing state, so update the timer
				currentTime -= Time.deltaTime;
				if(mainTimerDisplay)
					mainTimerDisplay.text = currentTime.ToString ("0.00");				
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
	}

	void EndGame() {
		// game is over
		gameIsOver = true;

		//gameState is set GameOver
		gm.gameState = GameState.GameOver;
		// repurpose the timer to display a message to the player
		if(mainTimerDisplay)
			mainTimerDisplay.text = "GAME OVER";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (true);
	
		// activate the playAgainButtons gameObject, if it is set 
		if (playAgainButtons)
			playAgainButtons.SetActive (true);

		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}
	
	void BeatLevel() {
		// game is over
		gameIsOver = true;

		//gameState is set to WinLevel
		gm.gameState = GameState.winLevel;
		
		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "LEVEL COMPLETE";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
		{
			gameOverScoreOutline.SetActive (true);
			//then we will set the text to display the score
	       //gameOverScoreOutline.SetText("Your Score: "+score);
		}

		// activate the nextLevelButtons gameObject, if it is set 
		if (nextLevelButtons)
			nextLevelButtons.SetActive (true);
		
	}

	void WinGame(){
		// game is over
		gameIsOver = true;

		//gameState is set to beatGame
		gm.gameState = GameState.beatGame;
		
		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "GAME COMPLETE";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
		{
			gameOverScoreOutline.SetActive (true);
			//then we will set the text to display the score
	       //gameOverScoreOutline.SetText("Your Score: "+score);
		}

		// activate the backtoMenuButtons gameObject, if it is set 
		if (backtoMenuButtons)
			backtoMenuButtons.SetActive (true);

	}

	// public function that can be called to update the score or time
	public void targetHit (int scoreAmount, float timeAmount)
	{
		// increase the score by the scoreAmount and update the text UI
		score += scoreAmount;
		mainScoreDisplay.text = score.ToString ();
		
		// increase the time by the timeAmount
		currentTime += timeAmount;
		
		// don't let it go negative
		if (currentTime < 0)
			currentTime = 0.0f;

		// update the text UI
		mainTimerDisplay.text = currentTime.ToString ("0.00");
	}

	// public function that can be called to restart the game
	public void RestartGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
        SceneManager.LoadScene(playAgainLevelToLoad);
		CurrentLevel = playAgainLevelToLoad;
	}

	// public function that can be called to go to the next level of the game
	public void NextLevel ()
	{
		// we are just loading the specified next level (scene)
        SceneManager.LoadScene(nextLevelToLoad);
		CurrentLevel = nextLevelToLoad;
	}
	//public function that can be called to pause the game
	public void PauseGame()
	{
		   //If the game is not paused
           if(!isPaused)
           {
			   gm.gameState = GameState.pause;
			   pauseMenuUI.SetActive(true);
			   //the timeScale component is important because it toggles whether time is in motion or not.
      		   Time.timeScale = 0f;
               isPaused = true;
           }
		   //elsewhere
           else
           {
              pauseMenuUI.SetActive(false);
       		  Time.timeScale = 1f;
       		  isPaused = false;
			  //The switch statement is there because it makes the gameState accurate to the current level.
			  switch(CurrentLevel)
			  {
				  case "Level1":
				  	gm.gameState = GameState.Level1Playing;
					break;
				 case "Level2":
				 	gm.gameState = GameState.Level2Playing;
					break;
				 case "Level3":
				 	gm.gameState = GameState.Level3Playing;
					break;
			  }    
           }
       
	}
	
	public void UpdateGameState(GameState newState)
	{
        gameState = newState;
		switch(newState)
		{
			case GameState.Idle:
				gameState = GameState.Idle;
				break;
			case GameState.Level1Playing:
				gameState = GameState.Level1Playing;
				break;
			case GameState.Level2Playing:
				gameState = GameState.Level2Playing;
				break;
			case GameState.Level3Playing:
				gameState = GameState.Level3Playing;
				break;
			case GameState.pause:
				gameState = GameState.pause;
				break;
			case GameState.GameOver:
				gameState = GameState.GameOver;
				break;
			case GameState.winLevel:
				gameState = GameState.winLevel;
				break;
			case GameState.beatGame:
				gameState = GameState.beatGame;
				break;
		}
		


	}
}
public enum GameState
{
	Idle,
	Level1Playing,
	Level2Playing,
	Level3Playing,
	pause,
	GameOver,
	winLevel,
	beatGame
}
