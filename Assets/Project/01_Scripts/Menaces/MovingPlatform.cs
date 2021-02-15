using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] private AnimationCurve movement;
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float distance = 3f;

	private Rigidbody rigid;
	private float movingValue, curveTime;

	private enum AxisDirection {X, Y, Z};
	[SerializeField] private AxisDirection axis = AxisDirection.X;
	
	private bool direction;
	private float startPosition;
	public int pv;

	void Awake ()
	{
		this.rigid = this.GetComponent<Rigidbody>();
		
		switch(this.axis) {
			case AxisDirection.X:
				this.startPosition = this.transform.position.x;
				break;
			case AxisDirection.Y:
				this.startPosition = this.transform.position.y;
				break;
			case AxisDirection.Z:
				this.startPosition = this.transform.position.z;
				break;
		}

		this.curveTime = 0f;
		//this.movement.postWrapMode = WrapMode.PingPong;
	}

	public void CustomFixedUpdate () {
		this.movingValue = this.movement.Evaluate(this.curveTime);
		MoveTransform(this.distance);
		this.curveTime += Time.fixedDeltaTime * this.moveSpeed;
	
	}

	private void MoveTransform(float addPos) {
		switch(this.axis) {
			case AxisDirection.X:
				this.rigid.position = new Vector3(this.startPosition + addPos * movingValue, this.rigid.position.y, this.rigid.position.z);
				break;
			case AxisDirection.Y:
				this.rigid.position = new Vector3(this.rigid.position.x, this.startPosition + addPos * movingValue, this.rigid.position.z);
				break;
			case AxisDirection.Z:
				this.rigid.position = new Vector3(this.rigid.position.x, this.rigid.position.y, this.startPosition + addPos * movingValue);
				break;
		}
	}

	public void checkLife()
	{
		if (pv <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
