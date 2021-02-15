using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters : MonoBehaviour {

	public float volume_SFX;
	public float volume_Music;
	public float brightness;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
}
