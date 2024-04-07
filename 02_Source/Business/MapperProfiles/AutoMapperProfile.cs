using AutoMapper;
using Domain.Filters;
using Domain.Models;
using Dtos.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TaskModel, TaskDto>();
            CreateMap<TaskDto, TaskModel>();


            CreateMap<TaskFilterDto, TaskFilter>();

        }
    }
}
