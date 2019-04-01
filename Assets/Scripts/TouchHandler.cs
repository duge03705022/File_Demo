using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchHandler : MonoBehaviour
{
    public RFIBManager rFIBManager;
    public CardHandler cardHandler;
    public GameController gameController;

    # region Touch Parameters
    private bool ifTouch;
    private int touchTime;
    private int notTouchTime;

    private Tuple<int, int> nowTouch;
    private Tuple<int, int> lastTouch;

    private string touchAction;

    private int clickCount;

    private Tuple<int, int>[] touchHistory;
    private int touchHistoryCount;
    private string swipeDirection;

    # endregion

    // Start is called before the first frame update
    void Start()
    {
        ifTouch = false;
        touchTime = 0;
        notTouchTime = 0;

        nowTouch = Tuple.Create(-1, -1);
        lastTouch = Tuple.Create(-1, -1);

        touchAction = "Idle";

        clickCount = 0;

        touchHistory = new Tuple<int, int>[RFIBParameter.maxTouch];
        touchHistoryCount = 0;
        swipeDirection = "None";
    }

    // Update is called once per frame
    void Update()
    {
        SenseTouch();
        DoAction();
        KeyPressed();

        // Debug msg
        bool ifDebug = false;
        if (ifDebug)
        {
            Debug.Log(string.Format("touchTime:{0} notTouchTime:{1} - {2}({4}) - clickCount:{3}", touchTime, notTouchTime, touchAction, clickCount, swipeDirection));

            string str = "Position history: ";
            for (int i = 0; i < touchHistoryCount; i++)
            {
                str += string.Format("({0}, {1}) ", touchHistory[i].Item1, touchHistory[i].Item2);
            }
            if (touchHistoryCount != 0)
            {
                Debug.Log(str);
            }
        }
    }

    private void SenseTouch()
    {
        ifTouch = false;
        nowTouch = Tuple.Create(-1, -1);
        // Find touching position (*Guarantee one touch per frame)
        for (int i = 0; i < RFIBParameter.touchCol; i++)
        {
            for (int j = 0; j < RFIBParameter.touchRow; j++)
            {
                if (rFIBManager.touchBlock[i, j])
                {
                    ifTouch = true;
                    nowTouch = Tuple.Create(i, j);
                }
            }
        }

        // Touch calculating
        if (ifTouch)
        {
            if (touchTime == 0 || !nowTouch.Equals(lastTouch))
            {
                if (!nowTouch.Equals(lastTouch))
                {
                    clickCount = 0;
                }

                clickCount++;

                touchAction = "ClickAgain";
                touchHistory[touchHistoryCount] = Tuple.Create(nowTouch.Item1, nowTouch.Item2);
                touchHistoryCount++;
            }

            touchTime++;
            notTouchTime = 0;
            lastTouch = Tuple.Create(nowTouch.Item1, nowTouch.Item2);
        }
        else
        {
            touchTime = 0;
            notTouchTime++;
        }

        // Identify the touch action
        IdentifyAction();
    }

    private void ResetTouch()
    {
        touchHistory = new Tuple<int, int>[RFIBParameter.maxTouch];
        clickCount = 0;
        touchHistoryCount = 0;
        swipeDirection = "None";
        lastTouch = Tuple.Create(-1, -1);
    }

    private void DoAction()
    {
        switch (touchAction)
        {
            case "Click":
                Click(touchHistory[touchHistoryCount - 1].Item1, touchHistory[touchHistoryCount - 1].Item2);
                break;
            case "DoubleClick":
                DoubleClick(touchHistory[touchHistoryCount - 1].Item1, touchHistory[touchHistoryCount - 1].Item2);
                break;
            case "Hold":
                Hold(touchHistory[touchHistoryCount - 1].Item1, touchHistory[touchHistoryCount - 1].Item2);
                break;
            case "Swipe":
                Swipe(swipeDirection, touchHistory[touchHistoryCount - 3].Item1, touchHistory[touchHistoryCount - 3].Item2);
                break;
            case "Idle":
                Idle();
                break;
        }
    }

    private void Click(int x, int y)
    {
        if (cardHandler.IfFile(x / 3, y / 3) && x % 3 == 2 && gameController.ifPlaceSelect)
        {
            cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().SelectTxt(y % 3 * (-1) + 2, x / 3, y / 3);
        }

        touchAction = "ClickDone";
    }

    private void DoubleClick(int x, int y)
    {
        Debug.Log(string.Format("DoubleClick ({0}, {1})", x / 3, y / 3));

        touchAction = "DoubleClickDone";
    }

    private void Hold(int x, int y)
    {
        Debug.Log(string.Format("Hold ({0}, {1})", x / 3, y / 3));

        if (cardHandler.IfFile(x / 3, y / 3))
        {
            if (cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().txtCount > 0)
            {
                if (!cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().ifOpen)
                {
                    cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().ifOpen = true;
                    cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().txtParent.SetActive(true);
                }
                else
                {
                    gameController.txtMask.SetActive(true);
                    cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().ShowAllTxt();
                }
            }
            else
            {
                StartCoroutine(ShowNoFile(x / 3, y / 3));
            }
        }

        touchAction = "HoldDone";
    }

    private void Swipe(string direction, int x, int y)
    {
        Debug.Log(string.Format("Swipe {0} ({1}, {2})", direction, x, y));

        if (cardHandler.IfFile(x / 3, y / 3) && gameController.txtMask.activeSelf && (x % 3 == 0 || x % 3 == 1))
        {
            StartCoroutine(cardHandler.cardInstance[x / 3, y / 3].GetComponent<FileController>().SwipeTxt(direction));
        }

        touchAction = "SwipeDone";
    }

    private void Idle()
    {
        touchAction = "IdleDone";
    }

    private void IdentifyAction()
    {
        // Click
        if (clickCount == 1 && touchAction != "ClickDone")
        {
            touchAction = "Click";
        }
        // DoubleClick
        if (clickCount == 2 && touchAction != "DoubleClickDone")
        {
            touchAction = "DoubleClick";
        }
        // Hold
        if (touchTime >= 30 && touchAction != "HoldDone" && touchAction != "SwipeDone")
        {
            touchAction = "Hold";
            clickCount = 0;
        }
        // Swipe
        if (touchHistoryCount >= 3 && touchAction != "SwipeDone")
        {
            if (touchHistory[2].Item1 > touchHistory[1].Item1 &&
                touchHistory[1].Item1 > touchHistory[0].Item1 &&
                touchHistory[2].Item2 == touchHistory[1].Item2 &&
                touchHistory[1].Item2 == touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Left";
                clickCount = 0;
            }
            else if (touchHistory[2].Item1 == touchHistory[1].Item1 &&
                touchHistory[1].Item1 == touchHistory[0].Item1 &&
                touchHistory[2].Item2 > touchHistory[1].Item2 &&
                touchHistory[1].Item2 > touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Up";
                clickCount = 0;
            }
            else if (touchHistory[2].Item1 < touchHistory[1].Item1 &&
                touchHistory[1].Item1 < touchHistory[0].Item1 &&
                touchHistory[2].Item2 == touchHistory[1].Item2 &&
                touchHistory[1].Item2 == touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Right";
                clickCount = 0;
            }
            else if (touchHistory[2].Item1 == touchHistory[1].Item1 &&
                touchHistory[1].Item1 == touchHistory[0].Item1 &&
                touchHistory[2].Item2 < touchHistory[1].Item2 &&
                touchHistory[1].Item2 < touchHistory[0].Item2)
            {
                touchAction = "Swipe";
                swipeDirection = "Down";
                clickCount = 0;
            }
        }
        // Idle
        if (notTouchTime >= 40 && touchAction != "IdleDone")
        {
            touchAction = "Idle";
            ResetTouch();
        }
    }

    private IEnumerator ShowNoFile(int x, int y)
    {
        cardHandler.cardInstance[x, y].GetComponent<FileController>().noFile.SetActive(true);
        yield return new WaitForSeconds(1f);
        cardHandler.cardInstance[x, y].GetComponent<FileController>().noFile.SetActive(false);
    }

    private void KeyPressed()
    {
        Press("z", 9, 6);
        Press("x", 10, 6);
        Press("c", 11, 6);
        Press("a", 9, 7);
        Press("s", 10, 7);
        Press("d", 11, 7);
        Press("q", 9, 8);
        Press("w", 10, 8);
        Press("e", 11, 8);

        Press("v", 15, 6);
        Press("b", 16, 6);
        Press("n", 17, 6);
        Press("f", 15, 7);
        Press("g", 16, 7);
        Press("h", 17, 7);
        Press("r", 15, 8);
        Press("t", 16, 8);
        Press("y", 17, 8);

        //if (Input.GetKeyUp("z"))
        //{
        //    ResetSelect();
        //}
    }

    void Press(string key, int x, int y)
    {
        if (Input.GetKey(key))
        {
            rFIBManager.touchBlock[x, y] = true;
        }
        else
        {
            rFIBManager.touchBlock[x, y] = false;
        }
    }
}
