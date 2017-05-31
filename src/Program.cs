using System;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Data.Entity;

namespace Devart.EF.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Devart.Data.Oracle.OracleMonitor monitor
            //          = new Devart.Data.Oracle.OracleMonitor() { IsActive = true };

            //--------------------------------------------------------------
            // You use the capability for configuring the behavior of the EF-provider:
            //Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig devartConfig =
            //    Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            // Now, you switch off schema name generation while generating 
            // DDL scripts and DML:
            //devartConfig.Workarounds.IgnoreSchemaName = true;
            //devartConfig.Workarounds.DisableQuoting = true;
            //devartConfig.CodeFirstOptions.UseDateTimeAsDate = true;
            //devartConfig.CodeFirstOptions.UseNonLobStrings = true;
            //devartConfig.CodeFirstOptions.UseNonUnicodeStrings = true;
            //devartConfig.CodeFirstOptions.TruncateLongDefaultNames = true;
            //--------------------------------------------------------------

            /*--------------------------------------------------------------
            You can set up a connection string for DbContext in different ways.
            It can be placed into the app.config (web.config) file.
            The connection string name must be identical to the DbContext descendant name.

            <add name="MyDbContext" connectionString="Data Source=ora1020;
            User Id=test;Password=test;" providerName="Devart.Data.Oracle" />

            After that, you create a context instance, while a connection string is 
            enabled automatically:
            MyDbContext context = new MyDbContext();
            ---------------------------------------------------------------*/

            /*--------------------------------------------------------------
            And now it is possible to create an instance of the provider-specific connection 
            and send it to the context constructor, like we did in this application. 
            That allows us to use the StateChange connection event to change the Oracle 
            current schema on its occurrence. Thus, we can connect as one user and 
            work on a schema owned by another user.
            ---------------------------------------------------------------*/
            //DbConnection con = new Devart.Data.Oracle.OracleConnection(
            //    "user id=abp1;password=123;server=orcl;persist security info=True");
            //DbConnection con = new Devart.Data.Oracle.OracleConnection(
            //   "User Id=abp1;Password=123;Data Source=orcl");
            DbConnection con = new Oracle.ManagedDataAccess.Client.OracleConnection(
              "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL))) ;Persist Security Info=True;User ID=abp1;Password=123;");
            con.Open();
            //con.StateChange += new StateChangeEventHandler(Connection_StateChange);
            /*---------------------------------------------------------------*/

            /*--------------------------------------------------------------
            You can choose one of database initialization
            strategies or turn off initialization:
            --------------------------------------------------------------*/
            //System.Data.Entity.Database.SetInitializer
            //  <MyDbContext>(new MyDbContextDropCreateDatabaseAlways());
            //System.Data.Entity.Database.SetInitializer
            //  <MyOracleContext>(new MyDbContextCreateDatabaseIfNotExists());
            //System.Data.Entity.Database.SetInitializer
            //  <MyDbContext>(new MyDbContextDropCreateDatabaseIfModelChanges());
            //System.Data.Entity.Database.SetInitializer<MyOracleContext>(null);
            System.Data.Entity.Database.SetInitializer<MyDbContext>(new MigrateDatabaseToLatestVersion<MyDbContext, Configuration>());
            //--------------------------------------------------------------

            /*--------------------------------------------------------------
            Let's create MyDbContext and execute a database query.
            Depending on selected database initialization strategy,
            database tables can be deleted/added, and filled with source data.
            ---------------------------------------------------------------*/

            using (MyDbContext context = new MyDbContext(con))
            {
                var query = context.Products.ToList();
                //var query = context.Products.Include("Category")
                //               .Where(p => p.Price > 20.0)
                //               .ToList();

                foreach (var product in query)
                    Console.WriteLine("{0,-10} | {1,-50} | {2}",
                      product.ProductID, product.ProductName, product.Category.CategoryName);

                Console.ReadKey();
            }
        }

        // On connection opening, we change the current schema to "TEST":
        static void Connection_StateChange(object sender, StateChangeEventArgs e)
        {

            if (e.CurrentState == ConnectionState.Open)
            {
                DbConnection connection = (DbConnection)sender;
                connection.ChangeDatabase("wf");
            }
        }
    }
}
