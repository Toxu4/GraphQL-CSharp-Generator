namespace Toxu4.GraphQl.Client
{
    public class ErrorResult
    {
        public class LocationResult
        {
            public int Line { get; set; }
            public int Column { get; set; }
        }

        public class ExtensionsResult
        {
            public string Code { get; set;}
        }
        
        public string Message { get; set;}
        public LocationResult[] Locations { get; set;}
        public ExtensionsResult Extensions { get; set;}
    }
}
