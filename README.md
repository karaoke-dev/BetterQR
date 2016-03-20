# BetterQR

Better QR Code

#0.1
Upload basic code

#「中文说明」
#命名空间规则：
QR -> 根空间
QR.Drawing -> 绘图空间，二维码生成部分的内容全部放在这里。
QR.Drawing.Data -> 绘图信息空间，专门放置二维码的图像的格点信息数据，并不负责其他信息。
QR.Drawing.Open -> 有关使用OpenGL的类，负责绘制二维码的屏幕显示，需要OpenTK环境支持
QR.Drawing.Graphic -> 负责二维码的绘制逻辑。
QR.Util -> 工具类空间，放置辅助工具类，不负责有关二维码生成的逻辑功能。

#主程序
##namespace QR.Drawing
###Program.cs
xx.display()用来显示，图片在屏幕上，需要OpenTK环境支持，否则报错。
xx.save("path")用来将图片保存在path路径上，path需要以.png .jpg .bmp等图像格式结尾。

#类文件说明
##namespace QR.Drawing.Data
###DataCell.cs
格点对象，储存每个格点的信息，包括颜色，位置及其他信息。
###DataMatrix.cs
整个二维码阵列的矩阵对象，矩阵中的每个元素都是一个DataCell，包含了对该矩阵的一些操作。
###CellExtraData.cs
备用存放额外的DataCell有关信息的类，目前并没有被使用。

##namespace QR.Drawing.Graphic
###Styler.cs
绘图样式基类，包含对DataMatrix的调用和图像生成的基本功能函数，供子集调用。可用来生成基本的二维码图形。所有其他带有Styler的类全都直接或间接继承自此类。
###BlockStyler.cs
生成描边样式的二维码绘图子类，继承自Styler.cs。
###BarStyler.cs
生成条状图案匹配的二维码绘图子类，继承自Styler.cs。

##namespace QR.Drawing.Util
###Default.cs
存放绘图中需要调用的默认值信息，信息全部为readonly。
###Grid.cs
用于存放DataCell中的行列信息，只被DataCell使用。
###Traverse.cs
用于遍历DataMatrix的工具类，实现一定的遍历功能，需要时可调用。
###Keys.cs
保存绘图过程中需要用到的key字符串信息，信息全部为readonly。
###Values.cs
保存绘图过程中需要用到的Value字符串信息，信息全部为readonly。

##namespace QR.Drawing.Open
###OpenDisplay.cs
在屏幕中显示生成的二维码图片的类，需要OpenTK环境支持。
###OpenTexture.cs
供OpenDisplay使用的二维贴图类，需要OpenTK环境支持。


