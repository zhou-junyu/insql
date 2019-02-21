# Insql
[![Build status](https://ci.appveyor.com/api/projects/status/92f8ydwwu5nile9q?svg=true)](https://ci.appveyor.com/project/rainrcn/insql)
![](https://img.shields.io/github/license/rainrcn/insql.svg?style=flat)

Insql is a lightweight ORM for .Net . Object mapping based on Dapper , Sql configuration was inspired by Mybatis.

[中文](https://github.com/rainrcn/insql/blob/master/README.zh_cn.md) | [English](https://github.com/rainrcn/insql/blob/master/README.md)

# Packages

| Package  |  Nuget Stable  |  Downloads  |
| ------------ | ------------ | ------------ |
| [Insql](https://www.nuget.org/packages/Insql/)  | [![Insql](https://img.shields.io/nuget/v/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)  |  [![Insql](https://img.shields.io/nuget/dt/Insql.svg?style=flat)](https://www.nuget.org/packages/Insql/)  |
| [Insql.MySql](https://www.nuget.org/packages/Insql.MySql/)  | [![Insql.MySql](https://img.shields.io/nuget/v/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)  |  [![Insql.MySql](https://img.shields.io/nuget/dt/Insql.MySql.svg?style=flat)](https://www.nuget.org/packages/Insql.MySql/)  |
| [Insql.Oracle](https://www.nuget.org/packages/Insql.Oracle/)  | [![Insql.Oracle](https://img.shields.io/nuget/v/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)  |  [![Insql.Oracle](https://img.shields.io/nuget/dt/Insql.Oracle.svg?style=flat)](https://www.nuget.org/packages/Insql.Oracle/)  |
| [Insql.PostgreSql](https://www.nuget.org/packages/Insql.PostgreSql/)  | [![Insql.PostgreSql](https://img.shields.io/nuget/v/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/)  |  [![Insql.PostgreSql](https://img.shields.io/nuget/dt/Insql.PostgreSql.svg?style=flat)](https://www.nuget.org/packages/Insql.PostgreSql/)  |
| [Insql.Sqlite](https://www.nuget.org/packages/Insql.Sqlite/)  | [![Insql.Sqlite](https://img.shields.io/nuget/v/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)  |  [![Insql.Sqlite](https://img.shields.io/nuget/dt/Insql.Sqlite.svg?style=flat)](https://www.nuget.org/packages/Insql.Sqlite/)  |

# Features
- **Support DoNet Core 2.0+ && DotNet Framework 4.6.1+**
- **Support for dependency injection system**
- **MyBatis sql xml syntax**
- **Multiple database support**
- **Flexible scalability**
- **Simple and intuitive to use**

# Streamlined usage
Only use Insql as a load and parse Sql statement to use.
### Inject ISqlResolver
_Use the statement parser in the Domain Service, inject `ISqlResolver<T>` into the UserService, where the `T` type we specify as the `UserService` type_
```C#
public class UserService : IUserService
{
  private readonly ISqlResolver<UserService> sqlResolver;

  public UserService(ISqlResolver<UserService> sqlResolver)
  {
      this.sqlResolver = sqlResolver;
  }

  public void DeleteUser(int userId)
  {
      var resolveResult = this.sqlResolver.Resolve("DeleteUser", new { userId });

      //If you need to specify the database (matching the SqlId suffix to .SqlServer), you need to set the parameters of the DbType.
      //var resolveResult = this.sqlResolver.Resolve("SqlServer", "DeleteUser", new { userId });

      //connection.Execute(resolveResult.Sql,resolveResult.Param) ...
  }
}
```
### Create UserService.insql.xml
_Create `UserService.insql.xml`, used as the Sql statement configuration, and modify this file attribute to `embedded file`. insql type specified as `ISqlResolver<T>` `T` type_
```xml
<insql type="Insql.Tests.Domain.Services.UserService,Insql.Tests" >
  
  <delete id="DeleteUser">
    delete from user_info where user_id = @userId
  </delete>
  
</insql>
```
### Add Insql
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql();

  services.AddScoped<IUserService, UserService>();
}
```

# Basic usage
Basic usage can be used by creating DbContext
### Add Insql
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddInsql();

    services.AddInsqlDbContext<UserDbContext>(options =>
    {
      //options.UseSqlServer(this.Configuration.GetConnectionString("sqlserver"));
      options.UseSqlite(this.Configuration.GetConnectionString("sqlite"));
    });
}
```
### Create DbContext
```c#
public class UserDbContext : Insql.DbContext  
{
    public UserDbContext(Insql.DbContextOptions<UserDbContext> options) 
    : base(options)
    {
    }

    public IEnumerable<UserInfo> GetUserList(string userName)
    {
        //sqlId = "GetUserList"
        //sqlParam is PlainObject or IDictionary<string,object>
        return this.Query<UserInfo>(nameof(GetUserList), new { userName, userGender = Gender.W });
    }

    public void InsertUser(UserInfo info)
    {
        var userId = this.ExecuteScalar<int>(nameof(InsertUser),info);

        info.UserId = userId;
    }

    public void UpdateUserSelective(UserInfo info)
    {
        this.Execute(nameof(UpdateUserSelective), info);
    }
}
	
//user model
public class UserInfo
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public Gender? UserGender { get; set; }
}

public enum Gender
{
    M,
    W
}
```
### Create DbContext.insql.xml
_create `UserDbContext.insql.xml` and modify this file attribute to `embedded file`. `insql type` mapping to `UserDbContext` Type._
```xml
<insql type="Example.Domain.Contexts.UserDbContext,Example.Domain" >

    <map type="Example.Domain.Model.UserInfo,Example.Domain">
      <column name="user_id" to="UserId" />
      <column name="user_name" to="UserName" />
      <column name="user_gender" to="UserGender" />
    </map>

    <select id="GetUserList">
      select * from user_info
      <where>
        <if test="userName != null">
          <bind name="likeUserName" value="'%' + userName + '%'" />
          user_name like @likeUserName
        </if>
        <if test="userGender != null and userGender != 'M' ">
          and user_gender = @userGender
        </if>
      </where>
      order by  user_id
    </select>

    <insert id="InsertUser">
      insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
      select last_insert_rowid() from user_info;
    </insert>

    <update id="UpdateUserSelective">
      update user_info
      <set>
        <if test="UserName != null">
          user_name=@UserName,
        </if>
        <if test="UserGender != null">
          user_gender=@UserGender
        </if>
      </set>
      where user_id = @UserId
    </update>
	
</insql>
```
### Use DbContext
_use `UserDbContext` in `Domain Service` or `Web Controller`_
```c#
public class ValuesController : ControllerBase
{
    private readonly UserDbContext userDbContext;

    public ValuesController(UserDbContext userDbContext)
    {
        this.userDbContext = userDbContext;	
    }

    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
        //use Transaction
        this.userDbContext.DoWithTransaction(() =>
        {
            this.userDbContext.InsertUser(new Domain.UserInfo
            {
                UserName = "loveW",
                UserGender = Domain.Gender.M
            });

            this.userDbContext.UpdateUserSelective(new Domain.UserInfo
            {
                UserId = 1,
                UserName = "loveWWW",
            });
        });

        var list = this.userDbContext.GetUserList("love");
    }
}
```
_can also create only a common DbContext, without the need to create multiple DbContext types to use, can be viewed in the detailed document_
# Code generator
In the `tools` directory, there is a CodeSmith generator file. Can generate DbContext, DbContext.insql.xml, Models
# Documentation
Please see the detailed [documentation](https://github.com/rainrcn/insql/blob/master/doc/doc.md).
