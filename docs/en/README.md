# Insql documentation

[![Build status](https://ci.appveyor.com/api/projects/status/92f8ydwwu5nile9q/branch/master?svg=true)](https://ci.appveyor.com/project/rainrcn/insql/branch/master)
![](https://img.shields.io/github/license/rainrcn/insql.svg?style=flat)
[![GitHub stars](https://img.shields.io/github/stars/rainrcn/insql.svg?style=social)](https://github.com/rainrcn/insql)
[![star](https://gitee.com/rainrcn/insql/badge/star.svg?theme=white)](https://gitee.com/rainrcn/insql)

> A lightweight .NET ORM framework

## 1. Introduction

**Insql is a lightweight .NET ORM framework. Object mapping is based on Dapper, and the Sql configuration is inspired by Mybatis.**

**🚀 Pursuit of simplicity, elegance, performance and quality**

Insql Advocate to write native The SQL way to access the database, the overall function is divided into three:

- Unified management SQL statement, use XML as The vector of the SQL statement will be hard-coded in the program. SQL statements are externally and managed in a unified manner. Offer can be loaded from multiple sources SQL statements and matching across multiple databases The function of SQL .
- Provides a rich mapping mechanism, using Annotation , Fluent , and XML Map to implement mapping of database tables to object properties.
- Flexible dependency injection and the use of domain-driven patterns can better manage database connections and the lifecycle of database contexts.

## 2. Installation

| Package                                                              | Nuget Stable                                                                                                                            | Downloads                                                                                                                                |
| -------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| [Insql](https://www.nuget.org/packages/Insql/)                       | [![Insql](https://img.shields.io/nuget/v/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  | [![Insql](https://img.shields.io/nuget/dt/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  |
| [Insql.MySql](https://www.nuget.org/packages/Insql.MySql/)           | [![Insql.MySql](https://img.shields.io/nuget/v/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                | [![Insql.MySql](https://img.shields.io/nuget/dt/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                |
| [Insql.Oracle](https://www.nuget.org/packages/Insql.Oracle/)         | [![Insql.Oracle](https://img.shields.io/nuget/v/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             | [![Insql.Oracle](https://img.shields.io/nuget/dt/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             |
| [Insql.PostgreSql](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/v/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/dt/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) |
| [Insql.Sqlite](https://www.nuget.org/packages/Insql.Sqlite/)         | [![Insql.Sqlite](https://img.shields.io/nuget/v/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             | [![Insql.Sqlite](https://img.shields.io/nuget/dt/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             |

## 3. Features

- **Support DotNet Core 2.0+ & DotNet Framework 4.6.1+**
- **Support for dependency injection systems**
- **Similar to MyBatis sql xml configuration syntax**
- **Multiple database support**
- **high performance**
- **Flexible scalability**
- **Simple and intuitive to use**

## 4. Use

### 4.1 Using Insql

`Startup.cs`
```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql();  //Will use the default configuration
}
```

### 4.2 Sample Code

#### 4.2.1 Basic usage examples

`AuthDbContext.cs`
```csharp
public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public UserInfo GetUser(int userId)
    {
        //The first parameter corresponds to the select id , the second data parameter supports the PlainObject and IDictionary < string, object > type
        return this.Query<UserInfo>(nameof(GetUser), new { userId }).SingleOrDefault();
    }
}
```

`AuthDbContext.insql.xml`
```xml
<!--type corresponds to DbContext-->
<insql type="Insql.Tests.Domain.Contexts.AuthDbContext,Insql.Tests" >
  <select id="GetUser">
    select * from user_info where user_id = @userId
  </select>

  <select id="GetRoleList">
    select * from role_info order by sort_order
  </select>
</insql>
```
**_Note: When using the default settings The AuthDbContext.insql.xml file requires the right-click property to select the `embedded assembly file` method to be searched_**

`Controllers` or `Domain Services`

```csharp
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext dbContext;

    public UserService(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public UserInfo GetUser(string userId)
    {
        return this.dbContext.GetUser(userId);
    }

    [HttpGet]
    public UserInfo GetRoleList()
    {
        //Can be called directly dbContext
        return this.dbContext.Query<RoleInfo>("GetRoleList");
    }
}
```

`Startup.cs`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql();

    //Add AuthDbContext to the dependency injection container, the default life cycle is Scoped , a WEB request will create a DbContext object, a DbContext object will also contain a database connection
    services.AddDbContext<AuthDbContext>(options =>
    {
      //options.UseSqlServer(this.Configuration.GetConnectionString("sqlserver"));
      options.UseSqlite(this.Configuration.GetConnectionString("sqlite"));
    });
}
```

#### 4.2.2 Example of Common Context Usage

`CommonDbContext.cs`
```csharp
public class CommonDbContext<T> : DbContext where T : class
{
    public CommonDbContext(CommonDbContextOptions<T> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseResolver<T>();  //When resoving SQL , it will find those SQL statements corresponding to T type and insql type .

        optionsBuilder.UseSqlite("Data Source= ./insql.tests.db");  //specify the database connectionString used
    }
}

public class CommonDbContextOptions<T> : DbContextOptions<CommonDbContext<T>> where T : class
{
    public CommonDbContextOptions(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }
}
```

`UserInfo.insql.xml`
```xml
<!--Type and corresponding to a T CommonDbContext -->
<insql type="Insql.Tests.Models.UserInfo,Insql.Tests.Models" >
  <select id="GetUser">
    select * from user_info where user_id = @userId
  </select>
  <select id="GetUserList">
    select * from user_info where user_id in @userIdList
  </select>
</insql>
```

**_Note: When using the default settings The AuthDbContext.insql.xml file requires the right-click property to select the `embedded assembly file` method to be searched_**

`Controllers` or `Domain Services`

```csharp
[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly CommonDbContext<UserInfo> userDbContext;

    public UserService(CommonDbContext<UserInfo> userDbContext)
    {
        this.userDbContext = userDbContext;
    }

    [HttpGet]
    public UserInfo GetUser(string userId)
    {
        return this.userDbContext.Query<UserInfo>("GetUser",{ userId }).SingleOrDefault();
    }

    [HttpGet]
    public IEnumerable<UserInfo> GetUserList()
    {
        return this.userDbContext.Query<UserInfo>("GetUserList",{ userIdList = new string[] {'tome','jerry'} });
    }
}
```

`Startup.cs`

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql();

    //Add CommonDbContext to the dependency injection container
    services.AddSingleton(typeof(CommonDbContextOptions<>));
    services.AddScoped(typeof(CommonDbContext<>));
}
```

#### 4.2.3 Use transaction

```csharp
public void InsertUserList(IEnumerable<UserInfo> infoList)
{
    try
    {
        this.dbContext.BeginTransaction();

        foreach (var item in infoList)
        {
            this.dbContext.InsertUserSelective(item);
        }

        this.dbContext.CommitTransaction();
    }
    catch
    {
        this.dbContext.RollbackTransaction();

        throw;
    }
}
```

Use the `DoWithTransaction` extension method to automatically start and commit the transaction, and automatically roll back when an exception is encountered. If it is currently in a transaction, this extension will not start and commit the transaction again.

```csharp
public void InsertUserList(IEnumerable<UserInfo> infoList)
{
    this.dbContext.DoWithTransaction(() =>
    {
        foreach (var item in infoList)
        {
            this.dbContext.InsertUserSelective(item);
        }
    });
}
```

Use the `DoWithOpen` extension method to automatically open the connection and close the connection. If the current connection has already been opened, the connection will not be opened and closed again.

```csharp
public void InsertUserList(IEnumerable<UserInfo> infoList)
{
    this.dbContext.DoWithOpen(() =>
    {
        foreach (var item in infoList)
        {
            this.dbContext.InsertUserSelective(item);
        }
    });
}
```

#### 4.2.4 SELECT IN

```csharp
  var sqlParam = new { userIdList = new string[]{ "tom","jerry" } };
```

**1.Use List parameter conversion function provided by Dapper**

```xml
<select id="GetUserList">
  select * from user_info
  where user_id in @userIdList
</select>
```
When Dapper executes, it will be the original The SQL statement is converted to the following SQL , and then executed :

```sql
select * from user_info where user_id in (@userIdList1,@userIdList2)
```

**~~2.Use Each (not recommended)~~**

```xml
<select id="GetUserList">
  select * from user_info
  where user_id in
  <each name="userIdList" open="(" separator="," close=")" prefix="@"  />
</select>
```

After Insql Resolve , it will be the original The SQL statement is converted to the following SQL:

```sql
select * from user_info where user_id in (@userIdList1,@userIdList2)
```


#### 4.2.5 RESOLVE SQL

Only use InsqlResolver to parse SQL statements

`Controller.cs`
```csharp
[Route("api/[controller]/[action]")]
[ApiController]
public class ResolveController : ControllerBase
{
    private readonly IInsqlResolver<UserInfo> userResolver;

    public UserService(IInsqlResolver<UserInfo> userResolver)
    {
        this.userResolver = userResolver;
    }

    [HttpGet]
    public string GetUserSql(string userId)
    {
        var resolveResult = sqlResolver.Resolve("GetUser", new { userId });
        //resolveResult = { Sql:"select....",Param:{"userId":"..."} }

        return resolveResult.Sql;
    }
}
```

#### 4.2.6 Query Params

1. Query parameter prefix symbols, according to the implementation of different databases client decision

```sql
select * from user_info where user_id = @userId   //SqlServer,Sqlite,Postgres,MySql Use @xx
select * from user_info where user_id = :userId   //Oracle Use :xx
```

2. Raw parameter

```sql
select * from user_info order by ${ orderBy}  //The parameters of the ${xx} package will output the raw value. Note that there is a security risk of SQL injection. Be sure to check whether the original value of the output contains malicious characters.
```


## 5. Extended usage

### 5.1 Extended CURD

`DbDialectExtensions.cs`
```csharp
public static partial class DbDialectExtensions
{
  public static string Quote(this IDbDialect dialect, string value)
  {
      return $"{dialect.OpenQuote}{value}{dialect.CloseQuote}";
  }
}
```

`DbContextExtensions.cs`
```csharp
public static partial class DbContextExtensions
{
  public static TEntity Select<TEntity>(this DbContext context, object keys) where TEntity : class
  {
      var map = context.Model.FindMap(typeof(TEntity));

      if (map == null)
      {
        throw new Exception($"insql entity type : {typeof(TEntity)} is not mapping!");
      }

      var wcols = map.Properties.Where(p => !p.IsIgnored && p.IsKey).ToList();

      var sql = $"SELECT * FROM {context.Dialect.Quote(map.Table)} " +
          $"WHERE {string.Join(" AND ", wcols.Select(col => $"{context.Dialect.Quote(col.ColumnName)} = {context.Dialect.ParameterPrefix}{col.PropertyInfo.Name}"))}";

      return context.Session.Connection.Query(sql, keys, context.Session.Transaction, true, context.Session.CommandTimeout).SingleOrDefault();
  }

  public static int Insert<TEntity>(this DbContext context, TEntity entity) where TEntity : class
  {
      var map = context.Model.FindMap(typeof(TEntity));

      if (map == null)
      {
        throw new Exception($"insql entity type : {typeof(TEntity)} is not mapping!");
      }

      var cols = map.Properties.Where(p => !(p.IsIgnored || p.IsIdentity || p.IsReadonly)).ToList();

      var sql = $"INSERT INTO {context.Dialect.Quote(map.Table)} " +
          $"({string.Join(",", cols.Select(col => context.Dialect.Quote(col.ColumnName)))}) " +
          $"VALUES ({string.Join(",", cols.Select(col => $"{context.Dialect.ParameterPrefix}{col.PropertyInfo.Name}"))})";

      return context.Session.Connection.Execute(sql, entity, context.Session.Transaction, context.Session.CommandTimeout);
  }

  public static int Update<TEntity>(this DbContext context, TEntity entity) where TEntity : class
  {
      var map = context.Model.FindMap(typeof(TEntity));

      if (map == null)
      {
        throw new Exception($"insql entity type : {typeof(TEntity)} is not mapping!");
      }

      var ucols = map.Properties.Where(p => !(p.IsIgnored || p.IsIdentity || p.IsKey || p.IsReadonly)).ToList();
      var wcols = map.Properties.Where(p => !p.IsIgnored && p.IsKey).ToList();

      var sql = $"UPDATE {context.Dialect.Quote(map.Table)} SET " +
          $"{string.Join(",", ucols.Select(col => $"{context.Dialect.Quote(col.ColumnName)} = {context.Dialect.ParameterPrefix}{col.PropertyInfo.Name}"))}" +
          $"WHERE {string.Join(" AND ", wcols.Select(col => $"{context.Dialect.Quote(col.ColumnName)} = {context.Dialect.ParameterPrefix}{col.PropertyInfo.Name}"))}";

      return context.Session.Connection.Execute(sql, entity, context.Session.Transaction, context.Session.CommandTimeout);
  }

  public static int Delete<TEntity>(this DbContext context, TEntity entity) where TEntity : class
  {
      return context.Delete<TEntity>((object)entity);
  }

  public static int Delete<TEntity>(this DbContext context, object entity) where TEntity : class
  {
      var map = context.Model.FindMap(typeof(TEntity));

      if (map == null)
      {
        throw new Exception($"insql entity type : {typeof(TEntity)} is not mapping!");
      }

      var wcols = map.Properties.Where(p => !p.IsIgnored && p.IsKey).ToList();

      var sql = $"DELETE FROM {context.Dialect.Quote(map.Table)} " +
          $"WHERE {string.Join(" AND ", wcols.Select(col => $"{context.Dialect.Quote(col.ColumnName)} = {context.Dialect.ParameterPrefix}{col.PropertyInfo.Name}"))}";

      return context.Session.Connection.Execute(sql, entity, context.Session.Transaction, context.Session.CommandTimeout);
  }
}
```
**_Note: The above extension methods are not included in the library and need to be extended by themselves and require entity mapping data support_**

## 6. Object mapping

Support Map, Annotation, Fluent three ways object attribute mapping

### 6.1 Xml

`UserInfo.insql.xml`
```xml
<insql type="Insql.Tests.Models.UserInfo,Insql.Tests.Models" >
  <map table="user_info" type="Insql.Tests.Models.UserInfo,Insql.Tests.Models">
   <key name="user_id" property="UserId" />
   <column name="user_name" property="UserName" />
   <column name="user_gender" property="UserGender" />
  </map>
</insql>
```
Set the XML MAP mapping, this mode will be enabled by default

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql(builder=>
    {
        builder.AddProvider(providerBuilder =>
        {
            //You can set the assembly you want to scan, if not set, scan all the assemblies in the current AppDomain by default
            //This setting works on both the sql and map configuration sections
            providerBuilder.AddEmbeddedXml(options =>
            {
                options.Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            });
        });

        builder.AddMapper(options =>
        {
            //options.ExcludeXmlMaps(); //can exclude the mapping of XmlMap
        });
    });
}
```

### 6.2 Fluent
`FluentModelInfoBuilder.cs`
```csharp
public class FluentModelInfoBuilder : InsqlEntityBuilder<FluentModelInfo>
{
    public FluentModelInfoBuilder()
    {
        this.Table("fluent_model_info");

        this.Property(o => o.Id).Column("id").Key().Identity();
        this.Property(o => o.Name).Column("name");
        this.Property(o => o.Size).Column("Size");
        this.Property(o => o.Extra).Ignore();
        this.Property(o => o.ReadOnlyExtra).Readonly();
    }
}
```
Set the Fluent mode mapping, it will not be enabled by default.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql(builder=>
    {
        builder.AddMapper(options =>
        {
            //Enable Fluent mode mapping scan, scan all the assemblies in the current AppDomain by default if no parameters are set
            //will scan and map all inheritance Entity type of InsqlEntityBuilder <T>
            options.IncludeFluentMaps();  
            //options.IncludeFluentMaps(assemblies..);
        });
    });
}
```

### 6.3 Annotation

`AnnotationModelInfo.cs`
```csharp
[Table("annotation_model_info")]
public class AnnotationModelInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    public int Size { get; set; }

    [NotMapped]
    public string Extra { get; set; }

    [Editable(AllowEdit=false)]
    public string ReadOnlyExtra { get; set; }
}
```

Set Annotation mode mapping, not enabled by default

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql(builder=>
    {
        builder.AddMapper(options =>
        {
            //Enable the mapping scan of the Annotation mode, if you do not set the parameters, the default scan all the assemblies in the current AppDomain
            //will scan and map all the bands [Table] attribute entity type
            options.IncludeAnnotationMaps();  
            //options.IncludeAnnotationMaps(assemblies..);
        });
    });
}
```

### 6.4 multiple mapping priorities
It is recommended to enable only one of the methods of entity mapping. However, if multiple mapping methods are enabled at the same time, the XmlMap -> FluentMap -> AnnotationMap will override the latter for the same entity type .

## 7. Configuration syntax

**xxx.insql.xml** configuration syntax is similar to the configuration syntax Mybatis currently supports the following configuration section :

- **map**
  - **key**
  - **column**
- **sql**
  - **include**
  - **bind**
  - **if**
  - **where**
  - **set**
  - **trim**
  - **~~each(not recommended)~~**
  - **ifNotNull(new)**
  - **ifNotEmpty(new)**
- **select** = **sql**
- **insert** = **sql**
- **update** = **sql**
- **delete** = **sql**

### 7.1 map

`map` the configuration section is used for mapping of database table fields to object properties so that they `DbContext.Query<UserInfo>`vwill be used as long as they are queried

```xml
<map type="Insql.Tests.Domain.Models.UserInfo,Insql.Tests">
  <key name="user_id" to="UserId" />
  <column name="user_name" to="UserName" />
  <column name="user_gender" to="UserGender" />
</map>
```

| Child element name | Attribute name | Property description | Description                      |
| ------------------ | -------------- | -------------------- | -------------------------------- |
| `key`              |                |                      | Indicates the primary key column |
|                    | `name*`        | Column name          |                                  |
|                    | `property*`    | Object property name |                                  |
|                    | `identity`     | Identity column      | identity="True,False"            |
| `column`           |                |                      | Represents a normal column       |
|                    | `name*`        | Column name          |                                  |
|                    | `property*`    | Object property name |                                  |
|                    | `readonly`     | Read-only attribute  | readonly="True,False"            |

### 7.2 sql

`sql` the configuration section is used to configure database execution statements.`select`,`insert`,`update`,`delete` with `sql` an `sql`alias that has the same functionality, just a configuration section.

```xml
<sql id="userColumns">
  user_id as UserId,user_name as UserName,user_gender as UserGender
</sql>

<select id="GetUser">
  select
  <include refid="userColumns" />
  from user_info
  where user_id = @userId
</select>
```

| Child element name          | Attribute name    | Property description                                                                                                     | Description                                                                                                                                                                                                                                 |
| --------------------------- | ----------------- | ------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `include`                   |                   |                                                                                                                          | Import other `sql` configuration sections                                                                                                                                                                                                   |
|                             | `refid*`          | Configuration section to import `id`                                                                                     |                                                                                                                                                                                                                                             |
| `bind`                      |                   |                                                                                                                          | Create a new query parameter to the current parameter list, such as like fuzzy query scene                                                                                                                                                  |
|                             | `name*`           | New parameter name created                                                                                               |                                                                                                                                                                                                                                             |
|                             | `value*`          | Dynamic script expression, for example: '%'+userName+'%'                                                                 |                                                                                                                                                                                                                                             |
|                             | `valueType`       | Specifies `value` the type returned, in the format System.TypeCode enumeration,value types are best specified explicitly |                                                                                                                                                                                                                                             |
| `if`                        |                   |                                                                                                                          | Determine the dynamic expression, if it is satisfied, output the internal content                                                                                                                                                           |
|                             | `test*`           | Dynamic expression, you need to return a bool type, for example: userName != null                                        |                                                                                                                                                                                                                                             |
| `where`                     |                   |                                                                                                                          | Add the `where` sql segment at the current position , whether the output `where` depends on whether its internal child elements have valid content output, and will overwrite the beginning `and`,`or`                                      |
| `set`                       |                   |                                                                                                                          | Add the `set` sql segment at the current position , mainly used in the `update` configuration section, whether the output `set` depends on whether its internal child elements have valid content output, and will overwrite the ending `,` |
| `trim`                      |                   |                                                                                                                          | Trimming the output of the wrapped element, wrapping the child elements with the specified prefix character and suffix character                                                                                                            |
|                             | `prefix`          | Package prefix character                                                                                                 |                                                                                                                                                                                                                                             |
|                             | `suffix`          | Package suffix character                                                                                                 |                                                                                                                                                                                                                                             |
|                             | `prefixOverrides` | Will overwrite the specified character at the beginning of the internal output                                           |                                                                                                                                                                                                                                             |
|                             | `suffixOverrides` | Will override the specified character at the end of the internal output                                                  |                                                                                                                                                                                                                                             |
| ~~`each(not recommended)`~~ |                   |                                                                                                                          | Loop array type of query parameter for each value                                                                                                                                                                                           |
|                             | `name*`           | Loop array parameter name                                                                                                |                                                                                                                                                                                                                                             |
|                             | `separator`       | Separator between each value                                                                                             |                                                                                                                                                                                                                                             |
|                             | `open`            | The left side of the package                                                                                             |                                                                                                                                                                                                                                             |
|                             | `close`           | The right side of the package                                                                                            |                                                                                                                                                                                                                                             |
|                             | `prefix`          | Each value name prefix                                                                                                   |                                                                                                                                                                                                                                             |
|                             | `suffix`          | Suffix for each value name                                                                                               |                                                                                                                                                                                                                                             |
| `ifNotNull(new)`            |                   |                                                                                                                          | If the query parameter value exists and is not null                                                                                                                                                                                         |
|                             | `name*`           | Query parameter name                                                                                                     |
| `ifNotEmpty(new)`           |                   |                                                                                                                          | If the query parameter string value exists and is not an empty string                                                                                                                                                                       |
|                             | `name*`           |
| Query parameter name        |


`include`,`where`,`if`,`bind`

```xml
<select id="GetUserList">
    <include refid="selectUserColumns" />
    <where>
        <if test="userName != null">
          <bind name="likeUserName" value="'%' + userName + '%'" />
          user_name like @likeUserName
        </if>
        and user_gender = @userGender
    </where>
    order by  user_id
  </select>
```

`set`

```xml
<update id="UpdateUserSelective">
  update user_info
  <set>
    <if test="UserName != null">
      user_name=@UserName,
    </if>
    user_gender=@UserGender
  </set>
  where user_id = @UserId
</update>
```

`trim`

```xml
<insert id="InsertUserSelective">
    insert into user
    <trim prefix="(" suffix=")" suffixOverrides=",">
      user_id,
      <if test="UserName != null">
        user_name,
      </if>
      <if test="UserGender != null">
        user_gender,
      </if>
      create_time,
      <if test="LastLoginTime != null">
        last_login_time,
      </if>
    </trim>
    <trim prefix="values (" suffix=")" suffixOverrides=",">
      @UserId,
      <if test="UserName != null">
        @UserName,
      </if>
      <if test="UserGender != null">
        @UserGender,
      </if>
      @CreateTime,
      <if test="LastLoginTime != null">
        @LastLoginTime,
      </if>
    </trim>
  </insert>
```

~~`each(not recommended)`~~

```xml
<select id="EachIn">
  select * from user_info
  where user_id in
  <each name="userIdList" open="(" separator="," close=")" prefix="@"  />
</select>
```

After SqlResolver parsing :

```sql
select * from user_info where user_id in (@userIdList1,@userIdList2)
```

**_Note: After parsing will delete the original `userIdList`parameters, and increase `userIdList1`,`userIdList2` the parameters_**

_Tip: Dapper's own parameter list conversion function can also be used on select in list._

## 8. Dynamic script

The dynamic script syntax is JAVASCRIPT. Support for common object properties of ECMAScript 6.

```xml
<if test="userGender !=null and userGender == 'W' ">
  and user_gender = @userGender
</if>
```

`userGender !=null and userGender == 'W'` Part of it is a dynamic script.

### 8.1 operator conversion

Because `&`, `<` these have special meaning in XML, so support for these symbols in the dynamic conversion script. The following symbol conversions are currently supported:

| Before conversion | After conversion |
| ----------------- | ---------------- |
| `and`             | `&&`             |
| `or`              | `\|\|`           |
| `gt`              | `>`              |
| `gte`             | `>=`             |
| `lt`              | `<`              |
| `lte`             | `<=`             |
| `eq`              | `==`             |
| `neq`             | `!=`             |

_The operator conversion function can be disabled or the conversion of some of the operators can be excluded._

**_Note: Please avoid the same query parameters as the above operator name. If it is unavoidable, you can set to exclude conflicting operators. Then implement the operator with the xml transfer symbol_**

### 8.2 Enumeration converted to a string

`userGender == 'W'` `userGender` the attribute is an enumerated type, which is converted to a character-for-format by default in dynamic scripts. This conversion can be disabled, and the enum will be converted to a `number` type after disabling .

### 8.3 Time type conversion

If the query parameter contains a time type `DateTime`, it will be converted to the `Date` type in JS, because the minimum time of Date is 1970.1.1, so if there is an unassigned DateTime (0001.1.1) in the query object, or less than 1970 The DateTime of the time will be converted to 1970.1.1 by default, and the conversion only happens when the dynamic script is run, and does not affect the original value of the query parameters. If there is an unassigned `DateTime?` type in the parameter object, it will be null itself and will not be converted.

### 8.4 Setting up dynamic scripts

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql(builder =>
    {
        builder.AddResolver(configure =>
        {
            configure.AddScripter(options =>
            {
                options.IsConvertEnum = false;  //Do not convert enumerations to strings
                options.IsConvertDateTimeMin = false; //Do not convert the minimum time
                options.IsConvertOperator = false;  //Do not convert operators
                options.ExcludeOperators = new string[] { "eq","neq" }; //exclude operator conversion
            });
        });
    });
}
```

## 9. Multiple database matching

```xml
<!--By default, the example uses the Sqlite database-->
<insert id="InsertUser">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select last_insert_rowid() from user_info;
</insert>
<!--MySql-->
<insert id="InsertUser.MySql">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select LAST_INSERT_ID();
</insert>
<!--SqlServer-->
<insert id="InsertUser.SqlServer">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select SCOPE_IDENTITY();
</insert>
```

### 9.1 Matching rule

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<UserDbContext>(options =>
    {
      //Which sql id to match, which database to use
      options.UseSqlServer(this.Configuration.GetConnectionString("sqlserver"));
      //options.UseSqlite(this.Configuration.GetConnectionString("sqlite"));
    });
}
```

**_If you are currently using SqlServer, the suffix with `.SqlServer` will be matched first. Matches the default statement without a suffix if it is not found._**

**_Currently supports matching database suffixes: `SqlServer` `Sqlite` `MySql` `Oracle` `PostgreSql`_**

### 9.2 Extended database support

Support for other databases is no limit, as long as required to support the database that supports the .NET client library, support is very easy. Need to implement `IDbDialect` `IDbSessionFactory` interface.

## 10. Multiple configuration sources

### 10.1 Embedding assembly file mode source

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql(builder =>
    {
        builder.AddProvider(providerBuilder =>
        {
            providerBuilder.AddEmbeddedXml(options =>
            {
                options.Enabled = false;    //This source feature can be disabled, the default is enabled
                //options.Matches = "**/*.insql.xml"; //glob file filter expression, this is the default value
                //options.Assemblies = AppDomain.CurrentDomain.GetAssemblies(); //Specify the assembly to be scanned, defaulting to the assembly in the current AppDomain .
            });
        });
    });
}
```

### 10.2 External file directory mode source

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql(builder =>
    {
        builder.AddProvider(providerBuilder =>
        {
            providerBuilder.AddExternalXml(options=>
            {
                options.Enabled = true; //This source can be started , the default is disabled
                options.Directory = "D:\\Insqls";   //Configure the load directory, support recursive search, subfolders will also scan, the default is the current program execution directory
                //options.Matches = "**/*.insql.xml"; //glob file filter expression, this is the default value
            });
        });
    });
}
```

### 10.3 Multi-configuration source merge function

`EmbeddedXml` and the `ExternalXml` mode can be enabled at the same time. For the same file with insql type, the latter will overwrite the same statement configuration with the former sqlId and the same mapping configuration with map type.

## 11. Extended function

### 11.1 Statement Parsing Filter

Create a statement-resolved logging filter that fires when the IInsqlResolver.Resolve method is called

```csharp
public class LogResolveFilter : IInsqlResolveFilter
{
  private readonly ILogger<LogResolveFilter> logger;

  public LogResolveFilter(ILogger<LogResolveFilter> logger)
  {
      this.logger = logger;
  }

  public void OnResolved(ResolveContext resolveContext, ResolveResult resolveResult)
  {
      this.logger.LogInformation($"insql resolved id : {resolveContext.InsqlSection.Id} , sql : {resolveResult.Sql}");
  }

  public void OnResolving(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam)
  {
  }
}
```

`OnResolving` execute before `OnResolved` parsing, execute after parsing

**Enable filters:**

```csharp
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder =>
  {
      builder.AddResolver(configure =>
      {
          configure.AddFilter<LogResolveFilter>();
      });
  });
}
```

## 12. Tools

### 12.1 Code Generator

The `tools` CodeSmith generator file is included in the source directory, and you can run these files directly after installing CodeSmith.

![code_generator](code_generator.zh_cn.png)

**Generate code example: show only one data table**


`AuthRoleInfo.cs`

```csharp
namespace Insql.Domain.Models
{
  /// <summary>
  /// auth_role_info
  /// </summary>
	public class AuthRoleInfo
	{   
      /// <summary>
      /// role_code
      /// </summary>
      public string RoleCode { get; set; }

      /// <summary>
      /// role_name
      /// </summary>
      public string RoleName { get; set; }

      /// <summary>
      /// role_description
      /// </summary>
      public string RoleDescription { get; set; }

      /// <summary>
      /// sort_order
      /// </summary>
      public int SortOrder { get; set; }

      /// <summary>
      /// is_default
      /// </summary>
      public bool IsDefault { get; set; }
	}
}
```

`AuthRoleInfo.insql.xml`
```xml
<insql type="Insql.Domain.Models.AuthRoleInfo,Insql.Domain">
  <map table="auth_role_info" type="Insql.Domain.Models.AuthRoleInfo,Insql.Domain">
   <key name="role_code" property="RoleCode" />
   <column name="role_name" property="RoleName" />
   <column name="role_description" property="RoleDescription" />
   <column name="sort_order" property="SortOrder" />
   <column name="is_default" property="IsDefault" />
  </map>
</insql>
```

`AuthDbContext.cs`

```csharp
namespace Insql.Domain.Contexts
{
  public class AuthDbContext : DbContext
  {
      public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
      {
      }
      
      #region AuthRoleInfo

      public void InsertRole(AuthRoleInfo info)
      {
          this.Execute(nameof(InsertRole),info);
      }
      
      public void UpdateRole(AuthRoleInfo info)
      {
          this.Execute(nameof(UpdateRole),info);
      }
      
      public void DeleteRole(string RoleCode)
      {
          this.Execute(nameof(DeleteRole),new { RoleCode });
      }
      
      public AuthRoleInfo SelectRole(string RoleCode)
      {
          return this.Query<AuthRoleInfo>(nameof(SelectRole),new { RoleCode }).SingleOrDefault();
      }
      
      #endregion
  }
}
```

`AuthDbContext.insql.xml`
```xml
<insql type="Insql.Domain.Contexts.AuthDbContext,Insql.Domain">
  
  <!-- 
    auth_role_info
  -->
  
  <insert id="InsertRole">
    INSERT INTO auth_role_info 
    (role_code,role_name,role_description,sort_order,is_default) 
    VALUES (@RoleCode,@RoleName,@RoleDescription,@SortOrder,@IsDefault)
  </insert>
  
  <update id="UpdateRole">
    UPDATE auth_role_info SET
    role_name = @RoleName,
    role_description = @RoleDescription,
    sort_order = @SortOrder,
    is_default = @IsDefault
    WHERE role_code = @RoleCode
  </update>
  
  <delete id="DeleteRole">
    DELETE FROM auth_role_info WHERE role_code = @RoleCode
  </delete>
  
  <select id="SelectRole">
    SELECT * FROM auth_role_info WHERE role_code = @RoleCode
  </select>
</insql>
```

## 13. Performance

To ask about the performance, there is no need to say more, OK will be done. :) just kidding. Because of the Dapper used for object mapping, there is no need to worry about performance. Basically, it is consistent with Dapper and has little fluctuation. A performance test may be written later.

## 14. Update
- 2.1.0

  - Support feature Attribute and Fluent mode database table and object mapping mode
  - Support for adding objects CURD extension
- 1.8.2

  - Rewrite and beautify the documentation
  - Optimize dynamic script execution engine to reduce resource allocation and improve performance
  - Optimize the code generator to solve some bugs in generated code

- 1.5.0
  - Supports map configuration blocks for mapping database table fields to class attribute fields. Make mapping when querying objects easier, without the need for an alias.
  - Supports SQL configuration file directory source, can load SQL configuration from specified file directory, and supports merge with embedded SQL configuration
  - Optimize dynamic script parsing for conversion of DateTime.Min