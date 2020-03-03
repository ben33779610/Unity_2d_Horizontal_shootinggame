using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float min;
	public float max;
	public Transform player;

	private void LateUpdate()
	{
		Track();
		
	}
	private void Track()
	{
		Vector3 posP = player.position;
		Vector3 posC = transform.position;
		posP.y = 0;
		posP.z = -10;

		posC.x = Mathf.Clamp(posC.x, min, max);   //Mathf.Clamp(夾住的值,下限,上限)
		transform.position = Vector3.Lerp(posC, posP, 0.3f * Time.deltaTime *5);

	}
}
