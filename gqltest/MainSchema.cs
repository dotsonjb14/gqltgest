using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gqltest
{
    public class MainSchema : Schema
    {
        public MainSchema(IDependencyResolver resolver): base(resolver)
        {
            Query = resolver.Resolve<MainQuery>();
            Mutation = resolver.Resolve<MainMutation>();
        }
    }
}
