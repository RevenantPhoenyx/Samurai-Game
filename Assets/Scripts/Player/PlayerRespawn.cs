using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header ("SFX")]
    [SerializeField] private AudioClip checkpointSound; //sound when you reach checkpoint
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //check if checkpoint available
        if(currentCheckpoint == null)
        {
            //Show Game Over Screen
            uiManager.GameOver();

            return; //don't execute the rest of this function
        }

        transform.position = currentCheckpoint.position; //move player to checkpoint position
        playerHealth.Respawn(); //Restore player health and reset the animation

        //move camera back to checkpoint room (for this to work the checkpoint object has to be placed as the child of the room object
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoints

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //store the checkpoint that we activate as the current checkpoint
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //disable collider
            collision.GetComponent<Animator>().SetTrigger("activate"); //Trigger checkpoint animation
        }
    }
}
