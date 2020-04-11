# MiniFramework
U3D客户端框架  
0.正在陆续完善MVC核心框架搭建（先不上传）  
1.支持下载、检测更新、支持断点续传  
2.UDPSocket\TCPSocket(包含心跳检测，解决沾包问题)  
3.http get、put和post封装  
4.添加工具：数字图片自动生成字体、压缩和解压、Protobuf和Json序列化、AB打包和资源对比  
5.Debugger工具，可视化debug信息，硬件信息，内存信息，优化大量debug信息造成的卡顿（大幅优化)  
6.接入ILRuntime,加入IMessage适配器，支持新协议，解决分析生成代码的bug（修改了protobuf源码不影响核心功能）,protobuf采用3.6.0(支持unity.net3.5以及IL2CPP)  