# Moments

> 打造专属于博友们的朋友圈

* `当前属于测试阶段，没有版本号，更新方向未定。`
* `我们接受任何合理的 pr 和 issue `

## 功能概述

* Feed订阅（支持包括RSS，Atom）
* 支持友链API，方便博客对接
* 简洁的界面风格，仿照微信朋友圈更美观易用

## 快速开始

### 1.原生部署

下载构建好的二进制文件直接运行即可，建议使用`supervisord`进行进程守护

### 2.Docker部署

```
docker run -d \
--name moments \
-p 3000:80 \
-v moments:/app \
drizzle2001/moments
```

一键运行即可

## 其他相关

### 如何绑定域名

程序运行后的默认端口是3000，可以使用 nginx 配置反向代理监听本地3000端口

### 如何配置

所有配置项目均在根目录下的 appsettings.json 文件中

```json
{
  "Title": "Moments",
  "Origin": "https://dearain.cn/",
  "Interval": 3600000,
  "Token": "lantin",
  "Name": "时雨",
  "Avatar": "https://q1.qlogo.cn/g?b=qq&nk=396823203&s=100",
  "Sentence": "天行健，君子以自强不息。"
}
```

## 一些图片

![display.jpg](wwwroot/display.jpg)
