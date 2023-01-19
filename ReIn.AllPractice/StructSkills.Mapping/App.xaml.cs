using AutoMapper;
using AutoMapper.Configuration;

using StructSkills.Mapping.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StructSkills.Mapping
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            List<Employee> employees = EmployeeRepository.GetAll().ToList();

            IConfigurationProvider configProvider = new MapperConfiguration(new MapperConfigurationExpression());

            Mapper mapper = new Mapper(configProvider);

            var juniorToLead = mapper.Map<Lead>(employees.Last());

            base.OnStartup(e);
        }
    }
}
