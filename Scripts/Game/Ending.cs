using UnityEngine;
using Wilson.Item;
using System.Linq;
using Wilson.Utility;
using UnityEngine.SceneManagement;

namespace Wilson.Game
{
    public class Ending : MonoBehaviour, IInteractable
    {
        #region Variables
        public GameObject[] partObjects = new GameObject[3];
        public SceneFader fader;
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //초기화
            foreach (GameObject part in partObjects)
            {
                part.SetActive(false);
            }
        }
        #endregion
        public string GetPrompt()
        {
            return (partObjects.All(part => part.activeSelf)) ? "신호 보내기" : "부품 넣기";
        }

        public void Interact()
        {
            //부품 다 모았을 때
            if (partObjects.All(part => part.activeSelf))
            {
                //게임 클리어 처리, 엔딩씬 불러오기
                fader.FadeTo("EndingScene");
            }
            else    //부품 다 못 모았을 때
            {
                //인벤토리 내 Craftable Item 찾기
                //itemCode 추적, 해당 게임오브젝트 활성화
                //인벤토리에서 아이템 제거

                foreach (ItemData item in Inventory.Instance.Items)
                {
                    //Debug.Log("foreach");
                    if (item == null)
                        return;
                    if (item.itemType != ItemType.Craftable)
                        continue;
                    //Debug.Log("Craftable");

                    switch (item.itemCode)
                    {
                        case "Crf0_00":
                            partObjects[0].SetActive(true);
                            Inventory.Instance.RemoveItem(item);
                            break;
                        case "Crf0_01":
                            partObjects[1].SetActive(true);
                            Inventory.Instance.RemoveItem(item);
                            break;
                        case "Crf0_02":
                            partObjects[2].SetActive(true);
                            Inventory.Instance.RemoveItem(item);
                            break;
                        default:
                            //Debug.Log("조합아이템 없음");
                            break;
                    }
                }
            }
            
        }
        
    }

}
