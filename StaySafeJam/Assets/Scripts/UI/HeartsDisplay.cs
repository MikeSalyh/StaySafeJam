using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsDisplay : MonoBehaviour
{
  private GameManager gameManager;
  public GameObject heartfab;
  private int internalHPMemory;
  public Heart[] hearts;

  // Start is called before the first frame update
  void Start()
  {
    gameManager = GameObject.FindObjectOfType<GameManager>();
    hearts = new Heart[gameManager.startingHP];
    for (int i = 0; i < gameManager.startingHP; i++)
    {
      hearts[i] = Instantiate(heartfab, this.transform).GetComponent<Heart>();
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (internalHPMemory != gameManager.HP)
    {
      for (int i = 0; i < gameManager.startingHP; i++)
      {
        if (gameManager.HP > i)
          hearts[i].TurnOn();
        else
          hearts[i].TurnOff();
      }
    }
  }
}
