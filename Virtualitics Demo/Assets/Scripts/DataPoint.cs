using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class DataPoint : MonoBehaviour, IPointerDownHandler {
	public float[] irisData;
	public int dataClass;

//	public int xPosIndex; 
//	public int yPosIndex;
//	public int zPosIndex;
//	public int fourthVariableIndex;

//	public int oldXPosIndex; 
//	public int oldYPosIndex;
//	public int oldZPosIndex;
//	public int oldFourthVariableIndex;

	private Vector3 startPosition;
	private Vector3 endPosition;
	private float startTime; 
	private float speed; 
	private float journeyLength;
	private bool dataInitialized = false;
	private Color baseColor;


	void Awake(){
		irisData = new float[4];
//		xPosIndex = 0;
//		yPosIndex = 1;
//		zPosIndex = 2;
//		fourthVariableIndex = 3;
	}

	// Update is called once per frame
	void Update () 
	{
		if (dataInitialized) 
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			if (fracJourney > 0) {
				transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
			}
		}

	}

	public void initDataPoint(string pointData)
	{
		string[] formattedData = pointData.Split (',');
		for (int i = 0; i < 4; i++) 
		{
			float.TryParse (formattedData [i], out irisData[i]);
		}
		int.TryParse (formattedData [4], out dataClass);
		transform.position = new Vector3 (irisData[0], irisData[1], irisData[2]);
		startPosition = transform.position;
		endPosition = transform.position;
		speed = 100.0f;
		dataInitialized = true;
	}

	public void permuteDataPoint( int xIndex, int yIndex, int zIndex, int fourthIndex)
	{
//		xPosIndex = DataManager.instance().getXPosIndex();
//		yPosIndex = DataManager.instance().getYPosIndex();
//		zPosIndex = DataManager.instance().getZPosIndex();
//		fourthVariableIndex = DataManager.instance().getFourthVariableIndex();
		//dataInitialized = false;
		startPosition = transform.position;
		endPosition = new Vector3 (irisData [xIndex], irisData [yIndex], irisData [zIndex]);

		startTime = Time.time;
		journeyLength = Vector3.Distance(startPosition, endPosition);



		//transform.position = new Vector3 (irisData[xIndex], irisData[yIndex], irisData[zIndex]);
		setDataPointSize(fourthIndex, DataManager.instance().getMaxDataValue(fourthIndex, dataClass));
	}

	public void setDataPointSize(int dataIndex, float maxDataValue)
	{
		float value = (irisData [dataIndex]/maxDataValue) * 2.0f;
		transform.localScale = new Vector3 (value, value, value);
	}

	public void setDataColor(int dataIndex, float maxDataValue)
	{
		float value = (irisData [dataIndex]/maxDataValue);

		switch (dataClass) {
		case 0:
			{
				
				//Color modifiedRed = Color.HSVToRGB(1.0f, value, 1.0f);
				gameObject.GetComponent<Renderer> ().material.color = Color.red;
				baseColor = Color.red;
				break;
			}
		case 1:
			{
				//Color modifiedBlue = Color.HSVToRGB(0.66f, value, 1.0f);
				gameObject.GetComponent<Renderer> ().material.color = Color.blue;
				baseColor = Color.blue;
				break;
			}
		default:
			{
				//Color modifiedGreen = Color.HSVToRGB(0.33f, value, 1.0f);
				gameObject.GetComponent<Renderer> ().material.color = Color.green;
				baseColor = Color.green;
				break;
			}
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		DataManager.instance().setUpDataPanel (this);

	}

	public void setSelectedColor(float emissionAmount){
		Renderer renderer = GetComponent<Renderer> ();
		Material mat = renderer.material;
		Color finalColor = Color.white * Mathf.LinearToGammaSpace (emissionAmount);
		mat.SetColor ("_EmissionColor", finalColor);
	}

}
