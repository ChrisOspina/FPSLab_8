using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour
{
    // respond on collisions
	void OnCollisionEnter(Collision newCollision)
	{
		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile") {
			//SceneManager will load the main menu
			 SceneManager.LoadScene("Menu");
             GameManager.gm.gameState = GameState.Idle;
		}
	}
}
