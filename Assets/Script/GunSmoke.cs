using UnityEngine;
using System.Collections;
[RequireComponent (typeof (LineRenderer))]

public class GunSmoke : MonoBehaviour {
	public int numberOfPoints = 15;
	public double updateSpeed = 0.25;
	public float riseSpeed = 0.5f;
	public float spread  = 0.25f;

	bool drawState = false;
	bool prevDrawState = false;

	LineRenderer line;
	Transform tr;
	Vector3[] positions;
	Vector3[] directions;
	int i;
	double timeSinceUpdate = 0.0;
	Material lineMaterial;
	double lineSegment = 0.0;
	int currentNumberOfPoints = 2;
	bool allPointsAdded = false;
	Vector3 tempVec;
	float transparency = 1f;


	void Start () {
		tr = this.transform;
		line = this.GetComponent<LineRenderer>();
		lineMaterial = line.material;
		line.useWorldSpace = true;
		lineSegment = 1.0 / numberOfPoints;
		positions = new Vector3[numberOfPoints];
		directions = new Vector3[numberOfPoints];
		line.SetVertexCount ( currentNumberOfPoints );

		for ( i = 0; i < currentNumberOfPoints; i++ ) {
			tempVec = getSmokeVec ();
			directions[i] = tempVec;
			positions[i] = tr.position;
			line.SetPosition ( i, positions[i] );
		}
	}

	void Update () {
		if (drawState != prevDrawState) {
			prevDrawState = drawState;
		}
		
		if (drawState) {
			timeSinceUpdate += Time.deltaTime;

			// if ( drawState && prevDrawState) {

			// }
			
			if ( timeSinceUpdate > updateSpeed ) {
				timeSinceUpdate -= updateSpeed;
				if (transparency >= 0) {
					transparency -= 0.05f;
				} else {
					transparency = 0;
				}
				
				if ( !allPointsAdded ) {
					currentNumberOfPoints++;
					line.SetVertexCount ( currentNumberOfPoints );
					tempVec = getSmokeVec ();
					directions[0] = tempVec;
					positions[0] = tr.position;
					line.SetPosition ( 0, positions[0] );
				}

				if ( !allPointsAdded && ( currentNumberOfPoints == numberOfPoints ) ) {
					allPointsAdded = true;
				}

				for ( i = currentNumberOfPoints - 1; i > 0; i-- ) {
					tempVec = positions[i-1];
					positions[i] = tempVec;
					tempVec = directions[i-1];
					directions[i] = tempVec;
				}
				tempVec = getSmokeVec ();
				directions[0] = tempVec;
			}

			for ( i = 1; i < currentNumberOfPoints; i++ ) {
				tempVec = positions[i];
				tempVec += directions[i] * Time.deltaTime;
				positions[i] = tempVec;
				line.SetPosition ( i, positions[i] );
			}
			positions[0] = tr.position;
			line.SetPosition ( 0, tr.position );

			if ( allPointsAdded ) {
				Vector2 vec2 = lineMaterial.mainTextureOffset;
				vec2.x = (float)(lineSegment * ( timeSinceUpdate / updateSpeed ));
				lineMaterial.mainTextureOffset = vec2;
				//lineMaterial.mainTextureOffset.x = lineSegment * ( timeSinceUpdate / updateSpeed );
				// transparency -= 0.1f;
				// Debug.Log(transparency);
				// lineMaterial.SetColor("_TintColor", new Color(1, 1, 1, transparency));
			}
			
			Debug.Log(transparency);
			lineMaterial.SetColor("_TintColor", new Color(1, 1, 1, transparency));
		}
	}
	
	public void DrawSmoke() {
		drawState = true;
		transparency = 1f;
	}

	public void StopSmoke() {
		transparency = 1f;
		drawState = false;
		currentNumberOfPoints = 2;
		allPointsAdded = false;
		if (line != null) {
			line.SetVertexCount(currentNumberOfPoints);	
		}
		
	}

	Vector3 getSmokeVec() {
		Vector3 smokeVec;
		smokeVec.x = Random.Range ( -1.0f, 1.0f );
		smokeVec.y = Random.Range ( 0.0f, 1.0f );
		smokeVec.z = Random.Range ( -1.0f, 1.0f );
		smokeVec.Normalize ();
		smokeVec *= spread;
		smokeVec.y += riseSpeed;
		return smokeVec;
	}
}
