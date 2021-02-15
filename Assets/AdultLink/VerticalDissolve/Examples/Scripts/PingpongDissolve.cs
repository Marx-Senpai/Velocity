using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingpongDissolve : MonoBehaviour {

	// Use this for initialization
	public float minValue;
	public float maxValue;
	public float freq;
	//public Material[] mat;
	public GameObject obj;
	private float fill = 0f;
	private float range;
	public float phase = 0f;
	void Start () {
		//setValueMaterials();
		//setValueGameobject();
		range = maxValue - minValue;
	}
	
	// Update is called once per frame
	void Update () {
		fill = (range / 2f) * (Mathf.Sin(freq * Time.time + phase) + 1f ) + minValue;
		//setValueMaterials();
		setValueGameobject();
	}

	/*private void setValueMaterials() {
		for (int i = 0; i < mat.Length; i++) {
			mat[i].SetFloat("_Fill", fill);
		}
	}*/
	
	private void setValueGameobject() {
		obj.GetComponent<Renderer>().material.SetFloat("_Fill", fill);
	}
}
