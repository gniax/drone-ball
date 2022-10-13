using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using System.Linq;

namespace MatchController
{
    public class SpawnController : MonoBehaviour
    {
        private readonly Dictionary<Transform, GameObject>
            _spawnPositionUsage = new Dictionary<Transform, GameObject>();

        private Transform _ballSpawnPosition;

        private Transform _blueFallBackSpawnPosition;
        private Transform _blueRespawnPositions;
        private Transform _blueSpawnPositions;

        private Transform _orangeFallBackSpawnPosition;
        private Transform _orangeRespawnPositions;
        private Transform _orangeSpawnPositions;
        
        private void Awake()
        {
            var spawnPositions = transform.Find("World").Find("Rocket_Map").Find("SpawnPositions");

            _orangeSpawnPositions = spawnPositions.Find("Orange").Find("Spawn");
            _blueSpawnPositions = spawnPositions.Find("Blue").Find("Spawn");
            _orangeRespawnPositions = spawnPositions.Find("Orange").Find("Respawn");
            _blueRespawnPositions = spawnPositions.Find("Blue").Find("Respawn");

            var up = transform.up;
            _orangeFallBackSpawnPosition =
                Instantiate(_orangeSpawnPositions.GetChild(_orangeSpawnPositions.childCount - 1));
            _orangeFallBackSpawnPosition.transform.localPosition += 10 * up;
            _blueFallBackSpawnPosition = Instantiate(_blueSpawnPositions.GetChild(_blueSpawnPositions.childCount - 1));
            _blueFallBackSpawnPosition.transform.localPosition += 10 * up;

            _ballSpawnPosition = spawnPositions.Find("Ball").Find("Center");
        }

        private Transform GetSpawnPosition(GameObject car, TeamController.Team team, bool wasDemolished)
        {
            Transform spawnPositions;
            if (team == TeamController.Team.ORANGE)
                spawnPositions = wasDemolished ? _orangeRespawnPositions : _orangeSpawnPositions;

            else
                spawnPositions = wasDemolished ? _blueRespawnPositions : _blueSpawnPositions;

            var childNum = spawnPositions.childCount;
            var idx = Random.Range(0, childNum - 1);
            for (var i = 0; i < childNum; i++)
            {
                idx = (idx + 1) % childNum;
                var spawnPosition = spawnPositions.GetChild(idx);
                if (_spawnPositionUsage.ContainsKey(spawnPosition))
                {
                    if (_spawnPositionUsage[spawnPosition] == car ||
                        (_spawnPositionUsage[spawnPosition].transform.position - spawnPosition.transform.position).magnitude > 0.1)
                    {
                        _spawnPositionUsage[spawnPosition] = car;
                        return spawnPosition;
                    }
                }
                else
                {
                    _spawnPositionUsage.Add(spawnPosition, car);
                    return spawnPosition;
                }
            }

            // this code should never be reached but it is here as a safety net
            return team == 0 ? _orangeFallBackSpawnPosition : _blueFallBackSpawnPosition;
        }
        
        public GameObject SpawnCar(GameObject car, TeamController.Team team, bool wasDemolished = false)
        {
            var spawnLocation = GetSpawnPosition(car, team, wasDemolished);
            car.transform.position = spawnLocation.position;
            car.transform.rotation = spawnLocation.rotation;
            Rigidbody rb = car.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            return car;
        }

        public void SpawnOppositeCars(GameObject[] teamBlue, GameObject[] teamOrange)
        {
            int blueTeamSize = teamBlue.Length;
            int orangeTeamSize = teamOrange.Length;
            int maxTeamSize = Math.Max(blueTeamSize, orangeTeamSize);
            int spawnNum = _blueSpawnPositions.childCount;

            var rnd = new System.Random();
            List<int> spawns = Enumerable.Range(0, spawnNum).ToList().OrderBy(x=>rnd.Next()).Take(maxTeamSize).ToList();
            int lSpawnCount = spawns.Count;

            for (int i = 0; i < maxTeamSize; i++)
            {
                if (i >= lSpawnCount)
                {
                    continue;
                }

                if (i < blueTeamSize && teamBlue[i])
                {
                    // horrible, il aurait fallu au prealable stocker dans un
                    // Singleton les véhicules en jeu afin d'y accéder et au pire dans le awake faire le getComponent

                    var lRigidbody = teamBlue[i].GetComponent<Rigidbody>();
                    Transform lBlueSpawnPos = _blueSpawnPositions.GetChild(spawns[i]);
                    teamBlue[i].transform.position = lBlueSpawnPos.position;
                    teamBlue[i].transform.rotation = lBlueSpawnPos.rotation;
                    lRigidbody.velocity = Vector3.zero;
                    lRigidbody.angularVelocity = Vector3.zero;
                }

                if (i < orangeTeamSize && teamOrange[i])
                {
                    var lRigidbody = teamOrange[i].GetComponent<Rigidbody>();
                    Transform lOrangeSpawnPos = _orangeSpawnPositions.GetChild(spawns[i]);
                    teamOrange[i].transform.position = lOrangeSpawnPos.position;
                    teamOrange[i].transform.rotation = lOrangeSpawnPos.rotation;
                    lRigidbody.velocity = Vector3.zero;
                    lRigidbody.angularVelocity = Vector3.zero;
                }
            }
        }

        public GameObject SpawnBall(GameObject ball)
        {
            ball.transform.position = _ballSpawnPosition.position;
            ball.transform.rotation = _ballSpawnPosition.rotation;
            return ball;
        }
    }
}