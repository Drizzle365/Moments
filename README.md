# <center> Moments

<div style="text-align: center;font-weight: bold">
构建博友们的朋友圈 —— 连接你我，创造价值
</div>

<div style="text-align: center;">

![](https://img.shields.io/badge/Blazor-Server-purple?style=for-the-badge&logo=blazor)
![](https://img.shields.io/badge/BootStrap-5-blue?style=for-the-badge&logo=bootstrap)
![](https://img.shields.io/badge/FreeSql-3-green?style=for-the-badge)
![](https://img.shields.io/badge/Flurl-3-yellow?style=for-the-badge)

</div>


---

* `如果你有好的想法希望可以共同参与下一版本的开发`
* `我们接受任何合理的 pr 和 issue `

## 功能概述

* Feed订阅（支持包括RSS，Atom）
* 支持友链,文章 API，方便博客对接
* 简洁的界面风格，仿照微信朋友圈更美观易用
* 支持友链检测自动验证
* 支持友链自助申请
* 单独查看某个好友的博文

## 快速开始

### 1.原生部署

下载源代码编译后运行即可，建议使用`supervisord`进行进程守护

### 2.Docker部署（⭐推荐）

```
docker run -d \
--name moments \
-p 3000:80 \
-v moments:/app \
drizzle2001/moments
```

运行后访问 `http://IP:3000` 即可  
建议使用反向代理绑定域名访问

图文教程（临时）: https://dearain.cn/archives/2069/   
完善的文档中心正在构建中

## 其他相关

### 如何绑定域名

程序运行后的默认端口是3000，可以使用 nginx 配置反向代理监听本地3000端口

### 如何配置

后台地址：` http://IP:3000/admin`  
默认密码：lantin （安装完成后应立即进入后台修改）

## 展示

![display.jpg](wwwroot/display.jpg)
