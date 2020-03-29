using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class N95Mask : MonoBehaviour
{
  private Vector3 mouseOffset;
  private Vector3 startingPosition;

  // Start is called before the first frame update
  void Start()
  {
    startingPosition = transform.position;
  }

  private bool _isDragging = false;

  // Update is called once per frame
  void Update()
  {

    if (_isDragging)
    {
      transform.position = mouseOffset + Input.mousePosition;
    }

    if (!Input.GetMouseButton(0) && _isDragging)
    {
      //Throw the mask
      Release();
      _isDragging = false;
    }
    else if (Input.GetMouseButtonDown(0))
    {
      //Check if it's over the mask starting area
      PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
      pointerEventData.position = Input.mousePosition;

      List<RaycastResult> raycastResultList = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
      for (int i = 0; i < raycastResultList.Count; i++)
      {
        if (raycastResultList[i].gameObject.GetComponent<N95Mask>() != null)
        {
          Debug.Log("Got a mask!");
          StartDrag();
          _isDragging = true;
          break;
        }
      }
    }
  }

  void StartDrag()
  {
    mouseOffset = transform.position - Input.mousePosition;
    transform.localScale = Vector2.one * 1.25f;
    transform.DOScale(0.5f, 0.25f).SetEase(Ease.OutBack);
  }

  void Release()
  {
    Doctor md = GetDoctor();
    if (md != null)
    {
      StartCoroutine(SuccessCoroutine());
      md.GiveMask();
    }
    else
    {
      //Return the mask to the starting place
      transform.DOMove(startingPosition, 0.4f);
      transform.DOScale(1f, 0.4f);
    }
  }

  private Doctor GetDoctor()
  {
    //Check if it's over the mask starting area
    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    pointerEventData.position = transform.position;

    List<RaycastResult> raycastResultList = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
    for (int i = 0; i < raycastResultList.Count; i++)
    {
      if (raycastResultList[i].gameObject.CompareTag("Drop"))
      {
        Doctor md = raycastResultList[i].gameObject.GetComponentInParent<Doctor>();
        if(md.state == Doctor.State.Ready)
          return md;
      }
    }

    return null;
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
