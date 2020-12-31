using UnityEngine;

namespace MMO_Card_Game.Scripts.UI
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject[] menus;

        private void Awake()
        {
            OpenMenu(0);
        }
        public void OpenMenu(int i)
        {
            foreach (var menu in menus)
            {
                menu.SetActive(false);
            }
            menus[i].SetActive(true);
        }
    }
}