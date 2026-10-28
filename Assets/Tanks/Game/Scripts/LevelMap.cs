using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Game.Scripts
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        public List<Transform> SpawnPoints => _spawnPoints;
    }
}