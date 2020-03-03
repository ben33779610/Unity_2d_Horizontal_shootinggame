using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
	public Image missioncom;

	/// <summary>
	/// 開始遊戲，選擇關卡
	/// </summary>
	public void GameStart()
	{
		SceneManager.LoadScene("選擇關卡");
	}

	public void Exit()
	{
		Application.Quit();
	}

	public void pass()
	{
		missioncom.gameObject.SetActive(true);
		SceneManager.LoadScene("開始");
	}

}
