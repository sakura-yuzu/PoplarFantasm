using UnityEngine;

class CameraController : MonoBehaviour
{
	public GameObject player;
	private Vector3 playerPosition;
	private Vector3 cameraPosition;
	private Vector3 diff;

	private float pre_angle = 0.0f;
	//回転させるスピード
	public float rotateSpeed = 3.0f;
	Transform transform;

	void Start()
	{
		playerPosition = player.GetComponent<Transform>().position;
		transform = GetComponent<Transform>();
		cameraPosition = transform.position;
		diff = playerPosition - cameraPosition;
	}

	void Update()
	{
		playerPosition = player.GetComponent<Transform>().position;
		cameraPosition = playerPosition - diff;
		transform.position = cameraPosition;

		// 右スティック入力
		float vert_r = Input.GetAxis("RightStick_H");
		if ((vert_r != 0))
		{
			rotate(vert_r);
		}
	}

	private void rotate(float angle)
	{
		transform.RotateAround(playerPosition, Vector3.up, angle * rotateSpeed);
		pre_angle = angle;
	}
}