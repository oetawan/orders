// <auto-generated />
namespace order.data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    public sealed partial class AddBranchIdFieldToOrdersTable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddBranchIdFieldToOrdersTable));
        
        string IMigrationMetadata.Id
        {
            get { return "201212091327457_AddBranchIdFieldToOrdersTable"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}