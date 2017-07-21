using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour {
	public DataPoint dataPrefab;
	public List<DataPoint> dataPoints; 
	private float[,] maxIrisData;
	public int[] dataClassCount;

	private float maxValue;
	private float minValue;

	private int xPosIndex; 
	private int yPosIndex;
	private int zPosIndex;
	private int fourthVariableIndex;

	public Text sepalLengthValue;
	public Text sepalWidthValue;
	public Text petalLengthValue;
	public Text petalWidthValue;
	public Text irisClassValue; 

	public Text[] dataTextComponents;
	public Text[] classTextComponents;

	private DataPoint selectedDataPoint;
	public GameObject dataPanel;

	private static DataManager dataManager;

	public static DataManager instance () {
		if (!dataManager) {
			dataManager = FindObjectOfType(typeof (DataManager)) as DataManager;
			if (!dataManager)
				Debug.LogError ("There needs to be one active DataManager script on a GameObject in your scene.");
		}

		return dataManager;
	}

	void Awake()
	{
		dataPoints = new List<DataPoint> ();
		maxIrisData = new float[4,3] { {0,0,0}, {0,0,0}, {0,0,0}, {0,0,0} };
		dataClassCount = new int[3] { 0, 0, 0 };
		maxValue = 0;
		minValue = int.MaxValue;
		xPosIndex = 0;
		yPosIndex = 1;
		zPosIndex = 2;
		fourthVariableIndex = 3;
		toggleDataPanel (false);

	}
	// Use this for initialization
	void Start () 
	{
		initDataSets ();
		normalizeDataValues (3);
		generateGrid ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void initDataSets()
	{
		string initialText;
		TextAsset tAsset = (TextAsset)Resources.Load("iris-test",typeof(TextAsset));
		initialText = tAsset.text;
		string normalizedText = initialText.Substring (34);

		string[] inputData = normalizedText.Split ('\r');


		for (int i = 0; i < inputData.Length; i++) 
		{
			DataPoint dataPoint = Instantiate(dataPrefab, Vector3.zero, Quaternion.identity);

			string[] dataPointValues = inputData [i].Split (',');
			int irisClass = 0;

			float[] dataValues = new float[4];
			int.TryParse (dataPointValues [4], out irisClass);
			dataClassCount [irisClass] += 1;

			for(int j = 0; j < dataPointValues.Length - 1; j++)
			{
				float.TryParse(dataPointValues[j], out dataValues[j]);

				if (dataValues [j] > maxIrisData [j, irisClass])
				{
					maxIrisData [j, irisClass] = dataValues [j];
				}

				if (dataValues [j] > maxValue) 
				{
					maxValue = dataValues [j];
				}

				if (dataValues [j] < minValue) 
				{
					minValue = dataValues [j];
				}
			}

			dataPoint.initDataPoint (inputData [i]);
			dataPoints.Add (dataPoint);
		}

		Debug.Log ("Max Value is: " + maxValue);

	}

	public void normalizeDataValues(int indexToNormalize)
	{
		foreach (DataPoint d in dataPoints) 
		{
			d.setDataPointSize (indexToNormalize, maxIrisData [indexToNormalize, d.dataClass]);
			d.setDataColor (indexToNormalize, maxIrisData [indexToNormalize, d.dataClass]);
		}
		setUpDataSetPanel ();
	}

	public float getMaxDataValue (int forIndex, int dataClass)
	{
		return maxIrisData [forIndex, dataClass];
	}

	public void setUpDataPanel(DataPoint selectedPoint)
	{
		if (this.selectedDataPoint != null) {
			selectedDataPoint.setSelectedColor (0.0f);
		}
		sepalLengthValue.text = ""+selectedPoint.irisData[0];
		sepalWidthValue.text = ""+selectedPoint.irisData[1];
		petalLengthValue.text = ""+selectedPoint.irisData[2];
		petalWidthValue.text = ""+selectedPoint.irisData[3];
		irisClassValue.text = ""+selectedPoint.dataClass;
		selectedDataPoint = selectedPoint;
		selectedDataPoint.setSelectedColor(0.2f);
		toggleDataPanel (true);
	}


	public void generateGridLines(int minimum)
	{
		float width = 0.5f;
		for (int i = minimum; i <= maxValue + 10; i += 10) 
		{
			GameObject xyHorizontalGridLine = new GameObject ("xyHorizontalLine");
			LineRenderer xyHorizontalLineRenderer = xyHorizontalGridLine.AddComponent<LineRenderer> ();
			xyHorizontalLineRenderer.material = new Material(Shader.Find("Mobile/VertexLit"));
			xyHorizontalLineRenderer.startColor = Color.black;
			xyHorizontalLineRenderer.endColor = Color.black;
			xyHorizontalLineRenderer.startWidth = width; 
			xyHorizontalLineRenderer.endWidth = width;
			xyHorizontalLineRenderer.SetPosition(0,new Vector3(minimum,i,minimum));
			xyHorizontalLineRenderer.SetPosition(1, new Vector3(maxValue+1,i,minimum));
			xyHorizontalLineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			GameObject xyVerticalGridLine = new GameObject ("xyVerticalLine");
			LineRenderer xyVerticalLineRenderer = xyVerticalGridLine.AddComponent<LineRenderer> ();
			xyVerticalLineRenderer.material = new Material(Shader.Find("Mobile/VertexLit"));
			xyVerticalLineRenderer.startColor = Color.black;
			xyVerticalLineRenderer.endColor = Color.black;
			xyVerticalLineRenderer.startWidth = width; 
			xyVerticalLineRenderer.endWidth = width;
			xyVerticalLineRenderer.SetPosition(0,new Vector3(i,minimum,minimum));
			xyVerticalLineRenderer.SetPosition(1, new Vector3(i,maxValue+1,minimum));
			xyVerticalLineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;


			GameObject xzHorizontalGridLine = new GameObject ("xzHorizontalLine");
			LineRenderer xzHorizLineRenderer = xzHorizontalGridLine.AddComponent<LineRenderer> ();
			xzHorizLineRenderer.material = new Material(Shader.Find("Mobile/VertexLit"));
			xzHorizLineRenderer.startColor = Color.black;
			xzHorizLineRenderer.endColor = Color.black;
			xzHorizLineRenderer.startWidth = width; 
			xzHorizLineRenderer.endWidth = width;
			xzHorizLineRenderer.SetPosition(0,new Vector3(i,minimum,minimum));
			xzHorizLineRenderer.SetPosition(1, new Vector3(i,minimum,maxValue + 1));
			xzHorizLineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			GameObject yzVerticalGridLine = new GameObject ("yzVerticalLine");
			LineRenderer yzVerticalLineRenderer = yzVerticalGridLine.AddComponent<LineRenderer> ();
			yzVerticalLineRenderer.material = new Material(Shader.Find("Mobile/VertexLit"));
			yzVerticalLineRenderer.startColor = Color.black;
			yzVerticalLineRenderer.endColor = Color.black;
			yzVerticalLineRenderer.startWidth = width; 
			yzVerticalLineRenderer.endWidth = width;
			yzVerticalLineRenderer.SetPosition(0,new Vector3(minimum,minimum,i));
			yzVerticalLineRenderer.SetPosition(1, new Vector3(minimum,maxValue+1,i));
			yzVerticalLineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			GameObject yzHorizontalGridLine = new GameObject ("yzHorizontalLine");
			LineRenderer yzHorizontalLineRenderer = yzHorizontalGridLine.AddComponent<LineRenderer> ();
			yzHorizontalLineRenderer.material = new Material(Shader.Find("Mobile/VertexLit"));
			yzHorizontalLineRenderer.startColor = Color.black;
			yzHorizontalLineRenderer.endColor = Color.black;
			yzHorizontalLineRenderer.startWidth = width; 
			yzHorizontalLineRenderer.endWidth = width;
			yzHorizontalLineRenderer.SetPosition(0,new Vector3(minimum,i,minimum));
			yzHorizontalLineRenderer.SetPosition(1, new Vector3(minimum,i,maxValue+1));
			yzHorizontalLineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			GameObject xzVerticalGridLine = new GameObject ("xzVerticalLine");
			LineRenderer xzVerticalLineRenderer = xzVerticalGridLine.AddComponent<LineRenderer> ();
			xzVerticalLineRenderer.material = new Material(Shader.Find("Mobile/VertexLit"));
			xzVerticalLineRenderer.startColor = Color.black;
			xzVerticalLineRenderer.endColor = Color.black;
			xzVerticalLineRenderer.endWidth = width;
			xzVerticalLineRenderer.SetPosition(0,new Vector3(minimum,minimum,i));
			xzVerticalLineRenderer.SetPosition(1, new Vector3(maxValue+1,minimum,i));
			xzVerticalLineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
	}

	public void generateGrid()
	{
		int minimum = (int)(minValue - (minValue % 10));
		generateGridLines (minimum);
	}

	public void permuteData()
	{
		List<int> dataIndexes = new List<int> ();
		for (int i = 0; i < 4; i++) {
			dataIndexes.Add (i);
		}
		xPosIndex = dataIndexes[Random.Range (0, dataIndexes.Count)];
		dataIndexes.Remove (xPosIndex);

		yPosIndex = dataIndexes[Random.Range (0, dataIndexes.Count)];
		dataIndexes.Remove (yPosIndex);

		zPosIndex = dataIndexes[Random.Range (0, dataIndexes.Count)];
		dataIndexes.Remove (zPosIndex);

		fourthVariableIndex = dataIndexes[Random.Range (0, dataIndexes.Count)];
		dataIndexes.Remove (fourthVariableIndex);

		foreach (DataPoint d in dataPoints) 
		{
			d.permuteDataPoint (xPosIndex, yPosIndex, zPosIndex, fourthVariableIndex);
		}

		setUpDataSetPanel ();

	}

	public void toggleDataPanel(bool active)
	{
		this.dataPanel.SetActive (active);
		if (!active && selectedDataPoint) {
			selectedDataPoint.setSelectedColor (0.0f);
			selectedDataPoint = null;
		}
	}

	public string getDataLabel(int index)
	{
		switch (index) {
		case 0:
			{
				return "Sepal Length";
			}
		case 1:
			{
				return "Sepal Width";
			}
		case 2:
			{
				return "Petal Length";
			}
		default:
			{
				return "Petal Width";
			}
		}
	}

	public void setUpDataSetPanel()
	{
		dataTextComponents [0].text = getDataLabel (xPosIndex);
		dataTextComponents [1].text = getDataLabel (yPosIndex);
		dataTextComponents [2].text = getDataLabel (zPosIndex);
		dataTextComponents [3].text = getDataLabel (fourthVariableIndex);
		classTextComponents [0].text = "" + dataClassCount [0];
		classTextComponents [1].text = "" + dataClassCount [1];
		classTextComponents [2].text = "" + dataClassCount [2];
	}
}
