using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gqltest
{
    public class MainQuery : ObjectGraphType<object>
    {
        public MainQuery()
        {
            Name = "Query";
            FieldAsync<DudeType, Dude>(
                "dude", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the human" }
                ),
                resolve: context => DataGetter.getDude(context.GetArgument<int>("id"), ((GraphQLUserContext)context.UserContext).id)
            );

            Field<ListGraphType<DudeType>>(
                "dudes",
                resolve: context => DataGetter.getDudes()
            );

            Field<ListGraphType<StringGraphType>>("tester",
                resolve: context => DataGetter.getStrings());
        }
    }

    public class MainMutation : ObjectGraphType
    {
        public MainMutation()
        {
            Name = "Mutation";

            Field<DudeType>(
                "createDude",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<InputDudeType>> { Name = "dude" }
                ),
                resolve: context =>
                {
                    var human = context.GetArgument<Dude>("dude");
                    return DataGetter.addDude(human);
                }
            );
        }
    }

    public static class DataGetter
    {
        private static List<Dude> _dudes;

        static DataGetter()
        {
            _dudes = new List<Dude>()
            {
                new Dude()
                {
                    Id = 1,
                    Name = "John"
                },
                new Dude()
                {
                    Id = 2,
                    Name = "Bob",
                    Cars = new List<string>() {"ford"}
                }
            };
        }

        public static List<Dude> getDudes()
        {
            return _dudes;
        }

        public static Dude addDude(Dude d)
        {
            d.Id = _dudes.Max(x => x.Id) + 1;
            _dudes.Add(d);
            return d;
        }

        public static List<string> getStrings()
        {
            return new List<string>() { "hello", "world" };
        }
        public static async Task<Dude> getDude(int id, string id2)
        {
            return _dudes.FirstOrDefault(x => x.Id == id);
        }
    }

    public class InputDudeType : InputObjectGraphType<Dude>
    {
        public InputDudeType()
        {
            Name = "DudeInput";

            Field(d => d.Name).Description("The dudes name");
            Field(x => x.Cars, nullable: true).Description("Mah cars");
        }
    }

    public class DudeType: ObjectGraphType<Dude>
    {
        public DudeType()
        {
            Name = "Dude";

            Field(d => d.Id).Description("The ID of the dude");
            Field(d => d.Name).Description("The dudes name");
            Field(x => x.Cars, nullable: true).Description("Mah cars");
        }
    }

    public class Dude
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Cars { get; set; }
    }
}
