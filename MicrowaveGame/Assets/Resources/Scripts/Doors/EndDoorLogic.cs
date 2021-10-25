using UnityEngine;
using Scripts.Scenes;

public class EndDoorLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player" && (Scripts.Persistent.CollectedKeycardCount == 3))
        {
            SceneFaderBehaviour.Instance.FadeInto("Menu");
        }
    }
}
