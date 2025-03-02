using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A Pooling System for GameObjects
*/

namespace DentedPixel
{
    public class LeanPool : object
    {
        private UnityEngine.GameObject[] array;

        private Queue<UnityEngine.GameObject> oldestItems;

        private int retrieveIndex = -1;

        public UnityEngine.GameObject[] init(UnityEngine.GameObject prefab, int count, Transform parent = null, bool retrieveOldestItems = true)
        {
            array = new UnityEngine.GameObject[count];

            if (retrieveOldestItems)
                oldestItems = new Queue<UnityEngine.GameObject>();

            for (int i = 0; i < array.Length; i++)
            {
                UnityEngine.GameObject go = UnityEngine.GameObject.Instantiate(prefab, parent);
                go.SetActive(false);

                array[i] = go;
            }

            return array;
        }

        public void init(UnityEngine.GameObject[] array, bool retrieveOldestItems = true){
            this.array = array;

            if (retrieveOldestItems)
                oldestItems = new Queue<UnityEngine.GameObject>();
        }

        public void giveup(UnityEngine.GameObject go)
        {
            go.SetActive(false);
            oldestItems.Enqueue(go);
        }

        public UnityEngine.GameObject retrieve()
        {
            for (int i = 0; i < array.Length; i++)
            {
                retrieveIndex++;
                if (retrieveIndex >= array.Length)
                    retrieveIndex = 0;

                if (array[retrieveIndex].activeSelf == false)
                {
                    UnityEngine.GameObject returnObj = array[retrieveIndex];
                    returnObj.SetActive(true);

                    if (oldestItems != null)
                    {
                        oldestItems.Enqueue(returnObj);
                    }

                    return returnObj;
                }
            }

            if (oldestItems != null)
            {
                UnityEngine.GameObject go = oldestItems.Dequeue();
                oldestItems.Enqueue(go);// put at the end of the queue again

                return go;
            }

            return null;
        }
    }

}