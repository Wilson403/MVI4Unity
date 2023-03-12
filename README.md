### 序言
MVI是纯响应式、函数式编程的架构，更加强调数据的单向流动和唯一数据源。这种架构思想多用于传统Web前端领域，当然它同样可以用于Unity的UI设计，毕竟都是数据的同步以及表现层的刷新，该方案就是该思想的Unity实现版本。不管你是使用UGUI，NGUI或者其它，MVI4Unity都可以适用



### 框架概述

*框架大致可以划分为2部分，1是数据的同步管理，这部分就是MVI思想的体现。2是表现层的刷新，表现层是由一个个节点组成的UI树，每当数据更新，都会比较每一个节点来重组UI树以达到界面刷新的目的*



### 框架目录

-- Core 框架核心逻辑
  * Pool : 框架使用的对象池工具，对于关闭的界面并不是直接销毁，而是回收待重新利用
  * StateManager : 状态管理，UI框架数据管理的部分
  * Windows : 表现层部分
  * Utils : 部分工具代码

-- Sample 演示代码

  * StartDemo.cs : 演示代码入口
  * Window01.cs是一个基本界面的演示，注释比较全



### 如何管理数据

![image](https://user-images.githubusercontent.com/38308449/222396422-2055b233-e8a1-4e2a-8834-3e436f1ed7e7.png)
* State: 业务逻辑的状态，表现层就是获取最新的State来刷新界面的
* Reducer: 业务逻辑集合，就是一堆函数，采用函数式设计，不维护状态，只负责接收和返回新状态
* Store: 相当于Reducer与表现层交互的中间件，存储维护Reducer派发下来的State并通知表现层



### 如何管理界面（表现层）

![image](https://user-images.githubusercontent.com/38308449/222775679-ec4bca4b-ab88-4b30-93fa-22e3df8d79f5.png)
这里使用节点树来维护UI结构，View就是界面，在这里它作为一个根节点，包含了其他的子元素。ViewNode是节点单元，任何UI元素都是有它构成的
使用这种层次分明的结构，能轻易地使用同样的方式来处理“整体/部分”的关系，提供代码复用率，同时也方便框架后续维护扩展



### 如何运行游戏演示

* 游戏的演示场景是Start，双击运行游戏即可



### 如何使用该框架

* 先从数据方面开始说起，UI需要数据来刷新样式，数据这块代码要如何写呢。以Window01.cs作为例子，需要一个Stata类作为状态数据，一个Reduce来更新数据。Reucer与View之间的交互由Store作为桥梁来完成，通过Store.Dispatch来通知状态更新从而刷新界面。

  ```c#
  //状态类
  public class State01 : AStateBase
  {
      public int count;
  }
  
  //用于刷新状态的函数集合
  public class Reducer01 : Reducer<State01 , Reducer01.Reducer01MethodType>
  {
  
  }
  ```

  

* Reduce函数定义需要规范，必须符合如下委托签名的任意一种

  ```c#
          /// <summary>
          /// 同步委托
          /// </summary>
          /// <param name="lastState"></param>
          /// <param name="param"></param>
          /// <returns></returns>
          public delegate S Reducer (S lastState , object @param);
  
          /// <summary>
          /// 异步委托
          /// </summary>
          /// <param name="lastState"></param>
          /// <param name="param"></param>
          /// <returns></returns>
          public delegate Task<S> AsyncReducer (S lastState , object @param);
  
          /// <summary>
          /// 回调委托
          /// </summary>
          /// <param name="lastState"></param>
          /// <param name="param"></param>
          /// <param name="setNewState"></param>
          public delegate void CallbackReducer (S lastState , object @param , Action<S> setNewState);
  ```

  

* 接着来说表现层的代码组织，同样使用Window01.cs来作为例子。继承于Awindow的就是界面预制体组件管理类，提供按钮，文本之类的字段访问，以及一些基本的销毁，激活接口。***它的最大作用就是提供可访问的组件字段，其它的事情不用管，你可以这么理解***

  ```c#
  public class Window01 : AWindow
  {
      [AWindowCom ("container1")]
      public Transform container1;
  
      [AWindowCom ("container2")]
      public Transform container2;
  
      [AWindowCom ("btn")]
      public Button btn;
  
      [AWindowCom ("btn2")]
      public Button btn2;
  
      [AWindowCom ("btnClose")]
      public Button btnClose;
  }
  ```

  

* 然后就是关键一步了，创建对应界面的WindowNodeType节点对象。主界面的就是根节点，主界面下的UI单元就是子节点。我们拿背包来举一个例子，背包主界面是一个根节点，背包里的物品是属于子节点，背包物品也可以有自己的子节点，可以这样套娃写下去。通过代码来构建这样的UI树，当状态刷新时，整棵树都会一起刷新，当然你也可以通过state.currentTag来获取引起刷新的事件来差异化处理刷新

  ```c#
  		/// <summary>
          /// 这是Windown01的界面，同时也是根节点
          /// </summary>
          public static WindowNodeType<Window01 , State01> root = new WindowNodeType<Window01 , State01> ("Windown01" ,
              containerCreator: (window) =>
              {
                  //添加2个Window01里的容器container1，container2
                  List<Transform> containerList = PoolMgr.Ins.GetList<Transform> ().Pop ();
                  containerList.Add (window.container1);
                  containerList.Add (window.container2);
                  return containerList;
              } ,
              childNodeCreator: (state) =>
              {
                  List<List<WindowNode>> childNodeGroup = PoolMgr.Ins.GetList<List<WindowNode>> ().Pop ();
                  List<WindowNode> childNodeList1 = PoolMgr.Ins.GetList<WindowNode> ().Pop ();
                  List<WindowNode> childNodeList2 = PoolMgr.Ins.GetList<WindowNode> ().Pop ();
  
                  //给这2个容器添加子节点
                  for ( int i = 0 ; i < state.count ; i++ )
                  {
                      childNodeList1.Add (item.CreateWindowNode (state));
                      childNodeList2.Add (item.CreateWindowNode (state));
                  }
  
                  childNodeGroup.Add (childNodeList1);
                  childNodeGroup.Add (childNodeList2);
  
                  return childNodeGroup;
              } ,
              //状态变更时执行
              fillProps: (state , window , store , prop) =>
              {
                  if ( state.currentFunTag == ( int ) Reducer01.Reducer01MethodType.Func01 ) 
                  {
                      //由Func01引起的变化
                  }
              });
  
   		/// <summary>
          /// 这是WindowItem的节点
          /// </summary>
          public static WindowNodeType<WindowItem , State01> item = new WindowNodeType<WindowItem , State01> ("WindownItem" ,
              fillProps: (state , window , store , prop) =>
              {
                  
              });
  ```

  

* 结合Sample案例，你会理解得更快

  

### 关于框架稳定性

* 为了简化框架，除了核心逻辑部分其他功能都从简了，比如加载资源，框架是直接使用Resource加载的，如果正式使用可以替换接口
* 会**长期维护**，发现问题可以提Issues，第一时间解决
* 所在公司的卡牌养成游戏使用了该方案，**已上线测试**



### 推荐项目
  - [JEngine](https://github.com/JasonXuDeveloper/JEngine) - The solution that allows unity games update in runtime. 使Unity开发的游戏支持热更新的解决方案。
  - [BDFramework](https://github.com/yimengfan/BDFramework.Core) - Simple and powerful Unity3d game workflow! 简单、高效、高度工业化的商业级unity3d 工作流。本项目借鉴了其状态管理部分
  - [Html2UnityRich](https://github.com/Wilson403/Html2UnityRich) - 能够将Html标签转化为Unity支持的富文本标签的库（UGUI or TextPro）
