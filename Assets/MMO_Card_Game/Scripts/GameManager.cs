using System;
using System.Collections;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Networking.Commands;
using MMO_Card_Game.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MMO_Card_Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public List<Pawn> pawns;
        public List<Command> commands;
        private bool _sceneLoaded = false;

        private void Awake()
        {
            pawns = new List<Pawn>();

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                _sceneLoaded = true;
            };
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
            SceneManager.LoadScene(buildIndex);
        }
    }
}
