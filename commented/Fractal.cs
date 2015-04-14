using UnityEngine;
using System.Collections;

/**
 * Constants for the fractal
 */
public class Fractal {

	public static int maxIter = 1; // the number of iterations
	public static int maxSize = 50; // the size of the entire fractal
	public static float relaSize = 0.2f; // the relative size of the child

	// parameter values to pass to the l-system's function

	// the children's position
	public static Vector2[] blockPos = new Vector2[]{
		new Vector2 (0.4f, 0.4f),
		new Vector2 (relaSize, 0),
		new Vector2 (relaSize * 3, 0),
		new Vector2 (0, relaSize),
		new Vector2 (relaSize * 4, relaSize),
		new Vector2 (0, relaSize * 3),
		new Vector2 (relaSize * 4, relaSize * 3),
		new Vector2 (relaSize, relaSize * 4),
		new Vector2 (relaSize * 3, relaSize * 4)
	};

	// the wall positions
	public static Vector2[] wallPos = new Vector2[]{
		new Vector2 (0.1f, 0.1f),
		new Vector2 (0.9f, 0.1f),
		new Vector2 (0.1f, 0.9f),
		new Vector2 (0.9f, 0.9f)}; // center wall position

	// the corner wall rotation
	public static int[] wallRot = new int[]{
		0, -90, 90, 180}; 
}
