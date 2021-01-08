using System;
using System.Collections;
using MMO_Card_Game.Scripts.Networking;

namespace MMO_Card_Game.Scripts
{
    public static class Game
    {
        public static GameManager Manager;
        public static WebsocketManager Network;
        public static Player.Player LocalPlayer;
        
        public static IEnumerator WaitForLocalPlayer(Action callbackFunc)
        {
            while (!Game.LocalPlayer)
            {
                yield return false;
            }
            callbackFunc();
        }
    }
}