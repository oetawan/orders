// <auto-generated />
namespace order.data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    public sealed partial class AddOrderDateStringToOrdersTable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddOrderDateStringToOrdersTable));
        
        string IMigrationMetadata.Id
        {
            get { return "201212091052484_AddOrderDateStringToOrdersTable"; }
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