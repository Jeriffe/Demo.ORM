using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using Demo.DapperConsole.Models;

namespace Demo.DapperConsole
{
    public class FluentMappers 
    {
        public static void Initialize()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new PatientMap());
                config.ForDommel();
            });
        }
    }

    public class PatientMap : DommelEntityMap<Patient>
    {

        public PatientMap()
        {
            ToTable("T_PATIENT","dbo");
            Map(p => p.ID).ToColumn("PatientID").IsIdentity().IsKey();
            Map(p => p.MedRecordNumber).ToColumn("MedRecNumber");
            Map(p => p.FullName).Ignore();
        }
    }
}
