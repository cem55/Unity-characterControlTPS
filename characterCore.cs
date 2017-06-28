using UnityEngine;
using System.Collections;

public class characterCore : MonoBehaviour {
	public Camera charaCamera;
    public float gravity = 4f;
    public float jumpPower = 4f;
    public float walkSpeed = 2f;
	public float smoothTime = 0.1f;

	private CharacterController cc;

	private Vector3 movement;
	private float movXZ;
	private float movY;
	private CollisionFlags collisionFlag;
    private bool isFalling = false;
    private bool isJumping = false;

	//smoothdamp vars
	private float _movtmp;
	private float _rottmp;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
	}

	public void Move(Vector3 dir, bool _jump){
		dir = dir.normalized;
        if (isGrounded() && !isJumping)
        {
            if (_jump) {
                isJumping = true;
                isFalling = false;
            }
            if(isJumping == false)
            {
                movY = 0f;
                isJumping = false;
                isFalling = false;
                if (dir != Vector3.zero)
                {
                    float rot = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + charaCamera.transform.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rot, ref _rottmp, smoothTime);
                }
                float _mov = walkSpeed * dir.magnitude;
                movXZ = Mathf.SmoothDamp(movXZ, _mov, ref _movtmp, smoothTime);
            }
        }
        else
        {
            if (isJumping)
            {
                Jump();
            }
            if (!isGrounded() || isFalling)
            {
                movY -= gravity * Time.fixedDeltaTime;
            }
        }
        Debug.Log(isGrounded());
        movement = transform.forward * movXZ + Vector3.up * movY;
        collisionFlag = cc.Move(movement * Time.fixedDeltaTime);
    }

    void Jump() {
        Debug.Log("-------------Jumping");
        float _maxheight = jumpPower * gravity;
        if (movY < _maxheight)
        {
            movY += (movY + jumpPower) * 0.2f;
        }
        else
        {
            isFalling = true;
            isJumping = false;
        }
    }

    bool isGrounded() {
        //Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.3f); //Physics.SphereCast(ray, 0.2f, 2f);
    }

}
