using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;//3维的向量，存储玩家的移动信息
    Animator anim;
    Rigidbody PlayerRigibidy;
    int floorMask; 
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        PlayerRigibidy = GetComponent<Rigidbody>();
    }

    void FixedUpdate() //物理更新
    {
        float h = Input.GetAxisRaw("Horizontal");//只取-1 0 1
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h,float v)
    {
        movement.Set(h, 0f, v); //设置值
        movement = movement.normalized * speed * Time.deltaTime;
        PlayerRigibidy.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if(Physics.Raycast(camRay,out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            PlayerRigibidy.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
