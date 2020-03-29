using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
  [Header("Game Balancing Vars")]

  public int startingHP = 3;
  private int _remainingHP;
  public int HP
  {
    get { return _remainingHP; }
  }

  private int _score = 0;
  public int Score
  {
    get { return _score; }
  }

  //Delegates
  public delegate void StartDragDelegate();
  public StartDragDelegate OnStartDrag;
  public delegate void StopDragDelegate();
  public StopDragDelegate OnStopDrag;
  private bool _isDragging = false;

  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Gameplay);

    _remainingHP = startingHP;

    foreach (Doctor md in GameObject.FindObjectsOfType<Doctor>())
    {
      md.OnGiveMask += HandleGiveMask;
      md.OnFail += HandleFail;
    }
  }

  //// Update is called once per frame
  //void Update()
  //{
  //  if (!Input.GetMouseButton(0) && _isDragging)
  //  {
  //    //Throw the mask
  //    if (OnStopDrag != null)
  //      OnStopDrag();
  //    _isDragging = false;
  //  }
  //  else if (Input.GetMouseButtonDown(0))
  //  {
  //    //Check if it's over the mask starting area
  //    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
  //    pointerEventData.position = Input.mousePosition;

  //    List<RaycastResult> raycastResultList = new List<RaycastResult>();
  //    EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
  //    for (int i = 0; i < raycastResultList.Count; i++)
  //    {
  //      if (raycastResultList[i].gameObject.GetComponent<N95Mask>() != null)
  //      {
  //        Debug.Log("Got a mask!");
  //        if (OnStartDrag != null)
  //          OnStartDrag();
  //        _isDragging = true;
  //        break;
  //      }
  //    }
  //  }
  //}

  void HandleGiveMask(Doctor md)
  {
    _score += md.pointValue;
    Debug.Log("Gave a dr mask: " + md.pointValue);
  }

  void HandleFail(Doctor md)
  {
    if (md.givesPenalty)
    {
      _remainingHP--;
      if (_remainingHP == 0)
        FinishGameplay();
    }
  }

  public void FinishGameplay()
  {
    MetagameManager.GoToFinale();
  }
}
