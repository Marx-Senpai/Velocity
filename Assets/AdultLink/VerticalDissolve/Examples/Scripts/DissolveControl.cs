using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveControl : MonoBehaviour
{

	public float decr = 0.1f;
	public float minValue;
	public float maxValue;
	private float valueRange;
	public bool autoUpdate = false;
	//public Material mat;
	public GameObject[] obj;
	[Range(-2f, 2f)]
	public float fill = 1;
	// Use this for initialization
	void Start () {
		valueRange = maxValue - minValue;
		//fill = obj.GetComponent<Renderer>().material.GetFloat("_Fill");
	}
	
	// Update is called once per frame
	void Update () {
		if (autoUpdate) {
			setFill(fill);
		}
	}

	public void setFill(float _fillPercentage) {
		fill = Mathf.Clamp(fill - Time.deltaTime * decr, 0f, 1f);
		//mat.SetFloat("_Fill", minValue + valueRange * _fillPercentage);
		for (int i = 0; i < obj.Length; i++) {
			obj[i].GetComponent<Renderer>().material.SetFloat("_Fill", minValue + valueRange * _fillPercentage);
		}
	}
}
