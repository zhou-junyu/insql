# Insql
[![Build status](https://ci.appveyor.com/api/projects/status/92f8ydwwu5nile9q?svg=true)](https://ci.appveyor.com/project/rainrcn/insql)

Insql 是一个轻量级的.NET ORM类库 . 对象映射基于Dapper , Sql配置灵感来自于Mybatis.

[中文](https://github.com/rainrcn/insql/blob/master/README.zh_cn.md) | [English](https://github.com/rainrcn/insql/blob/master/README.md)

# Packages

| Package  |  Url |
| ------------ | ------------ |
| Insql  |  https://www.nuget.org/packages/Insql/ |
| Insql.MySql  |  https://www.nuget.org/packages/Insql.MySql/ |
| Insql.Oracle      |  https://www.nuget.org/packages/Insql.Oracle/ |
| Insql.PostgreSql  | https://www.nuget.org/packages/Insql.PostgreSql/  |
| Insql.Sqlite  |  https://www.nuget.org/packages/Insql.Sqlite/ |

# 功能
### DbContext and DependencyInjection
EntityFramework类似的DbContext使用方式
### Mybatis xml syntax 
mybatis 很相近的sql xml配置语法，目前支持：

 - sections
    - **sql**
    `[id]`
    - **code**
    `[id]`
    `javascript` 语法
    - **select**
    `sql` 节的别名
    - **insert**
    `sql` 节的别名
    - **update**
    `sql` 节的别名
    - **delete**
    `sql` 节的别名
 - elements
    - **include**
    `[refid(引用code配置节)]`
    - **bind**
    `[name][value(javascript 语法)` or `refid(引用code配置节)]`
    - **if**
    `[test(javascript 语法)` or `refid(引用code配置节)]`
    - **where**
    添加 `where` sql 语句并且移除开头的and 或者or 
    - **set**
    添加 `set` sql 语句到update后. 并且删除最后的 `,`
    - **trim**
    可以添加和移除开头和结尾自定义的字符

# 用法
* **Add Insql**
 
    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
		services.AddInsql();

		services.AddInsqlDbContext<UserDbContext>(options =>
		{
			options.UseSqlite(this.Configuration.GetConnectionString("sqlite"));
		});
    }
    ```
* **Create DbContext**
 
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
* **Create DbContext.insql.xml**
	> 创建 `UserDbContext.insql.xml` 文件并且修改这个文件的属性为`嵌入式文件`类型 . `insql type` 与 `UserDbContext` 类型对应.

    ```xml
    <insql type="Example.Domain.Contexts.UserDbContext,Example.Domain" >
    
      <sql id="selectUserColumns">
        select user_id as UserId,user_name as UserName,user_gender as UserGender from user_info
      </sql>
    
      <select id="GetUserList">
        <include refid="selectUserColumns" />
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
      <update id="UpdateUser">
        update user_info set user_name=@UserName,user_gender=@UserGender where user_id = @userId
      </update>
    
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
 
* **Use DbContext**

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
            //可以这样简单的使用事务
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
    
    
# 其他用法
* **Create Common DbContext**

    ```c#
    public class SqliteDbContext<T> : DbContext where T : class
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext<T>> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptions options)
        {
            var configuration = options.ServiceProvider.GetRequiredService<IConfiguration>();

            //T type mapping to insql.xml type
            options.UseSqlResolver<T>();

            options.UseSqlite(configuration.GetConnectionString("sqlite"));
        }
    }
    ```
* **Create Domain Service**

    ```c#
    public interface IUserService
    {
        IEnumerable<UserInfo> GetUserList(string userName,Gender? userGender);
    }
    
    public class UserService : IUserService
    {
        private readonly DbContext dbContext;

        //T is UserService
        public UserService(SqliteDbContext<UserService> dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<UserInfo> GetUserList(string userName, Gender? userGender)
        {
            return this.dbContext.Query<UserInfo>(nameof(GetUserList), new { userName, userGender });
        }
    }
    ```
* **Create Service.insql.xml**
	> 创建 `UserService.insql.xml` 文件并且修改这个文件的属性为`嵌入式文件`类型 . `insql type` 与 `UserService` 类型对应.

    ```xml
    <insql type="Example.Domain.Services.UserService,Example.Domain" >
    
      <sql id="selectUserColumns">
        select user_id as UserId,user_name as UserName,user_gender as UserGender from user_info
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
        order by  user_id
      </select>
    
    </insql>
    ```
* **Add Insql**

    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
		services.AddInsql();

		services.AddScoped(typeof(DbContextOptions<>));
        services.AddScoped(typeof(SqliteDbContext<>));

        services.AddScoped<IUserService, UserService>();
    }
    ```
* **Use Domain Service**

   ```c#
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
        }
    }
    ```  

# License
[MIT](http://opensource.org/licenses/MIT) License
