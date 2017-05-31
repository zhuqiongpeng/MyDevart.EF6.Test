using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devart.EF.Test
{
    public sealed class Configuration : DbMigrationsConfiguration<MyDbContext>
    {
        private const string ProviderName = "Devart.Data.Oracle";

        public Configuration()
        {
            //AutomaticMigrationsEnabled = false;
            //ContextKey = "AbpZeroTemplate";

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = typeof(MyDbContext).FullName;

            //SetSqlGenerator(ProviderName, new Devart.Data.Oracle.Entity.Migrations.OracleEntityMigrationSqlGenerator());

            //InintDevartOracle();
        }

        //protected override void Seed(EntityFramework.AbpZeroTemplateDbContext context)
        protected override void Seed(MyDbContext context)
        {
            MyDbContextSeeder.Seed(context);

            //context.MyAccounts.Add(new MyAccount(1, "AccountName1"));
            //context.SaveChanges();

            //context.MyAccountTwos.Add(new MyAccountTwo(1, "AccountTwoName1"));
            //context.SaveChanges();
        }

        private void InintDevartOracle()
        {
            Devart.Data.Oracle.OracleMonitor monitor = new Devart.Data.Oracle.OracleMonitor() { IsActive = true };
            Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig devartConfig = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            devartConfig.Workarounds.IgnoreSchemaName = true;
            devartConfig.Workarounds.DisableQuoting = true;
            devartConfig.CodeFirstOptions.UseDateTimeAsDate = true;
            devartConfig.CodeFirstOptions.UseNonLobStrings = true;
            devartConfig.CodeFirstOptions.UseNonUnicodeStrings = true;
            devartConfig.CodeFirstOptions.TruncateLongDefaultNames = true;
            //devartConfig.DatabaseScript.Column.MaxStringSize = Devart.Data.Oracle.Entity.Configuration.OracleMaxStringSize.Standard;
        }
    }
}
