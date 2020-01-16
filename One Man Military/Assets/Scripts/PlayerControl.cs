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

    public SpriteRenderer srPlayer;

	private FixedJoystick joystick;
	
	void Start () 
    {
		controller = GetComponent<CharacterController>();
		joystick = GameObject.Find("虛擬搖桿").GetComponent<FixedJoystick>();
		lookX = transform.localScale.x;
	}
	
	void Update () 
    {
		//重力
		if(!controller.isGrounded)
        {
			jumpCounter += Time.deltaTime;
			vel.y -= Time.deltaTime*40;
		}
        else
        {
			jumpCounter = 0.0f;
			vel.y = -1;
		}
		
		//翻轉人物
		if(controller.velocity.x > 0)
        {
            // transform.localScale = new Vector3(lookX,transform.localScale.y,transform.localScale.z);
            srPlayer.flipX = false;
		}
		if(controller.velocity.x < 0)
        {
            // transform.localScale = new Vector3(-lookX,transform.localScale.y,transform.localScale.z);
            srPlayer.flipX = true;
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
				SceneManager.LoadScene("選擇關卡");
			}
		
	}
	
	public IEnumerator resetCeiling () 
    {
		yield return new WaitForSeconds (0.25f);
		canCeiling = true;
	}
	
	void died () 
    {
		canControl = false;
		vel.x = 0;
	}
}
