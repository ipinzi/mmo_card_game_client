using System;
using System.Collections;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Networking;
using MMO_Card_Game.Scripts.Networking.Commands;
using MMO_Card_Game.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MMO_Card_Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public CardDatabase cardDatabase;
        public GameObject playerPrefab;
        public List<Pawn> pawns;
        public NetworkSettings networkSettings;
        private bool _sceneLoaded = false;

        private void Awake()
        {
            if (!networkSettings)
            {
                Debug.LogError("No network settings attached to game manager.");
            }
            
            pawns = new List<Pawn>();

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                _sceneLoaded = true;
            };

            Game.Manager = this;
        }

        public IEnumerator WaitForSceneLoaded(Action callbackFunc)
        {
            while (!_sceneLoaded)
            {
                yield return new WaitForSeconds(0.1f);
            }

            callbackFunc();
        }

        public void LoadScene(int buildIndex)
        {
            _sceneLoaded = false;
            pawns.Clear();
            SceneManager.LoadScene(buildIndex);
        }
    }
}
