﻿using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dapper.FluentMap.Dommel.Mapping;
using Demo.Data.Models;

namespace Demo.Data.DapperRepository.Mappers
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

    public class PatientMap : DommelEntityMap<TPatient>
    {

        public PatientMap()
        {
            ToTable("T_PATIENT", "dbo");
            Map(p => p.PatientId).IsIdentity().IsKey();
            // Map(p => p.PatientId).ToColumn("PatientId").IsIdentity().IsKey();
            // Map(p => p.MedRecNumber).ToColumn("MedRecNumber");
            // Map(p => p.FullName).Ignore();
        }
    }
}