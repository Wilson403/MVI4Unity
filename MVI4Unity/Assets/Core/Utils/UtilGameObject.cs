using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MVI4Unity
{
    public class UtilGameObject : SafeSingleton<UtilGameObject>
    {
        public void LogChildren (Transform node)
        {
            for ( int i = 0 ; i < node.childCount ; i++ )
            {
                Transform child = node.GetChild (i);
                Debug.Log ($"i[{i}] name[{child.gameObject.name}] pos[{child.localPosition}]");
            };
        }

        private bool SortByDistanceWithCamera_GetActive (Transform trans)
        {
            return trans.gameObject.activeInHierarchy;
        }

        private float SortByDistanceWithCamera_GetPosY (Transform trans)
        {
            return trans.localPosition.y;
        }

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<Transform> GetChildrenList (Transform root)
        {
            List<Transform> childrenList = new List<Transform> ();
            for ( int i = 0 ; i < root.childCount ; i++ )
            {
                childrenList.Add (root.GetChild (i));
            };
            return childrenList;
        }

        /// <summary>
        /// 名字列表
        /// </summary>
        List<string> _listName = new List<string> ();

        /// <summary>
        /// 获取一个节点在场景中的路径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetDir (Transform obj , Transform root = default)
        {
            _listName.Clear ();
            while ( obj != root )
            {
                _listName.Add (obj.name);
                obj = obj.parent;
            };
            _listName.Reverse ();
            return String.Join ("/" , _listName);
        }

        /// <summary>
        /// 尽量查找节点，优先使用路径查找，其次使用名称查找，再其次忽略大小写
        /// </summary>
        /// <param name="root"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Transform FindAsPossible (Transform root , string tag)
        {
            // 依据的信息为空，则返回根节点
            if ( tag == null || tag == "" )
            {
                return root;
            };

            // 当作路径查找一遍
            Transform foundTransform = root.Find (tag);
            if ( foundTransform != null )
            {
                return foundTransform;
            };
            // 当作名字匹配一遍
            foundTransform = FindByMatchName (root , tag);
            if ( foundTransform != null )
            {
                return foundTransform;
            };
            // 当作名字，忽略大小写匹配一遍
            foundTransform = FindByMatchNameIgnoreCase (root , tag);
            if ( foundTransform == null )
            {
                return root;
            };

            return foundTransform;
        }

        /// <summary>
        /// 通过名称来查找
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform FindByMatchNameIgnoreCase (Transform root , string name)
        {
            Transform target = null;
            name = name.ToLower ();
            WalkDepth (root.gameObject , (childNode) =>
              {
                  // 查找名称中包含敏感内容的节点
                  if ( Regex.Match (childNode.transform.name.ToLower () , name).Value != "" )
                  {
                      target = childNode.transform;
                  };
              });
            return target;
        }

        List<GameObject> _walkerList = new List<GameObject> ();
        List<GameObject> _collectionList = new List<GameObject> ();
        List<GameObject> _cloneList = new List<GameObject> ();

        /// <summary>
        /// 遍历节点，深度相关作为优先
        /// </summary>
        /// <param name="gameObj"></param>
        /// <param name="walker"></param>
        /// <param name="isLeafFirst"></param>
        public void WalkDepth (GameObject gameObj , Action<GameObject> walker , bool depthFirst = true)
        {
            _walkerList.Clear ();
            _collectionList.Clear ();
            _collectionList.Add (gameObj);
            // 只要还有剩余的没处理的节点
            while ( _collectionList.Count > 0 )
            {
                _cloneList.Clear ();
                _cloneList.AddRange (_collectionList);
                _collectionList.Clear ();
                _cloneList.ForEach ((item) =>
                 {
                     // 把当前集合放进遍历集合里面
                     _walkerList.Add (item);
                     // 把他们的子节点放进当前集合里面
                     for ( int i = 0 ; i < item.transform.childCount ; i++ )
                     {
                         Transform childTransForm = item.transform.GetChild (i);
                         _collectionList.Add (childTransForm.gameObject);
                     };
                 });
            };

            if ( depthFirst )
            {
                _walkerList.Reverse ();
            };

            _walkerList.ForEach ((tar) =>
             {
                 walker (tar);
             });
        }

        /// <summary>
        /// 设置标签
        /// </summary>
        /// <param name="gameObj"></param>
        public void SetTag (GameObject gameObj , int layer)
        {
            WalkDepth (gameObj , (gb) =>
              {
                  gb.layer = layer;
              });
        }

        /// <summary>
        /// 锁定到某个对象上
        /// </summary>
        /// <param name="self"></param>
        /// <param name="theTarget"></param>
        /// <param name="lerpSpeed"></param>
        public void LookTarget (Transform self , Transform theTarget , float lerpSpeed = 0.1f)
        {
            //获取方向向量
            Vector3 direction = theTarget.position - self.position;
            Quaternion lookRotation = Quaternion.LookRotation (direction);
            self.rotation = Quaternion.Slerp (self.rotation , lookRotation , lerpSpeed);
        }

        /// <summary>
        /// 遍历一个节点为根的所有节点的材质
        /// </summary>
        /// <param name="rootObj"></param>
        /// <param name="matWalker"></param>
        public void WalkMaterial (GameObject rootObj , Action<Material> matWalker)
        {
            SkinnedMeshRenderer [] skinnedMeshRenderArr = rootObj.GetComponentsInChildren<SkinnedMeshRenderer> ();
            if ( skinnedMeshRenderArr != null )
            {
                foreach ( SkinnedMeshRenderer smr in skinnedMeshRenderArr )
                {
                    foreach ( Material mat in smr.materials )
                    {
                        if ( mat != null )
                        {
                            matWalker (mat);
                        };
                    };
                };
            }
        }

        /// <summary>
        /// 遍历一个节点为根的所有节点的材质
        /// </summary>
        /// <param name="rootObj"></param>
        /// <param name="matWalker"></param>
        public void WalkMaterial (GameObject rootObj , Action<Material> matWalker , List<string> matNameFilter)
        {
            Dictionary<string , bool> recDict = new Dictionary<string , bool> ();
            foreach ( string item in matNameFilter )
            {
                recDict [item] = true;
            };
            // 如果有该 shader 名字的记录，那么就触发回调
            WalkMaterial (rootObj , (mat) =>
              {
                  if ( recDict.ContainsKey (mat.shader.name) )
                  {
                      matWalker (mat);
                  };
              });
        }

        /// <summary>
        /// 遍历一个节点为根的所有节点的材质
        /// </summary>
        /// <param name="rootObj"></param>
        /// <param name="matWalker"></param>
        public Material [] GetMaterials (GameObject rootObj , List<string> matNameFilter)
        {
            List<Material> matList = new List<Material> ();
            Dictionary<string , bool> recDict = new Dictionary<string , bool> ();
            foreach ( string item in matNameFilter )
            {
                recDict [item] = true;
            };
            // 如果有该 shader 名字的记录，那么就触发回调
            WalkMaterial (rootObj , (mat) =>
              {
                  if ( recDict.ContainsKey (mat.shader.name) )
                  {
                      matList.Add (mat);
                  };
              });
            return matList.ToArray ();
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        /// <param name="gamObj"></param>
        public void ClearChildren (GameObject gamObj)
        {
            if ( gamObj == default )
            {
                return;
            };
            ClearChildren (gamObj.transform);
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="isImmediate"></param>
        public void ClearChildren (Transform trans , bool isImmediate = false)
        {
            if ( trans == default )
            {
                return;
            };
            List<GameObject> list = new List<GameObject> ();
            for ( int i = 0 ; i < trans.childCount ; i++ )
            {
                list.Add (trans.GetChild (i).gameObject);
            };
            list.ForEach ((gameObj) =>
            {
                if ( isImmediate )
                {
                    UnityEngine.Object.DestroyImmediate (gameObj);
                    return;
                }
                UnityEngine.Object.Destroy (gameObj);
            });
        }

        /// <summary>
        /// 清除所有子节点(在删除节点前，先设置父节点为空， 这样可以避免影响布局)
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="isImmediate"></param>
        public void ClearChildrenAndRemove (Transform trans , bool isImmediate = false)
        {
            if ( trans == default )
            {
                return;
            };
            List<GameObject> list = new List<GameObject> ();
            for ( int i = 0 ; i < trans.childCount ; i++ )
            {
                list.Add (trans.GetChild (i).gameObject);
            };
            for ( int i = 0 ; i < list.Count ; ++i )
            {
                list [i].transform.SetParent (null);
                UnityEngine.Object.Destroy (list [i]);
            }
        }

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Transform [] GetChildArray (Transform target)
        {
            if ( target.childCount == 0 ) return null;
            Transform [] array = new Transform [target.childCount];
            for ( int i = 0 ; i < target.childCount ; i++ )
            {
                array [i] = target.GetChild (i);
            }
            return array;
        }

        /// <summary>
        /// 交换两个GameObj位置
        /// </summary>
        public void SwapDoubleGameObjPosition (Transform target1 , Transform target2)
        {
            Vector3 target1StartPos = target1.position;
            Vector3 target2StartPos = target2.position;

            target1.position = target2StartPos;
            target2.position = target1StartPos;
        }

        /// <summary>
        /// 查找节点，优先使用路径查找，其次使用名称查找
        /// </summary>
        /// <param name="root"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Transform Find (Transform root , string tag)
        {
            // 当作路径查找一遍
            Transform foundTransform = root.Find (tag);
            if ( foundTransform != null )
            {
                return foundTransform;
            };
            // 当作名字匹配一遍
            foundTransform = FindByMatchName (root , tag);
            if ( foundTransform != null )
            {
                return foundTransform;
            };
            // 当作名字，忽略大小写匹配一遍
            foundTransform = FindByMatchNameIgnoreCase (root , tag);
            if ( foundTransform == null )
            {
                return root;
            };

            return foundTransform;
        }

        /// <summary>
        /// 通过名称来查找
        /// </summary>
        /// <param name="root"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Transform FindByMatchName (Transform root , string name)
        {
            Transform target = null;
            WalkDepth (root.gameObject , (childNode) =>
            {
                // 查找名称中包含敏感内容的节点
                if ( Regex.Match (childNode.transform.name , name).Value != "" )
                {
                    target = childNode.transform;
                };
            });
            return target;
        }

        /// <summary>
        /// 销毁所有的直接子节点
        /// </summary>
        /// <param name="gamObj"></param>
        public void DestoryDirectChildren (GameObject gamObj)
        {
            List<GameObject> list = new List<GameObject> ();
            for ( int i = 0 ; i < gamObj.transform.childCount ; i++ )
            {
                list.Add (gamObj.transform.GetChild (i).gameObject);
            };
            list.ForEach ((gameObj) =>
            {
                GameObject.Destroy (gameObj);
            });
        }

        /// <summary>
        /// 隐藏所有子节点
        /// </summary>
        /// <param name="gamObj"></param>
        public void HideChildren (GameObject gamObj)
        {
            List<GameObject> list = new List<GameObject> ();
            for ( int i = 0 ; i < gamObj.transform.childCount ; i++ )
            {
                list.Add (gamObj.transform.GetChild (i).gameObject);
            };
            list.ForEach ((gameObj) =>
            {
                gameObj.SetActive (false);
            });
        }

        /// <summary>
        /// 获取从根节点到该节点的路径节点
        /// </summary>
        /// <param name="gameObj"></param>
        /// <returns></returns>
        public List<GameObject> GetDirGameObjArr (GameObject gameObj)
        {
            List<GameObject> arr = new List<GameObject> ();

            Transform transForm = gameObj.transform;

            while ( transForm != null )
            {
                arr.Add (transForm.gameObject);
                transForm = transForm.parent;
            };

            arr.Reverse ();

            return arr;
        }

        /// <summary>
        /// 获取全部的子节点位置
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 [] GetChildrenPositions (Transform transform)
        {
            Vector3 [] v3s = new Vector3 [transform.childCount];
            for ( int i = 0 ; i < transform.childCount ; i++ )
            {
                v3s [i] = transform.GetChild (i).position;
            }
            return v3s;
        }

        /// <summary>
        /// 设置层级
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        public void SetLayer (GameObject obj , int layer)
        {
            obj.layer = layer;
            Transform [] arr = GetChildArray (obj.transform);
            for ( int i = 0 ; i < arr.Length ; i++ )
            {
                arr [i].gameObject.layer = layer;
            }
        }
    }
}