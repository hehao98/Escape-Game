using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	[SerializeField] private Text scoreLabel;
	[SerializeField] private SettingPopup settingPopup;
	[SerializeField] private Text messageLabel;
	[SerializeField] private SceneController sceneController;

	private bool settingPopupOpen = false;

	// Use this for initialization
	void Start () {
		settingPopup.Close ();
		Cursor.visible = false;
		scoreLabel.text = "待收集物品: " + sceneController.getRemainItem().ToString ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {  
			if (settingPopupOpen) {
				settingPopup.Close (); 
				Cursor.visible = false;
				settingPopupOpen = !settingPopupOpen;
			} else {
				settingPopup.Open (); 
				Cursor.visible = true;
				settingPopupOpen = !settingPopupOpen;
			}
		}  
	}

	public void PickedUpItem() {
		scoreLabel.text = "待收集物品: " + sceneController.getRemainItem().ToString ();
		PopUpMessage ("好样的！你捡到了一个物品！");
	}

	public void PlayerCatched() {
		PopUpMessage ("你被鬼抓住了！");
		settingPopup.Open ();
		Cursor.visible = true;
	}

	public void GameRestart() {
		Application.LoadLevel ("Prototype1");
	}

	public void GameContinue() {
		settingPopup.Close ();
		Cursor.visible = false;
	}

	public void GameWin() {
		PopUpMessage ("恭喜你！你完成了任务！");
		settingPopup.Open ();
		Cursor.visible = true;
	}

	public void PopUpMessage(string message) {
		messageLabel.CrossFadeAlpha (1.0f, 0.0f, false);
		messageLabel.text = message;
		messageLabel.CrossFadeAlpha (0.0f, 5.0f, false);
	}
}
