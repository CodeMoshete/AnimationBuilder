using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class UnityUtils
    {
        private static List<Material> materials;
        private static List<Mesh> meshes;

        /// <summary>
        /// Find the game object with the given name under the given parent object, recursively.
        /// </summary>
        /// <returns>The game object.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="name">Name.</param>
        public static GameObject FindGameObject(GameObject parent, string name)
        {
            if (parent.name == name)
            {
                return parent;
            }
            
            Transform transform = parent.transform;
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                GameObject gameObject = FindGameObject(transform.GetChild(i).gameObject, name);
                if (gameObject != null)
                {
                    return gameObject;
                }
            }
            
            return null;
        }

        private static void GetAllChildren(GameObject parent, ref List<GameObject> results)
        {
            Transform transform = parent.transform;
            results.Add(parent);

            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                GetAllChildren(transform.GetChild(i).gameObject, ref results);
            }
        }

        public static List<GameObject> GetAllChildren(GameObject parent)
        {
            List<GameObject> results = new List<GameObject>();
            GetAllChildren(parent, ref results);

            return results;
        }

        /// <summary>
        /// Finds the game object containing the name, recursively
        /// </summary>
        /// <returns>The game object that contains the name.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="name">Name.</param>
        public static GameObject FindGameObjectContains(GameObject parent, string name)
        {
            if (parent.name.Contains(name))
            {
                return parent;
            }
            
            Transform transform = parent.transform;
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                GameObject gameObject = FindGameObjectContains(transform.GetChild(i).gameObject, name);
                if (gameObject != null)
                {
                    return gameObject;
                }
            }
            
            return null;
        }

        /// <summary>
        /// Finds all children contained inside the given parent whose names contain the passed 'name' string.
        /// </summary>
        /// <returns>The game object that contains the name.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="name">Name.</param>
        public static List<GameObject> FindAllGameObjectContains(GameObject parent, string name, string excludes = "")
        {
            List<GameObject> returnList = new List<GameObject>();

            if (parent.name.Contains(name) && (string.IsNullOrEmpty(excludes) || !parent.name.Contains(excludes)))
            {
                returnList.Add(parent);
            }
            
            Transform transform = parent.transform;
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                List<GameObject> gameObjects = 
                    FindAllGameObjectContains(transform.GetChild(i).gameObject, name, excludes);
                if (gameObjects.Count > 0)
                {
                    returnList.AddRange(gameObjects);
                }
            }
            
            return returnList;
        }

        /// <summary>
        /// Searches all GameObjects in scene for objects whose names contain the passed 'name' string.
        /// </summary>
        /// <returns>The game object that contains the name.</returns>
        /// <param name="name">Name.</param>
        /// /// <param name="excludes">Exclude objects containing this string in their names.</param>
        public static List<GameObject> FindAllGameObjectContains(string name, string excludes = "", bool exact = false)
        {
            List<GameObject> returnList = new List<GameObject>();

            if (!string.IsNullOrEmpty(name))
            {
                List<GameObject> allObjects = new List<GameObject>(GameObject.FindObjectsOfType<GameObject>());
                foreach (GameObject gameObj in allObjects)
                {
                    if ((exact && string.Equals(gameObj.name, name)) || 
                        (!exact && gameObj.name.Contains(name) && 
                        (string.IsNullOrEmpty(excludes) || 
                        !gameObj.name.Contains(excludes))))
                    {
                        returnList.Add(gameObj);
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// Find all GameObjects with a specific tag.
        /// </summary>
        /// <returns>All GameObjects that contain a specific tag.</returns>
        /// <param name="parent">Parent.</param>
        /// <param name="tag">Tag.</param>
        public static List<GameObject> FindAllGameObjectWithTag(GameObject parent, string tag)
        {
            List<GameObject> returnList = new List<GameObject>();
            
            if (!string.IsNullOrEmpty(parent.tag) && parent.tag.Equals(tag))
            {
                returnList.Add(parent);
            }
            
            Transform transform = parent.transform;
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                List<GameObject> gameObjects = FindAllGameObjectWithTag(transform.GetChild(i).gameObject, tag);
                if (gameObjects.Count > 0)
                {
                    returnList.AddRange(gameObjects);
                }
            }
         
            return returnList;
        }

        /// <summary>
        /// Searches all GameObjects in scene for objects which contain a given MonoBehavior T.
        /// </summary>
        /// <returns>A list of all objects in scene containing MonoBehavior T.</returns>
        public static List<GameObject> FindAllGameObjectContains<T>() where T:Component
        {
            List<GameObject> returnList = new List<GameObject>();
            
            List<GameObject> allObjects = 
                new List<GameObject>(GameObject.FindObjectsOfType<GameObject>());

            foreach (GameObject gameObj in allObjects)
            {
                Component component = gameObj.GetComponent<T>();
                if (component != null)
                {
                    returnList.Add(gameObj);
                }
            }
            
            return returnList;
        }

        /// <summary>
        /// Searches all GameObjects in scene for objects which contain a given MonoBehavior T.
        /// </summary>
        /// <returns>A list of all objects in scene containing MonoBehavior T.</returns>
        public static List<GameObject> FindAllGameObjectContains<T>(GameObject baseObject) 
            where T:Component
        {
            List<GameObject> returnList = new List<GameObject>();

            Component parentComponent = baseObject.GetComponent<T>();
            if (parentComponent != null)
            {
                returnList.Add(baseObject);
            }

            List<GameObject> allObjects = GetAllChildren(baseObject);
            foreach (GameObject gameObj in allObjects)
            {
                Component childComponent = gameObj.GetComponent<T>();
                if (childComponent != null)
                {
                    returnList.Add(gameObj);
                }
            }
            
            return returnList;
        }

        /// <summary>
        /// Adds a GameObject to a parent with the transform properties it was loaded with.
        /// </summary>
        /// <param name="gameObject">Game object to add.</param>
        /// <param name="parent">The parent object to add gameObject to.</param>
        public static void SetParent(GameObject gameObject, GameObject parent)
        {
            Vector3 originalPos = gameObject.transform.localPosition;
            Quaternion originalRot = gameObject.transform.localRotation;
            Vector3 originalScale = gameObject.transform.localScale;

            gameObject.transform.SetParent(parent.transform);

            gameObject.transform.localPosition = originalPos;
            gameObject.transform.localRotation = originalRot;
            gameObject.transform.localScale = originalScale;
        }

        /// <summary>
        /// Sets the layer on the given game object and all its descendants.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="layer">Layer.</param>
        public static void SetLayerRecursively(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            
            Transform transform = gameObject.transform;
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                SetLayerRecursively(transform.GetChild(i).gameObject, layer);
            }
        }

        public static void SetActiveRecursively(GameObject go, bool active)
        {
            foreach (GameObject child in GetAllChildren(go))
            {
                child.SetActive(active);
            }
        }

        /// <summary>
        /// Gets the bounds of a GameObject, including the area occupied by its children.
        /// Note that inactive GameObjects are not considered in this calculation.
        /// </summary>
        /// <returns>The game object bounds.</returns>
        /// <param name="gameObject">Game object.</param>
        public static Bounds GetGameObjectBounds(GameObject gameObject)
        {
            Bounds bounds = new Bounds(gameObject.transform.position, Vector3.zero);
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            
            for (int i = 0, count = renderers.Length; i < count; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            
            return bounds;
        }
        
        // Sets up the given mesh for rendering on the given GameObject.
        public static void SetupMeshMaterial(
            GameObject gameObject, Mesh mesh, Material material)
        {
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        public static GameObject DuplicateGameObject(GameObject original)
        {
            GameObject newObj = GameObject.Instantiate(original) as GameObject;
            newObj.transform.SetParent(original.transform.parent);
            newObj.transform.localScale = original.transform.localScale;
            newObj.transform.localRotation = original.transform.localRotation;
            newObj.transform.localPosition = original.transform.localPosition;
            return newObj;
        }

        public static Vector3 Vector3FromString(string value)
        {
            string[] splitVal = { ", " };
            string leftPar = "(";
            string rightPar = ")";
            value = value.Replace(leftPar, "");
            value = value.Replace(rightPar, "");
            string[] parts = value.Split(splitVal, System.StringSplitOptions.None);
            float xVal = System.Convert.ToSingle(parts[0]);
            float yVal = System.Convert.ToSingle(parts[1]);
            float zVal = System.Convert.ToSingle(parts[2]);
            return new UnityEngine.Vector3(xVal, yVal, zVal);
        }

        public static Vector3 GetGroundPositionFromCamRay(Ray camRay)
        {
            Vector3 spawnPos = Vector3.zero;
            MathUtils.LinePlaneIntersection(out spawnPos, camRay.origin, camRay.direction,
                Vector3.up, Vector3.zero);

            Vector3 vecToSpawn = spawnPos - camRay.origin;
            float dotDir = Vector3.Dot(camRay.direction, vecToSpawn);
            if (dotDir <= 0f)
            {
                // Since we're only looking for positions on y=0, a nonzero y value will
                // indicate the selected location is invalid.
                spawnPos = Vector3.down;
            }
            return spawnPos;
        }

		public static Transform GetRootTransform(Transform childObect)
		{
			if (childObect.parent != null)
			{
				return(GetRootTransform (childObect.parent));
			}
			return childObect;
		}

        public static string PreciseStringFromVector3(Vector3 value)
        {
            return "(" + value.x + ", " + value.y + ", " + value.z + ")";
        }

        public static Dictionary<T, S> CloneDictionary<T,S>(Dictionary<T,S> original)
        {
            Dictionary<T, S> retDict = new Dictionary<T, S>();
            foreach (KeyValuePair<T, S> pair in original)
            {
                retDict.Add(pair.Key, pair.Value);
            }
            return retDict;
        }
    }
}

