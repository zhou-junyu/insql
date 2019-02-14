
# Insql 说明文档
## 目录
* **[MyBatis Sql Xml 语法](https://github.com/rainrcn/insql/blob/master/doc/doc.zh_cn.md#1mybatis-sql-xml-%E8%AF%AD%E6%B3%95)**
* **[多数据库支持](https://github.com/rainrcn/insql/blob/master/doc/doc.zh_cn.md#2%E5%A4%9A%E6%95%B0%E6%8D%AE%E5%BA%93%E6%94%AF%E6%8C%81)**
* **[动态脚本支持](https://github.com/rainrcn/insql/blob/master/doc/doc.zh_cn.md#3%E5%8A%A8%E6%80%81%E8%84%9A%E6%9C%AC%E6%94%AF%E6%8C%81)**
* **[语句解析过滤器，实现日志记录](https://github.com/rainrcn/insql/blob/master/doc/doc.zh_cn.md#4%E8%AF%AD%E5%8F%A5%E8%A7%A3%E6%9E%90%E8%BF%87%E6%BB%A4%E5%99%A8%E5%AE%9E%E7%8E%B0%E6%97%A5%E5%BF%97%E8%AE%B0%E5%BD%95)**
* **[查询语法](https://github.com/rainrcn/insql/blob/master/doc/doc.zh_cn.md#5%E6%9F%A5%E8%AF%A2%E8%AF%AD%E6%B3%95)**
* **[其他用法](https://github.com/rainrcn/insql/blob/master/doc/doc.zh_cn.md#6%E5%85%B6%E4%BB%96%E7%94%A8%E6%B3%95)**

### 1.MyBatis Sql Xml 语法
Mybatis 3 sql xml 类似的配置语法，目前支持以下配置节和元素。可以查看 [Mybatis文档](http://www.mybatis.org/mybatis-3/zh/dynamic-sql.html)
- sections
    - **sql**
    `[id]`
    - **select** : _sql节的别名_
    - **insert** : _sql节的别名_
    - **update** : _sql节的别名_
    - **delete** : _sql节的别名_
 - elements
    - **include** `[refid(引用sql配置节)]`
    - **bind** `[name]` `[value(javascript 语法)]`
    - **if** `[test(javascript 语法)]`
    - **where** ：_添加 `where` sql 语句并且移除开头的and 或者or_ 
    - **set** ：_添加 `set` sql 语句到update后. 并且删除最后的 `,`_
    - **trim** `[prefix]` `[suffix]` `[prefixOverrides]` `[suffixOverrides]` _可以添加和移除开头和结尾自定义的字符_

## 2.多数据库支持
多数据库支持为默认启用，使用时非常简单。
### 使用方式
_`xxx.insql.xml`中如果当前使用的是SqlServer数据库，则会优先使用`InsertUser.SqlServer`，如果未找到后缀是`.SqlServer`的配置节，则使用默认的`InsertUser`_
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
### 如何禁用多数据库匹配支持
```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddInsql(builder=> 
        {
            builder.AddDefaultResolveMatcher(options => 
            {
                options.CorssDbEnabled = false;  //默认为 true
            });
        });
}
```
### 如何修改匹配分割符
```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddInsql(builder=> 
        {
            builder.AddDefaultResolveMatcher(options => 
            {
                options.CorssDbSeparator = "@"; //默认是 `.`
            });
        });
}
```
_`xxx.insql.xml`中修改为 `InsertUser@SqlServer`_
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
## 3.动态脚本支持
### 操作符转换
_`xxx.insql.xml`中 `test="userGender !=null and userGender == 'W' "` 为动态脚本，因为`&&` 在xml中有特殊意义，所以使用 `and` 来替换 `&&`操作符。_
``` xml
<if test="userGender !=null and userGender == 'W' ">
 and user_gender = @userGender
</if>
```
_操作符转换映射表：_
`"and"->"&&"` `"or"->"||"` `"gt"->">"` `"gte"->">="` `"lt"->"<"` `"lte"->"<="` `"eq"->"=="` `"neq"->"!="`

### 禁用操作符转换
```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddInsql(builder=> 
        {
            builder.AddScriptCodeResolver(options => 
            {
                options.IsConvertOperator = false;  //默认为 true
            });
        });
}
```
### 枚举转换为字符串
_`xxx.insql.xml`中`userGender == 'W'`,`userGender`为枚举类型，这里默认转换为字符串类型_
``` xml
<if test="userGender !=null and userGender == 'W' ">
 and user_gender = @userGender
</if>
```
### 禁用枚举转换为字符串
```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddInsql(builder=> 
        {
            builder.AddScriptCodeResolver(options => 
            {
                options.IsConvertEnum = false; //默认为 true
            });
        });
}
```
_`xxx.insql.xml`中需要修改为`userGender == 1`_
``` xml
<if test="userGender !=null and userGender == 1 ">
 and user_gender = @userGender
</if>
```
## 4.语句解析过滤器，实现日志记录
### 创建并使用过滤器
_`OnResolving`为语句解析前执行，`OnResoved` 为语句解析后执行_
```C#
public class LogResolveFilter : ISqlResolveFilter
    {
        private readonly ILogger<LogResolveFilter> logger;

        public LogResolveFilter(ILogger<LogResolveFilter> logger)
        {
            this.logger = logger;
        }

        public void OnResolved(InsqlDescriptor insqlDescriptor, ResolveContext resolveContext, ResolveResult resolveResult)
        {
            this.logger.LogInformation($"insql resolved id : {resolveContext.InsqlSection.Id} , sql : {resolveResult.Sql}");
        }

        public void OnResolving(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam, IDictionary<string, string> envParam)
        {
        }
    }
```
_启用过滤器_
```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddInsql(builder =>
        {
            builder.AddResolveFilter<LogResolveFilter>();
        });
}
```

## 5.查询语法
### SELECT IN 查询
_使用Dapper支持的列表参数转换功能_
``` C#
var sqlParam = new { userNameList = new string[] { 'love1','love2' } };
```
``` xml
<select id="selectInList">
 select * from user_info where user_name in @userNameList
</select>
```
_将被转换为：_
``` sql
select * from user_info where user_name in (@userNameList1,@userNameList2)
```

## 6.其他用法
### 创建公共 DbContext
```C#
public class CommonDbContext<TInsql> : DbContext where TInsql : class
{
	public CommonDbContext(CommonDbContextOptions<TInsql> options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptions options)
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
### 创建 Domain Service
```c#
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
### 创建 Service.insql.xml
_创建 `
