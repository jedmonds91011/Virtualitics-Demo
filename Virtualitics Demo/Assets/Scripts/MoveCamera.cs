using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera: MonoBehaviour {
	[SerializeField] public float speed; 
	private bool rotating = false;
	public int angle ;
	public IEnumerator rotateCoroutineVariable;

	public float startTime;
	public Vector2 startPosition;
	public bool potentialSwipe;
	private float swipeCap;
	private float minSwipeDistance;
	public float maxSwipeTime;
	public float sensitivity; 
	public float perspectiveZoomSpeed = 0.15f;  
	private Vector3 forward;

	// Use this for initialization
	void Start () 
	{
		forward = Camera.main.transform.TransformVector(Vector3.right);
		minSwipeDistance = Screen.width / 10;
		maxSwipeTime = 0.20f;
		rotating = false;
		sensitivity = 1;
		angle = -90;
		rotateCameraAssist ();

	}
	
	/* 
	 * Function called every frame. Listens for input from the keyboard to manipulate the camera
	 * W Key - Used to move Forward relative to the camera focus point
	 * S Key - Used to move Backward relative to the camera focus point
	 * A Key - Used to move Left relative to the camera focus point
	 * S Key - Used to move Right relative to the camera focus point
	 * R Key - Used to move Up relative to the camera focus point
	 * F Key - Used to move Down relative to the camera focus point
	 * Q Key - Used to rotate the camera 90 degrees left
	 * E Key - Used to rotate the camera 90 degress right
	*/
	void Update () 
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
		} 
		else if (Input.GetKey (KeyCode.S)) 
		{
			transform.Translate (Vector3.back * speed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.A)) 
		{
			transform.Translate (Vector3.left * speed * Time.deltaTime);
		} 
		else if (Input.GetKey (KeyCode.D)) 
		{
			transform.Translate (Vector3.right * speed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.R)) 
		{
			transform.Translate (Vector3.up * speed * Time.deltaTime);
		} 
		else if (Input.GetKey (KeyCode.F)) 
		{
			transform.Translate (Vector3.down * speed * Time.deltaTime);
		}

		if (Input.GetKeyUp (KeyCode.Q)) 
		{
			angle += 90;
			rotateCameraAssist();
		} 
		if (Input.GetKeyUp (KeyCode.E)) 
		{
			angle -= 90;
			rotateCameraAssist();
		}

		if (Input.touchCount > 0) 
		{
			if (Input.touchCount == 2) {
				// Store both touches.
				Touch touchZero = Input.GetTouch (0);
				Touch touchOne = Input.GetTouch (1);

				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				Vector3 newPosition = Vector3.forward * -5 * deltaMagnitudeDiff * Time.deltaTime;
				transform.Translate (newPosition);
			} 
			else 
			{
				Touch touch = Input.GetTouch (0);

				switch (touch.phase) {
				case TouchPhase.Began:
					{
						potentialSwipe = true;
						startPosition = touch.position;
						startTime = Time.time;
						break;
					}

				case TouchPhase.Moved:
					{
						if (Time.time - startTime > maxSwipeTime) {
							potentialSwipe = false;
							Vector3 newPosition = new Vector3 (touch.deltaPosition.x, touch.deltaPosition.y, 0) * Time.deltaTime * -1.5f * sensitivity;
							transform.Translate (newPosition, Space.Self);
						}
						break;
					}

				case TouchPhase.Ended:
					{
						float currentSwipeTime = Time.time - startTime;
						float horizontalSwipeDist = (touch.position.x - startPosition.x);

						if (potentialSwipe && (currentSwipeTime < maxSwipeTime) && ((horizontalSwipeDist > minSwipeDistance) || (-1 * horizontalSwipeDist > minSwipeDistance))) {
							angle += (int)(90 * Mathf.Sign (touch.position.x - startPosition.x));
							rotateCameraAssist ();
						}
						break;
					}
				}
			}
		}

		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, 0.0f, 125.0f), Mathf.Clamp (transform.position.y, 0.0f, 125.0f), Mathf.Clamp (transform.position.z, 0.0f, 125.0f)); 
	}

	public void rotateCameraAssist()
	{
		if (!rotating) 
		{
			rotateCoroutineVariable = rotateCamera();
			StartCoroutine(rotateCoroutineVariable);
		}
	}

	public IEnumerator rotateCamera()
	{
		rotating = true;

		Quaternion newRotation = Quaternion.AngleAxis (angle, Vector3.up);
		float t = 0.0f;

		while(t <= 1.0f)
		{
			t += Time.deltaTime/2f;

			if(
				Mathf.Abs(transform.rotation.eulerAngles.x - newRotation.eulerAngles.x) <= 5f &&
				Mathf.Abs(transform.rotation.eulerAngles.y - newRotation.eulerAngles.y) <= 5f && 
				Mathf.Abs(transform.rotation.eulerAngles.z - newRotation.eulerAngles.z) <= 5f
			)
			{
				break;
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Mathf.SmoothStep(0, 1, t));
			yield return 0;
		}
		rotating = false;
	}


}
