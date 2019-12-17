CGSS Unity Scripts
========

☆在Unity里捣鼓CGSS人物模型的时候可能用得到的脚本

JoinModel.cs
------------
CGSS人物模型的Head和Body是两个分开的部分 "Body hierarchy" 和 "Head hierarchy"，可以用这个脚本使Head和Body在播放动画时同步位置


LiveLookAt.cs
------------
视线追踪脚本，用于 Head hierarchy

原理是让'Eye_L'，'Eye_R'根据相机位置左右偏移

需要和 LookAtHandler.cs 配合使用

#### 食用方法： ####

* 使 'Eye_locator_L'，'Eye_locator_R' 作为 'Eye_L'，'Eye_R' 的父级，用来设定 'Eye_L'，'Eye_R' 的左右移动轨道

* 在 Head heiarchy 下新建一个 GameObject 'CamLookAt'，变换为零，用于反应 Main Camera 的 Head 本地变换

* 绑定 'CamLookAt'，'Eye_L'，'Eye_R'

* 变量的默认数值适用于十倍大小的模型，可以根据需要自行调整数值

'Max Out' =视线向外偏移限制，'Max In' =视线向内偏移限制，分别在74行和87行调整

另外两个 'eRelativeModDivisor'(72, 85行) 和 'QuaternionDifferenceWeight'(73, 86行) 是敏感度系数

LookAtHandler.cs
------------
视线追踪辅助脚本，用于 Head hierarchy

将 Main Camera 的世界变换反应到 Head 的本地变换

把 Main Camera 和 'CamLookAt' 绑上去就行了


LiveLookAt_v006.cs
------------
无效
