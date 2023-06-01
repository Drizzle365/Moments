<div align="center">
<img src="./wwwroot/character.png" alt="Logo" style="width:96px">

# Moments
构建博友们的朋友圈 —— 连接你我，创造价值

![](https://img.shields.io/badge/Blazor-Server-purple?style=for-the-badge&logo=blazor)
![](https://img.shields.io/badge/BootStrap-5-blue?style=for-the-badge&logo=bootstrap)
![](https://img.shields.io/badge/FreeSql-3-green?style=for-the-badge)
![](https://img.shields.io/badge/Flurl-3-yellow?style=for-the-badge)
</div>

Moments 为你提供了一个全新的方式来与你关注的博客作者和读者互动，让你的博客体验更加丰富和充实。

我们核心目标是通过整合各种订阅源，如 RSS 和
Atom，将你感兴趣的博客转化为一个个人朋友圈。你可以订阅来自世界各地的博客，并实时获取他们的最新动态。无论是热门博客、专业博主，还是你最喜爱的作者，你都可以通过
Moments 获得他们的最新博文、评论和互动。


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

## 特点和优势：

* 个性化订阅：通过使用 `Moments`，你可以根据自己的兴趣订阅并关注任何博客，无论是国内外的知名博主还是小众领域的专家。你将不再错过他们的精彩内容，同时也可以发现新的博客和创作者。
* 实时动态：`Moments` 实时获取订阅的博客的最新动态，包括新博文、更新通知和互动评论。你可以快速了解博客圈内的热门话题，与作者和其他读者进行互动和讨论。
* 独立博客聚合：`Moments` 提供了将多个独立博客聚合在一个平台的功能。这意味着你可以在一个地方展示你的多个博客内容，让读者更方便地获取你的全部创作。
* 个性化用户体验：`Moments` 提供了个性化的用户界面和设置选项，让你根据自己的喜好和需求定制博客社交的体验。你可以自由选择显示方式、排序规则和通知设置，以最符合你的个人偏好。
* 开源项目：`Moments` 是一个开源项目，意味着你可以参与项目的开发和改进。你可以自由定制代码，添加新功能，为社区的发展做出贡献。

## 快速开始

### 1.原生部署

下载源代码编译后运行即可，建议使用`supervisord`进行进程守护，或者使用`Systemd`启动，这需要一定的Linux经验，后面会补充相关的原生部署文档

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
