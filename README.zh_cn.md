# Insql

[![Build status](https://ci.appveyor.com/api/projects/status/92f8ydwwu5nile9q/branch/master?svg=true)](https://ci.appveyor.com/project/rainrcn/insql/branch/master)
![](https://img.shields.io/github/license/rainrcn/insql.svg?style=flat)

**Insql 是一个轻量级的.NET ORM 框架。对象映射基于 Dapper, Sql 配置灵感来自于 Mybatis。**

> 本项目因本人工作关系可能长时间内不会对此进行维护，建议用于借鉴或二次开发。

Insql 提倡以写原生 SQL 的方式来访问数据库，整体功能分为三块：

- 统一管理 SQL 语句，使用 XML 作为 SQL 语句的载体，将原本需要在程序中硬编码的 SQL 语句外置并统一管理。提供可以从多种来源加载 SQL 语句以及跨多种数据库匹配 SQL 的功能。
- 提供丰富的映射机制，使用Annotation方式，Fluent方式，以及XML Map方式来实现数据库表到对象属性的映射。
- 灵活的依赖注入与领域驱动模式的使用方式，可以更好的管理数据库连接以及数据库上下文的生命周期。

[详细说明文档](https://rainrcn.github.io/insql)

## Packages

| Package                                                              | Nuget Stable                                                                                                                            | Downloads                                                                                                                                |
| -------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| [Insql](https://www.nuget.org/packages/Insql/)                       | [![Insql](https://img.shields.io/nuget/v/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  | [![Insql](https://img.shields.io/nuget/dt/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  |
| [Insql.MySql](https://www.nuget.org/packages/Insql.MySql/)           | [![Insql.MySql](https://img.shields.io/nuget/v/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                | [![Insql.MySql](https://img.shields.io/nuget/dt/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                |
| [Insql.Oracle](https://www.nuget.org/packages/Insql.Oracle/)         | [![Insql.Oracle](https://img.shields.io/nuget/v/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             | [![Insql.Oracle](https://img.shields.io/nuget/dt/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             |
| [Insql.PostgreSql](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/v/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/dt/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) |
| [Insql.Sqlite](https://www.nuget.org/packages/Insql.Sqlite/)         | [![Insql.Sqlite](https://img.shields.io/nuget/v/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             | [![Insql.Sqlite](https://img.shields.io/nuget/dt/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             |
