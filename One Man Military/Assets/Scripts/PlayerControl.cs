using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
	//走路速度
	public float runSpeed = 12.0f;
	//跳躍高度
	public float jumpHeight = 12.0f;
	//墜落死亡
	public float fall = -12;
	//音效.跳躍聲音
	public AudioClip jumpSound;

	public GameObject hand;
	public GameObject hand2;

	public SpriteRenderer srPlayer;

	public GameObject bullet;

	public WeaponData Wdata;

	public GameManger gm;
	//撞
	private RaycastHit hit;
	//跳躍計時
	private float jumpCounter = 0.0f;
	//角色控制
	private CharacterController controller;
	//方向速度
	private Vector3 vel;
	//判斷正負用
	private float lookX;
	//是否可以控制
	private bool canControl = true;
	//是否可以撞到天花板
	private bool canCeiling = true;

	private FixedJoystick joystick;  //腳色移動控制

	private FixedJoystick shotstick; //射擊控制

	public Transform shotpos;

	private SpriteRenderer srHand;
	private SpriteRenderer srHand2;

	private Vector2 inipos;		//初始位置

	private float shotcd;
	void Start () 
    {
		controller = GetComponent<CharacterController>();
		joystick = GameObject.Find("虛擬搖桿(移動)").GetComponent<FixedJoystick>();
		shotstick = GameObject.Find("虛擬搖桿(射擊)").GetComponent<FixedJoystick>();
		srHand = hand.GetComponent<SpriteRenderer>();
		srHand2 = hand2.GetComponent<SpriteRenderer>();
		gm = FindObjectOfType<GameManger>();
		lookX = transform.localScale.x;
		inipos = transform.position;
	}
	
	void Update () 
    {
		MoveControl();
		shotcontrol();

	}

	private void MoveControl()
	{
		//重力
		if (!controller.isGrounded)
		{
			jumpCounter += Time.deltaTime;
			vel.y -= Time.deltaTime * 40;
		}
		else
		{
			jumpCounter = 0.0f;
			vel.y = -1;
		}

		//翻轉人物
		if (controller.velocity.x > 0)
		{
			// transform.localScale = new Vector3(lookX,transform.localScale.y,transform.localScale.z);
			srPlayer.flipX = false;
			srHand2.flipX = false;
		}
		if (controller.velocity.x < 0)
		{
			// transform.localScale = new Vector3(-lookX,transform.localScale.y,transform.localScale.z);
			srPlayer.flipX = true;
			srHand2.flipX = true;
		}

		//設定移動按鍵
		//鍵盤上下左右作為移動按鍵
		if (canControl)
		{
			/*if(Input.GetKey("left") || Input.GetKey("right"))
            {
				//如果按下左鍵往左走
				if(Input.GetKey(KeyCode.LeftArrow))
                {
					
					vel.x = -runSpeed;
				}
				//如果按下右鍵往右走
				if(Input.GetKey(KeyCode.RightArrow))
                {
					vel.x = runSpeed;
				}
			}
            //如果沒有按的話就停止不動
            else
            {
				vel.x = 0;
			}
			
			//跳躍的按鈕是上鍵
			//如果人物離地<0.1的話可以跳躍
			if(Input.GetKey(KeyCode.UpArrow))
            {
				if(jumpCounter < 0.1f)
                {
					vel.y = jumpHeight;
					jumpCounter = 0.1f;
					GetComponent<AudioSource>().PlayOneShot(jumpSound);
				}
			}
		}*/

			if (joystick.Vertical > 0.5f)
			{
				if (jumpCounter < 0.1f)
				{
					vel.y = jumpHeight;
					jumpCounter = 0.1f;
					GetComponent<AudioSource>().PlayOneShot(jumpSound);
				}
			}
			//如果跳躍撞到東西時會停止跳躍的加速度並且重製碰撞的處理
			if ((controller.collisionFlags & CollisionFlags.Above) != 0 && canCeiling)
			{
				canCeiling = false;
				vel.y = 0;
				StartCoroutine(resetCeiling());
			}
			vel.x = joystick.Horizontal * runSpeed;
		}

		//應用動作向量加速度到玩家
		controller.Move(vel * Time.deltaTime);

		//如果玩家落到限制高度以下將恢復原本位置
		if (transform.position.y < fall)
		{
			transform.position = inipos;
		}
	}

	private void shotcontrol()
	{
		float velx = shotstick.Horizontal;
		float velz = shotstick.Vertical;
		Quaternion ro = Quaternion.Euler(hand.transform.position.x, hand.transform.position.y, velz*90);
		hand.transform.rotation = ro;
		print(Mathf.Sqrt(Mathf.Pow(velx, 2) + Mathf.Pow(velz, 2)));
		if (Mathf.Sqrt(Mathf.Pow(velx, 2) + Mathf.Pow(velz, 2)) >= 0.5) 
		{
			if (shotcd < Wdata.cdtime && shotcd != 0)
			{
				shotcd += Time.deltaTime;
			}
			else
			{
				shotcd = 0;
				StartCoroutine(CreateBullet());
				shotcd += Time.deltaTime;
			}
		}

	}
	
	public IEnumerator resetCeiling () 
    {
		yield return new WaitForSeconds (0.25f);
		canCeiling = true;
	}

	private IEnumerator CreateBullet()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject temp;
		temp = Instantiate(bullet, shotpos.position,shotpos.rotation);
		temp.GetComponent<Rigidbody>().AddForce(temp.transform.right * Wdata.shootspeed);
		//temp.AddComponent<Weapon>();
		Destroy(temp, 3);
	}
	private void died () 
    {
		canControl = false;
		vel.x = 0;
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		print(hit.gameObject.name);
		if (hit.collider.name == "通關")
		{
			gm.pass();
		}

	}
}
