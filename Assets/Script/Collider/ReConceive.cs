using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReConceive : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
        {
            return;
        }

        // 获取玩家的位置和当前物体位置
        Vector3 playerPos = RWarmDNA.Instance.Handle.transform.position;
        Vector3 myPos = transform.position;

        // 根据物体的标签进行不同的处理
        switch (transform.tag)
        {
            // 如果物体是"Ground"标签，当x或y方向距离超过40个单位时进行相应重定位
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            // 如果物体是"Enemy"标签，暂无处理
            case "Enemy":
                break;
        }
    }
}
