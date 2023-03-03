## 序言
MVI是纯响应式、函数式编程的架构，更加强调数据的单向流动和唯一数据源。这种架构思想多用于传统Web前端领域，当然它同样可以用于Unity的UI设计，毕竟都是数据的同步以及表现层的刷新，该方案就是该思想的Unity实现版本。不管你是使用UGUI，NGUI或者其它，MVI4Unity都可以适用


## 框架概述
框架大致可以划分为2部分，1是数据的同步管理，这部分就是MVI思想的体现。2是表现层的刷新，表现层是由一个个节点组成的UI树，每当数据更新，都会比较每一个节点来重组UI树以达到界面刷新的目的


## 框架目录
-- Core 框架核心逻辑
  * Pool : 框架使用的对象池工具，对于关闭的界面并不是直接销毁，而是回收待重新利用
  * StateManager : 状态管理，UI框架数据管理的部分
  * Windows : 表现层部分
  * Utils : 部分工具代码
  
-- Sample 演示代码
  * StartDemo.cs : 演示代码入口

## 如何管理数据
![image](https://user-images.githubusercontent.com/38308449/222396422-2055b233-e8a1-4e2a-8834-3e436f1ed7e7.png)
* State: 业务逻辑的状态，表现层就是获取最新的State来刷新界面的
* Reducer: 业务逻辑集合，就是一堆函数，采用函数式设计，不维护状态，只负责接收和返回新状态
* Store: 相当于Reducer与表现层交互的中间件，存储维护Reducer派发下来的State并通知表现层


## 如何管理界面（表现层）
![image](https://user-images.githubusercontent.com/38308449/222775679-ec4bca4b-ab88-4b30-93fa-22e3df8d79f5.png)
这里使用节点树来维护UI结构，View就是界面，在这里它作为一个根节点，包含了其他的子元素。ViewNode是节点单元，任何UI元素都是有它构成的
使用这种层次分明的结构，能轻易地使用同样的方式来处理“整体/部分”的关系，提供代码复用率，同时也方便框架后续维护扩展

## 关于框架稳定性
为了简化框架，除了核心逻辑部分其他功能都从简了，比如加载资源，框架是直接使用Resource加载的，如果正式使用可以替换接口

所在公司的卡牌养成游戏使用了该方案，已上线测试

## 框架教程
后续待补充，Sample只是一个简单案例，结合框架源码很快就能掌握

## 推荐项目
  - [JEngine](https://github.com/JasonXuDeveloper/JEngine) - The solution that allows unity games update in runtime. 使Unity开发的游戏支持热更新的解决方案。
  - [BDFramework](https://github.com/yimengfan/BDFramework.Core) - Simple and powerful Unity3d game workflow! 简单、高效、高度工业化的商业级unity3d 工作流。
