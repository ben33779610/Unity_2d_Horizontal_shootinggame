using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
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

}
