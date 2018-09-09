using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenue : MonoBehaviour {

	private bool isPause = false;

	public GameObject panel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*DUMMY MENUE FUNCTIONS*/
		if(Input.GetKeyDown(KeyCode.Escape)){
			ToggleTimeScale();
		}
	}

	void ToggleTimeScale(){
		if (!isPause) {
			Time.timeScale = 0;
			panel.SetActive (true);
		} else {
			Time.timeScale = 1;
			panel.SetActive (false);
		}
		isPause = !isPause;
	}

	public void Resume(){
		panel.SetActive (false);
		Time.timeScale = 1;
		isPause = !isPause;
	}

	public void ExitApplication(){
		Application.Quit ();
	}

	public void jumpToMainMenue(){
		SceneManager.LoadScene("MainMenu");
	}
}
