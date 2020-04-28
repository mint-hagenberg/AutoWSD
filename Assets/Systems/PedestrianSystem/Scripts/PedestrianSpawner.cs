using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace ANR
{
    public class PedestrianSpawner : MonoBehaviour
    {
        #region Properties

        public List<GameObject> pedestrianPrefabs;
        public int pedestriansToSpawn = 10;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnPedestrians());
        }

        IEnumerator SpawnPedestrians()
        {
            int count = 0;
            while (count < pedestriansToSpawn)
            {
                int randomPrefabIndex = Random.Range(0, pedestrianPrefabs.Count);
                GameObject obj = Instantiate(pedestrianPrefabs[randomPrefabIndex]);
                obj.SetActive(true);
                Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
                obj.GetComponent<PedestrianWaypointNavigator>().currentWaypoint =
                    child.GetComponent<PedestrianWaypoint>();
                obj.transform.position = child.position;

                yield return new WaitForEndOfFrame();

                count++;
            }
        }
    }
}