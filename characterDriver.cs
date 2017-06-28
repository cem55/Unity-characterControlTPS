using UnityEngine;
using System.Collections;

public class characterDriver : MonoBehaviour {
	private characterCore cCore;
	private Vector3 movement;
    private bool jump;

	// Use this for initialization
	void Start () {
		cCore = GetComponent<characterCore> ();
	}

	void FixedUpdate () {
		movement = new Vector3 (Input.GetAxisRaw ("Horizontal"),0f, Input.GetAxisRaw ("Vertical"));
        jump = Input.GetKeyDown(KeyCode.Space);
		cCore.Move (movement,jump);
	}
}
