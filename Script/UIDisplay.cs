using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDisplay : MonoBehaviour
{
    public static UIDisplay instance;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float playTime = 100f;
    [SerializeField] BoxCollider2D boxCollider2DWindow;
    [SerializeField] GameObject playAgainButton;
    private int score;
    private int reloadStep;
    public bool isGameOver;
    [SerializeField] private GunController gunController;
    public UI_Reload[] uI_Reload;
    
    void Awake() 
    {
        // int num = FindObjectsOfType<UIDisplay>().Length;
        // if(num > 1)
        // {
        //     Destroy(gameObject);
        // }
        // else
        // {
            instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
    }
    void Start() {
        
        timerText.text = ((int)playTime).ToString();
        uI_Reload = GetComponentsInChildren<UI_Reload>(true);
    }
    void Update()
    {
        UpdateTime();
        if(Input.GetKeyDown(KeyCode.Mouse1) && !isGameOver)
        {
            OnReloadButtons();
        }
    }
    void OnReloadButtons()
    {
       // gunController.SetStateReloadBullet(true);
       gunController.reloadingBullet = true;
        foreach(UI_Reload uI in uI_Reload)
        {
            uI.gameObject.SetActive(true);

            float randomX = Random.Range(boxCollider2DWindow.bounds.min.x, boxCollider2DWindow.bounds.max.x);
            float randomY = Random.Range(boxCollider2DWindow.bounds.min.y, boxCollider2DWindow.bounds.max.y);
            uI.transform.position = new Vector3(randomX, randomY);
        }
        reloadStep = uI_Reload.Length;
        Time.timeScale = 0.5f;
    }
    public void ReloadStepControl()
    {
        reloadStep--;
        if(reloadStep <= 0)
        {
            gunController.ReloadBullets();
           // gunController.SetStateReloadBullet(false);
           gunController.reloadingBullet = false;

        }
    }
    public int GetScore()
    {
        return score;
    }
    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString("#,#");
    }
    public void UpdateAmmo(int currentBullets, int maxBullets)
    {
        ammoText.text = currentBullets + "/" + maxBullets;
    }
    void UpdateTime()
    {
        if(playTime <= 0)
        {
            GameOverUI();
        }
        playTime -= Time.deltaTime;
        timerText.text = ((int)playTime).ToString("#,#");
    }
    void GameOverUI()
    {
        Time.timeScale = 0;
        playAgainButton.SetActive(true);
        isGameOver = true;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        isGameOver = false;
    }
}
