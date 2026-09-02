using UnityEngine;
using System.Collections.Generic;
using Wilson.Item;
using Wilson.Utility;
using TMPro;
using Wilson.UI;

namespace Wilson.Player
{
    public class ItemDetector : Singleton<ItemDetector>
    {
        [Header("설정")]
        [SerializeField] private float maxRayDistance = 5f;
        [SerializeField] private LayerMask obstructionMask;
        [SerializeField] private TextMeshProUGUI interactiveText;
        [SerializeField] private GameObject interactivePanel;
        [SerializeField] private InventoryUI_Expanded inventoryTab;

        [Header("감지된 오브젝트들")]
        [SerializeField] private List<IInteractable> nearbyInteractables = new List<IInteractable>();

        private IInteractable currentTarget;

        private string beforePrompt;

        private void Update()
        {
            DetectClosestVisibleInteractable();
        }

        private void DetectClosestVisibleInteractable()
        {
            float closestDistance = float.MaxValue;
            currentTarget = null;

            // 삭제된 오브젝트 정리
            nearbyInteractables.RemoveAll(item => item == null || (item as MonoBehaviour) == null);

            foreach (var interactable in nearbyInteractables)
            {
                if (interactable == null) continue;

                Transform targetTransform = (interactable as MonoBehaviour).transform;
                float distance = Vector3.Distance(transform.position, targetTransform.position);

                // 가려졌는지 확인
                Vector3 dir = (targetTransform.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, maxRayDistance, obstructionMask))
                {
                    // 레이에 맞은 게 interactable이 아니면 막힘
                    if (hit.transform != targetTransform) continue;
                }

                // 여기까지 통과했으면 가장 가까운 유효 대상일 수 있음
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = interactable;
                }
            }

            // 현재 타겟과 상호작용 활성화
            if (currentTarget != null)
            {
                if (!interactivePanel.activeSelf)
                {
                    interactivePanel.SetActive(true);
                }

                string prompt = currentTarget.GetPrompt();

                if (prompt != beforePrompt)
                {
                    interactiveText.text = prompt;
                }
                beforePrompt = prompt;
            }
            else
            {
                if (interactivePanel.activeSelf)
                {
                    interactivePanel.SetActive(false);
                }
            }
        }
        public void RemoveTarget(IInteractable target)
        {
            nearbyInteractables.Remove(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null && !nearbyInteractables.Contains(interactable))
            {
                nearbyInteractables.Add(interactable);
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
                nearbyInteractables.Remove(interactable);
        }

        public void TryInteract()
        {
            currentTarget?.Interact();
            inventoryTab.RefreshUI();
        }
    }
}
