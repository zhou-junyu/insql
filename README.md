# Insql

[![Build status](https://ci.appveyor.com/api/projects/status/92f8ydwwu5nile9q/branch/master?svg=true)](https://ci.appveyor.com/project/rainrcn/insql/branch/master)
![](https://img.shields.io/github/license/rainrcn/insql.svg?style=flat)

**Insql is a lightweight .NET ORM framework. Object mapping is based on Dapper, and the Sql configuration is inspired by Mybatis.**

**🚀 Pursuit of simplicity, elegance, performance and quality**

Insql Advocate to write native The SQL way to access the database, the overall function is divided into three:

- Unified management SQL statement, use XML as The vector of the SQL statement will be hard-coded in the program. SQL statements are externally and managed in a unified manner. Offer can be loaded from multiple sources SQL statements and matching across multiple databases The function of SQL .
- Provides a rich mapping mechanism, using Annotation , Fluent , and XML Map to implement mapping of database tables to object properties.
- Flexible dependency injection and the use of domain-driven patterns can better manage database connections and the lifecycle of database contexts.

[Detailed documentation](https://rainrcn.github.io/insql/#/en/)

## Packages

| Package                                                              | Nuget Stable                                                                                                                            | Downloads                                                                                                                                |
| -------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| [Insql](https://www.nuget.org/packages/Insql/)                       | [![Insql](https://img.shields.io/nuget/v/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  | [![Insql](https://img.shields.io/nuget/dt/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  |
| [Insql.MySql](https://www.nuget.org/packages/Insql.MySql/)           | [![Insql.MySql](https://img.shields.io/nuget/v/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                | [![Insql.MySql](https://img.shields.io/nuget/dt/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                |
| [Insql.Oracle](https://www.nuget.org/packages/Insql.Oracle/)         | [![Insql.Oracle](https://img.shields.io/nuget/v/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             | [![Insql.Oracle](https://img.shields.io/nuget/dt/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             |
| [Insql.PostgreSql](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/v/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/dt/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) |
| [Insql.Sqlite](https://www.nuget.org/packages/Insql.Sqlite/)         | [![Insql.Sqlite](https://img.shields.io/nuget/v/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             | [![Insql.Sqlite](https://img.shields.io/nuget/dt/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             |
