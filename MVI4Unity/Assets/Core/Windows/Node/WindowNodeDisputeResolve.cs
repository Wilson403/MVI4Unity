using System.Collections.Generic;
using UnityEngine;

namespace MVI4Unity
{
    public class WindowNodeDisputeResolver : SafeSingleton<WindowNodeDisputeResolver>
    {
        /// <summary>
        /// 解决视图节点冲突
        /// </summary>
        /// <param name="container"></param>
        /// <param name="state"></param>
        /// <param name="window"></param>
        /// <param name="currNodes"></param>
        /// <param name="newNodes"></param>
        public void ResolveDispute4List (Transform container , AStateBase state , IStore store , List<WindowNode> currNodes , List<WindowNode> newNodes)
        {
            currNodes.RemoveAll (x => x == null);
            newNodes.RemoveAll (x => x == null);

            //将当前节点列表转换为字典，方便索引
            Dictionary<int , WindowNode> id2WindowNode = PoolMgr.Ins.GetDict<int , WindowNode> ().Pop ();
            for ( int i = 0 ; i < currNodes.Count ; i++ )
            {
                WindowNode currNode = currNodes [i];
                currNode.ClearRecord ();
                //如果没有设置ID，就忽略转换过程
                if ( currNode.id == default )
                {
                    continue;
                }
                id2WindowNode [currNode.id] = currNode;
            }

            //先进行ID索引匹配
            for ( int i = 0 ; i < newNodes.Count ; i++ )
            {
                WindowNode newNode = newNodes [i];

                if ( newNode.id != default
                    && newNode.to == null
                    && id2WindowNode.TryGetValue (newNode.id , out WindowNode currNode)
                    && newNode.Equals (currNode) )
                {
                    newNode.to = currNode;
                    currNode.from = newNode;
                }
            }
            id2WindowNode.Push ();
            id2WindowNode = null;

            //根据类型进行匹配
            for ( int i = 0 ; i < newNodes.Count ; i++ )
            {
                WindowNode newNode = newNodes [i];
                if ( newNode.to != null )
                {
                    continue;
                }

                for ( int j = 0 ; j < currNodes.Count ; j++ )
                {
                    if ( currNodes [j].from != null )
                    {
                        continue;
                    }

                    if ( !newNode.Equals (currNodes [j]) )
                    {
                        continue;
                    }

                    newNode.to = currNodes [j];
                    currNodes [j].from = newNode;
                    break;
                }
            }

            //穷举当前的节点列表，如果存在未能匹配的节点，将这个节点删除（回收）
            for ( int i = 0 ; i < currNodes.Count ; )
            {
                WindowNode currNode = currNodes [i];
                if ( currNode.from == null )
                {
                    currNodes.RemoveAt (i);
                    RecoveryWindow (currNode);
                }
                else
                {
                    i++;
                }
            }

            for ( int i = 0 ; i < newNodes.Count ; i++ )
            {
                WindowNode newnode = newNodes [i];

                //匹配到当前的节点
                if ( newnode.to != null )
                {
                    Compare (newnode.to , newnode , state , store);
                }

                //没有匹配到，插入到当前列表
                else
                {
                    AWindow window = newnode.CreateAWindow (container);
                    currNodes.Add (newnode);
                    newnode.FillProps (window , state , store);
                    ResolveChildNodeDispute (newnode.CloneNodeShallow () , newnode , state , window , store);
                }
            }
        }

        void Compare (WindowNode curNode , WindowNode newNode , AStateBase state , IStore store)
        { 
            newNode.FillProps (curNode.Window , state , store);
            ResolveChildNodeDispute (curNode , newNode , state , curNode.Window , store);
        }

        /// <summary>
        /// 解决子节点冲突
        /// </summary>
        /// <param name="curNode"></param>
        /// <param name="newNode"></param>
        /// <param name="state"></param>
        /// <param name="window"></param>
        /// <param name="store"></param>
        void ResolveChildNodeDispute (WindowNode curNode , WindowNode newNode , AStateBase state , AWindow window , IStore store)
        {
            List<Transform> newNodeContainerList = newNode.GetContainerList (window);
            for ( int i = 0 ; i < newNodeContainerList.Count ; i++ )
            {
                Transform container = newNodeContainerList [i];

                while ( newNode.childNodeGroup.Count <= i )
                {
                    newNode.childNodeGroup.Add (PoolMgr.Ins.GetList<WindowNode> ().Pop ());
                }

                while ( curNode.childNodeGroup.Count <= i )
                {
                    curNode.childNodeGroup.Add (PoolMgr.Ins.GetList<WindowNode> ().Pop ());
                }

                ResolveDispute4List (container , state , store , curNode.childNodeGroup [i] , newNode.childNodeGroup [i]);
            }
            newNodeContainerList.Push ();
        }

        /// <summary>
        /// 回收窗口
        /// </summary>
        /// <param name="currentNode"></param>
        void RecoveryWindow (WindowNode currentNode)
        {
            List<Transform> containerList = currentNode.GetContainerList (currentNode.Window);
            for ( int containerIndex = 0 ; containerIndex < containerList.Count ; containerIndex++ )
            {
                List<WindowNode> childList = currentNode.childNodeGroup [containerIndex];
                for ( int childIndex = 0 ; childIndex < childList.Count ; childIndex++ )
                {
                    RecoveryWindow (childList [childIndex]);
                }
            }
            currentNode.Destory ();
            containerList.Push ();
        }
    }
}