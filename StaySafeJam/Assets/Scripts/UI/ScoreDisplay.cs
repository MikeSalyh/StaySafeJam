using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
  private GameManager gameManager;
  private TextMeshProUGUI text;

  // Start is called before the first frame update
  void Start()
  {
    gameManager = GameObject.FindObjectOfType<GameManager>();
    text = GetComponent<TextMeshProUGUI>();
  }

  // Update is called once per frame
  void Update()
  {
    text.text = "Score: " + gameManager.Score.ToString(); 
  }
}
