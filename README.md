# Insql
[![Build status](https://ci.appveyor.com/api/projects/status/92f8ydwwu5nile9q?svg=true)](https://ci.appveyor.com/project/rainrcn/insql)

Insql is a lightweight ORM for .Net , base on Dapper like Mybatis.

# Packages

| Package  |  Url |
| ------------ | ------------ |
| Insql  |  https://www.nuget.org/packages/Insql/ |
| Insql.MySql  |  https://www.nuget.org/packages/Insql.MySql/ |
| Insql.Oracle      |  https://www.nuget.org/packages/Insql.Oracle/ |
| Insql.PostgreSql  | https://www.nuget.org/packages/Insql.PostgreSql/  |
| Insql.Sqlite  |  https://www.nuget.org/packages/Insql.Sqlite/ |

# Features
### DbContext and DependencyInjection
like entiryframework dbcontext
### Mybatis xml syntax 
like mybatis xml syntax , supported :
    
 - sections
    - **sql**
    `[id]`
    - **code**
    `[id]`
    `javascript` syntax
    - **select**
    `sql` section alias
    - **insert**
    `sql` section alias
    - **update**
    `sql` section alias
    - **delete**
    `sql` section alias
 - elements
    - **include**
    `[refid(ref sql section)]`
    - **bind**
    `[value(javascript syntax)` or `refid(ref code section)]`
    - **if**
    `[test(javascript syntax)` or `refid(ref code section)]`
    - **where**
    add `where` sql and remove `and | or ` prefix
    - **set**
    add `set` sql in update. and remove `,` suffix
    - **trim**
    can add or remove prefix , suffix

# Usage
 1. add `Insql` and `DbContext` to `DependencyInjection`. 
 
    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
		services.AddInsql(builder =>
		{
			builder.AddEmbeddedXml();
		});

		services.AddInsqlDbContext<UserDbContext>(options =>
		{
			options.UserSqlite(this.Configuration.GetConnectionString("sqlite"));
		});
    }
    ```
 2. create `UserDbContext` and `UserInfo`.
 
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
            return this.Query<UserInfo>(nameof(GetUserList), new { userName, userGender = Gender.M });
        }

        public void InsertUser(UserInfo info)
        {
            var userId = this.ExecuteScalar<int>(nameof(InsertUser),info);

            info.UserId = userId;
        }

        public void UpdateUser(UserInfo info)
        {
            this.Execute(nameof(UpdateUser), info);
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
 3. create `UserDbContext.insql.xml` and modify this file attribute to `embedded file`.
`insql type` mapping to `UserDbContext` Type.

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
          <if test="userGender != 'M' ">
            and user_name = @userGender
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
    
      <delete id="DeleteUser">
        delete from user_info where user_id = @userId
      </delete>
    
    </insql>
    ```
4. use dbContext in `domain service` or `web controller`.

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

                this.userDbContext.DeleteUser(2);
            });

            var list = this.userDbContext.GetUserList("love");
        }
    }
    ```
    
    
# Other Uses
1. create a common dbContext type

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

            options.UserSqlite(configuration.GetConnectionString("sqlite"));
        }
    }
    ```
2. create domain service

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
3. create `UserService.insql.xml` and modify this file attribute to `embedded file`.
`insql type` mapping to `UserService` Type.

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
          <if test="userGender != 'M' ">
            and user_name = @userGender
          </if>
        </where>
        order by  user_id
      </select>
    
    </insql>
    ```
4. add `Insql` , `SqliteDbContext` , `UserService` to `DependencyInjection`.

    ```c#
    public void ConfigureServices(IServiceCollection services)
    {
		services.AddInsql(builder =>
		{
			builder.AddEmbeddedXml();
		});

		services.AddScoped(typeof(DbContextOptions<>));
        services.AddScoped(typeof(SqliteDbContext<>));

        services.AddScoped<IUserService, UserService>();
    }
    ```
5. use IUserService in `web controller`.

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
