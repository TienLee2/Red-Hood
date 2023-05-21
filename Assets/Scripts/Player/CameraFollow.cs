using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	//tốc độ đuổi theo nhân vật của camera
	public float FollowSpeed = 2f;
	//vị trí nhân vật
	public Transform Target;

	//vị trí của camera để lắc mỗi khi đánh
	private Transform camTransform;

	//thời gian shake camera
	public float shakeDuration = 0f;

	// Sức mạnh khi lắc camera, giá trị càng lớn lắc càng mạnh
	public float shakeAmount = 0.1f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		//set con trỏ chuột biến mất
		Cursor.visible = false;
		//Nếu vị trí của cam ko có
		if (camTransform == null)
		{
			//lấy vị trí của vật đang giữ script gán vào vị trí lắc
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		//khi bật script, gán vị trí local của cam vào một cái vector3
		originalPos = camTransform.localPosition;
	}

	private void Update()
	{
		//tạo một cái vector3 có giá trị là vị trí hiện tại của nhân vật
		Vector3 newPosition = Target.position;
		// trục z của vector3 giảm đi 10
		newPosition.z = -10;
		// Slerp(a,b,t) di chuyển từ a đến b với thời gian t
		transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);

		//Nếu có thời gian shake thì lắc camera
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
	}

	public void ShakeCamera()
	{
		originalPos = camTransform.localPosition;
		shakeDuration = 0.2f;
	}
}
