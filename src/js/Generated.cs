/** Generated in 2019/03/07 19:51:43 */
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlClasses
{
    public interface ICityQueriesProcessor
    {
        // Methods
        Task<QueryResult<GetCitiesQuery.Result>> GetCities(GetCitiesQuery query);
        Task<QueryResult<GetCityByShortNameQuery.Result>> GetCityByShortName(GetCityByShortNameQuery query);
        Task<QueryResult<GetCityByIdQuery.Result>> GetCityById(GetCityByIdQuery query);
        Task<QueryResult<GetCitiesByIdQuery.Result>> GetCitiesById(GetCitiesByIdQuery query);
    }
    
    public interface IMovieQueriesProcessor
    {
        // Methods
        Task<QueryResult<GetMoviesQuery.Result>> GetMovies(GetMoviesQuery query);
        Task<QueryResult<GetMovieByIdQuery.Result>> GetMovieById(GetMovieByIdQuery query);
    }
    
    public interface ISelectionQueriesProcessor
    {
        // Methods
        Task<QueryResult<GetSelectionsQuery.Result>> GetSelections(GetSelectionsQuery query);
        Task<QueryResult<GetSelectionQuery.Result>> GetSelection(GetSelectionQuery query);
    }
    
                                
    public class GetCitiesQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class CitiesResult
            {
                // Nested classes
                public class ListResult
                {
                    // Nested classes
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public string ShortName 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ListResult[] List 
                { 
                    get;
                    set;
                }
                public int Count 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
            public class AnotherCitiesResult
            {
                // Nested classes
                public class ListResult
                {
                    // Nested classes
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ListResult[] List 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public CitiesResult Cities 
            { 
                get;
                set;
            }
            public AnotherCitiesResult AnotherCities 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getCities($offset: Int, $limit: Int!) {
          cities {
            list(offset: $offset, limit: $limit) {
              ...cityFields
            }
            count
          }
          anotherCities {
            list {
              ...mainCityFields
            }
          }
        }
        fragment mainCityFields on CityType {
          id
          name
        }
        fragment cityFields on CityType {
          ...mainCityFields
          shortName
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "offset", null },
        	{ "limit", null }
        };
        public int? Offset 
        { 
            get => (int?)Variables["offset"];
            set => Variables["offset"] = value;
        }
        public int Limit 
        { 
            get => (int)Variables["limit"];
            set => Variables["limit"] = value;
        }
    
        // Methods
        public GetCitiesQuery(int limit, int? offset = null)
        { 
            Offset = offset;
            Limit = limit;
        }
    }

    public class GetCityByShortNameQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class CitiesResult
            {
                // Nested classes
                public class ByShortNameResult
                {
                    // Nested classes
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public string ShortName 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ByShortNameResult ByShortName 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public CitiesResult Cities 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getCityByShortName($shortName: String!) {
          cities {
            byShortName(shortName: $shortName) {
              ...cityFields
            }
          }
        }
        fragment cityFields on CityType {
          ...mainCityFields
          shortName
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "shortName", null }
        };
        public string ShortName 
        { 
            get => (string)Variables["shortName"];
            set => Variables["shortName"] = value;
        }
    
        // Methods
        public GetCityByShortNameQuery(string shortName)
        { 
            ShortName = shortName;
        }
    }

    public class GetCityByIdQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class CitiesResult
            {
                // Nested classes
                public class ByIdResult
                {
                    // Nested classes
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ByIdResult ById 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public CitiesResult Cities 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getCityById($id: Int!) {
          cities {
            byId(id: $id) {
              id
            }
          }
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "id", null }
        };
        public int Id 
        { 
            get => (int)Variables["id"];
            set => Variables["id"] = value;
        }
    
        // Methods
        public GetCityByIdQuery(int id)
        { 
            Id = id;
        }
    }

    public class GetCitiesByIdQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class CitiesResult
            {
                // Nested classes
                public class ByIdsResult
                {
                    // Nested classes
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public string ShortName 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ByIdsResult[] ByIds 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public CitiesResult Cities 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getCitiesById($ids: [Int]!) {
          cities {
            byIds(ids: $ids) {
              ...cityFields
            }
          }
        }
        fragment cityFields on CityType {
          ...mainCityFields
          shortName
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "ids", null }
        };
        public int[] Ids 
        { 
            get => (int[])Variables["ids"];
            set => Variables["ids"] = value;
        }
    
        // Methods
        public GetCitiesByIdQuery(int[] ids)
        { 
            Ids = ids;
        }
    }

    public class GetMoviesQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class MoviesResult
            {
                // Nested classes
                public class ListResult
                {
                    // Nested classes
                    public class ActorsResult
                    {
                        // Nested classes
                    
                        // Nested interfaces
                    
                        // Fields
                    
                        // Properties
                        public int Id 
                        { 
                            get;
                            set;
                        }
                        public string Name 
                        { 
                            get;
                            set;
                        }
                    
                        // Methods
                    }
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public string Country 
                    { 
                        get;
                        set;
                    }
                    public string ProductionYear 
                    { 
                        get;
                        set;
                    }
                    public ActorsResult[] Actors 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ListResult[] List 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public MoviesResult Movies 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getMovies($offset: Int, $limit: Int) {
          movies {
            list(offset: $offset, limit: $limit) {
              id
              name
              country
              productionYear
              actors {
                id
                name
              }
            }
          }
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "offset", null },
        	{ "limit", null }
        };
        public int? Offset 
        { 
            get => (int?)Variables["offset"];
            set => Variables["offset"] = value;
        }
        public int? Limit 
        { 
            get => (int?)Variables["limit"];
            set => Variables["limit"] = value;
        }
    
        // Methods
        public GetMoviesQuery(int? offset = null, int? limit = null)
        { 
            Offset = offset;
            Limit = limit;
        }
    }

    public class GetMovieByIdQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class MoviesResult
            {
                // Nested classes
                public class ByIdResult
                {
                    // Nested classes
                    public class ActorsResult
                    {
                        // Nested classes
                    
                        // Nested interfaces
                    
                        // Fields
                    
                        // Properties
                        public int Id 
                        { 
                            get;
                            set;
                        }
                        public string Name 
                        { 
                            get;
                            set;
                        }
                    
                        // Methods
                    }
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public string Country 
                    { 
                        get;
                        set;
                    }
                    public string ProductionYear 
                    { 
                        get;
                        set;
                    }
                    public ActorsResult[] Actors 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ByIdResult ById 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public MoviesResult Movies 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getMovieById($id: Int!) {
          movies {
            byId(id: $id) {
              id
              name
              country
              productionYear
              actors {
                id
                name
              }
            }
          }
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "id", null }
        };
        public int Id 
        { 
            get => (int)Variables["id"];
            set => Variables["id"] = value;
        }
    
        // Methods
        public GetMovieByIdQuery(int id)
        { 
            Id = id;
        }
    }

    public class GetSelectionsQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class SelectionsResult
            {
                // Nested classes
                public class ListResult
                {
                    // Nested classes
                    public class ElementsResult
                    {
                        // Nested classes
                        public abstract class SubjectResult
                        {
                            // Nested classes
                        
                            // Nested interfaces
                        
                            // Fields
                        
                            // Properties
                            public string __typename 
                            { 
                                get;
                                set;
                            }
                        
                            // Methods
                        }
                        public class MovieTypeResult: SubjectResult
                        {
                            // Nested classes
                            public class ActorsResult
                            {
                                // Nested classes
                            
                                // Nested interfaces
                            
                                // Fields
                            
                                // Properties
                                public int Id 
                                { 
                                    get;
                                    set;
                                }
                                public string Name 
                                { 
                                    get;
                                    set;
                                }
                            
                                // Methods
                            }
                        
                            // Nested interfaces
                        
                            // Fields
                        
                            // Properties
                            public int Id 
                            { 
                                get;
                                set;
                            }
                            public string Name 
                            { 
                                get;
                                set;
                            }
                            public string Country 
                            { 
                                get;
                                set;
                            }
                            public string ProductionYear 
                            { 
                                get;
                                set;
                            }
                            public ActorsResult[] Actors 
                            { 
                                get;
                                set;
                            }
                        
                            // Methods
                        }
                        public class ConcertTypeResult: SubjectResult
                        {
                            // Nested classes
                        
                            // Nested interfaces
                        
                            // Fields
                        
                            // Properties
                            public int Id 
                            { 
                                get;
                                set;
                            }
                            public string ConcertName 
                            { 
                                get;
                                set;
                            }
                            public string Comment 
                            { 
                                get;
                                set;
                            }
                            public string Program 
                            { 
                                get;
                                set;
                            }
                        
                            // Methods
                        }
                    
                        // Nested interfaces
                    
                        // Fields
                    
                        // Properties
                        public string Description 
                        { 
                            get;
                            set;
                        }
                        public string Verdict 
                        { 
                            get;
                            set;
                        }
                        public SubjectResult Subject 
                        { 
                            get;
                            set;
                        }
                    
                        // Methods
                    }
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public ElementsResult[] Elements 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public int Count 
                { 
                    get;
                    set;
                }
                public ListResult[] List 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public SelectionsResult Selections 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getSelections($offset: Int, $limit: Int) {
          selections {
            count
            list(offset: $offset, limit: $limit) {
              id
              name
              elements {
                description
                verdict
                subject {
                  __typename
                  ... on MovieType {
                    id
                    name
                    country
                    productionYear
                    actors {
                      id
                      name
                    }
                  }
                  ... on ConcertType {
                    id
                    concertName: name
                    comment
                    program
                  }
                }
              }
            }
          }
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "offset", null },
        	{ "limit", null }
        };
        public int? Offset 
        { 
            get => (int?)Variables["offset"];
            set => Variables["offset"] = value;
        }
        public int? Limit 
        { 
            get => (int?)Variables["limit"];
            set => Variables["limit"] = value;
        }
    
        // Methods
        public GetSelectionsQuery(int? offset = null, int? limit = null)
        { 
            Offset = offset;
            Limit = limit;
        }
    }

    public class GetSelectionQuery: IGraphQlQuery
    {
        // Nested classes
        public class Result
        {
            // Nested classes
            public class SelectionsResult
            {
                // Nested classes
                public class ByIdResult
                {
                    // Nested classes
                    public class ElementsResult
                    {
                        // Nested classes
                        public abstract class SubjectResult
                        {
                            // Nested classes
                        
                            // Nested interfaces
                        
                            // Fields
                        
                            // Properties
                            public string __typename 
                            { 
                                get;
                                set;
                            }
                        
                            // Methods
                        }
                        public class MovieTypeResult: SubjectResult
                        {
                            // Nested classes
                            public class ActorsResult
                            {
                                // Nested classes
                            
                                // Nested interfaces
                            
                                // Fields
                            
                                // Properties
                                public int Id 
                                { 
                                    get;
                                    set;
                                }
                                public string Name 
                                { 
                                    get;
                                    set;
                                }
                            
                                // Methods
                            }
                        
                            // Nested interfaces
                        
                            // Fields
                        
                            // Properties
                            public int Id 
                            { 
                                get;
                                set;
                            }
                            public string Name 
                            { 
                                get;
                                set;
                            }
                            public string Country 
                            { 
                                get;
                                set;
                            }
                            public string ProductionYear 
                            { 
                                get;
                                set;
                            }
                            public ActorsResult[] Actors 
                            { 
                                get;
                                set;
                            }
                        
                            // Methods
                        }
                        public class ConcertTypeResult: SubjectResult
                        {
                            // Nested classes
                        
                            // Nested interfaces
                        
                            // Fields
                        
                            // Properties
                            public int Id 
                            { 
                                get;
                                set;
                            }
                            public string ConcertName 
                            { 
                                get;
                                set;
                            }
                            public string Comment 
                            { 
                                get;
                                set;
                            }
                            public string Program 
                            { 
                                get;
                                set;
                            }
                        
                            // Methods
                        }
                    
                        // Nested interfaces
                    
                        // Fields
                    
                        // Properties
                        public string Description 
                        { 
                            get;
                            set;
                        }
                        public string Verdict 
                        { 
                            get;
                            set;
                        }
                        public SubjectResult Subject 
                        { 
                            get;
                            set;
                        }
                    
                        // Methods
                    }
                
                    // Nested interfaces
                
                    // Fields
                
                    // Properties
                    public int Id 
                    { 
                        get;
                        set;
                    }
                    public string Name 
                    { 
                        get;
                        set;
                    }
                    public ElementsResult[] Elements 
                    { 
                        get;
                        set;
                    }
                
                    // Methods
                }
            
                // Nested interfaces
            
                // Fields
            
                // Properties
                public ByIdResult ById 
                { 
                    get;
                    set;
                }
            
                // Methods
            }
        
            // Nested interfaces
        
            // Fields
        
            // Properties
            public SelectionsResult Selections 
            { 
                get;
                set;
            }
        
            // Methods
        }
    
        // Nested interfaces
    
        // Fields
    
        // Properties
        public string QueryText 
        { 
            get;
            set;
        } = 
        @"
        query getSelection($id: Int!) {
          selections {
            byId(id: $id) {
              id
              name
              elements {
                description
                verdict
                subject {
                  __typename
                  ... on MovieType {
                    ...mainMovieFields
                    country
                    productionYear
                    actors {
                      id
                      name
                    }
                  }
                  ... on ConcertType {
                    id
                    concertName: name
                    comment
                    program
                  }
                }
              }
            }
          }
        }
        fragment mainMovieFields on MovieType {
          id
          name
        }
        ";
        public IDictionary<string, object> Variables 
        { 
            get;
            set;
        } = new Dictionary<string, object>
        {
        	{ "id", null }
        };
        public int Id 
        { 
            get => (int)Variables["id"];
            set => Variables["id"] = value;
        }
    
        // Methods
        public GetSelectionQuery(int id)
        { 
            Id = id;
        }
    }
}
