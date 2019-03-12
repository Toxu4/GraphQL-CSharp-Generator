namespace Toxu4.GraphQl.Client
{
    public class QueryResult<TData>
    {
        public TData Data { get; set; }
        public ErrorResult[] Errors { get; set; }
        
        public void Deconstruct(out TData data, out ErrorResult[] errors)
        { 
            data = Data;
            errors = Errors;
        }        
    }
}
