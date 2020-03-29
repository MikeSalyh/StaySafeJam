using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class N95Mask : MonoBehaviour
{
  private GameManager gameManager;
  private bool isDragging;
  private Vector3 mouseOffset;
  private Vector3 startingPosition;

  // Start is called before the first frame update
  void Start()
  {
    gameManager = GameObject.FindObjectOfType<GameManager>();
    gameManager.OnStartDrag += StartDrag;
    gameManager.OnStopDrag += Release;
    startingPosition = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    if (isDragging)
    {
      transform.position = mouseOffset + Input.mousePosition;
    }
  }

  void StartDrag()
  {
    mouseOffset = transform.position - Input.mousePosition;
    isDragging = true;
    transform.localScale = Vector2.one * 1.25f;
    transform.DOScale(0.5f, 0.25f).SetEase(Ease.OutBack);
  }

  void Release()
  {
    isDragging = false;
    if (IsOverDoctor)
    {
      StartCoroutine(SuccessCoroutine());
    }
    else
    {
      //Return the mask to the starting place
      transform.DOMove(startingPosition, 0.4f);
      transform.DOScale(1f, 0.4f);
    }
  }

  private bool IsOverDoctor
  {
    get{ return true; }
  }

  private IEnumerator SuccessCoroutine()
  {
    GetComponent<Image>().enabled = false;
    yield return new WaitForSeconds(0.1f);
    GetComponent<Image>().enabled = true;
    transform.position = startingPosition + (Vector3.down * 100);
    transform.DOMove(startingPosition, 0.2f);
    transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
  }
}
