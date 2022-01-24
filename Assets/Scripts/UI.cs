using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Button restartBtn;
    public Button one;
    public Button two;
    public TextMeshProUGUI speedTxt;
    public Rigidbody ball;

    // Start is called before the first frame update
    void Start()
    {
        restartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        one.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("2");
        });
        two.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("3");
        });
    }

    private void Update()
    {
        speedTxt.text = Mathf.RoundToInt(ball.velocity.magnitude).ToString();
    }
}
