using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] CharacterMovement charMovement;
    public CameraMovement cameraMovement;
    private CharacterAttack charAttack;

    private void Awake() {
        if(charMovement != null) {
            charMovement.OnKeyPress += MoveCharacter; //subscribe
            charMovement.OnCollision += OnCollision;
            //charMovement.OnMousePress += MousePress; //subscribe
        }
        else {
            Debug.LogError("CharacterMovement component not found on this GameObject.");
        }
    }

    private void Start() {
        cameraMovement.Setup(() => charMovement.transform.position);
    }

    private void Update() {

    }

    // private void MousePress(object sender, CharacterMovement.OnMousePressEventArgs e){
    //     Debug.Log("We have reached the CharacterAttack event WITH MOUSECLICK!!! " + e.mousePress);
    //     //charMovement.PlayAnimation(charMovement.attackArray, .1f);
    // }
    private void MoveCharacter(object sender, CharacterMovement.OnKeyPressEventArgs e){
        //Debug.Log(e.keyValue);
        cameraMovement.Setup(() => charMovement.transform.position);
    }
    private void OnCollision(object sender, CharacterMovement.OnCollisionEventArgs e){
        Debug.Log("Collision detected with: " + e.collisionVar.collider.gameObject.name);
        Debug.Log("And it happened at : " + e.collisionVar.point);
    }

    void OnDestroy()
    {
        if(charMovement != null){
            charMovement.OnKeyPress -= MoveCharacter;
        }
    }
}
