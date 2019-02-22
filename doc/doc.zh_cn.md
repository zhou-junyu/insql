# Insql 说明文档

## 介绍

**Insql是一个轻量级的.NET ORM 类库。对象映射基于 Dapper, Sql 配置灵感来自于 Mybatis。**

TA的追求是：简洁、优雅、性能与质量
TA的宗旨是：让你用起来感觉到自由、直观与舒爽。让你有一种想飞的感觉🚀

[中文](https://github.com/rainrcn/insql/blob/master/README.zh_cn.md) | [English](https://github.com/rainrcn/insql/blob/master/README.md) 

QQ交流群：737771272

## 安装

| Package                                                              | Nuget Stable                                                                                                                            | Downloads                                                                                                                                |
| -------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| [Insql](https://www.nuget.org/packages/Insql)                        | [![Insql](https://img.shields.io/nuget/v/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  | [![Insql](https://img.shields.io/nuget/dt/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)                                  |
| [Insql.MySql](https://www.nuget.org/packages/Insql.MySql/)           | [![Insql.MySql](https://img.shields.io/nuget/v/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                | [![Insql.MySql](https://img.shields.io/nuget/dt/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)                |
| [Insql.Oracle](https://www.nuget.org/packages/Insql.Oracle/)         | [![Insql.Oracle](https://img.shields.io/nuget/v/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             | [![Insql.Oracle](https://img.shields.io/nuget/dt/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)             |
| [Insql.PostgreSql](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/v/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) | [![Insql.PostgreSql](https://img.shields.io/nuget/dt/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/) |
| [Insql.Sqlite](https://www.nuget.org/packages/Insql.Sqlite/)         | [![Insql.Sqlite](https://img.shields.io/nuget/v/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             | [![Insql.Sqlite](https://img.shields.io/nuget/dt/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)             |

## 特性

- **支持 DotNet Core 2.0+ & DotNet Framework 4.6.1+**
- **支持依赖注入系统**
- **类似MyBatis sql xml 配置语法**
- **多数据库支持**
- **高性能**
- **灵活扩展性**
- **使用简单直观**

## 目录


## 使用

### 1. 添加 Insql
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql();  //使用默认配置
}
```
### 2. 设置 Insql
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder =>
  {
      //添加嵌入程序集式的SQL XML配置文件
      builder.AddEmbeddedXml(); 

      //添加外部文件目录中的SQL XML配置文件，可指定目录地址
      builder.AddExternalXml(); 

      //添加SQL解析过滤器，可用于日志记录
      builder.AddResolveFilter(); 

      //添加SQL解析描述提供器，可扩展用于从多种来源加载SQL XML配置文件，例如从数据库中加载SQL XML配置。EmbeddedXml和ExternalXml就是其中的扩展
      builder.AddDescriptorProvider(); 

      //设置默认动态脚本解析器参数
      builder.AddDefaultScriptResolver();

      //设置默认多种数据库匹配器参数
      builder.AddDefaultResolveMatcher();
  });
}
```
我们平常使用时，使用默认配置即可，可以无需理睬这些设置项。
这些只是部分示例设置，其中的各个设置参数会在以后或者其他章节进行说明。
### 示例代码
#### 1. 语句加载与解析
`User.insql.xml`
```xml
<insql type="Insql.Tests.Domain.Services.UserService,Insql.Tests" >

<insert id="InsertUser">
  insert into user (user_name,user_gender) 
  values (@UserName,@UserGender)
</insert>

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
</insql>
```
`UserService.cs`
```C#
public class UserService : IUserService
{
  private readonly ISqlResolver<UserService> sqlResolver;

  //注入ISqlResolver<T>，insql.xml中的`type`需要与`T`对应
  public UserService(ISqlResolver<UserService> sqlResolver)
  {
      this.sqlResolver = sqlResolver;
  }

  public void UpdateUserSelective()
  {
      //解析SQL语句
      var resolveResult = this.sqlResolver.Resolve("UpdateUserSelective", new UserInfo
      {
        UserId="10000",
        UserName="tom",
        UserGender = UserGender.W
      });

      //执行语句
      //connection.Execute(resolveResult.Sql,resolveResult.Param) ...
  }
}
```
这样就可以实现语句加载与执行了。就这么简单。
***注意：在默认的设置下User.insql.xml文件需要右键属性选择`嵌入式程序集方式`类型***

#### 2. 基本用法示例
`UserDbContext.insql.xml`
```xml
<insql type="Insql.Tests.Domain.Contexts.UserDbContext,Insql.Tests" >

  <!--定义UserInfo类型数据库字段到对象属性映射-->
  <map type="Insql.Tests.Domain.Models.UserInfo,Insql.Tests">
    <key name="user_id" to="UserId" />
    <column name="user_name" to="UserName" />
    <column name="user_gender" to="UserGender" />
  </map>

  <select id="GetUser">
    select * from user_info where user_id = @userId
  </select>
  
</insql>
```
`UserDbContext.cs`
```C#
//insql.xml中的`type`需要与`UserDbContext`类型对应
public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
  
    public UserInfo GetUser(int userId)
    {
        return this.Query<UserInfo>(nameof(GetUser), new { userId }).SingleOrDefault();
    }
}
```
`UserService.cs` 使用UserDbContext
```C#
public class UserService : IUserService
{
    private readonly UserDbContext dbContext;

    public UserService(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public UserInfo GetUser(int userId)
    {
        return this.dbContext.GetUser(userId);
    }
}
```
`Startup.cs` 注册UserDbContext和UserService
```C#
public void ConfigureServices(IServiceCollection services)
{
    //注册Insql
    services.AddInsql();
    
    //注册UserDbContext
    services.AddInsqlDbContext<UserDbContext>(options =>
    {
      //选择UserDbContext数据库连接
      //options.UseSqlServer(this.Configuration.GetConnectionString("sqlserver"));
      options.UseSqlite(this.Configuration.GetConnectionString("sqlite"));
    });

    services.AddScoped<IUserService,UserService>();
}
```
这就是完整的使用流程，例子是使用领域驱动模型方式，自己使用时可以看情况而定。例如可以在Controller中注入UserDbContext使用，而不需要UserService。

## 配置语法
**xxx.insql.xml** 中的配置语法类似于Mybatis的配置语法，目前支持以下配置节：
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
  - **each**
- **select** = **sql**
- **insert** = **sql**
- **update** = **sql**
- **delete** = **sql**

### map
`map`配置节用于数据库表字段到对象属性的映射，这样只要是通过`DbContext.Query<UserInfo>`查询的都将遵从此映射
```xml
<map type="Insql.Tests.Domain.Models.UserInfo,Insql.Tests">
  <key name="user_id" to="UserId" />
  <column name="user_name" to="UserName" />
  <column name="user_gender" to="UserGender" />
</map>
````
| 子元素名 | 属性名 | 属性说明   | 说明       |
| -------- | ------ | ---------- | ---------- |
| `key`    |        |            | 表示主键列 |
|          | `name` | 表列名     |            |
|          | `to`   | 对象属性名 |            |
| `column` |        |            | 表示普通列 |
|          | `name` | 表列名     |            |
|          | `to`   | 对象属性名 |            |

### sql
`sql`配置节用于配置数据库执行语句。`select`,`insert`,`update`,`delete`与`sql`具有相同功能，只是`sql`配置节的别名。
```xml
<sql id="userColumns">
  user_id as UserId,user_name as UserName,user_gender as UserGender
</sql>

<select id="GetUser">
  select
  <include refid="selectUserColumns" />
  from user_info
  where user_id = @userId
</select>
```
| 子元素名  | 属性名            | 属性说明                                              | 说明                                                                                                                                |
| --------- | ----------------- | ----------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------- |
| `include` |                   |                                                       | 导入其他`sql`配置节                                                                                                                 |
|           | `refid`           | 要导入的配置节 `id`                                   |                                                                                                                                     |
| `bind`    |                   |                                                       | 创建新查询参数到当前参数列表，例如like模糊查询场景                                                                                  |
|           | `name`            | 创建的新参数名称                                      |                                                                                                                                     |
|           | `value`           | 动态脚本表达式，例如: '%'+userName+'%'                |                                                                                                                                     |
|           | `valueType`       | 指定`value`返回的类型，格式为System.TypeCode枚举      |                                                                                                                                     |
| `if`      |                   |                                                       | 判断动态表达式，满足则输出内部内容                                                                                                  |
|           | `test`            | 动态表达式，需要返回bool 类型，例如: userName != null |                                                                                                                                     |
| `where`   |                   |                                                       | 在当前位置添加`where` sql 段，具体是否输出`where`决定于其内部子元素是否有有效的内容输出，并且会覆盖开头的 `and `,`or `              |
| `set`     |                   |                                                       | 在当前位置添加`set` sql 段，主要用于`update`配置节中，具体是否输出`set`决定于其内部子元素是否有有效的内容输出，并且会覆盖结尾的 `,` |
| `trim`    |                   |                                                       | 输出指定的前缀字符和后缀字符来包裹子元素                                                                                            |
|           | `prefix`          | 包裹的前缀字符                                        |                                                                                                                                     |
|           | `suffix`          | 包裹的后缀字符                                        |                                                                                                                                     |
|           | `prefixOverrides` | 会覆盖内部输出开头指定字符                            |                                                                                                                                     |
|           | `suffixOverrides` | 会覆盖内部输出结尾指定字符                            |                                                                                                                                     |
| `each`    |                   |                                                       | 循环数组类型的查询参数每个值                                                                                                        |
|           | `name`            | 循环的数组参数名称                                    |                                                                                                                                     |
|           | `separator`       | 每个值之间的分隔符                                    |                                                                                                                                     |
|           | `open`            | 包裹的左侧字符                                        |                                                                                                                                     |
|           | `close`           | 包裹的右侧字符                                        |                                                                                                                                     |
|           | `prefix`          | 每个值名称前缀                                        |                                                                                                                                     |
|           | `suffix`          | 每个值名称后缀                                        |                                                                                                                                     |

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
`each`
```xml
<select id="EachIn">
  select * from user_info 
  where user_id in 
  <each name="userIdList" open="(" separator="," close=")" prefix="@"  />
</select>
```
SqlResolver解析之后:
```sql
select * from user_info where user_id in (@userIdList1,@userIdList2)
```
***注意：解析之后会删除原先的`userIdList`参数，并增加`userIdList1`,`userIdList2`参数***
## 动态脚本

## 工具

### 代码生成器

## 更新

## 计划

## 赞助
