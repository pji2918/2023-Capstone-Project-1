using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 오른쪽 마우스 클릭을 하면 플레이어를 클릭한 위치로 이동시킵니다.
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PlayerController.instance._PlayerAgent.SetDestination(mousePosition);
        }

        // 왼쪽 Shift 키를 누르면 플레이어를 돌진시키는 함수를 실행합니다.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(PlayerController.instance.Dash());
        }
    }
}
