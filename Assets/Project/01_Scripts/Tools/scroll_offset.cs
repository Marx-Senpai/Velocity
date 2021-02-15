using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scroll_offset : MonoBehaviour {

	public float scrollSpeed = 0.5F;
	public bool isUI = false;

	public enum Axe
	{
		Horizontal,
		Vertical,
		Both
	}
	public Axe axe;
	private Vector2 sens;
	
    //public Renderer rend;
    private Material[] mats;
	private Material mat;

	// Use this for initialization
	void Start () {
		//rend = GetComponent<Renderer>();
		if (isUI)
		{
			mat = this.GetComponent<Image>().material;
		}
		else
		{
			mats = GetComponent<Renderer>().materials;
		}
	}
	// Update is called once per frame
	void Update () {
		if (axe == Axe.Horizontal)
		{
			sens = Vector2.right;
		}
		else if(axe == Axe.Vertical)
		{
			sens = Vector2.down;
		}
		else
		{
			sens = Vector2.one;
		}

		float offset = Time.time * scrollSpeed;

		if (isUI)
		{
			mat.SetTextureOffset("_MainTex", sens * offset);
		}
		else
		{
			for (int i = 0; i < mats.Length; i++)
			{
				mats[i].SetTextureOffset("_MainTex", sens * offset);
				mats[i].SetTextureOffset("_EmissionMap", sens * offset);
				//rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
			}
		}
	}
}
