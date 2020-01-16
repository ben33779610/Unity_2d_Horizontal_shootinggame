using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Playerhealth : MonoBehaviour
{
	//血量
	public int hearts = 10;

	private AudioSource auo;
	//受到攻擊的聲音
	public AudioClip hitsound;
	//死亡動畫
	public GameObject deathanim;

	//三種血量變化
	public RectTransform HealthBar, Hurt;
	public AudioClip heartsound;

	//是否死亡
	private bool dead;
	//是否可接受傷害
	private bool cangetHurt = true;

	//2d圖型絢染
	private SpriteRenderer rend;


	private int health;
	private int curhealth;
	void TakeDamage(int amount)
		{

		if (cangetHurt && !dead)
			{
			cangetHurt = false;
			auo.PlayOneShot(hitsound);
			curhealth -= amount;
			StartCoroutine(CheckHealth());
			StartCoroutine(ResetCanHurt());
			}
	}
	public IEnumerator ResetCanHurt()
	{
		rend.color = new Vector4(1.0f, 0.5f, 0.5f, 1.0f);
		yield return new WaitForSeconds(1f);
		rend.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
		cangetHurt = true;
	}

	public IEnumerator CheckHealth()
	{

		UpdateHearts();

		if (curhealth <= 0 && dead == false)
		{
			dead = true;
			Instantiate(deathanim, transform.position, Quaternion.Euler(0, 180, 0));
			BroadcastMessage("died", SendMessageOptions.DontRequireReceiver);

			// 將2D渲染設為關閉
			rend = GetComponent<SpriteRenderer>();
			rend.enabled = false;

			//重新復活時間為2秒
			yield return new WaitForSeconds(2);

			//重新讀取關卡
			string lvlName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(lvlName);
		}

	}


	void AddHealth()
	{
		GetComponent<AudioSource>().PlayOneShot(heartsound);
		//吃到愛心所增加的血量
		curhealth += 2;
		//如果吃的血量超過設定值就不要再增加了
		if (health > 10)
		{
			health = 10;

		}
		//更新血量
		UpdateHearts();
	}

	public void UpdateHearts()
	{
		HealthBar.sizeDelta = new Vector2(curhealth*10, HealthBar.sizeDelta.y);
		if (Hurt.sizeDelta.x >= HealthBar.sizeDelta.x)

		{

			//讓傷害量(紅色血條)逐漸追上當前血量

			Hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime*10 ;

		}


	}



	void Start()
    {
		health = hearts ;
		curhealth = health;
		auo = GetComponent<AudioSource>();

		


	}

    
    void Update()
    {
		
		if (Input.GetKeyDown(KeyCode.H))

		{

			//接受傷害

			curhealth = curhealth - 1;
			UpdateHearts();
		}
	}


	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		
	/*	if (hit.gameObject.tag == "enemy" || hit.gameObject.tag == "trap")
		{
			if (cangetHurt && !dead)
			{
				cangetHurt = false;
				GetComponent<AudioSource>().PlayOneShot(hitsound);
				health -= 1;
				StartCoroutine(CheckHealth());
				StartCoroutine(ResetCanHurt());
				                             }
			                    }
		                     //碰到補血道具要增加血量
	if (hit.gameObject.tag == "heart")
		{
		Destroy(hit.gameObject);
		AddHealth();
		}
		*/
	}



}
