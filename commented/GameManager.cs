using UnityEngine;
using System.Collections;

/**
 * This class manages the fractal and the character interaction
 * 1) setup fractal at the begining of the game
 * 2) handle zooming 
 * 3) shedule animation
 */

public class GameManager : MonoBehaviour {

	// prefabs
	public FractalBox templateFractal;
	public Wall templateWall;
	public Wall templateCenter;
	public GameObject templateTile;

	public Character player;

	// for animation
	private bool zooming = false;
	private int zoomFrames = 20;
	private int zoomIter = 0;
	private Vector3 zoomFrac;
	private Vector3 zoomCam;

	// the largest detail level existing in the game currently
	public FractalBox baseFrac;

	// current detail level where the player is in 
	public FractalBox fractal;
	private int depth = 0;

	// create the first fractal at depth 0
	void Start () {
		fractal = (FractalBox)GameObject.Instantiate (templateFractal);
		fractal.initialize (depth, 0);
		player.transform.parent = fractal.transform;
		fractal.transform.parent = transform;
		baseFrac = fractal;
	}
	
	// handeling transition
	void Update () {
		isDesceding (); // check if character entered a child 
		isAscending (); // check if character left child
		if (zooming){ // preform animation
			if (zoomIter == zoomFrames){ // end of animation
				// reset camera size
				Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x,
				                                                  40, Camera.main.transform.localPosition.z);
				Camera.main.transform.localScale = new Vector3(1,1,1);
				zooming = false;
				return;
			}
			zoomIter ++;
			Camera.main.transform.localPosition = Camera.main.transform.localPosition + zoomCam;
			baseFrac.transform.localScale = baseFrac.transform.localScale + zoomFrac;
		}
	}

	/* Check if the character is in a child of the current fractal
	 * point-polygon intersection with all the child
	 */
	public bool isDesceding(){
		float x = player.transform.localPosition.x; 
		float y = player.transform.localPosition.z;
		for (int i=0; i<Fractal.blockPos.Length; i++){
			Vector2 t = Fractal.blockPos[i];
			if (x > t.x && x < t.x+Fractal.relaSize &&
			    y > t.y && y < t.y+Fractal.relaSize){
				changeFractal(fractal.getChild(i), true);
				fractal.removeGrandParent();
				zoomIn();
				return true;
			}
		}
		return false;
	}
	/* Check if the character is outside of the current fractal
	 * point-polygon intersection with the current fractal
	 */
	public bool isAscending(){
		if (fractal.depth == 0) return false;
		float x = player.transform.localPosition.x; 
		float y = player.transform.localPosition.z;
		if (x > 1 || x < 0 || y > 1 || y < 0){
			fractal.addGrandParent();
			changeFractal(fractal.Parent, false);
			zoomOut();
			return true;
		}
		return false;
	}

	// change current fractal
	private void changeFractal(FractalBox fb, bool desc){
		if (desc){
			fb.initialize (fb.depth, 0);
			fb.removeTile();
		}else{
			fractal.removeChild(); 
			fractal.addTexture();
		}
		fractal = fb;
		player.transform.parent = fb.transform;
	}

	/**
	 * Zooming
	 */
	public void zoomIn(){
		zoom (1/Fractal.relaSize);
	}

	public void zoomOut(){
		zoom (Fractal.relaSize);
	}

	// schedule animation
	private void zoom(float scale){
		player.transform.localScale = player.transform.localScale / scale;
		Debug.Log ("Player scaled to be " + player.transform.localScale);
		Camera.main.transform.parent = fractal.transform;
		Vector3 camPos = Camera.main.transform.localPosition;
		zoomCam = new Vector3(0.5f - camPos.x, 0, 0.5f - camPos.z)/zoomFrames;
		zoomFrac = baseFrac.transform.localScale * (scale-1) / zoomFrames;
		zoomIter = 0;
		// random background color
		Camera.main.backgroundColor = new Color(Random.value, Random.value, Random.value);
		zooming = true;
	}
}
