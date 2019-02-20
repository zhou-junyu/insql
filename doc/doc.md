# Insql documentation
## table of Contents
* **[MyBatis Sql Xml syntax](https://github.com/rainrcn/insql/blob/master/doc/doc.md#1mybatis-sql-xml-syntax)**
* **[Multiple database support](https://github.com/rainrcn/insql/blob/master/doc/doc.md#2multi-database-support)**
* **[Dynamic Script Support](https://github.com/rainrcn/insql/blob/master/doc/doc.md#3-dynamic-script-support)**
* **[Sql Resolve Filter for logging](https://github.com/rainrcn/insql/blob/master/doc/doc.md#4-sql-resolve-filter-for-logging)**
* **[Query Syntax](https://github.com/rainrcn/insql/blob/master/doc/doc.md#5-query-syntax)**
* **[Other usage](https://github.com/rainrcn/insql/blob/master/doc/doc.md#6-other-usage)**

### 1.MyBatis Sql Xml syntax
MyBatis 3 sql xml Similar configuration syntax, currently supports the following configuration sections and elements. Can view [MyBatis documentation](http://www.mybatis.org/mybatis-3/dynamic-sql.html)
- sections
  - **sql** `[id]`
  - **select**  : _sql section alias_
  - **insert**  : _sql section alias_
  - **update**  : _sql section alias_
  - **delete**  : _sql section alias_
- elements
  - **include** `[refid(ref sql section)]`
  - **bind** `[name]` `[value(javascript syntax)]`
  - **if** `[test(javascript syntax)]`
  - **where** : _add the `where` sql statement and remove the beginning `and`,`or`_
  - **set** : _add the `set` sql statement to the update. and delete the last `,`_
  - **trim** : `[prefix]` `suffix]` `[prefixOverrides]` `[suffixOverrides]`    _can add and remove custom characters at the beginning and end_
  - **each** `[name]` `[open]` `[close]` `[prefix]` `[suffix]` `[separator]` _the function of select in params can be implemented by looping list parameters_

## 2.Multi-database support
Multi-database support is enabled by default and is very simple to use.
### How to use
_`xxx.insql.xml` If you are currently using the SqlServer database, it will use `InsertUser.SqlServer` first. If the configuration section with the suffix `.SqlServer` is not found, the default `InsertUser` will be used._

_The currently defined database flag names are: `SqlServer`, `Sqlite`, `Oracle`, `MySql`, `PostgreSql`_
``` xml
<insert id="InsertUser">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select last_insert_rowid() from user_info;
</insert>

<insert id="InsertUser.SqlServer">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select SCOPE_IDENTITY();
</insert>
```
### How to disable multi-database matching support
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder=>
  {
      builder.AddDefaultResolveMatcher(options =>
      {
          options.CorssDbEnabled = false; //defaults to true
      });
  });
}
```
### How to modify the matching separator
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder=>
  {
      builder.AddDefaultResolveMatcher(options =>
      {
          options.CorssDbSeparator = "@"; //The default is `.`
      });
  });
}
```
_`xxx.insql.xml` is modified to `InsertUser@SqlServer`_
``` xml
<insert id="InsertUser">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select last_insert_rowid() from user_info;
</insert>

<insert id="InsertUser@SqlServer">
  insert into user_info (user_name,user_gender) values (@UserName,@UserGender);
  select SCOPE_IDENTITY();
</insert>
```
## 3. Dynamic script support
Dynamic script is Javascript syntax
### Operator conversion
_`xxx.insql.xml` `test="userGender !=null and userGender == 'W' "` is a dynamic script, because `&&` has special meaning in xml, so use `and` to replace ` &&` operator._
``` xml
<if test="userGender !=null and userGender == 'W' ">
  and user_gender = @userGender
</if>
```
_operator conversion mapping table:_ `"and"->"&&"` `"or"->"||"` `"gt"->">"` `"gte"->">="` `"lt"->"< "` `"lte"->"<="` `"eq"->"=="` `"neq"->"!="`

### Disable operator conversion
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder=>
  {
      builder.AddDefaultScriptResolver(options =>
      {
          options.IsConvertOperator = false; //defaults to true
      });
  });
}
```
### Enumeration converted to a string
_`xxx.insql.xml` `userGender == 'W'`, `userGender` is an enumerated type, which is converted to a string type by default_
``` xml
<if test="userGender !=null and userGender == 'W' ">
 and user_gender = @userGender
</if>
```
### Disable enum conversion to string
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder=>
  {
      builder.AddDefaultScriptResolver(options =>
      {
          options.IsConvertEnum = false; //defaults to true
      });
  });
}
```
_`xxx.insql.xml` needs to be modified to `userGender == 1`_
``` xml
<if test="userGender !=null and userGender == 1 ">
  and user_gender = @userGender
</if>
```
## 4. Sql Resolve Filter for logging
### Creating and using filters
_`OnResolving` is executed before the statement is parsed, and `OnResoved` is executed after parsing the statement._
```C#
public class LogResolveFilter : ISqlResolveFilter
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

  public void OnResolving(InsqlDescriptor insqlDescriptor, string dbType, string sqlId, IDictionary<string, object> sqlParam)
  {
  }
}
```
_Enable filter_
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql(builder =>
  {
      builder.AddResolveFilter<LogResolveFilter>();
  });
}
```

## 5. Query syntax
### SELECT IN Query
#### Use each configuration element
``` C#
var sqlParam = new { userIdList = new string[] { 'Tom','Jerry' } };
```
```xml
<select id="EachIn">
  select * from user_info where user_id in <each name="userIdList" open="(" separator="," close=")" prefix="@"  />
</select>
```
_After Sql Resolve will be converted to：_
``` sql
select * from user_info where user_id in (@userIdList1,@userIdList2)
```
***After each execution, the original userIdList parameter will be deleted and will be split into @userIdList1, @userIdList2..***
#### Use Dapper's supported list parameter conversion function
_If the each configuration element is not used, the `userIdList` parameter will not be deleted, and then Dapper's list parameter conversion function will be used when Dapper executes_
``` C#
var sqlParam = new { userIdList = new string[] { 'Tom','Jerry' } };
```
``` xml
<select id="selectInList">
  select * from user_info where user_id in @userIdList
</select>
```
_Dapper will be converted to:_
``` sql
select * from user_info where user_id in (@userIdList1,@userIdList2)
```
## 6. Other usage
### 1. The most streamlined usage, only use sql resolving
You can use only the statement parsing function, instead of creating a DbContext, just use Insql as a load and parse Sql statement.
#### Injecting ISqlResolver
_Use the sql resolver in the Domain Service, inject `ISqlResolver<T>` into the UserService, where the `T` type we specify as the `UserService` type_
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
#### Create UserService.insql.xml
_Create `UserService.insql.xml`, used as the Sql statement configuration, insql type specified as `ISqlResolver<T>` `T` type_
```xml
<insql type="Insql.Tests.Domain.Services.UserService,Insql.Tests" >
  
  <delete id="DeleteUser">
    delete from user_info where user_id = @userId
  </delete>
  
</insql>
```
#### Add Insql
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql();

  services.AddScoped<IUserService, UserService>();
}
```

---
### 2. Use the common DbContext usage
In the basic use example, we will create multiple DbContext types, and here we can create only one common DbContext type.
#### Create a common DbContext
```C#
public class CommonDbContext<TInsql> : DbContext where TInsql : class
{
  public CommonDbContext(CommonDbContextOptions<TInsql> options) : base(options)
  {
  }

  Protected override void OnConfiguring(DbContextOptions options)
  {
    var configuration = options.ServiceProvider.GetRequiredService<IConfiguration>();

    //TInsql type mapping to insql.xml type
    options.UseSqlResolver<TInsql>();

    options.UseSqlite(configuration.GetConnectionString("sqlite"));
  }
}

public class CommonDbContextOptions<TInsql> : DbContextOptions<CommonDbContext<TInsql>> where TInsql : class
{
  public CommonDbContextOptions(IServiceProvider serviceProvider) : base(serviceProvider)
  {
  }
}
```

#### Create Domain Service
```C#
public interface IUserService
{
  IEnumerable<UserInfo> GetUserList(string userName,Gender? userGender);
}

public class UserService : IUserService
{
  private readonly DbContext dbContext;

  //T is UserService
  public UserService(CommonDbContext<UserService> dbContext)
  {
      this.dbContext = dbContext;
  }

  public IEnumerable<UserInfo> GetUserList(string userName, Gender? userGender)
  {
      return this.dbContext.Query<UserInfo>(nameof(GetUserList), new { userName, userGender });
  }
}
```

#### Create Service.insql.xml
_Create the `UserService.insql.xml` file and modify the properties of this file to be the `embedded file` type. `insql type` corresponds to the `UserService` type._
```xml
    <insql type="Example.Domain.Services.UserService,Example.Domain" >
    
      <sql id="selectUserColumns">
        select user_id as UserId, user_name as UserName, user_gender as UserGender from user_info
      </sql>
    
      <select id="GetUserList">
        <include refid="selectUserColumns" />
        <where>
          <if test="userName != null">
            <bind name="likeUserName" value="'%' + userName + '%'" />
            user_name like @likeUserName
          </if>
          <if test="userGender != null ">
            and user_gender = @userGender
          </if>
        </where>
        order by user_id
      </select>
    
    </insql>
```

#### Add DbContext
```c#
public void ConfigureServices(IServiceCollection services)
{
  services.AddInsql();

  services.AddScoped(typeof(CommonDbContextOptions<>));
  services.AddScoped(typeof(CommonDbContext<>));

  services.AddScoped<IUserService, UserService>();
}
```

#### Use Domain Service
```C#
public class ValuesController : ControllerBase
{
  private readonly IUserService userService;

  public ValuesController(IUserService userService)
  {
      this.userService = userService;
  }

  [HttpGet]
  public ActionResult<IEnumerable<string>> Get()
  {
      var list = this.userService.GetUserList("11", Domain.Gender.M);
      //todo return
  }
}
```
