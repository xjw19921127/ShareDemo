using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using ContainerApp.Interface.Interfaces;
using System;
using Microsoft.Extensions.Configuration;

namespace ContainerApp.Extensions
{
    public static class DatabaseSetup
    {
        public static IFreeSql<IBaseDB> BaseFsql { get; private set; }
        public static IFreeSql<IBusinessDB> BusinessFsql { get; private set; }
        public static IFreeSql<IRoomControlDB> RoomControlFsql { get; private set; }

        public static void AddDatabaseSetup(this IServiceCollection services, IConfiguration configuration) 
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //配置核心库连接
            BaseFsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, configuration.GetConnectionString("BaseDB"))
                .Build<IBaseDB>();
            BaseFsql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = false);

            //配置业务库连接
            BusinessFsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, configuration.GetConnectionString("BusinessDB"))
                .Build<IBusinessDB>();
            BusinessFsql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = false);

            //配置客控库连接
            RoomControlFsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, configuration.GetConnectionString("RoomControlDB"))
                .Build<IRoomControlDB>();
            RoomControlFsql.SetDbContextOptions(opt => opt.EnableAddOrUpdateNavigateList = false);

            services.AddSingleton<IFreeSql<IBaseDB>>(BaseFsql);
            services.AddSingleton<IFreeSql<IBusinessDB>>(BusinessFsql);
            services.AddSingleton<IFreeSql<IRoomControlDB>>(RoomControlFsql);

            //增加freesql仓储模式
            services.AddFreeRepository();
        }
    }
}
