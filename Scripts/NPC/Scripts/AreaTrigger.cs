using UnityEngine;
using Wilson.NPC;
using Wilson.UI;

public class AreaTriggerToNPC : MonoBehaviour
{
    [SerializeField] private NPCFSM targetNPC; // 상태 바꿔줄 NPCFSM 참조
    public string blockingMessage ="";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("플레이어가 감시 구역에 들어옴");

            if (targetNPC != null)
            {
                StartCoroutine(MonologUI.Instance.ShowMonolog(blockingMessage));
                targetNPC.ChangeToWalkToPlayerFromTrigger();

                Invoke(nameof(HideMonolog), 1.5f);
            }
        }
    }

    private void HideMonolog()
    {
        MonologUI.Instance.group.alpha = 0f;
    }
}