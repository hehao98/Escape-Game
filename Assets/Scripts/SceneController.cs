using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	[SerializeField] private UIController uiController;

	private int remainItem = 5;
	public int getRemainItem() { return remainItem; }

	// Use this for initialization
	void Start () {
		//Debug.Log ("rrr");
		uiController.PopUpMessage ("收集所有的金色物品！");
		Messenger.AddListener (GameEvent.PICKED_UP_ITEM, PickedUpItem);
		Messenger.AddListener (GameEvent.PLAYER_CATCHED, PlayerCatched);
	}
	
	// Update is called once per frame
	void Update() {

	}

	void OnDestroy () {
		Messenger.RemoveListener (GameEvent.PICKED_UP_ITEM, PickedUpItem);
		Messenger.RemoveListener (GameEvent.PLAYER_CATCHED, PlayerCatched);
	}

	public void PickedUpItem() {
		Debug.Log ("asdasdsad");
		remainItem--;
		uiController.PickedUpItem ();
		if (remainItem == 0) {
			uiController.GameWin ();
		}
	}

	public void PlayerCatched() {
		uiController.PlayerCatched ();
	}
}
