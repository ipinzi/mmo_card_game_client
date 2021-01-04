using System.Linq;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking
{
    public class CommandDataObject
    {
        private string _data = "";
        
        public CommandDataObject(string cmd)
        {
            _data += "{'cmd': '" + cmd + "', 'data': {";
        }
        public void AddData(string key, string value)
        {
            _data += " '" + key + "': '" + value + "',";
        }
        public void AddData(string key, int value)
        {
            AddData(key, value.ToString());
        }
        public void AddData(string key, float value)
        {
            AddData(key, value.ToString());
        }
        /*public void AddData(string key, Vector3 value)
        {
            var str = "{'x':'"+value.x+"','y':'"+value.y+"','z':'"+value.z+"'}";
            AddData(key, str);
        }*/
        public string Data()
        {
            if (_data.Last().ToString() == ",") _data = _data.Remove(_data.Length - 1);
            return _data += "}}";
        }
    }
}