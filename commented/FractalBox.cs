using UnityEngine;
using System.Collections;

/**
 * This class implements the fractal in a tree structure and 
 * the l-system
 */
public class FractalBox : MonoBehaviour {
	
	public GameManager gm;

	// tree structure
	private FractalBox[] childs;
	public FractalBox getChild(int i){
		return childs [i];
	}
	public void setChild(int i, FractalBox fb){
		childs [i] = fb;
	}
	public void initChilds(){
		childs = new FractalBox[9];
	}
	public FractalBox parent;
	public FractalBox Parent{
		get { return parent;}
		set { parent = value;}
	}
	// this is to remember which child the parent of the current object is to the grand parent
	private int childIndex;
	
	private Wall[] walls;

	// the detail level number, starting with 0
	public int depth;
	
	private GameObject tile; // the texture is stored here

	// this is fractal generating algorithm
	// this returns the depth value of the last generated child
	public int initialize(int d, int iter){
		depth = d;
		if (iter == Fractal.maxIter) { // add texture instead of a fractal
			addTexture();
			return d;
		}
		if (d == 0){ // special initialization for the first detail level
			setFirst();
		}
		addWalls ();
		return generateChild (d+1, iter+1);
	}

	private void setFirst(){
		transform.localScale = new Vector3(Fractal.maxSize, Fractal.maxSize, Fractal.maxSize);
		transform.position = new Vector3(-Fractal.maxSize/2, 0, -Fractal.maxSize/2);
	}
		
	// setup children
	public int generateChild(int d, int iter){
		initChilds ();
		int newD = d;
		for (int i=0; i <Fractal.blockPos.Length; i++){
			newD = addChild(i, d, iter);
		}
		return newD;
	}

	// child-birth-and-death

	public int addChild(int i, int d, int iter){
		childs[i] = (FractalBox) GameObject.Instantiate(gm.templateFractal);
		childs[i].parent = this;
		childs[i].transform.parent = transform;
		childs[i].transform.localScale = new Vector3(Fractal.relaSize, Fractal.relaSize, Fractal.relaSize);
		childs[i].transform.localPosition = new Vector3(Fractal.blockPos[i].x, 0, Fractal.blockPos[i].y);
		return childs[i].initialize(d, iter);
	}
	
	public void removeChild(){
		for (int i=0; i<Fractal.blockPos.Length; i++){
			Destroy(childs[i].gameObject);
		}
		removeWalls ();
	}

	// grandParent-death-and-revive

	public void removeGrandParent(){
		if (depth < 2) return; // has no grand parent
		gm.baseFrac = parent;
		FractalBox grandp = parent.parent;
		grandp.removeWalls ();
		for (int i=0; i<Fractal.blockPos.Length; i++){
			if (grandp.getChild(i).Equals(parent)){ // find the childIndex
				childIndex = i;
				continue;
			}
			// distroy uncles
			Destroy(grandp.getChild(i).gameObject);
		}
		// set the parent as the first child of the GameManager object
		parent.transform.parent = gm.transform;
		parent.transform.localPosition = Vector3.zero;
		Destroy(grandp.gameObject);
	}

	public void addGrandParent(){
		if (depth < 2) return; // has no grand parent
		// revive grand parent
		FractalBox grandp = (FractalBox)GameObject.Instantiate(gm.templateFractal);
		grandp.addWalls ();
		grandp.initChilds ();
		grandp.depth = parent.depth - 1;
		// set the grand parent as the first child of the GameManager object
		grandp.transform.parent = gm.transform;
		// revive uncles
		for (int i=0; i<Fractal.blockPos.Length; i++){
			if (childIndex == i){ // make grand parent adopt parent
				grandp.transform.localScale = new Vector3(Fractal.maxSize, Fractal.maxSize, Fractal.maxSize)/Fractal.relaSize/Fractal.relaSize;
				parent.transform.parent = grandp.transform;
				parent.transform.localPosition = new Vector3(Fractal.blockPos[i].x, 0, Fractal.blockPos[i].y);
				grandp.setChild(i,parent);
			}else{
				grandp.addChild(i, parent.depth, 1);
			}
		}
		parent.parent = grandp;
		gm.baseFrac = grandp;
	}


	// construct walls 
	public void addWalls(){
		walls = new Wall[5];
		setWalls ();
		setCenterWall ();
	}
	private void setWalls(){ // place corner walls
		for (int i=0; i<4; i++){
			Wall w = (Wall) GameObject.Instantiate(gm.templateWall);
			w.transform.parent = transform; 
			w.transform.localPosition = new Vector3(Fractal.wallPos[i].x, 0, Fractal.wallPos[i].y);
			w.transform.localEulerAngles = new Vector3(0,Fractal.wallRot[i], 0);
			w.transform.localScale = new Vector3(Fractal.relaSize, Fractal.relaSize, Fractal.relaSize);
			walls[i] = w;
		}
	}
	private void setCenterWall(){ // place center walls
		Wall w = (Wall) GameObject.Instantiate(gm.templateCenter);
		w.transform.parent = transform; 
		w.transform.localPosition = new Vector3(0.5f, 0, 0.5f);
		w.transform.localScale = new Vector3(Fractal.relaSize, Fractal.relaSize, Fractal.relaSize);
		walls [4] = w;
	}

	private void removeWalls(){
		for (int i=0; i<walls.Length;i++){
			Destroy(walls[i].gameObject);
		}
	}

	/*
	 * For texture
	 */

	public void addTexture(){
		tile = (GameObject) GameObject.Instantiate(gm.templateTile);
		tile.transform.parent = transform;
		tile.transform.localPosition = new Vector3(0.5f, 0, 0.5f);
		tile.transform.localScale=new Vector3(1,1,1);
	}
		
	public void removeTile(){
		Destroy (tile.gameObject);
	}
}