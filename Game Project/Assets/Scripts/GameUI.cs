using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public Image healthSprite;
    public Image fadePlane;
    public GameObject gameOverUI;

    public Text HealthUI;

    Player player;

	void Start ()
    {
        player = FindObjectOfType<Player>(); 
        player.OnDeath += OnGameOver;
    }

    void Update()
    {
        float healthPercent = player.health / 5;
        healthSprite.fillAmount = healthPercent;
        // HealthUI.text = "Health: " + Hud.health.ToString();
    }

    void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        HealthUI.text = "";
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    //UI Input
    public void StartNewGame()
    {
        SceneManager.LoadScene("BossTroll");
    }

}
