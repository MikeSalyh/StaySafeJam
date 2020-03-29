using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
  public Sprite off, on;
  private Image img;

  private void Start()
  {
    img = GetComponent<Image>();
    TurnOn();
  }

  public void TurnOn()
  {
    img.sprite = on;
  }

  public void TurnOff()
  {
    img.sprite = off;
  }

  public bool Status
  {
    get { return img.sprite == on; }
  }
}
