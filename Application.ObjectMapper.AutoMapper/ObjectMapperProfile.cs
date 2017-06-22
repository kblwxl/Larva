
using Infrastructure.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.ObjectMapper.AutoMapper
{
    
    
    public class MemberMapProfile<TSource,  TDestination>
    {
        public Expression<Func<TSource, dynamic>> SourceMember { get; set; }
        public Expression<Func<TDestination, dynamic>> DestinationMember { get; set; }
    }
    
    public abstract class ObjectMapperProfile<TSource, TDestination>
        where TDestination:IDataObject
    {
        public List<MemberMapProfile<TSource, TDestination>> SourceToDestination { get; private set; }
        public List<MemberMapProfile<TDestination, TSource>> DestinationToSource { get; private set; }

        protected ObjectMapperProfile()
        {
            SourceToDestination = new List<MemberMapProfile<TSource, TDestination>>();
            DestinationToSource = new List<MemberMapProfile<TDestination, TSource>>();
        }
        protected void FromMember(
            Expression<Func<TDestination, dynamic>> dtoMember,
            Expression<Func<TSource, dynamic>> sourceMember
            )
        {
            MemberMapProfile<TSource, TDestination> profile = new MemberMapProfile<TSource, TDestination>();
            profile.SourceMember = sourceMember;
            profile.DestinationMember = dtoMember;

            SourceToDestination.Add(profile);
        }
        protected void ToMember(
            Expression<Func<TSource, dynamic>> sourceMember,
            Expression<Func<TDestination, dynamic>> dtoMember
            )
        {
            MemberMapProfile<TDestination, TSource> profile = new MemberMapProfile<TDestination, TSource>();
            profile.SourceMember = dtoMember;
            profile.DestinationMember = sourceMember;

            DestinationToSource.Add(profile);
        }
        public abstract void MapFrom();
        public abstract void MapTo();

    }
}
