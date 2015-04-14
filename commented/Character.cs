using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public float speed;
	public VerletOcto verletOcto;
	private Vector3 UP;
	private Vector3 RIGHT;

	// Use this for initialization
	void Start () {
		changeSpeed (speed);
	}

	// set and change moving speed 
	public void changeSpeed(float s){
		speed = s;
		UP = new Vector3 (0, 0, speed);
		RIGHT = new Vector3 (speed, 0, 0);
	}

	public void zoomOcto(float scale) {
		verletOcto.head.transform.localScale = verletOcto.head.transform.localScale / scale;
		//Rescale the tentacles
		for (int i=0; i<verletOcto.joints1.Length; i++) {
			verletOcto.joints1[i].transform.localScale = verletOcto.joints1[i].transform.localScale / scale;
			verletOcto.joints2[i].transform.localScale = verletOcto.joints2[i].transform.localScale / scale;
			verletOcto.joints3[i].transform.localScale = verletOcto.joints3[i].transform.localScale / scale;
		}
		//Rescale the constraint distances between tentacles and head
		verletOcto.headToTentacle = verletOcto.tentacleSegment / scale;
		verletOcto.tentacleSegment = verletOcto.tentacleSegment / scale;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.position = transform.position + UP;
		}else if (Input.GetKey(KeyCode.DownArrow)){
			transform.position = transform.position - UP;
		}else if (Input.GetKey(KeyCode.LeftArrow)){
			transform.position = transform.position - RIGHT;
		}else if (Input.GetKey(KeyCode.RightArrow)){
			transform.position = transform.position + RIGHT;
		}
	}
}
