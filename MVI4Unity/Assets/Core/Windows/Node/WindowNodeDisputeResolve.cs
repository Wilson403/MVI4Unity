﻿using System.Collections.Generic;

namespace MVI4Unity
{
    public class WindowNodeDisputeResolver : SafeSingleton<WindowNodeDisputeResolver>
    {
        /// <summary>
        /// 解决视图节点冲突
        /// </summary>
        /// <param name="currNodes"></param>
        /// <param name="newNodes"></param>
        public void ResolveDispute4List (List<WindowNode> currNodes , List<WindowNode> newNodes)
        {
            currNodes.RemoveAll (x => x== null);
            newNodes.RemoveAll (x => x==null);

            //将当前节点列表转换为字典，方便索引
            Dictionary<int , WindowNode> id2WindowNode = PoolMgr.Ins.GetDict<int , WindowNode> ().Pop ();
            for ( int i = 0 ; i < currNodes.Count ; i++ )
            {
                WindowNode currNode = currNodes [i];
                currNode.ClearRecord ();
                if ( currNode == null )
                {
                    continue;
                }
                id2WindowNode [currNode.id] = currNode;
            }

            //先进行ID索引匹配
            for ( int i = 0 ; i < newNodes.Count ; i++ )
            {
                WindowNode newNode = newNodes [i];
                if ( newNode.to == null && id2WindowNode.TryGetValue (newNode.id , out WindowNode currNode) && newNode.Equals (currNode) )
                {
                    newNode.to = currNode;
                    currNode.from = newNode;
                }
            }
            id2WindowNode.Push ();

            //根据类型进行匹配
            for ( int i = 0 ; i < newNodes.Count ; i++ )
            {
                WindowNode newNode = newNodes [i];
                for ( int j = 0 ; j < currNodes.Count ; j++ )
                {
                    WindowNode currNode = currNodes [j];
                    if ( newNode.to == null && newNode.Equals (currNode) ) 
                    {
                        newNode.to = currNode;
                        currNode.from = newNode;
                    }
                }
            }

            //删除currNodes没有匹配上的元素

            for ( int i = 0 ; i < newNodes.Count ; i++ )
            {
                WindowNode newnode = newNodes [i];

                //匹配到当前的节点
                if ( newnode.to != null )
                {

                }
                //没有匹配到，插入到当前列表
                else 
                {
                    newnode.windowNodeType.CreateAWindow ();
                }
            }
        }

        private void Insert () 
        {

        }
    }
}