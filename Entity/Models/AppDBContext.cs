using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entity.Models
{
    public partial class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }
        
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accesstoken> Accesstoken { get; set; }
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Demo> Demo { get; set; }
        public virtual DbSet<GroupInfo> GroupInfo { get; set; }
        public virtual DbSet<GroupPowerinfo> GroupPowerinfo { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<LoginUser> LoginUser { get; set; }
        public virtual DbSet<MenuInfo> MenuInfo { get; set; }
        public virtual DbSet<Org> Org { get; set; }
        public virtual DbSet<OrgUser> OrgUser { get; set; }
        public virtual DbSet<PetInfo> PetInfo { get; set; }
        public virtual DbSet<Trips> Trips { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("server=localhost;user id=sa;pwd=sa;database=AppDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accesstoken>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Accessto__F3BEEBFFB975E9BF");

                entity.HasComment("用于存储每个用户登录成功之后的票据（加密串）");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(50)
                    .HasComment("用户id");

                entity.Property(e => e.AccessToken1)
                    .HasColumnName("ACCESS_TOKEN")
                    .HasMaxLength(100)
                    .HasComment("token串");

                entity.Property(e => e.ExpiredTime)
                    .HasColumnName("EXPIRED_TIME")
                    .HasColumnType("datetime")
                    .HasComment("yyyy-mm-dd:hh:mm:ss过期时间");
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasKey(e => e.ConfCode)
                    .HasName("PK__Config__19FA37A8077C3821");

                entity.HasComment("记录平台中的基本配置信息");

                entity.Property(e => e.ConfCode)
                    .HasColumnName("CONF_CODE")
                    .HasMaxLength(30)
                    .HasComment("配置项");

                entity.Property(e => e.ConfName)
                    .HasColumnName("CONF_NAME")
                    .HasMaxLength(60)
                    .HasComment("配置说明");

                entity.Property(e => e.ConfValue)
                    .HasColumnName("CONF_VALUE")
                    .HasMaxLength(30)
                    .HasComment("配置值");
            });

            modelBuilder.Entity<Demo>(entity =>
            {
                entity.HasComment("示例表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .HasComment("id");

                entity.Property(e => e.Age)
                    .HasColumnName("AGE")
                    .HasComment("年龄");

                entity.Property(e => e.Createdate)
                    .HasColumnName("CREATEDATE")
                    .HasColumnType("datetime")
                    .HasComment("创建日期");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50)
                    .HasComment("姓名");
            });

            modelBuilder.Entity<GroupInfo>(entity =>
            {
                entity.HasComment("记录系统所拥有的所有权限组");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .HasComment("主键");

                entity.Property(e => e.GroupCode)
                    .HasColumnName("GROUP_CODE")
                    .HasMaxLength(50)
                    .HasComment("角色编码");

                entity.Property(e => e.GroupCodeUpper)
                    .HasColumnName("GROUP_CODE_UPPER")
                    .HasMaxLength(50)
                    .HasComment("上级角色编码");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasMaxLength(50)
                    .HasComment("角色id");

                entity.Property(e => e.GroupName)
                    .HasColumnName("GROUP_NAME")
                    .HasMaxLength(50)
                    .HasComment("角色名称");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasMaxLength(120)
                    .HasComment("备注");

                entity.Property(e => e.SysCode)
                    .HasColumnName("SYS_CODE")
                    .HasMaxLength(50)
                    .HasComment("系统编号-不同的系统下权限组不同");
            });

            modelBuilder.Entity<GroupPowerinfo>(entity =>
            {
                entity.HasComment("记录当前系统下的组权限");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .HasComment("主键");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasMaxLength(50)
                    .HasComment("角色id");

                entity.Property(e => e.MenuId)
                    .HasColumnName("MENU_ID")
                    .HasMaxLength(50)
                    .HasComment("菜单id");
            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.HasComment("角色用户表");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .HasComment("主键");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasMaxLength(50)
                    .HasComment("角色id");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(50)
                    .HasComment("用户id");
            });

            modelBuilder.Entity<LoginUser>(entity =>
            {
                entity.HasComment("记录当前当前登录账号与用户之间的关联关系");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .HasComment("主键");

                entity.Property(e => e.LoginId)
                    .HasColumnName("LOGIN_ID")
                    .HasMaxLength(50)
                    .HasComment("主账号id");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(50)
                    .HasComment("关联账号id");
            });

            modelBuilder.Entity<MenuInfo>(entity =>
            {
                entity.HasKey(e => e.MenuId)
                    .HasName("PK__MenuInfo__6C47297981CF63EE");

                entity.HasComment("记录平台所有系统的菜单信息");

                entity.Property(e => e.MenuId)
                    .HasColumnName("MENU_ID")
                    .HasMaxLength(50)
                    .HasComment("id");

                entity.Property(e => e.MenuIcon)
                    .HasColumnName("MENU_ICON")
                    .HasComment("图标");

                entity.Property(e => e.MenuIdUpper)
                    .HasColumnName("MENU_ID_UPPER")
                    .HasMaxLength(50)
                    .HasComment("上级菜单id");

                entity.Property(e => e.MenuName)
                    .HasColumnName("MENU_NAME")
                    .HasMaxLength(30)
                    .HasComment("菜单名称");

                entity.Property(e => e.MenuOrder)
                    .HasColumnName("MENU_ORDER")
                    .HasComment("排序");

                entity.Property(e => e.MenuProp)
                    .HasColumnName("MENU_PROP")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("时候显示（0：不显示；1：显示）");

                entity.Property(e => e.ModuleObj)
                    .HasColumnName("MODULE_OBJ")
                    .HasMaxLength(30)
                    .HasComment("对应文件名");

                entity.Property(e => e.ModuleRoute)
                    .HasColumnName("MODULE_ROUTE")
                    .HasMaxLength(100)
                    .HasComment("路由");

                entity.Property(e => e.ModuleUrl)
                    .HasColumnName("MODULE_URL")
                    .HasComment("地址");

                entity.Property(e => e.SysCode)
                    .HasColumnName("SYS_CODE")
                    .HasMaxLength(20)
                    .HasComment("系统编号");
            });

            modelBuilder.Entity<Org>(entity =>
            {
                entity.HasComment("定义组织机构的信息");

                entity.Property(e => e.OrgId)
                    .HasColumnName("ORG_ID")
                    .HasMaxLength(50)
                    .HasComment("主键");

                entity.Property(e => e.Isdelete)
                    .HasColumnName("ISDELETE")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("是否删除 0删除 1 未删除");

                entity.Property(e => e.Isinvalid)
                    .HasColumnName("ISINVALID")
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("是否有效 1有效0无效");

                entity.Property(e => e.OrgCode)
                    .HasColumnName("ORG_CODE")
                    .HasMaxLength(50)
                    .HasComment("组织机构代码");

                entity.Property(e => e.OrgCodeUpper)
                    .HasColumnName("ORG_CODE_UPPER")
                    .HasMaxLength(50)
                    .HasComment("父级code");

                entity.Property(e => e.OrgIdUpper)
                    .HasColumnName("ORG_ID_UPPER")
                    .HasMaxLength(50)
                    .HasComment("父ID");

                entity.Property(e => e.OrgName)
                    .HasColumnName("ORG_NAME")
                    .HasMaxLength(255)
                    .HasComment("全称");

                entity.Property(e => e.OrgShortName)
                    .HasColumnName("ORG_SHORT_NAME")
                    .HasMaxLength(255)
                    .HasComment("简称");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasMaxLength(120)
                    .HasComment("备注");
            });

            modelBuilder.Entity<OrgUser>(entity =>
            {
                entity.HasKey(e => e.OrgId)
                    .HasName("PK__OrgUser__66696A8CC4A6192C");

                entity.HasComment("记录系统中组织机构和用户之间的关系");

                entity.Property(e => e.OrgId)
                    .HasColumnName("ORG_ID")
                    .HasMaxLength(50)
                    .HasComment("组织机构id");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(50)
                    .HasComment("用户id");
            });

            modelBuilder.Entity<PetInfo>(entity =>
            {
                entity.HasKey(e => e.PetId)
                    .HasName("PK__UserInfo__F3BEEBFF28705786");

                entity.Property(e => e.PetId)
                    .HasColumnName("PET_ID")
                    .HasMaxLength(255)
                    .HasComment("宠物ID");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OWNER_ID")
                    .HasMaxLength(255)
                    .HasComment("拥有者ID");

                entity.Property(e => e.PetAge)
                    .HasColumnName("PET_AGE")
                    .HasComment("宠物年龄");

                entity.Property(e => e.PetName)
                    .HasColumnName("PET_NAME")
                    .HasMaxLength(255)
                    .HasComment("宠物姓名");

                entity.Property(e => e.PetSex)
                    .HasColumnName("PET_SEX")
                    .HasComment("宠物性别");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.PetInfo)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__PetInfo__OWNER_I__5165187F");
            });

            modelBuilder.Entity<Trips>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(255);

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.ClientId).HasColumnName("Client_Id");

                entity.Property(e => e.DriverId).HasColumnName("Driver_Id ");

                entity.Property(e => e.RequestAt)
                    .HasColumnName("Request_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(255)
                    .HasComment("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(255)
                    .HasComment("账号");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasComment("密码");

                entity.Property(e => e.PetId).HasMaxLength(255);

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.PetId)
                    .HasConstraintName("FK_User_PetInfo");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__F3BEEBFFCDA61878");

                entity.HasComment("平台用户信息表，包括所有应用系统的用户信息");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(50)
                    .HasComment("系统自动生成的用户唯一ID");

                entity.Property(e => e.AssociatedAccount)
                    .HasColumnName("ASSOCIATED_ACCOUNT")
                    .HasMaxLength(150)
                    .HasComment("关联账号");

                entity.Property(e => e.AuthenticationType)
                    .HasColumnName("AUTHENTICATION_TYPE")
                    .HasComment("账号类型0本地 1ptr");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasComment("1：激活；0：未激活");

                entity.Property(e => e.PhoneMobile)
                    .HasColumnName("PHONE_MOBILE")
                    .HasMaxLength(20)
                    .HasComment("手机号");

                entity.Property(e => e.PhoneOffice)
                    .HasColumnName("PHONE_OFFICE")
                    .HasMaxLength(20)
                    .HasComment("办公电话");

                entity.Property(e => e.RegTime)
                    .HasColumnName("REG_TIME")
                    .HasComment("注册时间");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasMaxLength(120)
                    .HasComment("备注");

                entity.Property(e => e.UserCode)
                    .HasColumnName("USER_CODE")
                    .HasMaxLength(50)
                    .HasComment("员工编号");

                entity.Property(e => e.UserDomain)
                    .HasColumnName("USER_DOMAIN")
                    .HasMaxLength(30)
                    .HasComment("账号");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("USER_EMAIL")
                    .HasMaxLength(100)
                    .HasComment("用户邮箱");

                entity.Property(e => e.UserIp)
                    .HasColumnName("USER_IP")
                    .HasMaxLength(50)
                    .HasComment("ip");

                entity.Property(e => e.UserName)
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(20)
                    .HasComment("姓名");

                entity.Property(e => e.UserPass)
                    .HasColumnName("USER_PASS")
                    .HasMaxLength(255)
                    .HasComment("密码");

                entity.Property(e => e.UserSex)
                    .HasColumnName("USER_SEX")
                    .HasComment("0：男性；1：女性");

                entity.Property(e => e.UserType)
                    .HasColumnName("USER_TYPE")
                    .HasComment("用户类型(0:管理员1:普通用户)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.UsersId)
                    .HasColumnName("Users_Id")
                    .HasMaxLength(255);

                entity.Property(e => e.Banned).HasMaxLength(255);

                entity.Property(e => e.Role).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
